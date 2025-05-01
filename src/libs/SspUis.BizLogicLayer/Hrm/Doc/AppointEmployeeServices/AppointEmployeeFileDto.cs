using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System;

namespace SspUis.BizLogicLayer
{
    public class AppointEmployeeFileDto :
        AppointEmployeeFileDlDto, ILinkToEntity<AppointEmployeeFile>
    {
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}