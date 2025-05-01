using SspUis.BizLogicLayer.Administration.CountryServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.CountryServices
{
    public interface ICountryService : IBaseEntityService<Country, CountryListDto, CountryDto, CreateCountryDlDto, UpdateCountryDlDto>
    {
        SelectList<int> AsSelectList();
        Task<SelectList<int>> AsSelectListForBojxona(AsSelectListForBojxonaDto dto);
    }
}
