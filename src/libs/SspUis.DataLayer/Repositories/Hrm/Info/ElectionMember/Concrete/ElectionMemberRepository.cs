using System.Linq;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using SspUis.DataLayer.EfClasses.Hrm;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories;

public class ElectionMemberRepository : BaseEntityRepository<int, ElectionMember, CreateElectionMemberDlDto, UpdateElectionMemberDlDto>, IElectionMemberRepository
{
    public ElectionMemberRepository(ICrudServices crudServices)
        : base(crudServices)
    {
    }

    protected override IQueryable<ElectionMember> ByIdQuery()
        => AllAsQueryable.Include(p => p.Translates);
}
