using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.Core.Security;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Corruption;
using StatusGeneric;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Corruption.ManualServices
{
    public class CorruptionManualService : StatusGenericHandler, ICorruptionManualService
    {
        private readonly ICrudServices _service;
        private readonly DbContext _context;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public CorruptionManualService(
            ICrudServices crudService,
            DbContext context,
            IAuthService authService,
            IUnitOfWork unitOfWork)
        {
            _service = crudService;
            _context = context;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public SelectList<int> CorruptionReviewTypeSelectList()
        {
            return _context.Set<CorruptionReviewType>().Include(a => a.Translates).AsSelectList();
        }
        public SelectList<int> JoinAntiCorruptionResultTypeSelectList()
        {
            return _context.Set<JoinAntiCorruptionResultType>().Include(a => a.Translates).AsSelectList();
        }
    }
}
