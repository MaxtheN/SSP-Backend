using SspUis.DataLayer.EfClasses;
using SspUis.BizLogicLayer.ModuleServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class ModuleSelectListQueryObject
    {
        public static IEnumerable<ModuleGroupSelectListDto> AsSelectList(this IQueryable<Module> query)
        {
            return query
                    .Include(a => a.Translates)
                    .Include(a => a.SubGroup)
                    .Include(a => a.SubGroup.Group)
                    .IsActive()
                    .AsEnumerable()
                    .GroupBy(a => a.SubGroup.Group, (groupKey, groupResults) => new ModuleGroupSelectListDto
                    {
                        Id = groupKey.Id,
                        ShortName = groupKey.ShortName,
                        FullName = groupKey.FullName,
                        SubGroups = groupResults.GroupBy(b => b.SubGroup, (subGroupKey, subGroupResults) => new ModuleSubGroupSelectListDto
                        {
                            Id = subGroupKey.Id,
                            Code = subGroupKey.Code,
                            ShortName = subGroupKey.ShortName,
                            FullName = subGroupKey.FullName,
                            Modules = subGroupResults.Select(c => new ModuleSelectListDto
                            {
                                Id = c.Id,
                                Code = c.Code,
                                ShortName = c.ShortName,
                                FullName = c.FullName,
                            })
                        })
                    });
        }
    }
}
