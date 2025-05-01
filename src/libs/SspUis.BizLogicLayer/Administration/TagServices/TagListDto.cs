using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.TagServices
{
    public class TagListDto : ILinkToEntity<Tag>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int StateId { get; set; }
        public string State { get; set; }
    }
}
