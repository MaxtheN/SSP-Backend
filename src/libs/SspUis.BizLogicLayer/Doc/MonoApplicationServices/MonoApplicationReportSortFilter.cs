using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Doc.MonoApplicationServices
{
	public class MonoApplicationReportSortFilter
	{
		public int? RegionId { get; set; }
		public int? DistrictId { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
}
