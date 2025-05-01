using Humanizer;
using Ssp.DataLayer.EFClasses.Edoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis.BizLogicLayer.Edoc.AppointmentOrderService
{
    public interface IAppointmentOrderService
    {
        void Create(User dto, AppointmentOrderDto appointDto);
        void Update(User dto, AppointmentOrderDto appointDto);
        void Delete(User dto);
    }
}
