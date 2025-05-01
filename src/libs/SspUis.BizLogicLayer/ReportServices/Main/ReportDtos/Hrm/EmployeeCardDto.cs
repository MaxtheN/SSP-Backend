using Newtonsoft.Json;
using SspUis.BizLogicLayer.Hrm;
using SspUis.BizLogicLayer.PersonServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.ReportServices
{
    public class EmployeeCardDto
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string State { get; internal set; }
        public DateOnly BirthDate { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Citizenship { get; set; }
        public string Pinfl { get; set; }
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public string BirthCountry { get; set; }
        public string BirthRegion { get; set; }
        //public string BirthDistrict { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string LivingRegion { get; set; }
        public string LivingDistrict { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public Guid? PictureId { get; set; }
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
}
