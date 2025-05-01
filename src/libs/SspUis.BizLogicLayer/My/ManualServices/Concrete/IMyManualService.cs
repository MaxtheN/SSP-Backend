using SspUis.BizLogicLayer.EnumServices;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.My.ManualServices
{
    public interface IMyManualService : IStatusGeneric
    {
        SelectList<int> ContactTypeSelectList();
    }
}
