using GenericServices;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer
{
	public class SrvDeedDto : UpdateServiceDeedDlDto, ILinkToEntity<ServiceDeed>, IDocument
	{
		[IgnoreWordProperty]
		public Guid Id2 { get; set; }
		public int StatusId { get; set; }
		public string Status { get; set; }
		public string ContractorInn { get; set; }
		public string ContractorFullName { get; set; }
		public string ContractorAddress { get; set; }
		public string ContractorBankAccount { get; set; }
		public string ContractorBankName { get; set; }
		public string ContractorBankMFO { get; set; }
		public string ContractorDirector { get; set; }
		public string ContractorPhoneNumber { get; set; }
		public string ContractorRegion { get; set; }
		public string ContractorDistrict { get; set; }
		public string SrvContractDocOn { get; set; }
		public string SrvContractDocNumber { get; set; }
		
		//public int TableId { get => TableIdConst.DOC_SERVICE_DEED; }
		public string Organization { get; set; }
		public string Region { get; set; }
		public string OrganizationAddress { get; set; }
		public string OrganizationInn { get; set; }
		public string OrganizationDirector { get; set; }
		public string OrganizationBankAccount { get; set; }
		public string OrganizationBankName { get; set; }
		public string OrganizationPhoneNumber { get; set; }
		public string OrganizationBankMFO { get; set; }
        public string? Message { get; set; }

        public decimal? ServicePrice
		{
			get
			{
				return SrvGroups.Select(group => group.Price).Sum();
			}
		}
		public decimal? RealCoef { get { return SrvGroups.Select(group => group.RealCoef).Sum(); } }
		//public int Bhm { get =>  }
		public List<SrvDeedDtoForGroup> SrvGroups
		{
			get => Groups
				.SelectMany(group => group.Tables)
				.Select(table => new SrvDeedDtoForGroup()
				{
					Name = table.NeedChamberService,
					Price = table.Price,
					RealCoef = table.RealCoef,
				})
				.ToList();
		}
		[IgnoreWordProperty]
		public new List<SrvDeedGroupDto> Groups { get; set; } = new();
		[IgnoreWordProperty]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		#region Action
		public bool CanSign { get; set; }
		public bool CanReject { get; set; }
		public bool CanCancel { get; set; }
		public bool CanCreatePaymentOrder { get; set; }
		#endregion
	}

	public class SrvDeedDtoForGroup
	{
		public string Name { get; set; }
		public decimal? Price { get; set; }
		public decimal? RealCoef { get; set; }

	}
}