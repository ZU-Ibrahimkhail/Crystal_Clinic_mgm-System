using System.Text.Json.Serialization;
using System.Xml.Linq;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Look.BranchDetail
{
    #region Create BranchDetails
    public class CreateBranchDetailsCommand : IRequest<int>
    {
        public int BranchId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string HeaderNote { get; set; } = string.Empty;
        public string FooterNote { get; set; } = string.Empty;
        public List<string> PhoneNumbers { get; set; } = [];
        public string Address { get; set; } = string.Empty;
    }

    public class CreateBranchDetailsHandler(ERP_DbContext context) : IRequestHandler<CreateBranchDetailsCommand, int>
    {
        public async Task<int> Handle(CreateBranchDetailsCommand request, CancellationToken cancellationToken)
        {
            int DetailsId = 0;
            var executionStrategy = context.Database.CreateExecutionStrategy();

            DetailsId = await executionStrategy.ExecuteAsync(async () =>
            {
                try
                {
                    await context.BranchDetails
                        .Where(x => x.BranchId == request.BranchId)
                        .ForEachAsync(x => x.IsActive = false, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    var branchDetails = new BranchDetails
                    {
                        BranchId = request.BranchId,
                        Title = request.Title,
                        HeaderNote = request.HeaderNote,
                        FooterNote = request.FooterNote,
                        PhoneNumbers = request.PhoneNumbers != null ? string.Join(",", request.PhoneNumbers) : string.Empty,
                        Address = request.Address,
                    };

                    await context.BranchDetails.AddAsync(branchDetails, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    return branchDetails.Id;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred: " + ex.Message);
                }

            });
            return DetailsId;
        }
        #endregion

    #region Update BranchDetails
        public class UpdateBranchDetailsCommand : IRequest<bool>
        {
            [JsonIgnore]
            public int Id { get; set; }
            public int BranchId { get; set; }
            public string Title { get; set; } = string.Empty;
            public string HeaderNote { get; set; } = string.Empty;
            public string FooterNote { get; set; } = string.Empty;
            public List<string> PhoneNumbers { get; set; } = [];
            public string Address { get; set; } = string.Empty;
        }

        public class UpdateBranchDetailsHandler(ERP_DbContext context) : IRequestHandler<UpdateBranchDetailsCommand, bool>
        {
            public async Task<bool> Handle(UpdateBranchDetailsCommand request, CancellationToken cancellationToken)
            {
                var branchDetails = await context.BranchDetails.FindAsync(request.Id);
                if (branchDetails == null) return false;

                branchDetails.BranchId = request.BranchId;
                branchDetails.Title = request.Title;
                branchDetails.HeaderNote = request.HeaderNote;
                branchDetails.FooterNote = request.FooterNote;
                branchDetails.PhoneNumbers = request.PhoneNumbers != null ? string.Join(",", request.PhoneNumbers) : string.Empty;
                branchDetails.Address = request.Address;

                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
        #endregion

    #region Delete BranchDetails
        public class DeleteBranchDetailsCommand : IRequest<bool>
        {
            public int Id { get; set; }
        }

        public class DeleteBranchDetailsHandler(ERP_DbContext context) : IRequestHandler<DeleteBranchDetailsCommand, bool>
        {

            public async Task<bool> Handle(DeleteBranchDetailsCommand request, CancellationToken cancellationToken)
            {
                var branchDetails = await context.BranchDetails.FindAsync(request.Id);
                if (branchDetails == null) return false;

                branchDetails.IsActive = false;
                context.BranchDetails.Update(branchDetails);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
    }
    #endregion

    #region Activate BranchDetails
    public class ActivateBranchDetailsCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class ActivateBranchDetailsHandler(ERP_DbContext context) : IRequestHandler<ActivateBranchDetailsCommand, bool>
    {

        public async Task<bool> Handle(ActivateBranchDetailsCommand request, CancellationToken cancellationToken)
        {
            var branchDetails = await context.BranchDetails.FindAsync(request.Id);
            if (branchDetails == null) return false;

            branchDetails.IsActive = true;
            context.BranchDetails.Update(branchDetails);
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }

    #endregion

    #region Get All BranchDetails
    public class GetBranchDetailsQuery : IRequest<GetBranchDetailsResponse>
    {
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetBranchDetailsHandler(ERP_DbContext context) : IRequestHandler<GetBranchDetailsQuery, GetBranchDetailsResponse>
    {
        public async Task<GetBranchDetailsResponse> Handle(GetBranchDetailsQuery request, CancellationToken cancellationToken)
        {
            var query = context.BranchDetails.AsQueryable();

            // Apply search filter if provided
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(b => b.Title.Contains(request.SearchText) ||
                                          b.PhoneNumbers.Contains(request.SearchText) ||
                                          b.Address.Contains(request.SearchText));
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var branchDetails = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BranchDetailsDto(x)).ToListAsync(cancellationToken);

            return new GetBranchDetailsResponse
            {
                BranchDetails = branchDetails,
                TotalCount = totalCount
            };
        }
    }

    public class GetBranchDetailsResponse
    {
        public List<BranchDetailsDto> BranchDetails { get; set; } = [];
        public int TotalCount { get; set; }
    }

    public class BranchDetailsDto(BranchDetails branchDetails)
    {
        public int Id { get; set; } = branchDetails.Id;
        public string Title { get; set; } = branchDetails.Title ?? string.Empty;
        public string HeaderNote { get; set; } = branchDetails.HeaderNote ?? string.Empty;
        public string FooterNote { get; set; } = branchDetails.FooterNote ?? string.Empty;
        public List<string> PhoneNumbers { get; set; } = branchDetails.PhoneNumbers.Split(",").ToList() ?? [];
        public string Address { get; set; } = branchDetails.Address ?? string.Empty;
        public bool IsActive { get; set; } = branchDetails.IsActive;
    }



    #endregion
    public class GetOneBranchDetailsQuery : IRequest<BranchDetailsDto>
    {
        public int? BranchId { get; set; }
    }

    public class GetOneBranchDetailsHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<GetOneBranchDetailsQuery, BranchDetailsDto>
    {
        public async Task<BranchDetailsDto> Handle(GetOneBranchDetailsQuery request, CancellationToken cancellationToken)
        {
            var query = context.BranchDetails.AsQueryable();
            if (request.BranchId.HasValue)
            {
                query.Where(x => x.BranchId == request.BranchId);
            }
            else
            {
                query.Where(x => x.BranchId == loggedInUser.BranchId);
            }
         

            // Apply pagination
            var branchDetails = await query
                .Select(x => new BranchDetailsDto(x)).FirstOrDefaultAsync(cancellationToken);

            return branchDetails ?? new BranchDetailsDto(new BranchDetails
            {
                Title = "Afghan Crystal Beauty Clinic",
                HeaderNote = "Your Beauty Our Duty",
                FooterNote = "Than you for selecting us \n Please keep this paper with yourself!", 
                Address = "Kabul Lesei maryam!"
            });
        }
    }

}
