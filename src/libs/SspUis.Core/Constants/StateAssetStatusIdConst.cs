using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SspUis.Core
{
    public class StateAssetStatusIdConst
    {
        /// <summary>
        /// Новый
        /// </summary>
        public const int NEW = 10;

        /// <summary>
        /// Отклонен
        /// </summary>
        public const int REJECTED = 5;

        /// <summary>
        /// Согласие получено
        /// </summary>
        public const int AGREED = 20;

        public static int GetStatusId(int stateAssetStatusId)
        {
            return stateAssetStatusId switch
            {
                NEW => StatusIdConst.SENT,
                REJECTED => StatusIdConst.REJECTED,
                AGREED => StatusIdConst.ACCEPTED,
                _ => throw new NotImplementedException("Dav. aktiv tizimidan noma'lum status qabul qilindi"),
            };
        }
    }
}
