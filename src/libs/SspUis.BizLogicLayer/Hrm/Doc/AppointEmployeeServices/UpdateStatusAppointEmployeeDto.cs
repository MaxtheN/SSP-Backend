using SspUis.Core;
using SspUis.DataLayer.Repositories.Hrm;
using System;
using WEBASE.Attributes;

namespace SspUis.BizLogicLayer.Hrm
{
    public class UpdateStatusAppointEmployeeDto : UpdateStatusAppointEmployeeDlDto
    {
        internal new int StatusId { get => base.StatusId; set => base.StatusId = value; }
        [LocalizedRequired]
        public string SignedData { get; set; }
    }
    public class SignStatusAppointEmployeeDto : UpdateStatusAppointEmployeeDto
    {
        public SignStatusAppointEmployeeDto()
        {
            base.StatusId = StatusIdConst.SIGNING;
        }
    }
    public class AcceptStatusAppointEmployeeDto : UpdateStatusAppointEmployeeDto
    {
        public AcceptStatusAppointEmployeeDto()
        {
            base.StatusId = StatusIdConst.ACCEPTED;
        }
    }


    public class CreateUserResponseModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
    }


    public static class CreateErrorResponseModel
    {
        public static string ErrorName { get; set; }
        public static long ErrorId { get; set; }
    }
}
