using AutoMapper;
using GenericServices;
using GenericServices.Configuration;
using System;
using SspUis.DataLayer.EfClasses;

namespace SspUis.BizLogicLayer.Hrm.StaffingServices
{
    public class PositionDto : ILinkToEntity<Position>
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? OrderCode { get; set; }
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
    }
}
