using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Crystal_ClinicServices.Patients
{

    public class CreatePatientCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Email { get; set; }
        public decimal? age { get; set; }
    }

    public class CreatePatientHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreatePatientCommand, int>
    {
        public async Task<int> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = new Patient
            {
                name = request.Name,
                contactInfo = request.ContactInfo,
                email = request.Email,
                age = request.age,
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.UtcNow
            };
            context.Patient.Add(patient);
            await context.SaveChangesAsync(cancellationToken);
            return patient.patientId;
        }
    }

    public class UpdatePatientCommand : IRequest<bool>
    {
        public int PatientId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Email { get; set; }
        public decimal? age { get; set; }
        public string? gender { get; set; }
    }
    public class UpdatePatientHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdatePatientCommand, bool>
    {
        public async Task<bool> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await context.Patient.FindAsync(request.PatientId);
            if (patient == null)
                return false;

            patient.name = request.Name;
            patient.contactInfo = request.ContactInfo;
            patient.email = request.Email;
            patient.ModifiedOn = DateTime.UtcNow;
            patient.ModifiedBy = loggedInUser.Id;

            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
   
    public class DeletePatientCommand : IRequest<bool>
    {
        public int PatientId { get; set; }
    }
    public class DeletePatientHandler(ERP_DbContext context) : IRequestHandler<DeletePatientCommand, bool>
    {
        public async Task<bool> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patient = await context.Patient.FindAsync(request.PatientId);
            if (patient == null)
                return false;

            patient.IsDeleted = true;
            context.Patient.Update(patient);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    
    
    public class GetPatientByIdQuery : IRequest<Patient?>
    {
        public int PatientId { get; set; }
    }
    public class GetPatientByIdHandler(ERP_DbContext context) : IRequestHandler<GetPatientByIdQuery, Patient?>
    {
        public async Task<Patient?> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            return await context.Patient.FindAsync(request.PatientId);
        }


    }


    public class GetAllPatientsQuery : IRequest<List<Patient>>
    {
        public string? searchby { get; set; }
        public int pageSize { get; set; } = 30;
        public int? lastId { get; set; }
    }
    public class GetAllPatientsHandler(ERP_DbContext context) : IRequestHandler<GetAllPatientsQuery, List<Patient>>
    {
        public async Task<List<Patient>> Handle(GetAllPatientsQuery request, CancellationToken cancellationToken)
        {
            var data = context.Patient.Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.patientId)
                .AsQueryable();
            if (!string.IsNullOrEmpty(request.searchby))
            {
                data = data.Where(x => 
                x.name.Contains(request.searchby) || 
                x.contactInfo.Contains(request.searchby) || 
                x.gender.Contains(request.searchby) || 
                x.email.Contains(request.searchby));
            }
            if (request.lastId.HasValue)
            {
                data = data.Where(x => x.patientId < request.lastId);
            }
            return await data.Take(request.pageSize).ToListAsync();
        }
    }
}