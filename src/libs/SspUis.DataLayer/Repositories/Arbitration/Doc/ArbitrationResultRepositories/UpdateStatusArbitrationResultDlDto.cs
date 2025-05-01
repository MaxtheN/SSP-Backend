using SspUis.DataLayer.EfClasses;
using WEBASE.Attributes;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer.Repositories;

public class UpdateStatusArbitrationResultDlDto :
	EntityDto
    <UpdateStatusArbitrationResultDlDto,
		ArbitrationResult>, IHaveIdProp<long>

{
    [LocalizedRange(1, long.MaxValue)]
    [LocalizedRequired]
    public long Id { get; set; }

    [LocalizedRange(1, int.MaxValue)]
    [LocalizedRequired]
    public int StatusId { get; set; }

    public string? Message { get; set; }
}
