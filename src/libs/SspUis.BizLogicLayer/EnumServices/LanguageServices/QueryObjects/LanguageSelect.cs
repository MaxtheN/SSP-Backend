using SspUis.DataLayer.EfClasses;
using SspUis.BizLogicLayer.EnumServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public static class LanguageSelect
    {
        public static SelectList<int, LanguageSelectListDto> AsSelectList(this IQueryable<Language> query)
        {
            return new SelectList<int, LanguageSelectListDto>(
                query.Select(a => new LanguageSelectListDto
                {
                    Value = a.Id,
                    Text = a.FullName,
                    Code = a.Code,
                    ShortName = a.ShortName,
                    FullName = a.FullName
                }));
        }

    }
}
