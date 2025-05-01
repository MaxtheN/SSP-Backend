using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Presentation;
using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer
{
    public class SrvContractDto : UpdateServiceContractDlDto, ILinkToEntity<ServiceContract>, IDocument
    {
        [IgnoreWordProperty]
        public Guid Id2 { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string ContractorInn { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorRegion { get; set; }
        public string ContractorDistrict { get; set; }
        public int TableId { get => TableIdConst.DOC_SERVICE_CONTRACT; }
        public string Organization { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public new List<SrvContractGroupDto> Groups { get; set; } = new();

        #region Actions
        public bool CanSign { get; set; }
        public bool CanReject { get; set; }
        public bool CanCancel { get; set; }
        public bool CanCreatePaymentOrder { get; set; }
        public bool CanCreateDeedDoc { get; set; } = false;
        #endregion
    }
}