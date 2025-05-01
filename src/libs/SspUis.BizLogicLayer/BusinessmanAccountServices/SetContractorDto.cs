using Hangfire.Annotations;
using SspUis.BizLogicLayer.BusinessmanAccountServices;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer
{
    public class SetContractorDto
    {
        [LocalizationRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BusinessmanUserId { get; set; }

        [LocalizationRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }
    }

    public class ToAddOrganizationDto : OfferDto
    {
        public string Inn { get; set; }
        public string Pinfl { get; set; }
    }

    public class DeactivateAssociationDto
    {
        [LocalizedRequired]
        public string Username { get; set; }

        [LocalizedRequired]
        public string SmsCode { get; set; }

        [LocalizationRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long ContractorId { get; set; }
    }
}