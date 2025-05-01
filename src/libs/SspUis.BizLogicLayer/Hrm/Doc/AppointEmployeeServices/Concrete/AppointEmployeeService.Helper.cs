using AutoMapper;
using SspUis.DataLayer.Repositories;
using System.Linq;

namespace SspUis.BizLogicLayer.Hrm;

public partial class AppointEmployeeService
{
    private IMapper Mapper() => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<CreateEmployeeManageDlDto, UpdateEmployeeManageDlDto>().ReverseMap();
    }).CreateMapper();

}
