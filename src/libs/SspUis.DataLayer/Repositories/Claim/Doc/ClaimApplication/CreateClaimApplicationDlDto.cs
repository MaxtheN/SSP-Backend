using SspUis.DataLayer.EfClasses;

namespace SspUis.DataLayer.Repositories
{
    public class CreateClaimApplicationDlDto : ClaimApplicationDlDto<CreateClaimApplicationDlDto>
    {
        public CreateClaimApplicationDlDto()
        {
            
        }

        public CreateClaimApplicationDlDto(Contractor contractor)
        {
            IntegrationContractor = contractor;
        }


        internal Contractor IntegrationContractor { get; }
    }
    
}
