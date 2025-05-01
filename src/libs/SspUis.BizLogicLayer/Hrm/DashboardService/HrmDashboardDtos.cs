using DocumentFormat.OpenXml.Wordprocessing;
using SspUis.Core;
using System;
using System.Collections.Generic;

namespace SspUis.BizLogicLayer.Hrm;
public class HrmEmployeeDto
{
    public int TotalEmployeeCount { get; set; }
    public int TotalEmployeeMenCount { get; set; }
    public int TotalEmployeeWomenCount { get; set; }
}
public class HrmEmployeeLeaveOrderDto
{
    public string Employee { get; set; }
    public DateOnly EmployeeDateBeforeRecall { get; set; }
    public int DaysUntilBeforeRecall { get; set; }
}
public class HrmEmployeeByRegionDto
{
    public int TotalEmployeeCount { get; set; }
    public string Region{ get; set; }
}
public class HrmEmployeeGenderDto 
{
    public int TotalEmplpyeeMen { get; set; }
    public int TotalEmplpyeeWomen { get; set; }
}
public class HrmEmployeeAgeDto
{
    public int TotalUpTo20 { get; set; }
    public int TotalFrom20UpTo30 { get; set; }
    public int TotalFrom30UpTo40 { get; set; }
    public int TotalFrom40UpTo { get; set; }
}
public class HrmEmployeeHigherEduCount
{
    public int TotalEmployeeHigerEduCount { get; set; }
    public int TotalEmployeeSecondaryEduCount { get; set; }
    public int TotalEmployeeHigerEduMenCount { get; set; }
    public int TotalEmployeeHigerEduWomenCount { get; set; }
    public int TotalEmployeeSecondaryEduMenCount { get; set; }
    public int TotalEmployeeSecondaryEduWomenCount { get; set; }
}
public class HrmEmployeeAcademicDegree
{
    public int TotalEmployeePhdCount { get; set; }
    public int TotalEmployeeMasterCount { get; set; }
    public int TotalEmployeeBachelorCount { get; set; }
    public int TotalEmployeePhdMenCount { get; set; }
    public int TotalEmployeePhdWomenCount { get; set; }
    public int TotalEmployeeMasterMenCount { get; set; }
    public int TotalEmployeeMasterWomenCount { get; set; }
    public int TotalEmployeeBachelorMenCount { get; set; }
    public int TotalEmployeeBachelorWomenCount { get; set; }
}
public class HrmEmployeeLegalEducation
{
    public int TotalEmployeeLegalEducationMenCount { get; set; }
    public int TotalEmployeeLegalEducationWomenCount { get; set; }
}
public class HrmEmployeeExperience
{
    public int TotalUpTo1 { get; set; }
    public int TotalUpTo2 { get; set; }
    public int TotalUpTo3 { get; set; }
    public int TotalFrom3 { get; set; }
}
public class HrmEmployeeBirthDay
{
    public string Employee { get; set; }
    public DateOnly EmployeeBithDate { get; set; }
    public int DaysUntilBirthday { get; set; }
    public Guid? PersonPictureId { get; set; }
}
public class PersonPhotoFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
}
public class StaffingSinglePageReportDto
{
    public int DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public decimal Quantity { get; set; }
    public decimal QuantityForNow { get; set; }
    //public string Employees { get; set; }
    //public decimal? EmployeeRate { get; set; }
    //public DateOnly? DocOn { get; set; }
    //public DateTime? CreatedAt { get; set; }
    //public int? StatusId { get; set; }
    public List<EmployeeManageTable> EmployeeManageTables { get; set; }
}

public class EmployeeManageTable
{
    public string Pinfl { get; set; }
    public string Employees { get; set; }
    public int? EmployeeId { get; set; }
    public long? EmployeeManageId { get; set; }
    public long? EmployeeManageDocId { get; set; }
    public decimal? EmployeeRate { get; set; }
    public List<AppointEmployeeTables> AppointEmployees { get; set; }
}
public class AppointEmployeeTables
{
    public string DocNumber { get; set; }
    public DateOnly? DocOn { get; set; }
    public DateTime? CreatedAt { get; set; }
    public int? StatusId { get; set; }
}


