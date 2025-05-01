using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;
using System;

namespace SspUis.BizLogicLayer.Hrm;

public class EmployeeLeaveOrderTableDto : EmployeeLeaveOrderTableDlDto, ILinkToEntity<EmployeeLeaveOrderTable>
{

    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; }   // pechat uchun qo'shildi frontga chiqmidi, Tegmelar 
    public string Department { get; set; } 
    public string Position { get; set; }
    public string Employee { get; set; } 
    public string EmployeeManage { get; set; } 
    public new bool IsWithOutPay { get; set; } = false;
    public new bool IsConscription { get; set; } = false;
    public string DocDetails { get; set; }
    public string TableOrganization { get; set; }
    public DateOnly DayOfStartWork { get; set; }
}
