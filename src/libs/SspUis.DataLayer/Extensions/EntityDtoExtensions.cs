//using SspUis.DataLayer.Interfaces;
using SspUis.DataLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.EF;
using WEBASE.Models;

namespace SspUis.DataLayer;

public static class EntityDtoExtensions
{
    public static void ApplyChangesTo<TId, TDto, TEntity>(this IEnumerable<TDto> dtoList, ICollection<TEntity> entityNavigationProperty, Action<TEntity, TDto> onAddToNavigationProperty = null, Action<TEntity, TDto> onUpdateFromNavigationProperty = null) where TDto : EntityDto<TDto, TEntity>, IHaveIdProp<TId> where TEntity : class, IHaveIdProp<TId> where TId : struct
    {
        Dictionary<object, TEntity> dictionary = new Dictionary<object, TEntity>();
        List<TEntity> list = new List<TEntity>();
        foreach (TEntity entity in entityNavigationProperty)
        {
            if (dtoList.Any((TDto a) => a.Id.Equals(entity.Id)))
            {
                dictionary.Add(entity.Id, entity);
            }
            else
            {
                list.Add(entity);
            }
        }

        foreach (TEntity item in list)
        {
            entityNavigationProperty.Remove(item);
        }

        foreach (TDto dto in dtoList)
        {
            if (dictionary.ContainsKey(dto.Id))
            {
                onUpdateFromNavigationProperty?.Invoke(dictionary[dto.Id], dto);
                dto.UpdateEntity(dictionary[dto.Id]);
            }
            else
            {
                TEntity val = dto.CreateEntity();
                onAddToNavigationProperty?.Invoke(val, dto);
                entityNavigationProperty.Add(val);
            }
        }

        dictionary.Clear();
    }

    public static void ApplyChangesByStateIdTo<TId, TDto, TEntity>(this IEnumerable<TDto> dtoList, ICollection<TEntity> entityNavigationProperty, Action<TEntity, TDto> onAddToNavigationProperty = null, Action<TEntity, TDto> onUpdateFromNavigationProperty = null) where TDto : EntityDto<TDto, TEntity>, IHaveIdProp<TId> where TEntity : class, IHaveIdProp<TId>, IHaveStateId where TId : struct
    {
        Dictionary<object, TEntity> dictionary = new Dictionary<object, TEntity>();
        List<TDto> list = dtoList.ToList();
        foreach (TEntity entity in entityNavigationProperty)
        {
            TDto[] array = list.Where((TDto a) => a.Id.Equals(entity.Id)).ToArray();
            if (array.Any())
            {
                if (entity.StateId == 1)
                {
                    dictionary.Add(entity.Id, entity);
                    continue;
                }

                TDto[] array2 = array;
                foreach (TDto item in array2)
                {
                    list.Remove(item);
                }
            }
            else
            {
                entity.StateId = 2;
            }
        }

        foreach (TDto item2 in list)
        {
            if (dictionary.ContainsKey(item2.Id))
            {
                onUpdateFromNavigationProperty?.Invoke(dictionary[item2.Id], item2);
                item2.UpdateEntity(dictionary[item2.Id]);
            }
            else
            {
                TEntity val = item2.CreateEntity();
                onAddToNavigationProperty?.Invoke(val, item2);
                entityNavigationProperty.Add(val);
            }
        }

        dictionary.Clear();
    }

    public static void UpdateStateIdActiveOrPassive<TForeignKey, TManyToManyEntity>(this ICollection<TManyToManyEntity> manyToManyEntityNavigationProperty, IEnumerable<TForeignKey> foreignKeyList) where TManyToManyEntity : class, IHaveSingleUniqueForeignKey<TForeignKey>, IHaveStateId
    {
        HashSet<TForeignKey> foundedEntitiesForeignKeys = new HashSet<TForeignKey>();
        foreach (TManyToManyEntity item in manyToManyEntityNavigationProperty)
        {
            if (foreignKeyList.Contains((TForeignKey)item.GetUniqueForeignKey()))
            {
                if (item.StateId != 1)
                {
                    item.StateId = 1;
                }

                foundedEntitiesForeignKeys.Add((TForeignKey)item.GetUniqueForeignKey());
            }
            else
            {
                item.StateId = 2;
            }
        }

        manyToManyEntityNavigationProperty.AddFromForeignKeys(foreignKeyList.Where((TForeignKey a) => !foundedEntitiesForeignKeys.Contains(a)));
        foundedEntitiesForeignKeys.Clear();
    }

