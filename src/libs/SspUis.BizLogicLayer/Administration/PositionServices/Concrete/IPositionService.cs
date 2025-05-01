using Microsoft.EntityFrameworkCore.Storage;
using SspUis.DataLayer.EfClasses;
using SspUis.DataLayer.Repositories;
using System.IO;
using WEBASE;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.PositionServices
{
    public interface IPositionService : IBaseEntityService<Position, PositionListDto, PositionDto, CreatePositionDlDto, UpdatePositionDlDto>
    {
        //SelectList<int> AllAsSelectList();
        SelectList<int> AsSelectList(bool fromOrganizationalStructure = false);
        Stream SaveAsExecel(TableSortFilterPageOptions dto);
        void SyncEdocPosition(IDbContextTransaction outTransaction = null);
        HaveId<int> Create(CreatePositionDlDto dto);
        void Update(UpdatePositionDlDto dto);

    }
}
