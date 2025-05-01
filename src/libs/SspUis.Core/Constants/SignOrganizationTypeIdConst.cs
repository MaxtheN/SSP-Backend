using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public class SignOrganizationTypeIdConst
    {
        /// <summary>
        /// Tadbirkor
        /// </summary>
        public const int BUSINESSMAN = 1;

        /// <summary>
        /// Tuman
        /// </summary>
        public const int DISTRICT = 2;

        /// <summary>
        /// Viloyat
        /// </summary>
        public const int REGION = 3;

        /// <summary>
        /// Vazirlik
        /// </summary>
        public const int MINISTRY = 4;

        public static int GetPrtnContractTypeId(int prtnContractTypeId)
            => prtnContractTypeId switch
            {
                PrtnContractTypeIdConst._50_100 => DISTRICT,
                PrtnContractTypeIdConst._101_200 => REGION,
                PrtnContractTypeIdConst._201__ => MINISTRY,
                _ => 0
            };
    }
}
