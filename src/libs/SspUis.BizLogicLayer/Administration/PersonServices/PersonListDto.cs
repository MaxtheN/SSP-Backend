using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PersonServices
{
    public class PersonListDto : ILinkToEntity<Person>, IHaveIdProp<int>, IPerson
    {
        public int Id { get; set; }
        public string Pinfl { get; set; }
        public string Inn { get; set; }
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public DateTime? PassportDate { get; set; }
        public DateTime? PassportExpiration { get; set; }
        public string SurnameLatin { get; set; }
        public string NameLatin { get; set; }
        public string PatronymLatin { get; set; }
        public string SurnameEng { get; set; }
        public string NameEng { get; set; }
        public DateOnly BirthDate { get; set; }
        public int? GenderId { get; set; }
        public string PassportDivName { get; set; }
        public int? BirthCountryId { get; set; }
        public int? BirthRegionId { get; set; }
        public int? BirthDistrictId { get; set; }
        public int? NationalityId { get; set; }
        public int? CitizenshipId { get; set; }
        public int? LivingRegionId { get; set; }
        public int? LivingDistrictId { get; set; }

        public string State { get; internal set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string Citizenship { get; set; }
        public string BirthCountry { get; set; }
        public string BirthRegion { get; set; }
        public string BirthDistrict { get; set; }
        public string LivingRegion { get; set; }
        public string LivingDistrict { get; set; }

    }
}
