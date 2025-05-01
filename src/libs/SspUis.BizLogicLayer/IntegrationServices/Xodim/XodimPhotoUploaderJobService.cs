using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace SspUis.BizLogicLayer.IntegrationServices.Xodim
{
    public class XodimPhotoUploaderJobService : IJob
    {
        private readonly IXodimPhotoUploader _xodimPhotoUploader;

        public XodimPhotoUploaderJobService(IXodimPhotoUploader xodimPhotoUploader)
        {
            _xodimPhotoUploader = xodimPhotoUploader;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _xodimPhotoUploader.UploadPhotos();
        }
    }
}
