using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.BusinessmanAccountServices
{
    public static class ContractorAuthModelSelect
    {
        public static IQueryable<ContractorAuthModel> MapToAuthModel(this IQueryable<Contractor> contractors)
        {
            return contractors.Select(a => new ContractorAuthModel
            {
                Id = a.Id,
                ShortName = a.ShortName,
                FullName = a.FullName,
                Inn = a.Inn,
                RegionId = a.RegionId,
                CountryId = a.CountryId,
                RegistrationDate = a.RegistrationDate,
                RegistrationNumber = a.RegistrationNumber,
                DistrictId = a.DistrictId,
                OkedId = a.OkedId,
                Pinfl = a.Pinfl,
                PhoneNumber = a.BusinessmanUserInContractors.Any() ? (a.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUser != null).BusinessmanUser != null ? a.BusinessmanUserInContractors.FirstOrDefault(a => a.BusinessmanUser != null).BusinessmanUser.UserName : a.PhoneNumber) : null,
            }); ;
        }
    }
}
