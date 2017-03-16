using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    IBaseRepository<TEntity> _repo = null;
    public BaseService()
    {
        this._repo = new BaseRepository<TEntity>();
    }

    public TEntity GetByID(object id)
    {
        try
        {
            return _repo.GetByID(id);
        }
        catch
        {
            throw;
        }
    }

    public IQueryable<TEntity> Table()
    {
        try
        {
            return _repo.Table;
        }
        catch
        {
            throw;
        }
    }

    public int Insert(TEntity entity)
    {
        try
        {
            return _repo.Insert(entity);
        }
        catch
        {
            throw;
        }
    }

    public int Delete(int id)
    {
        try
        {
            return _repo.Delete(id);
        }
        catch
        {
            throw;
        }
    }

    public int Delete(TEntity entity)
    {
        try
        {
            return _repo.Delete(entity);
        }
        catch
        {
            throw;
        }
    }

    public int Update(TEntity entity)
    {
        try
        {
            return _repo.Update(entity);
        }
        catch
        {
            throw;
        }
    }

    public void DelayInsert(TEntity entity)
    {
        _repo.DelayInsert(entity);
    }

    public void DelayDelete(TEntity entity)
    {
        _repo.DelayDelete(entity);
    }

    public void DelayUpdate(TEntity entity)
    {
        _repo.DelayUpdate(entity);
    }

    public void DelaySubmit()
    {
        _repo.DelaySubmit();
    }

    public IQueryable<TEntity> GetWithTargetSql(string query)
    {
        return _repo.GetWithTargetSql(query);
    }

    public int SqlCommand(string query)
    {
        return _repo.SqlCommand(query);
    }
}
