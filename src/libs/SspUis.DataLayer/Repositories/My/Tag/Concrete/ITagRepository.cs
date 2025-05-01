using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface ITagRepository : IBaseEntityRepository<int,Tag,CreateTagDlDto,UpdateTagDlDto>
    {
        IQueryable<Tag> ByNames(List<string> tags);
        IQueryable<TDto> ByNames<TDto>(List<string> tags) where TDto : class;
    }
}
