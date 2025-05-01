using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using GenericServices.Configuration;
using AutoMapper;
using SspUis.Core;

namespace SspUis.BizLogicLayer.NewsServices
{
    public class NewsDto : UpdateNewsDlDto, ILinkToEntity<News>
    {
        public int Id { get; set; }
        public int ViewCount { get; set; }
        public string State { get; set; }
        new public List<NewsTranslateDto> Translates { get; set; } = new();
        public new List<TagDto> Tags { get; internal set; } = new();
        public new NewsImageDto Image { get; set; }

    }

    public class NewsImageDto : NewsImageDlDto
    {
        public string FileName { get; internal set; }
        public int? CreatedUserId { get; internal set; }
        public string CreatedUser { get; internal set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatedAt { get; set; }
    }

    public class NewsImageDtoConfig : PerDtoConfig<NewsImageDto, NewsImage>
    {
        public override Action<IMappingExpression<NewsImage, NewsImageDto>> AlterReadMapping =>
           cfg => cfg.ReverseMap();
    }
}
