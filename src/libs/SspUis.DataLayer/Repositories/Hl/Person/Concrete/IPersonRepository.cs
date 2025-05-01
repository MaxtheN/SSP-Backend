using SspUis.DataLayer.EfClasses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public interface IPersonRepository : IBaseEntityRepository<int, Person, CreatePersonDlDto, UpdatePersonDlDto>
    {
        Person ByPinfl(string pinfl);
        void UpdatePersonFiles(int personId, Guid pictureId);
    }
}
