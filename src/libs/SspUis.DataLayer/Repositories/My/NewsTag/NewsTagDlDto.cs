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
    public class NewsTagDlDto<TDto> : EntityDto<TDto, NewsTag>
        where TDto : NewsTagDlDto<TDto>
    {
        public int NewsId { get; set; }
        public int TagId { get; set; }

    }
}
