using System;
using GenericServices;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class EmployeeListDto : ILinkToEntity<Employee>, IHaveIdProp<int>
{
    public int Id { get; set; }
    public int? OrganizationId { get; set; }
    public int PositionId { get; set; }
    public int StateId { get; set; }
    public Guid? PictureId { get; set; }
    public string PassportInfo { get; set; }
    public string FullName { get; set; }
    public string Pinfl { get; set; }
    public string Organization { get; set; }
    public string Region { get; set; }
    public int? RegionId { get; set; }
    public string District { get; set; }
    public int? DistrictId { get; set; }
    public string State { get; set; }
    public bool HasMilitary { get; set; }
    public DateOnly BirthDate { get; set; }
    public int? TotalWorkedYear { get; set; }
    public int? TotalWorkedMonth { get; set; }
    public int? TotalWorkedDay { get; set; }
    public string PhoneNumber { get; set; }
}
