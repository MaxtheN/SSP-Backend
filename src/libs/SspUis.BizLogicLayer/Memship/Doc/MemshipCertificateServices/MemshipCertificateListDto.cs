using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class MemshipCertificateListDto : DocumentListDto<long>, ILinkToEntity<MemshipCertificate>, IHaveIdProp<long>, IHaveStatusId
{
    public string DocNumber { get; set; } = null!;
    public Guid Id2 { get; set; }
    public string Status { get; set; }
    public string Organization { get; set; } = null!;
    public string Contractor { get; set; }
    public string Director { get; set; }
    public string ContractorInn { get; set; }
    public string ContractorPinfl { get; set; }
    public string ContractorOked { get; set; }
	public string PhoneNumber { get; set; }
	public string ContractorSettlementAccount { get; set; }
    public long? MemshipApplicationId { get; set; }
    public bool IsRead { get; set; }
    public int MemshipContractTypeId { get; set; }
    public string MemshipContractType { get; set; }
    public int RegionId { get; set; }
    public int DistrictId { get; set; }
    public int? OpfId { get; set; }
    public int? ContractorOkedId { get; set; }
    public string? Opf { get; set; }
    public string Region { get; set; }
    public string District { get; set; }
    public DateOnly ExpireOn { get; set; }
    public int? ContractorCategoryId { get; set; }
    public string ContractorCategory { get; set; }

    #region Actions
    public bool CanEdit { get; set; }
    public bool CanAccept { get; set; }
    public bool CanCancel { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}

public class MemshipCertificateExpiredPaymentDto
{
    public bool IsExpired { get; set; }
    public int ExpiredCount { get; set; }
    public string Message { get; set; }
    public string Url { get; set; }
}

public class MemshipCertificateExpiredPaymentsDto
{
    public List<MemshipCertificateExpiredPaymentDto> Deadlines { get; set; }

    public MemshipCertificateExpiredPaymentsDto()
    {
        Deadlines = new List<MemshipCertificateExpiredPaymentDto>();
    }
}

public class MemshipCertificateToPaidNotificationDto
{
    public long Id { get; set; }
    public bool IsNotification { get; set; }
    public string DocNumber { get; set; }
    public string Contractor { get; set; }
    public string ContractorInn { get; set; }
}

public class MemshipCertificateToPaidNotificationsDto
{
    public List<MemshipCertificateToPaidNotificationDto> Notifications { get; set; }
    public MemshipCertificateToPaidNotificationsDto()
    {
        Notifications = new List<MemshipCertificateToPaidNotificationDto>();
    }
}