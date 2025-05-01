using System;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;
public class EmployeeRelativeDlDto : EntityDto<EmployeeRelativeDlDto, EmployeeRelative>,IHaveIdProp<int>
{
    public int Id { get; set; }
    public DateOnly OnDate { get; set; } 
    public int RelativeDegreeId { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(100)]
    public string FamilyName { get; set; }
    [LocalizedRequired]
    [LocalizedStringLength(100)]
    public string FirstName { get; set; }
    [LocalizedStringLength(100)]
    public string LastName { get; set; }
    [JsonIgnore]
    public string? ShortName { get; set; }
    [JsonIgnore]
    public string? FullName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    [LocalizedStringLength(14)]
    public string Pinfl { get; set; }
    public bool HasDied { get; set; }
    public DateOnly? DateOfDeath { get; set; }
    public int? CountryId { get; set; }
    public int? RegionId { get; set; }
    public int? DistrictId { get; set; }
    [LocalizedStringLength(300)]
    public string Address { get; set; }
    [LocalizedStringLength(30)]
    public string PhoneNumber { get; set; }
    public int? IdentityDocumentId { get; set; }
    [LocalizedStringLength(5)]
    public string DocumentSeries { get; set; }
    [LocalizedStringLength(10)]
    public string DocumentNumber { get; set; }
    public DateOnly? DateOfIssue { get; set; }
    public DateOnly? DateOfExpire { get; set; }
    [LocalizedStringLength(100)]
    public string IssueOrganization { get; set; }
    public int? NationalityId { get; set; }
    public int? CitizenshipId { get; set; }
    public string? RelativeWorkPlace { get; set; }
    public string? RelativeWorkPlacePosition { get; set; }
}
