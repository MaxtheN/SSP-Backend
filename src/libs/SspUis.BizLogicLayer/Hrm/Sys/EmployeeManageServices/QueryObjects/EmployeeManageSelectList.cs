using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using System.ComponentModel.DataAnnotations.Schema;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.EmployeeManageServices
{
    public static class EmployeeManageSelectList
    {
        public static SelectList<long> AsSelectList(this IQueryable<EmployeeManage> query)
        {
            return new SelectList<long>(
            query
                .Select(a => new SelectListItem<long>
                {
                    Value = a.Id,
                    Text = a.DocId + " - " + a.EndOn
                })
                .OrderBy(a => a.Text)
            );
        }
    }
}
