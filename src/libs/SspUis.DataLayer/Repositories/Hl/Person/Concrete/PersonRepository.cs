using GenericServices;
using Microsoft.EntityFrameworkCore;
using Ssp.DataLayer.EFClasses.Edoc;
using SspUis.Core;
using SspUis.DataLayer.EfClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.EF;

namespace SspUis.DataLayer.Repositories
{
    public class PersonRepository : BaseEntityRepository<int, Person, CreatePersonDlDto, UpdatePersonDlDto>, IPersonRepository
    {
        public PersonRepository(ICrudServices crudServices)
            : base(crudServices)
        {
        }

        protected override IQueryable<Person> ByIdQuery()
        {
            var query = base.ByIdQuery();

            return query;
        }

        public Person ByPinfl(string pinfl)
        {
            return ByIdQuery().FirstOrDefault(a => a.Pinfl == pinfl);
        }

        public override Person Create(CreatePersonDlDto createDto, Action<Person> validation = null)
        {
            if (validation == null)
            {
                CreateValidate(createDto);
            }

            if (base.HasErrors)
            {
                return null;
            }

            Person val = createDto.CreateEntity();
            if (base.IsValid)
            {
                if (validation == null)
                {
                    CreateValidate(val, createDto);
                }
                else
                {
                    validation(val);
                }
            }

            OnCreate(val, createDto);
            if (base.HasErrors)
            {
                return null;
            }

            base.DbSet.Add(val);
            base.Context.Entry(val).State = EntityState.Added;
            return val;
        }

        public void UpdatePersonFiles(int personId, Guid pictureId)
        {
            var person = base.ById(personId);
            var pictureIds = new List<Guid>();
            if (person.PictureId != null)
                pictureIds.Add((Guid)person.PictureId);

            pictureIds.UpdateFromFile(DocumentStorageConst.HL_PERSON_FILES, person.Id.ToString(), new List<Guid> { pictureId });

            if (pictureIds.Count > 0)
                person.PictureId = pictureIds[0];
            else
                throw new Exception("No picture IDs found after update");
        }
    }
}