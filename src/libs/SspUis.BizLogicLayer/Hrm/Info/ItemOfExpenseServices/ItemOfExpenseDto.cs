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

namespace SspUis.BizLogicLayer.Hrm.ItemOfExpenseServices
{
    public class ItemOfExpenseDto : UpdateItemOfExpenseDlDto, ILinkToEntity<ItemOfExpense>
    {
        public string State { get; internal set; }
        public string Parent { get; set; }
        new public List<ItemOfExpenseTranslateDto> Translates { get; set; } = new();
    }
}
