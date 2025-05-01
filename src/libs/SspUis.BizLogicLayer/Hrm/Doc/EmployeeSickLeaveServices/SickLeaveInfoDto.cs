using System;

namespace SspUis.BizLogicLayer.Hrm
{
    public class SickLeaveInfoDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string OrganizationName { get; set; }
        public string Seria { get; set; }
        public string Number { get; set; }
        public string Diagnos { get; set; }
        public DateTime GivenDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsChecked { get; set; }
        public bool? Checked { get; set; }
        public bool IsClosed { get; set; }
        public bool IsMaternityLeave { get; set; }

    }
}
