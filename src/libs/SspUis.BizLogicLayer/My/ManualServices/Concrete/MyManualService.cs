using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.My.ManualServices
{
    public class MyManualService : StatusGenericHandler, IMyManualService
    {
        private readonly DbContext _context;

        public MyManualService(
            DbContext context)
        {
            _context = context;
        }

        public SelectList<int> ContactTypeSelectList()
        {
            return _context.Set<ContactType>().Include(a => a.Translates).AsSelectList();
        }
    }
}
