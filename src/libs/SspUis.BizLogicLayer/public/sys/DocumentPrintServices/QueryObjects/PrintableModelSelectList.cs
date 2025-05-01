using System.Collections.Generic;
using System.Linq;
using WEBASE.Models;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer;

public static class PrintableModelSelectList
{
    public static SelectList<int> AsSelectList(this List<PrintableModelAttribute> query)
    {
        return new SelectList<int>(
            query
                .Select(a => new SelectListItem<int>
                {
                    Value = a.TableId,
                    Text = a.Text
                })
                .OrderBy(a => a.Text)
            );
    }
}
