using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using WEBASE.Models;

namespace SspUis.BizLogicLayer
{
    public class SrvManualService : ISrvManualService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SrvManualService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public SelectList<int> ServicePriceTypeSelectList()
            => _unitOfWork.Context.Set<ServicePriceType>().AsSelectList();
    }
}
