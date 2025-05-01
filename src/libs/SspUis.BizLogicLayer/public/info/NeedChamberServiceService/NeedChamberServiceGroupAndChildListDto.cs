using SspUis.BizLogicLayer.Hrm.NeedChamberServiceServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer;

public class NeedChamberServiceGroupAndChildListDto
{
	public int Id { get; set; }
	public string Code { get; set; }
	public string OrderCode { get; set; }
	public string ShortName { get; set; }
	public string FullName { get; set; }
	public int StateId { get; set; }
	public string State { get; set; }
	public List<NeedChamberServiceListDto> NeedChambers { get; set; }
}

