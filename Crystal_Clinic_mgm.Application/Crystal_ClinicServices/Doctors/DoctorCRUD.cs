using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.CrystalClinic.Doctors
{
    #region Create Doctor

    public class CreateDoctorCommand : IRequest<int>
    {
        public int EmployeeId { get; set; }  // The Employee ID
        public string Specialty { get; set; } = string.Empty;  // Specialty of the doctor
        public List<int> services { get; set; } = [];
    }

    public class CreateDoctorHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateDoctorCommand, int>
    {
        public async Task<int> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            // Fetch the employee details using EmployeeId
            var employee = await context.EmployeeProfiles.FirstOrDefaultAsync(e => e.ID == request.EmployeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {request.EmployeeId} not found.");
            }

            // Create a new Doctor entity using data from the EmployeeProfile and provided Specialty
            var doctor = new Doctor
            {
                firstName = employee.EnglishFirstName,
                lastName = employee.EnglishSurName,
                specialty = request.Specialty,
                contactInfo = employee.PhoneNumber,
                services = string.Join(",", request.services),
                isAvailable = true,  // By default, the doctor is available
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
            };

            // Add the new doctor to the database
            context.Doctor.Add(doctor);
            await context.SaveChangesAsync(cancellationToken);

            return doctor.doctorId;  // Return the doctor ID after insertion
        }
    }

    #endregion

    #region Update Doctor

    public class UpdateDoctorCommand : IRequest<int>
    {
        public int DoctorId { get; set; }  // The ID of the doctor
        public string Specialty { get; set; } = string.Empty;  // Specialty of the doctor to update
        public List<int> services { get; set; } = [];
        public bool isAvailable { get; set; }
    }

    public class UpdateDoctorHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateDoctorCommand, int>
    {
        public async Task<int> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            // Find the doctor by DoctorId
            var doctor = await context.Doctor.FirstOrDefaultAsync(d => d.doctorId == request.DoctorId, cancellationToken);

            if (doctor == null || doctor.IsDeleted)
            {
                throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");
            }

            // Update the doctor's specialty
            doctor.specialty = request.Specialty;
            doctor.isAvailable = request.isAvailable;
            doctor.services = string.Join(",", request.services);
            doctor.ModifiedBy = loggedInUser.Id;
            doctor.ModifiedOn = DateTime.Now;

            // Save the updated doctor to the database
            context.Doctor.Update(doctor);
            await context.SaveChangesAsync(cancellationToken);

            return doctor.doctorId;  // Return the updated doctor ID
        }
    }

    #endregion

    #region Delete Doctor

    public class DeleteDoctorCommand : IRequest<bool>
    {
        public int DoctorId { get; set; }  // The ID of the doctor to delete
    }

    public class DeleteDoctorHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteDoctorCommand, bool>
    {
        public async Task<bool> Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            // Find the doctor by DoctorId
            var doctor = await context.Doctor.FirstOrDefaultAsync(d => !d.IsDeleted && d.doctorId == request.DoctorId);

            if (doctor == null)
            {
                return false;  // Doctor not found
            }

            // Remove the doctor from the database
            doctor.IsDeleted = true;
            doctor.ModifiedBy = loggedInUser.Id;
            doctor.ModifiedOn = DateTime.Now;
            context.Doctor.Update(doctor);
            await context.SaveChangesAsync(cancellationToken);

            return true;  // Doctor deleted successfully
        }
    }

    #endregion

    #region Get Doctor Details

    public class GetDoctorDetailsQuery : IRequest<DoctorDto>
    {
        public int DoctorId { get; set; }  // The ID of the doctor to fetch
    }

    public class GetDoctorDetailsHandler(ERP_DbContext context) : IRequestHandler<GetDoctorDetailsQuery, DoctorDto>
    {
        public async Task<DoctorDto> Handle(GetDoctorDetailsQuery request, CancellationToken cancellationToken)
        {
            // Find the doctor by DoctorId
            var doctor = await context.Doctor.FirstOrDefaultAsync(d => !d.IsDeleted && d.doctorId == request.DoctorId);

            if (doctor == null)
            {
                throw new KeyNotFoundException($"Doctor with ID {request.DoctorId} not found.");
            }
            var services = doctor.services.Split(new char[] { ',' }).Select(int.Parse).ToList();

            var serviceNames = context.Services.Where(x => !x.IsDeleted && services.Contains(x.ServiceId)).Select(x => x.Name).ToList();
            // Map the doctor to a DTO
            return new DoctorDto
            {
                DoctorId = doctor.doctorId,
                FirstName = doctor.firstName,
                LastName = doctor.lastName,
                services = services,
                serviceNames = serviceNames,
                Specialty = doctor.specialty,
                ContactInfo = doctor.contactInfo,
                IsAvailable = doctor.isAvailable
            };
        }
    }

    #endregion

    #region Get Doctor List

    public class GetDoctorListQuery : IRequest<DoctorListDto>
    {
        public string? Search { get; set; }
        public int? LastDoctorId { get; set; } // For cursor pagination
        public int PageSize { get; set; } = 20;
    }

    public class GetDoctorListHandler(ERP_DbContext context) : IRequestHandler<GetDoctorListQuery, DoctorListDto>
    {
        public async Task<DoctorListDto> Handle(GetDoctorListQuery request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
              {

                  var doctor = context.Doctor.Where(d => !d.IsDeleted).AsQueryable();
                  if (!string.IsNullOrWhiteSpace(request.Search))
                  {
                      doctor = doctor.Where(x => x.firstName.Contains(request.Search) || x.lastName.Contains(request.Search) || x.specialty.Contains(request.Search));
                  }
                  int totalCount = doctor.Count();
                  if (request.LastDoctorId.HasValue)
                  {
                      doctor = doctor.Where(x => x.doctorId > request.LastDoctorId);
                  }
                  doctor = doctor.OrderBy(x => x.doctorId).Take(request.PageSize);


                  return new DoctorListDto
                  {
                      Data = [.. doctor.Select(doctor =>
                        new DoctorDto
                        {
                            DoctorId = doctor.doctorId,
                            FirstName = doctor.firstName,
                            LastName = doctor.lastName,
                            services = doctor.services.Split(new char[] { ',' }).Select(int.Parse).ToList(),
                            Specialty = doctor.specialty,
                            ContactInfo = doctor.contactInfo,
                            IsAvailable = doctor.isAvailable
                        })],
                      TotalCount = totalCount,
                  };
              });
        }
    }


    #endregion

    #region Doctor DTO

    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public List<int> services { get; set; } = [];
        public List<string> serviceNames { get; set; } = [];
        public bool IsAvailable { get; set; }
    }

    public class DoctorListDto
    {
        public List<DoctorDto> Data { get; set; } = [];
        public int TotalCount { get; set; }
    }
    #endregion
}
