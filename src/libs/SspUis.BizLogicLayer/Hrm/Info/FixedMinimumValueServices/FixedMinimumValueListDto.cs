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

namespace SspUis.BizLogicLayer.Hrm.FixedMinimumValueServices
{
    public class FixedMinimumValueListDto : ILinkToEntity<FixedMinimumValue>, IHaveIdProp<long>
    {
        public long Id { get; set; }
        public decimal FixedValue { get; set; }
        public decimal ChangePercentage { get; set; }
        public string NormativeDoc { get; set; } = null!;
        public DateOnly DateOn { get; set; }
        public int MinimumValueTypeId { get; set; }
        public string MinimumValueType { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
