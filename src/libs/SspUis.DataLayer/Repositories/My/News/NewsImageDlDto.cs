using SspUis.DataLayer.EfClasses;
using GenericServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class NewsImageDlDto : EntityDto<NewsImageDlDto, NewsImage>, ILinkToEntity<NewsImage>
    {
        [LocalizedRequired]
        public Guid Id { get; set; }
    }
}
