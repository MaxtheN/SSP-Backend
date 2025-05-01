using AutoMapper;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class TagDlDto<TDto> : EntityDto<TDto, Tag>
        where TDto : TagDlDto<TDto>
    {
        public string Name { get; set; }
    }
}
