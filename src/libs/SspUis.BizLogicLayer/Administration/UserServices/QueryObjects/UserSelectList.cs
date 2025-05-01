using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.UserServices;

namespace SspUis.BizLogicLayer
{
    public static class UserSelectList
    {
        public static PagedSelectList<int> AsSelectList(this PagedResult<UserListDto> pagedResult)
        {
            return new PagedSelectList<int>(
                pagedResult,
                pagedResult
                    .Rows
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        Text = a.FullName
                    }));
        }
    }
}
