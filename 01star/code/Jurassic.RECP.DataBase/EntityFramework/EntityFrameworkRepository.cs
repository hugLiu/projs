using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Jurassic.RECP.DataBase.EntityFramework;

namespace Jurassic.RECP.DataBase.EntityFramework
{

    public abstract class EntityFrameworkRepository<TEntity> : IEntityFrameworkRepository<TEntity>, IDisposable
        where TEntity : class
    {
        protected DbContext _context;

        protected EntityFrameworkRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 查询接口
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetQuery()
        {
            return _context.Set<TEntity>();
        }

        /// <summary>
        /// 数据集接口
        /// </summary>
        /// <returns></returns>
        protected IDbSet<TEntity> GetDbSet()
        {
            return _context.Set<TEntity>();
        }

        #region 事务

        /// <summary>
        /// 事务
        /// </summary>
        DbContextTransaction _trans;

        /// <summary>
        /// 开启事务
        /// </summary>
        public virtual void BeginTrans()
        {
            _trans = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// 事务提交
        /// 判断:_trans对象不为空前提下
        /// </summary>
        public virtual void EndTrans()
        {
            try
            {
                if (_trans != null)
                {
                    _trans.Commit();
                    _trans.Dispose();
                    _trans = null;
                }
            }
            catch
            {
                RollbackTrans();
                throw;
            }
        }

        /// <summary>
        /// 事务回滚
        /// 判断:_trans对象不为空前提下
        /// </summary>
        public virtual void RollbackTrans()
        {
            if (_trans != null)
            {
                _trans.Rollback();
                _trans.Dispose();
                _trans = null;
            }
        }

        #endregion

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            GetDbSet().Add(entity);

            Submit();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            DbEntityEntry<TEntity> entry = _context.Entry<TEntity>(entity);
            // entry.State = EntityState.Unchanged;
            Submit();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            GetDbSet().Remove(entity);
            Submit();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="whereExpr"></param>
        public virtual void DeleteList(Expression<Func<TEntity, bool>> whereExpr)
        {
            IEnumerable<TEntity> entities = GetQuery().Where(whereExpr).ToList();

            DeleteList(entities);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entities"></param>
        public virtual void DeleteList(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                GetDbSet().Remove(entity);
            }

            Submit();
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<TEntity, bool>> whereExpr)
        {
            return (this.Count(whereExpr) > 0);
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        public int Count(Expression<Func<TEntity, bool>> whereExpr)
        {
            return GetQuery().Where(whereExpr).Count();
        }

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public List<TEntity> GetAll()
        {
            return GetQuery().ToList();
        }

        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity Find(object key)
        {
            return GetDbSet().Find(new object[] { key });
        }

        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        public TEntity Find(Expression<Func<TEntity, bool>> whereExpr)
        {
            return GetQuery().Where(whereExpr).FirstOrDefault();
        }

        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> FindList<TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TKey>> orderbyExpr, int orderDirection)
        {
            return this.FindList<TEntity, TKey>(whereExpr, t => t, orderbyExpr, orderDirection);
        }


        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="returnCount"></param>
        /// <returns></returns>
        public IEnumerable<TResult> FindList<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int returnCount = -1)
        {
            var result = GetQuery().Where(whereExpr).Select(selectExpr);
            if (result != null && result.Count() > 0)
            {
                if (returnCount > 0)
                {
                    if (orderDirection > 0)
                    {
                        result = result.OrderByDescending(orderbyExpr).Take(returnCount);
                    }
                    else
                    {
                        result = result.OrderBy(orderbyExpr).Take(returnCount);
                    }
                }
                return result.ToList();
            }
            return null;
        }

        /// <summary>
        /// 分页查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public IEnumerable<TResult> FindListByPage<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int pageSize, int pageNo, out int recordCount)
        {
            var result = GetQuery().Where(whereExpr).Select(selectExpr);
            recordCount = result.Count();

            if (pageNo > recordCount) pageNo = recordCount;
            if (pageNo <= 0) pageNo = 1;

            if (recordCount > 0)
            {
                if (recordCount > pageSize)
                {
                    if (orderDirection > 0)
                    {
                        return result.OrderByDescending(orderbyExpr).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    }
                    else
                    {
                        return result.OrderBy(orderbyExpr).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (orderDirection > 0)
                    {
                        return result.OrderByDescending(orderbyExpr).ToList();
                    }
                    else
                    {
                        return result.OrderBy(orderbyExpr).ToList();
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 提交保存所有变更操作
        /// </summary>
        public void Submit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_trans != null)
            {
                _trans.Rollback();
            }
            _context.Dispose();
        }
    }
}
