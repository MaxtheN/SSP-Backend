using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusGeneric;

namespace SspUis.BizLogicLayer.IntegrationServices.Xodim
{
    public interface IXodimPhotoUploader : IStatusGenericHandler
    {
        Task UploadPhotos();
        Task UploadByEmployeeId(int Id);
        int NoPhotoCount();
    }
}
