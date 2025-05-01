using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;
using SspUis.DataLayer;
using SspUis.BizLogicLayer.OkedServices;
using Microsoft.IdentityModel.Tokens;

namespace SspUis.BizLogicLayer
{
    public static class OkedSelectList
    {
        public static SelectList<int> AsSelectList(this IQueryable<OkedListDto> query  )
        {
            return new SelectList<int>(
                query
                    .Select(a => new SelectListItem<int>
                    {
                        Value = a.Id,
                        OrderCode = a.Code + (a.Code == "00000" ? (" " + a.FullName) : ""),
                        Text = a.Code + " - " + a.FullName
                    })
                    .OrderBy(a => a.OrderCode)
                    .ThenBy(a => a.Text)
                );
        }
		public static SelectList<int> AsSelectList(this IQueryable<OkedListDto> query, string? search)
		{
			return new SelectList<int>(
				query
					.Select(a => new SelectListItem<int>
					{
						Value = a.Id,
						OrderCode = a.Code + (a.Code == "00000" ? (" " + a.FullName) : ""),
						Text = a.Code + " - " + a.FullName
					})
				.Where(x => search.IsNullOrEmpty() ?true: x.Text.ToLower().Contains(search) ||
						   x.OrderCode.ToLower().Contains(search) )
					.OrderBy(a => a.OrderCode)
					.ThenBy(a => a.Text)
				); 
		}
	}
}
