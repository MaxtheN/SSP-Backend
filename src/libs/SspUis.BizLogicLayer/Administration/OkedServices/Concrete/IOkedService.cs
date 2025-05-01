using SspUis.DataLayer.Repositories;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.AspNet;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.OkedServices
{
    public interface IOkedService : IStatusGeneric
    {
        PagedResult<OkedListDto> GetList(SortFilterPageOptions dto);
        OkedDto Get();
        OkedDto Get(int id);
        SelectList<int> AsSelectList(int level = 5);
        HaveId<int> Create(CreateOkedDlDto dto);
        void Update(UpdateOkedDlDto dto);
        Stream SaveAsExecel(SortFilterPageOptions dto);
        void Delete(int id);
		SelectList<int> SelectList(string? search);
	}
}
