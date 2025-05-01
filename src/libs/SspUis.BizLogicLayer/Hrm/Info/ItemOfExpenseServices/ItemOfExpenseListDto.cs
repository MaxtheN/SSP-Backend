using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public class ItemOfExpenseListDto : ILinkToEntity<ItemOfExpense>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public int NumberOfGroup { get; set; }
        public string OrderCode { get; set; }
        public string ShortName { get; set; }
        public string CodeAndText { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string Code3 { get; set; }

        public bool IsGroup { get; set; }
        public bool AgeingAllowed { get; set; }

        public int StateId { get; set; }
        public string State { get; set; }
    }
}
