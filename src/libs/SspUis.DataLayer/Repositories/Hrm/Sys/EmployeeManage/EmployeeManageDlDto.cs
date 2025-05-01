using SspUis.DataLayer.EfClasses.Hrm;
using System;
using WEBASE.Attributes;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class EmployeeManageDlDto<TDto> : EntityDto<TDto, EmployeeManage>
        where TDto : EmployeeManageDlDto<TDto>
    {
        //[LocalizedStringLength(50)]
        //public string OrderCode { get; set; }
        //[LocalizedRequired]
        //[LocalizedStringLength(50)]
        //public string Code { get; set; }
        //[LocalizedRequired]
        //[LocalizedStringLength(250)]
        //public string ShortName { get; set; }
        //[LocalizedRequired]
        //[LocalizedStringLength(500)]
        //public string FullName { get; set; }
        //[LocalizedRequired]
        //public int EmployeeManageKindId { get; set; }
        //[LocalizedRequired]
        //[LocalizedRange(1, int.MaxValue)]
        //public int PositionId { get; set; }
        //public decimal EmploymentRate { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long Id { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DocTableId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, long.MaxValue)]
        public long DocId { get; set; }
        [LocalizedRequired]
        public DateOnly StartOn { get; set; }
        public DateOnly? EndOn { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EmpAppointOrderTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int DepartmentId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int PositionId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EmployeeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int EmploymentTypeId { get; set; }
        [LocalizedRequired]
        [LocalizedRange(1, int.MaxValue)]
        public int WorkScheduleId { get; set; }
        public DateOnly? EndByDocumentOn { get; set; }
        public decimal? EmploymentRate { get; set; }
        public int OrganizationId { get; set; }

        public override EmployeeManage CreateEntity()
        {
            var entity = base.CreateEntity();
            entity.IsDeleted = false;
            return entity;
        }

        public override void UpdateEntity(EmployeeManage entity)
        {
            base.UpdateEntity(entity);
        }
    }
}
