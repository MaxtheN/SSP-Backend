using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdatePersonLogDlDto : PersonLogDlDto<UpdatePersonLogDlDto>, IHaveIdProp<int>
{
    public int Id { get; set; }
}
