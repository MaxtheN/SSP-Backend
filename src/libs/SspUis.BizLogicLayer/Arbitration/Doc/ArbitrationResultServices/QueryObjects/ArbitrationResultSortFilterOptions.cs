using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class ArbitrationResultSortFilterOptions : DocumentSortFilterOptions
{
	public string Contractor { get; set; }
	public long ResponsibleContractorId { get; set; }
	public string ResponsibleContractor { get; set; }
}
