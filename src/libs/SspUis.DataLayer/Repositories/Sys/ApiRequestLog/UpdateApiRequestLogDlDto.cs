using System;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories
{
    public class UpdateApiRequestLogDlDto : ApiRequestLogDlDto<UpdateApiRequestLogDlDto>, IHaveIdProp<Guid>
    {
        public Guid Id { get; set; }
    }
}
