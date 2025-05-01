using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public class PrtnContractTypeIdConst
    {
        /// <summary>
        /// 51 - 100
        /// </summary>
        public const int _50_100 = 1;

        /// <summary>
        /// 101 - 200
        /// </summary>
        public const int _101_200 = 2;

        /// <summary>
        /// 201 - ...
        /// </summary>
        public const int _201__ = 3;

        public static int GetBySignOrganizationTypeId(int signOrganizationTypeId)
            => signOrganizationTypeId switch
            {
                SignOrganizationTypeIdConst.DISTRICT => _50_100,
                SignOrganizationTypeIdConst.REGION => _101_200,
                SignOrganizationTypeIdConst.MINISTRY => _201__,
                _ => 0
            };
    }
}
