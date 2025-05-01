using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE.Models;

namespace SspUis.DataLayer.EfClasses.Hrm;

[Table("hl_employee", Schema = "hrm")]
[Index(nameof(OrganizationId), nameof(StateId), Name = "hl_employee_index_organization_id_state_id")]
public partial class Employee : IHaveIdProp<int>, IHaveStateId
{
    public Employee()
    {
        Relatives = new HashSet<EmployeeRelative>();
        HigherEdu = new HashSet<EmployeeHigherEdu>();
        PlaceOfWorks = new HashSet<EmployeePlaceOfWork>();
        AcademicDegrees = new HashSet<EmployeeAcademicDegree>();
        DegreeTitles = new HashSet<EmployeeDegreeTitle>();
        ElectionMembers = new HashSet<EmployeeElectionMember>();
        LanguageProficiencys = new HashSet<EmployeeLanguageProficiency>();
        Partisanships = new HashSet<EmployeePartisanship>();
        ScientificDegrees = new HashSet<EmployeeScientificDegree>();
        StateAwards = new HashSet<EmployeeStateAward>();
        MilitaryRanks = new HashSet<EmployeeMilitaryRank>();
    }
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("phone_number")]
    [StringLength(50)]
    public string PhoneNumber { get; set; }

    [Column("organization_id")]
    public int OrganizationId { get; set; }
    [Column("person_id")]
    public int PersonId { get; set; }
    [Column("state_id")]
    public int StateId { get; set; }
    [Column("personal_number")]
    public int PersonalNumber { get; set; }
    [Column("mehnat_worked_year")]
    public int? MehnatWorkedYear { get; set; }
    [Column("mehnat_worked_month")]
    public int? MehnatWorkedMonth { get; set; }
    [Column("mehnat_worked_day")]
    public int? MehnatWorkedDay { get; set; }
    [Column("ssp_worked_year")]
    public int? SspWorkedYear { get; set; }
    [Column("ssp_worked_month")]
    public int? SspWorkedMonth { get; set; }
    [Column("ssp_worked_day")]
    public int? SspWorkedDay { get; set; }
    [Column("has_military")]
    public bool HasMilitary { get; set; }
    [Column("has_legal_education")]
    public bool HasLegalEducation { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; }
    [Column("created_user_id")]
    public int? CreatedUserId { get; set; }
    [Column("modified_at", TypeName = "timestamp without time zone")]
    public DateTime? ModifiedAt { get; set; }
    [Column("modified_user_id")]
    public int? ModifiedUserId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization Organization { get; set; }
    [InverseProperty(nameof(EfClasses.Hrm.EmployeeManage.Employee))]
    public virtual EmployeeManage EmployeeManage { get; set; }

    [ForeignKey(nameof(PersonId))]
    public virtual Person Person { get; set; }
    [ForeignKey(nameof(StateId))]
    public virtual State State { get; set; }

    [ForeignKey(nameof(CreatedUserId))]
    public virtual User CreatedUser { get; set; }
    [InverseProperty(nameof(EmployeePlaceOfWork.Owner))]
    public virtual ICollection<EmployeePlaceOfWork> PlaceOfWorks { get; set; }
    [InverseProperty(nameof(EmployeeRelative.Owner))]
    public virtual ICollection<EmployeeRelative> Relatives { get; set; }
    [InverseProperty(nameof(EmployeeHigherEdu.Owner))]
    public virtual ICollection<EmployeeHigherEdu> HigherEdu { get; set; }
    [InverseProperty(nameof(EmployeeAcademicDegree.Owner))]
    public virtual ICollection<EmployeeAcademicDegree> AcademicDegrees { get; set; }
    [InverseProperty(nameof(EmployeeDegreeTitle.Owner))]
    public virtual ICollection<EmployeeDegreeTitle> DegreeTitles { get; set; }
    [InverseProperty(nameof(EmployeeElectionMember.Owner))]
    public virtual ICollection<EmployeeElectionMember> ElectionMembers { get; set; }
    [InverseProperty(nameof(EmployeeLanguageProficiency.Owner))]
    public virtual ICollection<EmployeeLanguageProficiency> LanguageProficiencys { get; set; }
    [InverseProperty(nameof(EmployeePartisanship.Owner))]
    public virtual ICollection<EmployeePartisanship> Partisanships { get; set; }
    [InverseProperty(nameof(EmployeeScientificDegree.Owner))]
    public virtual ICollection<EmployeeScientificDegree> ScientificDegrees { get; set; }
    [InverseProperty(nameof(EmployeeStateAward.Owner))]
    public virtual ICollection<EmployeeStateAward> StateAwards { get; set; }
    [InverseProperty(nameof(EmployeeMilitaryRank.Owner))]
    public virtual ICollection<EmployeeMilitaryRank> MilitaryRanks { get; set; }

    //public static Expression<Func<Employee, string>> GetFullName()
    //{
    //    return a => a.Person.SurnameLatin + " " + a.Person.NameLatin + " " + a.Person.PatronymLatin;
    //}

    //public static Expression<Func<Employee, string>> ShortName()
    //{
    //    return a => a.Person.NameLatin[0] + ". " + (a.Person.PatronymLatin != null ? a.Person.PatronymLatin[0] : "") + ". " + a.Person.SurnameLatin;
    //}
}
