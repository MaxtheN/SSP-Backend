using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using WEBASE.OfficeTools.Attributes;

namespace SspUis.BizLogicLayer.ExecutionApplicationServices
{
    public class ExecutionApplicationDto : UpdateExecutionApplicationDlDto, ILinkToEntity<ExecutionApplication>, IDocument
    {
        public string Region { get; set; }
        public string District { get; set; }
        public int StatusId {get; set;}
        public List<ExecutionApplicationTableDto> Tables { get; set; } = new();

        public bool CanEdit { get; set; }
        public bool CanSign { get; set; }
        public bool CanAccept { get; set; }
        public bool CanCancel { get; set; }
    }
}
