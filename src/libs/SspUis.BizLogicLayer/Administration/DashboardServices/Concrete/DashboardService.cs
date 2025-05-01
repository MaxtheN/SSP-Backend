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
using SspUis.Core;
using Microsoft.EntityFrameworkCore;
using SspUis.BizLogicLayer.RegionServices;
using SspUis.Core.Configurations;
using SspUis.BizLogicLayer.LandingPageDatumServices;

namespace SspUis.BizLogicLayer.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly SystemConf _systemConf;

        public DashboardService(IUnitOfWork unitOfWork, IAuthService authService, SystemConf systemConf)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _systemConf = systemConf;
        }

        public DashboardDataDto GetDashboardData()
        {
            
            return new DashboardDataDto
            {
               
            };
        }


        public LandingPageDataDto GetLandingPageData()
        {
          
            return new LandingPageDataDto
            {
               
            };
        }
    }
}
