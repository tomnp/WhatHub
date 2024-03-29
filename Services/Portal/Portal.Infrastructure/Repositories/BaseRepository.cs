﻿using Microsoft.EntityFrameworkCore;
using Portal.Domain.Core;
using Portal.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Portal.Infrastructure.Repositories
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> FindAll(bool trackChanges);

        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);

        List<T> GetAll();

        T GetById(int id);

        void Create(T entity);

        Task CreateAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }

    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext Context;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(
            ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = Context.Set<T>();
        }

        public void Create(T entity) => DbSet.Add(entity);

        public async Task CreateAsync(T entity) => await DbSet.AddAsync(entity);

        public void Delete(T entity) => DbSet.Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) => !trackChanges ? DbSet.AsNoTracking() : DbSet;

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return !trackChanges ? DbSet.Where(expression).AsNoTracking() : DbSet.Where(expression);
        }

        public List<T> GetAll() => DbSet.ToList();

        public T GetById(int id) => DbSet.Find(id);

        public void Update(T entity) => DbSet.Update(entity);
    }
}