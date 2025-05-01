using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Doc.BaseApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationDlDto<TDto> : BaseApplicationDlDto<TDto, MonoApplication>
        where TDto : MonoApplicationDlDto<TDto>
    {
        public long ApplicationId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long MfyId { get; set; }
        [LocalizedRequired]
        [LocalizedStringLength(600)]
        public string MonoAdress { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int CurrencyId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalCost { get; set; }
        [LocalizedRequired]
        public decimal SpendForBuild { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public long MonoMfyId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int MonoRegionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int MonoDistrictId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int BuildingCount { get; set; }
        [LocalizedRequired]
        public decimal LearningArea { get; set; }
        [LocalizedRequired]
        public decimal TotalArea { get; set; }
        public string ContractorInn { get; set; }
        public List<MonoApplicationStudentTableDlDto> StudentTables { get; set; } = new();
        public List<MonoApplicationItemTableDlDto> ItemTables { get; set; } = new();
        public List<MonoApplicationFileDlDto> Files { get; set; } = new();
        protected override Action<IMappingExpression<TDto, MonoApplication>> AlterMapping =>
           cfg =>
           {
               base.AlterMapping(cfg);
               cfg.ForMember(x => x.Files, x => x.Ignore());
               cfg.ForMember(x => x.StudentTables, x => x.Ignore());
               cfg.ForMember(x => x.ItemTables, x => x.Ignore());
           };

        public override MonoApplication CreateEntity()
        {
            MonoApplication entity = base.CreateEntity();
            entity.Application.StatusId = StatusIdConst.CREATED;
            entity.Application.ApplicationTypeId = ApplicationTypeIdConst.MONO;
            entity.Application.Id2 = Guid.NewGuid();
            entity.TableId = TableIdConst.DOC_MONO_APPLICATION;
            entity.StatusId = StatusIdConst.CREATED;
            entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, Files.Select(a => a.Id).ToList());
            foreach (var item in entity.Files)
            {
               
                var matchingFile = Files.FirstOrDefault(a => a.Id == item.Id);
                if (matchingFile != null)
                {
                    item.ColumnName = matchingFile.ColumnName;
                }

            }
            StudentTables.AddTo(entity.StudentTables);
            ItemTables.AddTo(entity.ItemTables);
            return entity;
        }
        public override void UpdateEntity(MonoApplication entity)
        {
            base.UpdateEntity(entity);
            entity.Application.StatusId = StatusIdConst.MODIFIED;
            entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_MONO_APPLICATION_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
        
            StudentTables.ApplyChangesTo<long, MonoApplicationStudentTableDlDto, MonoApplicationStudentTable>(entity.StudentTables);
            ItemTables.ApplyChangesTo<long, MonoApplicationItemTableDlDto, MonoApplicationItemTable>(entity.ItemTables);
        }
    }
}