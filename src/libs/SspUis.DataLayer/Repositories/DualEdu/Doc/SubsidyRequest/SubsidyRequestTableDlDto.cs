using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GenericServices;
using SspUis.Core;
using SspUis.DataLayer.EfClasses.DualEdu;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class SubsidyRequestTableDlDto : EntityDto<SubsidyRequestTableDlDto, SubsidyRequestTable>,
        IHaveIdProp<long>,
        ILinkToEntity<SubsidyRequestTable>
    {
        public long Id { get; set; }
        [LocalizedRequired]
        public string Seria { get; set; }
        [LocalizedRequired]
        public string Number { get; set; }
        [LocalizedRequired]
        public DateOnly DateOfBirth { get; set; }
        //[LocalizedRange(1, int.MaxValue)]
        public int PersonId { get; set; }
        [LocalizedRequired]
        public decimal Salary { get; set; }
        [LocalizedRequired]
        public decimal Subsidy { get; set; }
        public List<SubsidyRequestFileDlDto> Files { get; set; } = new();
        protected override Action<IMappingExpression<SubsidyRequestTableDlDto, SubsidyRequestTable>> AlterMapping =>
            cfg => cfg
            .ForMember(d => d.Files, c => c.Ignore());


        public override SubsidyRequestTable CreateEntity()
        {
            var entity = base.CreateEntity();

            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES,
                Files.Select(a => a.Id).ToList());

            return entity;
        }

        public override void UpdateEntity(SubsidyRequestTable entity)
        {
            base.UpdateEntity(entity);

            entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_DUAL_SUBSIDY_REQUEST_FILES,
                entity.Id.ToString(),
                Files.Select(a => a.Id).ToList());
        }

    }
}
