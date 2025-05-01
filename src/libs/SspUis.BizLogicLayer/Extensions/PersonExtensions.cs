using SspUis.BizLogicLayer.Models;
using SspUis.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE;
using WEBASE.Integration.Manuals.Services;
using WEBASE.Integration.MSPD.GSP;

namespace SspUis.BizLogicLayer
{
    public static class PersonExtensions
    {
        //public static string FullName(this IPerson person)
        //{
        //    var fullName = $"{person.SurnameLatin} {person.NameLatin}";
        //    if (person.PatronymLatin.NullOrEmpty())
        //        return fullName;
        //    return $"{fullName} {person.PatronymLatin}";
        //}

        //public static string ShortName(this IPerson person)
        //{
        //    if (!person.PatronymLatin.NullOrEmpty())
        //        return $"{person.NameLatin[0]}. {person.PatronymLatin[0]}. {person.SurnameLatin}";
        //    return $"{person.NameLatin[0]}. {person.SurnameLatin}";
        //}

        public static GSPPersonInfoResultWbCodes LoadCodes(this GSPPersonInfoResult result, IUOWIntegrationManuals uowIntegrationManuals)
        {
            return new GSPPersonInfoResultWbCodes
            {
                BirthCountryWbCode = uowIntegrationManuals.Country.MapGSPToWbCode(result.birth_country),
                NationalityWbCode = uowIntegrationManuals.Nationality.MapGSPToWbCode(result.nationality),
                CitizenshipWbCode = uowIntegrationManuals.Citizenship.MapGSPToWbCode(result.citizenship),
                LivingRegionWbCode = uowIntegrationManuals.Region.MapGSPToWbCode(result.living_region_id),
                LivingDistrictWbCode = uowIntegrationManuals.District.MapGSPToWbCode(result.living_district_id)
            };
        }
    }
}
