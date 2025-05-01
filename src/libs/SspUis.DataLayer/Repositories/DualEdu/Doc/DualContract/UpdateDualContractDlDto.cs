using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateDualContractDlDto
        : DualContractDlDto<UpdateDualContractDlDto>,
        IHaveIdProp<long>
{
    public long Id { get; set; }
}