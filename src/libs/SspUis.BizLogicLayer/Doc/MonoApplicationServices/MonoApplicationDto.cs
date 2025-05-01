using System;
using System.Collections.Generic;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer.MonoApplicationServices
{
    [PrintableModel("Mono arizasi.", TableIdConst.DOC_MONO_APPLICATION)]
    public class MonoApplicationDto : UpdateMonoApplicationDlDto, ILinkToEntity<MonoApplication>,
        IBaseApplication<SspUis.BizLogicLayer.ApplicationDto>
    {
        public new long Id { get => base.Id; set => base.Id = value; }
        public new SspUis.BizLogicLayer.ApplicationDto Application { get; set; }
        public string Mfy { get; set; }
        public string MonoMfy { get; set; }
        public string MonoRegion { get; set; }
        public string Currency { get; set; }
        public string MonoDistrict { get; set; }
        public int TableId { get; set; }
        public int BandlikResponseStatusId { get; set; }
        public List<MonoApplicationFileDlDto> Files { get; set; } = new();
        public List<MonoApplicationStudentTableDlDto> StudentTables { get; set; } = new();
        public List<MonoApplicationItemTableDlDto> ItemTables { get; set; } = new();

        public bool CanEdit { get; set; }
        public bool CanSend { get; set; }
        public bool CanRevoke { get; set; }
        public bool CanReject { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
    }
}
