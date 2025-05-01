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
    public class NationalityService : BaseManualService<Nationality>, INationalityService
    {
        const string FILE_NAME = "nationality.json";

        public NationalityService()
            : base(FILE_NAME)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gspId">Nationality id from GSP</param>
        /// <returns>WbCode</returns>
        public string MapGSPToWbCode(int gspId)
        {
            return Data.FirstOrDefault(a => a.GspId == gspId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Nationality id from GSP</returns>
        public int? MapWbCodeToGSP(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GspId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gnkId">Nationality id from GNK</param>
        /// <returns>WbCode</returns>
        public string MapGNKToWbCode(string gnkId)
        {
            return Data.FirstOrDefault(a => a.GnkId == gnkId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Nationality id from GNK</returns>
        public string MapWbCodeToGNK(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GnkId;
        }
    }
}
