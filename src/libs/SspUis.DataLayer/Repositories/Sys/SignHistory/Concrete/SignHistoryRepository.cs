using SspUis.DataLayer.EfClasses;
using GenericServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class SignHistoryRepository : BaseEntityRepository<long, SignHistory>, ISignHistoryRepository
    {
        private DbContext _context;
        private DbSet<SignHistory> _dbSet;

        public SignHistoryRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        public virtual SignHistory Create(CreateSignHistoryDlDto createDto)
        {
            var entity = createDto.CreateEntity();

            DbSet.Add(entity);
            Context.Entry(entity).State = EntityState.Added;
            return entity;
        }
    }
}
