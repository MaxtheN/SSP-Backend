using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.PersonServices;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer;

public class EmployeeDto : UpdateEmployeeDlDto, ILinkToEntity<Employee>
{
    public int OrganizationId { get; internal set; }
    public string Organization { get; set; }
    public string State { get; set; }
    public PersonDto Person { get; set; } = new();
    public bool IsPassedAttestation { get; set; }
    public string CertificateNumber { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public int OrderTypeId { get; set; }
    public string OrderType { get; set; }
    public bool HasMilitary { get; set; }
    public bool HasLegalEducation { get; set; }
    public UpdateEployeeUserDto User { get; set; } = new();
    public string CreatedUser { get; set; }
    public string CreatedUserName { get; set; }
    new public List<EmployeePlaceOfWorkDto> PlaceOfWorks { get; set; } = new();
    new public List<EmployeeRelativeDto> Relatives { get; set; } = new();
    new public List<EmployeeHigherEduDto> HigherEdu { get; set; } = new();
    new public List<EmployeeAcademicDegreeDto> AcademicDegrees { get; set; } = new();
    new public List<EmployeeDegreeTitleDto> DegreeTitles { get; set; } = new();
    new public List<EmployeeElectionMemberDto> ElectionMembers { get; set; } = new();
    new public List<EmployeeLanguageProficiencyDto> LanguageProficiencys { get; set; } = new();
    new public List<EmployeePartisanshipDto> Partisanships { get; set; } = new();
    new public List<EmployeeScientificDegreeDto> ScientificDegrees { get; set; } = new();
    new public List<EmployeeStateAwardDto> StateAwards { get; set; } = new();
    new public List<EmployeeMilitaryRankDto> MilitaryRanks { get; set; } = new();
}
