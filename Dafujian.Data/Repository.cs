using Dafujian.Data.Base;
using Dafujian.Data.Context;
using Dafujian.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Data
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        private DafujianContext db2;
        private bool isDisposed;

        public DafujianContext DB
        {
            get
            {
                if (db2 == null)
                    db2 = new DafujianContext();
                return db2;
            }
        }

        /// <summary>
        /// 获取DbSet 
        /// </summary>
        public DbSet<T> DbSet
        {
            get
            {
                return DB.Set<T>();
            }
        }


        /// <summary>
        /// 增加与修改方法
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selectKey"></param>
        /// <param name="updateKey"></param>
        /// <param name="defDate"></param>
        /// <returns></returns>
        public async Task<int> AddUpdateAsync(List<T> list, string[] selectKey, string[] updateKey, string[] defDate)
        {
            using (DafujianContext db = new DafujianContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = await db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefaultAsync<T>();
                        if (t == null)
                        {
                            foreach (var def in defDate)
                                current.GetType().GetProperty(def).SetValue(current, DateTime.Now, null);

                            current.GetType().GetProperty("IsDelete").SetValue(current, 0, null);
                            db.Set<T>().Add(current);
                        }
                        else
                        {
                            foreach (var k in updateKey)
                            {
                                if (k.Equals("UpdateTime"))
                                {
                                    t.GetType().GetProperty(k).SetValue(t, DateTime.Now, null);
                                }
                                else
                                {
                                    var value = current.GetType().GetProperty(k).GetValue(current, null);
                                    t.GetType().GetProperty(k).SetValue(t, value, null);
                                }
                            }
                            db.Set<T>().Attach(t);
                            db.Entry<T>(t).State = EntityState.Modified;
                        }
                    }

                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var msg = string.Concat(new object[]
                     {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                     });
                    Error.WriteLog("Repository-AddUpdateAsync-01", msg);
                    return -22;
                }
            }
        }

        /// <summary>
        /// 增加与修改方法,以及删除方法（根据DeleteKey=98则删除）
        /// isDeleteVirtual=true则只虚拟删除
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selectKey"></param>
        /// <param name="updateKey"></param>
        /// <param name="defDate"></param>
        /// <returns></returns>
        public async Task<int> AddUpdateAndDeleteAsync(List<T> list, string[] selectKey, string[] updateKey, string[] defDate, bool isDeleteVirtual)
        {
            using (DafujianContext db = new DafujianContext())
            {
                try
                {
                    foreach (var current in list)
                    {
                        var t = await db.Set<T>().Where(EfUtils.And<T>(selectKey, current)).FirstOrDefaultAsync<T>();
                        if (t == null)
                        {
                            foreach (var def in defDate)
                                current.GetType().GetProperty(def).SetValue(current, DateTime.Now, null);

                            current.GetType().GetProperty("IsDelete").SetValue(current, 0, null);
                            db.Set<T>().Add(current);
                        }
                        else
                        {
                            var deleteKey = current.GetType().GetProperty("IsDelete").GetValue(current, null).ToString();
                            if (deleteKey.Equals("98"))//执行删除
                            {
                                if (isDeleteVirtual == true)
                                {
                                    t.GetType().GetProperty("UpdateTime").SetValue(t, DateTime.Now, null);
                                    t.GetType().GetProperty("IsDelete").SetValue(t, 85, null);
                                    db.Set<T>().Attach(t);
                                    db.Entry<T>(t).State = EntityState.Modified;
                                }
                                else
                                {
                                    db.Entry<T>(t).State = EntityState.Deleted;
                                }
                            }
                            else
                            {
                                foreach (var k in updateKey)
                                {
                                    if (k.Equals("UpdateTime"))
                                    {
                                        t.GetType().GetProperty(k).SetValue(t, DateTime.Now, null);
                                    }
                                    else
                                    {
                                        var value = current.GetType().GetProperty(k).GetValue(current, null);
                                        t.GetType().GetProperty(k).SetValue(t, value, null);
                                    }
                                }
                                db.Set<T>().Attach(t);
                                db.Entry<T>(t).State = EntityState.Modified;
                            }
                        }
                    }

                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var msg = string.Concat(new object[]
                     {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                     });
                    Error.WriteLog("Repository-AddUpdateAndDeleteAsync-01", msg);
                    return -22;
                }
            }
        }




        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idKey"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Guid[] ids, string idKey)
        {
            using (DafujianContext db = new DafujianContext())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var model = await db.Set<T>().Where(EfUtils.AndByGuid<T>(idKey, id)).FirstOrDefaultAsync<T>();
                        if (model == null)
                            continue;
                        db.Entry<T>(model).State = EntityState.Deleted;
                    }
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var msg = string.Concat(new object[]
                     {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                     });
                    Error.WriteLog("Repository-DeleteAsync-01", msg);
                    return -22;
                }
            }
        }

        /// <summary>
        /// 业务删除
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<int> DeleteVirtualAsync(Guid[] ids, string idKey)
        {
            using (DafujianContext db = new DafujianContext())
            {
                try
                {
                    foreach (var id in ids)
                    {
                        var model = await db.Set<T>().Where(EfUtils.AndByGuid<T>(idKey, id)).FirstOrDefaultAsync<T>();
                        if (model == null)
                            continue;

                        model.GetType().GetProperty("UpdateTime").SetValue(model, DateTime.Now, null);
                        model.GetType().GetProperty("IsDelete").SetValue(model, 99, null);
                        db.Set<T>().Attach(model);
                        db.Entry<T>(model).State = EntityState.Modified;
                    }
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var msg = string.Concat(new object[]
                     {
                        "Message：",
                        ex.Message,
                        "。 Source:",
                        ex.Source,
                        "。 TargetSite:",
                        ex.TargetSite,
                        "。 InnerException：",
                        ex.InnerException
                     });
                    Error.WriteLog("Repository-DeleteVirtualAsync-01", msg);
                    return -22;
                }
            }
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            using (DafujianContext db = new DafujianContext())
            {
                if (predicate == null)
                    return null;

                return await db.Set<T>().Where(predicate).FirstOrDefaultAsync<T>();
            }
        }


        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                var list = await DbSet.Where(predicate).ToListAsync();
                return list;
            }
            else
            {
                var list = await DbSet.ToListAsync();
                return list;
            }
        }


        public DataTable QueryByTablde(string sql)
        {
            using (DafujianContext db = new DafujianContext())
            {
                var conn = new SqlConnection();

                conn = (SqlConnection)db.Database.Connection;
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = sql;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                adapter.Fill(table);
                conn.Close();//连接需要关闭
                conn.Dispose();
                return table;
            }
        }

        ~Repository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                if (db2 != null)
                {
                    db2.Dispose();
                    db2 = null;
                }
            }
            isDisposed = true;
        }
    }
}