    public static void ApplyChangesByColumnStateIdTo<TId, TDto, TEntity>(this IEnumerable<TDto> dtoList, ICollection<TEntity> entityNavigationProperty, string columnName, Action<TEntity, TDto> onAddToNavigationProperty = null, Action<TEntity, TDto> onUpdateFromNavigationProperty = null)
        where TId : struct
        where TDto : EntityDto<TDto, TEntity>, IHaveIdProp<TId>, IHaveStateIdForUpdate
        where TEntity : class, IHaveIdProp<TId>, IHaveStateId, IHaveStateIdForUpdate
    {
        Dictionary<object, TEntity> dictionary = new Dictionary<object, TEntity>();
        List<TDto> list = dtoList.ToList();
        foreach (TEntity entity in entityNavigationProperty)
        {
            TDto[] array = list.Where((TDto a) => a.GetColumnValue(columnName).Equals(entity.GetColumnValue(columnName))).ToArray();
            if (array.Any())
            {
                if (entity.StateId == 1)
                {
                    dictionary.Add(entity.GetColumnValue(columnName), entity);
                    continue;
                }
                else if (entity.StateId == 2)
                {
                    entity.StateId = 1;
                    dictionary.Add(entity.GetColumnValue(columnName), entity);
                    continue;
                }

                TDto[] array2 = array;
                foreach (TDto item in array2)
                {
                    list.Remove(item);
                }
            }
            else
            {
                entity.StateId = 2;
            }
        }

        foreach (TDto item2 in list)
        {
            if (dictionary.ContainsKey(item2.GetColumnValue(columnName)))
            {
                onUpdateFromNavigationProperty?.Invoke(dictionary[item2.GetColumnValue(columnName)], item2);
                item2.UpdateEntity(dictionary[item2.GetColumnValue(columnName)]);
            }
            else
            {
                TEntity val = item2.CreateEntity();
                onAddToNavigationProperty?.Invoke(val, item2);
                entityNavigationProperty.Add(val);
            }
        }

        dictionary.Clear();
    }

    public static void ApplyChangesByIsDeletedTo<TId, TDto, TEntity>(this IEnumerable<TDto> dtoList, ICollection<TEntity> entityNavigationProperty, Action<TEntity, TDto> onAddToNavigationProperty = null, Action<TEntity, TDto> onUpdateFromNavigationProperty = null, Func<TEntity, bool> entityNavigationPropertyFilter = null) 
        where TId : struct 
        where TDto : EntityDto<TDto, TEntity>, IHaveIdProp<TId> 
        where TEntity : class, IHaveIdProp<TId>, WEBASE.Models.IHaveIsDeleted
    {
        Dictionary<object, TEntity> dictionary = new Dictionary<object, TEntity>();
        List<TDto> list = dtoList.ToList();
        if (entityNavigationPropertyFilter == null)
        {
            entityNavigationPropertyFilter = (TEntity a) => true;
        }

        foreach (TEntity entity in entityNavigationProperty.Where(entityNavigationPropertyFilter))
        {
            TDto[] array = list.Where((TDto a) => a.Id.Equals(entity.Id)).ToArray();
            if (array.Any())
            {
                if (!entity.IsDeleted)
                {
                    dictionary.Add(entity.Id, entity);
                    continue;
                }

                TDto[] array2 = array;
                foreach (TDto item in array2)
                {
                    list.Remove(item);
                }
            }
            else
            {
                entity.IsDeleted = true;
            }
        }

        foreach (TDto item2 in list)
        {
            if (dictionary.ContainsKey(item2.Id))
            {
                onUpdateFromNavigationProperty?.Invoke(dictionary[item2.Id], item2);
                item2.UpdateEntity(dictionary[item2.Id]);
            }
            else
            {
                TEntity val = item2.CreateEntity();
                onAddToNavigationProperty?.Invoke(val, item2);
                entityNavigationProperty.Add(val);
            }
        }

        dictionary.Clear();
    }
}
