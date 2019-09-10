using System;
using System.Collections.Generic;
using System.Linq;
using TestSolution.Data;
using TestSolution.Interfaces;

namespace TestSolution.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : SystemBase
    {
        private readonly List<TEntity> _active;
        private readonly List<TEntity> _archived;
        private readonly string _notFoundMessage = "Записи с идентификатором {0} не найдено!";

        protected RepositoryBase() {
            _active = new List<TEntity>();
            _archived = new List<TEntity>();
        }

        public TEntity Create(TEntity newEntity)
        {
            newEntity.Id = _active.Count + _archived.Count + 1;
            newEntity.CreateTime = DateTime.Now;
            NullFields(newEntity);

            _active.Add(newEntity);

            return newEntity;
        }
        
        public TEntity Update(TEntity updatedEntity)
        {
            return Update(updatedEntity, true);
        }

        public TEntity Update(TEntity updatedEntity, bool outerUpdate)
        {
            var currentEntity = GetById(updatedEntity.Id);

            if (currentEntity == null)
                throw new Exception(string.Format(_notFoundMessage, updatedEntity.Id));
            else
            {
                if (outerUpdate)
                {
                    currentEntity.UpdateTime = DateTime.Now;
                    UpdateMutableFields(currentEntity, updatedEntity);

                    return currentEntity;
                }
                else
                {
                    var idx = _active.IndexOf(currentEntity);
                    if (idx > -1)
                    {
                        _active[idx] = updatedEntity;
                    }
                    else
                    {
                        idx = _archived.IndexOf(currentEntity);
                        _archived[idx] = updatedEntity;
                    }

                    return updatedEntity;
                }
            }
        }

        public virtual TEntity Get(long entityId)
        {
            return GetById(entityId);
        }

        public virtual TEntity Archive(long entityId)
        {
            var entity = _active.FirstOrDefault(e => e.Id == entityId);

            if (entity == null)
                throw new Exception(string.Format(_notFoundMessage, entityId));
            else
            {
                _active.Remove(entity);
                if (entity.ArchiveTime == null)
                    entity.ArchiveTime = DateTime.Now;
                _archived.Add(entity);

                return entity;
            }
        }

        private TEntity GetById(long entityId)
        {
            return _active.FirstOrDefault(e => e.Id == entityId) ??
                _archived.FirstOrDefault(e => e.Id == entityId);
        }

        protected virtual void NullFields(TEntity entity)
        {
            entity.UpdateTime = null;
            entity.ArchiveTime = null;
        }

        protected abstract void UpdateMutableFields(TEntity oldValue, TEntity newValue);
    }
}
