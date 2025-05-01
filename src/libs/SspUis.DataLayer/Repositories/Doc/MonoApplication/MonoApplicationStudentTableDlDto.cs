using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;
using SspUis.DataLayer.EfClasses;
using GenericServices;
using System.Text.Json.Serialization;
using System;
using AutoMapper;

namespace SspUis.DataLayer.Repositories
{
    public class MonoApplicationStudentTableDlDto : EntityDto<MonoApplicationStudentTableDlDto, MonoApplicationStudentTable>,
        IHaveIdProp<long>,
        ILinkToEntity<MonoApplicationStudentTable>
    {
        public long Id { get; set; }
        [JsonIgnore]
        public int PersonId { get; set; }
        public CreatePersonDlDto PersonInfo { get; set; } = new();
    }
}
