using AutoMapper;
using GenericServices.Configuration;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Utility;

namespace SspUis.BizLogicLayer.Memship.Doc.MemshipPaymentOrderServices;

public class MemshipPaymentOrderTableDtoConfig : 
	PerDtoConfig<MemshipPaymentOrderTableDto, MemshipPaymentOrderTable>
{
	public override Action<IMappingExpression<MemshipPaymentOrderTable, MemshipPaymentOrderTableDto>> AlterReadMapping => cfg => cfg
			.ForMember(x => x.ApplicationType, x => x.MapFrom(x => x.ApplicationType.FullName))
			.ForMember(x => x.NeedChamberService, x => x.MapFrom(x => x.NeedChamberService.FullName))
			;
}
