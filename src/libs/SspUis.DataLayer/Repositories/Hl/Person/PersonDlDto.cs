using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.DataLayer.Repositories
{
    public class PersonDlDto<TDto> : EntityDto<TDto, Person>
        where TDto : PersonDlDto<TDto>
    {
        [LocalizedStringLength(14)]
        public string Pinfl { get; set; }
        [LocalizedStringLength(9)]
        public string Inn { get; set; }
        [LocalizedStringLength(50)]
        public string PassportSeria { get; set; }
        [LocalizedStringLength(50)]
        public string PassportNumber { get; set; }
        public DateTime? PassportDate { get; set; } = DateTime.Now;
        public DateTime? PassportExpiration { get; set; } = DateTime.Now;
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string SurnameLatin { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(100)]
        public string NameLatin { get; set; }
        [LocalizedStringLength(100)]
        public string PatronymLatin { get; set; }
        [LocalizedStringLength(100)]
        public string SurnameEng { get; set; }
        [LocalizedStringLength(100)]
        public string NameEng { get; set; }
        public DateOnly BirthDate { get; set; }
        public int? GenderId { get; set; }
        [LocalizedStringLength(500)]
        public string PassportDivName { get; set; }
        public int? BirthCountryId { get; set; }
        public int? BirthRegionId { get; set; }
        public int? BirthDistrictId { get; set; }
        public int? NationalityId { get; set; }
        public int? CitizenshipId { get; set; }
        public int? LivingRegionId { get; set; }
        public int? LivingDistrictId { get; set; }
        public Guid? PictureId { get; set; }

        public override Person CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.StateId = StateIdConst.ACTIVE;
            entity.FullName = StringUtility.GetFullFIO(SurnameLatin, NameLatin, PatronymLatin);
            entity.ShortName = StringUtility.GetFIO(entity.FullName);

            if (PictureId != null)
            {
                var pictureIds = new List<Guid>();
                pictureIds.AddFromTempFile(DocumentStorageConst.HL_PERSON_FILES, new List<Guid> { (Guid)PictureId });
                entity.PictureId = pictureIds[0];
            }
            return entity;
        }

        public override void UpdateEntity(Person entity)
        {
            if (PictureId != null)
            {
                var pictureIds = new List<Guid>();
                if (entity.PictureId != null)
                    pictureIds.Add((Guid)entity.PictureId);
                pictureIds.UpdateFromFile(DocumentStorageConst.HL_PERSON_FILES, entity.Id.ToString(), new List<Guid> { (Guid)PictureId }); ;
                entity.PictureId = pictureIds[0];
            }
            base.UpdateEntity(entity);
        }
    }
}
