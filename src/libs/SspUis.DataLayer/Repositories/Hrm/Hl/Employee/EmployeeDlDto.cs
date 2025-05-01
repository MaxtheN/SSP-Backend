using AutoMapper;
using SspUis.DataLayer.EfClasses.Hrm;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.DataLayer.Repositories
{
    public class EmployeeDlDto<TDto> : EntityDto<TDto, Employee>
        where TDto : EmployeeDlDto<TDto>
    {
        [LocalizedStringLength(50)]
        [LocalizedRequired]
        public string PhoneNumber { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1,int.MaxValue)]
        public int? OrganizationId { get; set; }
        public int PersonId { get; set; }
        [LocalizedRequired]
        public bool HasMilitary { get; set; } = false;
        [LocalizedRequired]
        public bool HasLegalEducation { get; set; } = false;
        public int? TotalWorkedYear { get; set; }
        public int? TotalWorkedMonth { get; set; }
        public int? TotalWorkedDay { get; set; }
        public List<EmployeePlaceOfWorkDlDto> PlaceOfWorks { get; set; } = new List<EmployeePlaceOfWorkDlDto>();
        public List<EmployeeRelativeDlDto> Relatives { get; set; } = new List<EmployeeRelativeDlDto>();
        public List<EmployeeHigherEduDlDto> HigherEdu { get; set; } = new List<EmployeeHigherEduDlDto>();
        public List<EmployeeAcademicDegreeDlDto> AcademicDegrees { get; set; } = new List<EmployeeAcademicDegreeDlDto>();
        public List<EmployeeDegreeTitleDlDto> DegreeTitles { get; set; } = new List<EmployeeDegreeTitleDlDto>();
        public List<EmployeeElectionMemberDlDto> ElectionMembers { get; set; } = new List<EmployeeElectionMemberDlDto>();
        public List<EmployeeLanguageProficiencyDlDto> LanguageProficiencys { get; set; } = new List<EmployeeLanguageProficiencyDlDto>();
        public List<EmployeePartisanshipDlDto> Partisanships { get; set; } = new List<EmployeePartisanshipDlDto>();
        public List<EmployeeScientificDegreeDlDto> ScientificDegrees { get; set; } = new List<EmployeeScientificDegreeDlDto>();
        public List<EmployeeStateAwardDlDto> StateAwards { get; set; } = new List<EmployeeStateAwardDlDto>();
        public List<EmployeeMilitaryRankDlDto> MilitaryRanks { get; set; } = new List<EmployeeMilitaryRankDlDto>();

        protected override Action<IMappingExpression<TDto, Employee>> AlterMapping => cfg => cfg
        .ForMember(x => x.PlaceOfWorks, x => x.Ignore())
        .ForMember(x => x.HigherEdu, x => x.Ignore())
        .ForMember(x => x.Relatives, x => x.Ignore())
        .ForMember(x => x.AcademicDegrees, x => x.Ignore())
        .ForMember(x => x.DegreeTitles, x => x.Ignore())
        .ForMember(x => x.ElectionMembers, x => x.Ignore())
        .ForMember(x => x.LanguageProficiencys, x => x.Ignore())
        .ForMember(x => x.Partisanships, x => x.Ignore())
        .ForMember(x => x.ScientificDegrees, x => x.Ignore())
        .ForMember(x => x.StateAwards, x => x.Ignore())
        .ForMember(x => x.MilitaryRanks, x => x.Ignore())
        ;

        public override Employee CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            PlaceOfWorks.ForEach(a => a.IsImported = false);
            PlaceOfWorks.AddTo(entity.PlaceOfWorks);
            Relatives.ForEach(a =>
            {
                a.FullName = StringUtility.GetFullFIO(a.LastName, a.FirstName, a.FamilyName);
                a.ShortName = StringUtility.GetFIO(a.FamilyName);
            });
            Relatives.AddTo(entity.Relatives);
            HigherEdu.AddTo(entity.HigherEdu);
            AcademicDegrees.AddTo(entity.AcademicDegrees);
            DegreeTitles.AddTo(entity.DegreeTitles);
            ElectionMembers.AddTo(entity.ElectionMembers);
            LanguageProficiencys.AddTo(entity.LanguageProficiencys);
            Partisanships.AddTo(entity.Partisanships);
            ScientificDegrees.AddTo(entity.ScientificDegrees);
            StateAwards.AddTo(entity.StateAwards);
            MilitaryRanks.AddTo(entity.MilitaryRanks);
            return entity;
        }

        public override void UpdateEntity(Employee entity)
        {
            PersonId = entity.PersonId;
            base.UpdateEntity(entity);

            PlaceOfWorks.ForEach(a => a.IsImported = false);
            PlaceOfWorks.ApplyChangesTo<int, EmployeePlaceOfWorkDlDto, EmployeePlaceOfWork>(entity.PlaceOfWorks);
            HigherEdu.ApplyChangesTo<int, EmployeeHigherEduDlDto, EmployeeHigherEdu>(entity.HigherEdu);
            Relatives.ForEach(a =>
            {
                a.FullName = StringUtility.GetFullFIO(a.LastName, a.FirstName, a.FamilyName);
                a.ShortName = StringUtility.GetFIO(a.FamilyName);
            });
            Relatives.ApplyChangesTo<int, EmployeeRelativeDlDto, EmployeeRelative>(entity.Relatives);
            AcademicDegrees.ApplyChangesTo<long, EmployeeAcademicDegreeDlDto, EmployeeAcademicDegree>(entity.AcademicDegrees);
            DegreeTitles.ApplyChangesTo<long, EmployeeDegreeTitleDlDto, EmployeeDegreeTitle>(entity.DegreeTitles);
            ElectionMembers.ApplyChangesTo<long, EmployeeElectionMemberDlDto, EmployeeElectionMember>(entity.ElectionMembers);
            LanguageProficiencys.ApplyChangesTo<long, EmployeeLanguageProficiencyDlDto, EmployeeLanguageProficiency>(entity.LanguageProficiencys);
            Partisanships.ApplyChangesTo<long, EmployeePartisanshipDlDto, EmployeePartisanship>(entity.Partisanships);
            ScientificDegrees.ApplyChangesTo<long, EmployeeScientificDegreeDlDto, EmployeeScientificDegree>(entity.ScientificDegrees);
            StateAwards.ApplyChangesTo<long, EmployeeStateAwardDlDto, EmployeeStateAward>(entity.StateAwards);
            MilitaryRanks.ApplyChangesTo<long, EmployeeMilitaryRankDlDto, EmployeeMilitaryRank>(entity.MilitaryRanks);
        }
    }
}
