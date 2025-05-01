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
    public class CitizenshipService : BaseManualService<Citizenship>, ICitizenshipService
    {
        const string FILE_NAME = "citizenship.json";

        public CitizenshipService()
            : base(FILE_NAME)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gspId">Citizenship id from GSP</param>
        /// <returns>WbCode</returns>
        public string MapGSPToWbCode(int gspId)
        {
            return Data.FirstOrDefault(a => a.GspId == gspId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Citizenship id from GSP</returns>
        public int? MapWbCodeToGSP(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GspId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gnkId">Citizenship id from GNK</param>
        /// <returns>WbCode</returns>
        public string MapGNKToWbCode(string gnkId)
        {
            return Data.FirstOrDefault(a => a.GnkId == gnkId)?.WbCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wbCode"></param>
        /// <returns>Citizenship id from GNK</returns>
        public string MapWbCodeToGNK(string wbCode)
        {
            return Data.FirstOrDefault(a => a.WbCode == wbCode)?.GnkId;
        }
    }
}
