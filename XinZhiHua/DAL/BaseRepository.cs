using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    internal MyDbContext _context;
    internal DbSet<TEntity> _dbSet;

    public BaseRepository()
    {
        this._context = ContextFactory.GetCurrentContext();
        this._dbSet = this._context.Set<TEntity>();
    }

    public IQueryable<TEntity> Table
    {
        get
        {
            return this._dbSet;
        }
    }

    public TEntity GetByID(object id)
    {
        try
        {
            return _dbSet.Find(id);
        }
        catch
        {
            throw;
        }
    }

    public int Insert(TEntity entity)
    {
        int num = 0;
        try
        {
            _dbSet.Add(entity);
            num = _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
        return num;
    }

    public int Delete(int id)
    {
        int num = 0;
        try
        {
            TEntity entityToDelete = _dbSet.Find(id);
            num = Delete(entityToDelete);
        }
        catch (Exception)
        {
            throw;
        }
        return num;
    }

    public int Delete(TEntity entity)
    {
        int num = 0;
        try
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
            num = _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }

        return num;


    }

    public int Update(TEntity entity)
    {
        int num = 0;
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            num = _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
        return num;
    }

    public void DelayInsert(TEntity entity)
    {
        try
        {
            _dbSet.Add(entity);
        }
        catch
        {
            throw;
        }
    }

    public void DelayDelete(TEntity entity)
    {
        try
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            _dbSet.Remove(entity);
        }
        catch
        {
            throw;
        }
    }

    public void DelayUpdate(TEntity entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        catch
        {
            throw;
        }
    }

    public void DelaySubmit()
    {
        try
        {
            _context.SaveChanges();
        }
        catch
        {
            throw;
        }
    }

    public IQueryable<TEntity> GetWithTargetSql(string query)
    {
        return _dbSet.SqlQuery(query).ToList().AsQueryable();

    }

    public int SqlCommand(string query)
    {
        try
        {
            return _context.Database.ExecuteSqlCommand(query);
        }
        catch
        {
            throw;
        }
    }
}
