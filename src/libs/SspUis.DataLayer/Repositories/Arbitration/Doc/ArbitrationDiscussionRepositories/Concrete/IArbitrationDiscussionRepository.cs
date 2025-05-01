using SspUis.DataLayer.EfClasses;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public interface IArbitrationDiscussionRepository :
	IBaseEntityRepository<long,
		ArbitrationDiscussion,
		CreateArbitrationDiscussionDlDto,
		UpdateArbitrationDiscussionDlDto,
		UpdateStatusArbitrationDiscussionDlDto>
  
{

}

