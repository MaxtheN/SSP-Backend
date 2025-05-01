using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Minio;
using WEBASE.Storage;

namespace SspUis.BizLogicLayer.Minio
{
    public class SspUisMinioStorageService : MinioStorageService
    {
        public SspUisMinioStorageService(MinioStorageClient minioClient) : base(minioClient)
        {
        }

        public override IEnumerable<IStorageFileInfo> SaveTemp(string document, params StorageFile[] files)
        {
            if (files.Any(f => f.FileName.Length > 100))
            {
                AddError("Имя файла слишком длинное. Должен состоять менее чем из 100 символов.");
            }
            return base.SaveTemp(document, files);
        }
    }
}
