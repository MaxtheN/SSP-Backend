using GenericServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.AspNet;
using WEBASE.Models;
using SspUis.Core.Security;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer;
using SspUis.DataLayer.Repositories;
using SspUis.Integration.Bojxona.Services;
using Humanizer;
using SspUis.BizLogicLayer.ContractorServices;
using SspUis.BizLogicLayer.Administration.CountryServices;

namespace SspUis.BizLogicLayer.CountryServices
{
    public class CountryService
        : BaseEntityService<Country, CountryListDto, CountryDto, CreateCountryDlDto, UpdateCountryDlDto, ICountryRepository>,
        ICountryService
    {
        private readonly IAuthService _authService;
        private readonly IContractorService _contracatorService;
        private readonly IBojxonaService _bojxonaService;

        public CountryService(IUnitOfWork unitOfWork, IAuthService authService, IContractorService contracatorService, IBojxonaService bojxonaService)
            : base(unitOfWork)
        {
            _authService = authService;
            _contracatorService = contracatorService;
            _bojxonaService = bojxonaService;
        }

        protected override IQueryable<CountryListDto> SortFilter(IQueryable<CountryListDto> query, SortFilterPageOptions options)
        {
            return base.SortFilter(query, options).SortFilter(options);
        }

        public SelectList<int> AsSelectList()
        {
            return Repository.AllAsQueryable.AsSelectList();
        }
        public async Task<SelectList<int>> AsSelectListForBojxona(AsSelectListForBojxonaDto dto)
        {
            var result  =   await _bojxonaService.GetGTDByInn(new Integration.Bojxona.Models.GetGTDByInnRequestDto
            {
                Year = dto.Year.ToString(),
                Stir = dto.Inn
            });
            var countries = Repository.AllAsQueryable
                .Where(a => result.Select(b => b.EkimcountryCode).Contains(a.Code))
                .Select(a => a.Id);
            return Repository.AllAsQueryable
                .Where(a => countries.Contains(a.Id)).AsSelectList();
        }
    }
}
