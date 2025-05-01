using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using SspUis.Core.Security;
using AutoMapper;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateTagDlDto : TagDlDto<UpdateTagDlDto>, IHaveIdProp<int>
    {
        public int Id { get; set; }
        public int StateId { get; set; }
    }
}
