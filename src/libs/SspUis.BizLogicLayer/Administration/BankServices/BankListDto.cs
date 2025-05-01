using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.BankServices
{
    public class BankListDto : ILinkToEntity<Bank>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string Code { get; set; }
        public string BankName { get; set; }
        public int? BankCodeId { get; set; }
        public string BankCode { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }

}
