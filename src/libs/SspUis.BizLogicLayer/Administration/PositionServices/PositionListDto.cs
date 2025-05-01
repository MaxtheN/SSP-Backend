using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.PositionServices
{
    public class PositionListDto : ILinkToEntity<Position>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int? PositionClassificationId { get; set; }
        public string PositionClassification { get; internal set; }
        public int? PositionCategoryId { get; set; }
        public string PositionCategory { get; internal set; }
        public int? TariffScaleTypeId { get; set; }
        public string TariffScaleType { get; internal set; }
        public int? StaffTypeBasicTariffId { get; set; }
        public string StaffTypeBasicTariff { get; internal set; }
        public int StateId { get; set; }
        public string OrderCode { get; set; }
        public string State { get; set; }
    }

}
