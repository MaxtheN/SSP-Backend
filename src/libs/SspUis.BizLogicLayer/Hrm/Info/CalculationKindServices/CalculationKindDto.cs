using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using WEBASE.Attributes;
using WEBASE.Models;
using SspUis.DataLayer.Repositories;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.EfClasses.Hrm;

namespace SspUis.BizLogicLayer.Hrm.CalculationKindServices
{
    public class CalculationKindDto : UpdateCalculationKindDlDto, ILinkToEntity<CalculationKind>
    {
        public string State { get; internal set; }
        public string CalculationKindKind { get; internal set; }
        public string ItemOfExpenseCode { get; set; }
        new public List<CalculationKindTranslateDto> Translates { get; set; } = new();
        new public List<CalculationKindPercentDto> Percents { get; set; } = new();
        new public List<CalculationKindUsedTableDto> UsedTables { get; set; } = new();
        new public List<CalculationKindAllowedDocDto> AllowedDocs { get; set; } = new();
        new public List<CalculationKindStructureDto> CalculationStructure { get; set; } = new();
    }
}
