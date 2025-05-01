using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEBASE;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Utility;

namespace SspUis.DataLayer.Repositories
{
    public class ApiRequestLogDlDto<TDto> : EntityDto<TDto, ApiRequestLog>
       where TDto : ApiRequestLogDlDto<TDto>
    {
        public bool IsSuccess { get; set; }
        public int? UserId { get; set; }
        public string? UserInfo { get; set; }
        public int? TableId { get; set; }
        public long? DocumentId { get; set; }
        [LocalizedStringLength(500)]
        public string? RequestUrl { get; set; }
        public string? RequestContent { get; set; }
        public int? ResponseStatus { get; set; }
        public string? ResponseContent { get; set; }
        public string? Exception { get; set; }
        public DateTime RequestAt { get; set; }
        public DateTime? ResponseAt { get; set; }

        public override ApiRequestLog CreateEntity()
        {
            var ent = base.CreateEntity();
            ent.Id = Guid.NewGuid();
            return ent;
        }
    }
}
