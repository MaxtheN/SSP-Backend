using SspUis.Core;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer
{
    public class UpdateStatusSrvPriceDto : UpdateStatusServicePriceDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
    }

    public class CancelStatusSrvPriceDto : UpdateStatusSrvPriceDto
    {
        public CancelStatusSrvPriceDto()
        {
            base.StatusId = StatusIdConst.CANCELED;
        }

        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }

    public class AcceptStatusSrvPriceDto : UpdateStatusSrvPriceDto
    {
        public AcceptStatusSrvPriceDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }

        public new int StatusId { get => base.StatusId; internal set => base.StatusId = value; }
    }
}
