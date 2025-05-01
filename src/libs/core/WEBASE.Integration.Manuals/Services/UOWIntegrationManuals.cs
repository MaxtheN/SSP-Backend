using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBASE.Integration.Manuals.Services
{
    public class UOWIntegrationManuals : IUOWIntegrationManuals
    {
        private readonly IServiceProvider _serviceProvider;

        public UOWIntegrationManuals(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICountryService Country { get => _serviceProvider.GetRequiredService<ICountryService>(); }
        public IRegionService Region { get => _serviceProvider.GetRequiredService<IRegionService>(); }
        public IDistrictService District { get => _serviceProvider.GetRequiredService<IDistrictService>(); }
        public IGenderService Gender { get => _serviceProvider.GetRequiredService<IGenderService>(); }
        public INationalityService Nationality { get => _serviceProvider.GetRequiredService<INationalityService>(); }
        public ICitizenshipService Citizenship { get => _serviceProvider.GetRequiredService<ICitizenshipService>(); }
    }
}
