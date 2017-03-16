using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface IBaseRepository<TEntity> where TEntity : class
{
    #region 查询

    /// <summary>
    /// 获取对象的可查询表格
    /// </summary>
    IQueryable<TEntity> Table { get; }

    /// <summary>
    /// 获取一个实体对象
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity GetByID(object id);

    #endregion

    #region 增

    /// <summary>
    /// 添加一个实体对象
    /// </summary>
    /// <param name="entity"></param>
    int Insert(TEntity entity);


    #endregion

    #region 删

    /// <summary>
    /// 根据编号删除一个实体对象
    /// </summary>
    /// <param name="id"></param>
    int Delete(int id);


    /// <summary>
    /// 直接删除一个实体对象
    /// </summary>
    /// <param name="entityToDelete"></param>
    int Delete(TEntity entity);


    #endregion

    #region 改

    /// <summary>
    /// 更新一个实体对象
    /// </summary>
    /// <param name="entityToUpdate"></param>
    int Update(TEntity entity);

    #endregion

    #region 延迟操作

    /// <summary>
    /// 延迟添加一个实体对象(需要最后调用DelaySubmit进行提交)
    /// </summary>
    /// <param name="entity"></param>
    void DelayInsert(TEntity entity);

    /// <summary>
    /// 延迟删除一个实体对象(需要最后调用DelaySubmit进行提交)
    /// </summary>
    /// <param name="entity"></param>
    void DelayDelete(TEntity entity);

    /// <summary>
    /// 延迟修改一个实体对象(需要最后调用DelaySubmit进行提交)
    /// </summary>
    /// <param name="entity"></param>
    void DelayUpdate(TEntity entity);

    /// <summary>
    /// 把延迟操作全部提交
    /// </summary>
    void DelaySubmit();

    #endregion

    #region 使用SQL

    /// <summary>
    /// 根据SQL查询(必须是指定表查询)
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    IQueryable<TEntity> GetWithTargetSql(string query);

    /// <summary>
    /// 根据SQL增删改
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    int SqlCommand(string query);

    #endregion
}
