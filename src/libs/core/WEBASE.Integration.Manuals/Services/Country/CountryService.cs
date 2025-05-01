using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Integration.Manuals.Models;
using Newtonsoft.Json;

namespace WEBASE.Integration.Manuals.Services
{
    public class CountryService : BaseManualService<Country>, ICountryService
    {
        const string FILE_NAME = "country.json";

        public CountryService()
            : base(FILE_NAME)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gspId">Country id from GSP</param>
        /// <returns>WbCode</returns>
        public string MapGSPToWbCode(int gspId)
        {
            return Data.FirstOrDefault(a => a.GspId == gspId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Country id from GSP</returns>
        public int? MapWbCodeToGSP(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GspId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gnkId">Country id from GNK</param>
        /// <returns>WbCode</returns>
        public string MapGNKToWbCode(string gnkId)
        {
            return Data.FirstOrDefault(a => a.GnkId == gnkId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Country id from GNK</returns>
        public string MapWbCodeToGNK(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GnkId;
        }
    }
}
