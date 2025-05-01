using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
	public class UpdateExternalDocumentFromEdocDlDto : 
		ExternalDocumentFromEdocDlDto<UpdateExternalDocumentFromEdocDlDto>,
		IHaveIdProp<int>
	{
		public int Id { get; set; }
        //public long? AppealApplicationId { get; set; }
        //public long? CallCenterAppealId { get; set; }
    }
}
