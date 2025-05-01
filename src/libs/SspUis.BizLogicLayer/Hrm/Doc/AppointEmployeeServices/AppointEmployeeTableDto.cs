using GenericServices;
using Newtonsoft.Json;
using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories.Hrm;

namespace SspUis.BizLogicLayer.Hrm;

public class AppointEmployeeTableDto : AppointEmployeeTableDlDto, ILinkToEntity<AppointEmployeeTable>
{
    [JsonIgnore]
    [System.Text.Json.Serialization.JsonIgnore]
    public int index { get; set; }   // pechat uchun qo'shildi frontga chiqmidi, Tegmelar 
    public string EmpAppointOrderType { get; set; } = null!;
    public string Organization { get; set; } = null!;
    public int OrganizationId { get; set; }
    public string Department { get; set; } = null!;
    public string Position { get; set; } = null!;
    public string EmployeeFull { get; set; } = null!;
    public string EmploymentType { get; set; } = null!;
    public string WorkSchedule { get; set; } = null!;
    public string FromDepartment { get; set; } = null!;
    public string FromPosition { get; set; } = null!;
    public string MessageFromDoc { get; set; } = null!;
    public new string DetailForPrint { get; set; } = null!;
    public string ChoosenEmployee { get; set; } = null!;
}
