using GenericServices;
using System.Collections.Generic;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Interfaces;
using SspUis.BizLogicLayer.Administration;
using System;

namespace SspUis.BizLogicLayer.PersonServices
{
    public class PersonDto : CreatePersonDlDto, ILinkToEntity<Person>, IPerson
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string State { get; internal set; }
        public string? Gender { get; set; }
        public string? Nationality { get; set; }
        public string? Citizenship { get; set; }
        public string? BirthCountry { get; set; }
        //public string? BirthRegion { get; set; }
        public string? BirthDistrict { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string? LivingRegion { get; set; }
        public string? LivingDistrict { get; set; }
        public  string PassportSeria { get; set; }
        public  string PassportNumber { get; set; }
        new public Guid? PictureId { get; set; }
        //new public List<PersonFileDlDto> Files { get; set; } = new();
    }
}
