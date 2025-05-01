using System.Linq;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.TariffScaleCoefServices;

public static class TariffScaleCoefTableSelectList
{
    public static SelectList<int> AsSelectList(this IQueryable<TariffScaleCoefTable> query, int? tariffScaleId = null, int? tariffScaleTableId = null)
    {
        if (tariffScaleId.HasValue)
            query = query.Where(a => a.Owner.TariffScaleId == tariffScaleId);

        if (tariffScaleTableId.HasValue)
            query = query.Where(a => a.TariffScaleTableId == tariffScaleTableId);

        return new SelectList<int>(query
            .Select(a => new TariffScaleCoefTableSelectListItemDto
            {
                Value = a.Id,
                Text = a.Coef.ToString(),
                RankCode = a.RankCode,
                OrderCode = a.OrderCode.Value
            })
            .OrderBy(a => a.OrderCode));
    }
    private class TariffScaleCoefTableSelectListItemDto : SelectListItem<int>
    {
        public string RankCode { get; set; }
        new public int OrderCode { get; set; }
    }
}