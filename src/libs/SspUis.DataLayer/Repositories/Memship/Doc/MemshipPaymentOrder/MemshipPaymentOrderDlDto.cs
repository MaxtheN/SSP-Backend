using AutoMapper;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories.Memship.Doc.MemshipPaymentOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
	public class MemshipPaymentOrderDlDto<TDto> : EntityDto<TDto, MemshipPaymentOrder>
		where TDto : MemshipPaymentOrderDlDto<TDto>
	{
		[LocalizedRequired]
		public DateOnly DocOn { get; set; }

		[LocalizedStringLength(50)]
		public string DocNumber { get; set; }
		[LocalizedStringLength(1024)]
		public string Details { get; set; }
		[LocalizedRange(1, long.MaxValue)]
		public long ContractorId { get; set; }
		[LocalizedRange(1, long.MaxValue)]
		public long? MemshipContractId { get; set; }
		[LocalizedRange(1, int.MaxValue)]
		public int BankId { get; set; }
		[LocalizedRequired]
		public decimal Amount { get; set; }
		//[LocalizedRange(1, int.MaxValue)]
		//public int CurrencyId { get; set; }
		[LocalizedRange(1, int.MaxValue)]
		public int ApplicationTypeId { get; set; }
		public long? ServiceContractId { get; set; }
		public List<MemshipPaymentOrderFileDlDto> Files { get; set; } = new();
		public List<MemshipPaymentOrderTableDlDto> Tables { get; set; } = new();
		protected override Action<IMappingExpression<TDto, MemshipPaymentOrder>> AlterMapping =>
		   cfg => cfg
			   .ForMember(x => x.Files, opt => opt.Ignore())
			   .ForMember(x => x.Tables, opt => opt.Ignore())
			   ;
		public override MemshipPaymentOrder CreateEntity()
		{
			var entity = base.CreateEntity();
			entity.StatusId = StatusIdConst.CREATED;
			if (Files == null)
				Files = new();

			entity.Files.AddFromTempFiles(DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES, Files.Select(a => a.Id).ToList());
			Tables.AddTo(entity.Tables);
			return entity;
		}

		public override void UpdateEntity(MemshipPaymentOrder entity)
		{
			base.UpdateEntity(entity);
			entity.StatusId = StatusIdConst.MODIFIED;
			Tables.ApplyChangesTo<long,MemshipPaymentOrderTableDlDto,MemshipPaymentOrderTable>(entity.Tables);
			entity.Files.UpdateFromFiles(DocumentStorageConst.DOC_MEMSHIP_PAYMENT_ORDER_FILES, entity.Id.ToString(), Files.Select(a => a.Id).ToList());
		}
	}
}
