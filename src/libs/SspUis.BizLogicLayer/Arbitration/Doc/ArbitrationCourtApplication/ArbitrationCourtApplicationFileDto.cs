using System;
using GenericServices;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;

namespace SspUis.BizLogicLayer.ArbitrationCourtApplicationServices
{
    public class ArbitrationCourtApplicationFileDto
        : ArbitrationCourtApplicationFileDlDto
        , ILinkToEntity<ArbitrationCourtApplicationFile>
    {
        public string FileName { get; internal set; }
        public DateTime CreatedAt { get; set; }
        public int StepId { get; set; }
        public string FileExtension { get; set; }
    }
}
