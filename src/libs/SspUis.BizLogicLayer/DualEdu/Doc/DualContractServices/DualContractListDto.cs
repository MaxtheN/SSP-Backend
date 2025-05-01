using GenericServices;
using SspUis.DataLayer.EfClasses;
using System;
using WEBASE.Models;

namespace SspUis.BizLogicLayer;

public class DualContractListDto
        : DocumentListDto<long>, ILinkToEntity<DualContract>, IHaveIdProp<long>, IHaveStatusId
{
    public Guid Id2 { get; set; }
    public string DocNumber { get; set; }
    public DateOnly DocDate { get; set; }
    public string ContracttorFullName { get; set; }
    public string ContracttorInn { get; set; }
    public string StudentFullname { get; set; }
    public string Pinfl { get; set; }
    public string Institute { get; set; }
    public string Speciality { get; set; }
    public string EduType { get; set; }
    public string Status { get; set; }
    public string DualEducationType { get; set; }
}