using System;

namespace SspUis.BizLogicLayer.ReportServices;
public class PrntExpiredDocumentsDto
{
    public int? RegionId { get; set; }
    public string Region { get; set; }
    public int? DistrictId { get; set; }
    public string District { get; set; }
    public long? ContractorId { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPhoneNumber { get; set; }

    public long ExApplicationSentTheNeighborhood { get; set; } //ariza Mahalladan javob qaytishi 5 kun waiting  // SENT_FOR_REVIEW
    public long ExApplicationWaiting { get; set; } //ariza kutish WAITING
    public long ExContractSentForExpertise { get; set; } //shartnoma ekspertizaga yuborilgan 3 kun SENT_FOR_EXPERTISE
    public long ExContractReSentForExpertise { get; set; } //shartnoma ekspertizaga qayta yuborish 5 kun NOT_PASS_EXPERTISE
    public long ExContaractDeadlineAtSigning { get; set; } // shartnoma imzolash 5 kun PASS_EXPERTISE // SIGNING
    public long ExCertificate { get; set; } // muddati o'tgan arizalar 1 kun SIGNED
}
public static class DayLimitBeExpired
{
    public const int APP_RESPONSE_NEIGHBORHOOD = 5;
    public const int CON_SENT_EXPERTISE = 3;
    public const int CON_RE_SENT_EXPERTISE = 5;
    public const int CON_DEADLINE_SIGNING = 5;
    public const int CER_SIGNED = 1;
}