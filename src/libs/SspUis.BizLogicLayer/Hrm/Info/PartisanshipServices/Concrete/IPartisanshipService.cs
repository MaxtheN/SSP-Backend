using SspUis.DataLayer.EfClasses.Hrm;
using SspUis.DataLayer.Repositories;
using WEBASE.Models;

namespace SspUis.BizLogicLayer.Hrm.PartisanshipServices
{
    public interface IPartisanshipService : IBaseEntityService<Partisanship,PartisanshipListDto,PartisanshipDto,CreatePartisanshipDlDto,UpdatePartisanshipDlDto>
    {
        SelectList<int> AsSelectList();
    }
}
