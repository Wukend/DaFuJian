using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Data.Interface
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 增加与修改方法
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selectKey"></param>
        /// <param name="updateKey"></param>
        /// <param name="defDate"></param>
        /// <returns></returns>
        Task<int> AddUpdateAsync(List<T> list, string[] selectKey, string[] updateKey, string[] defDate);

        /// <summary>
        /// 增加与修改方法,以及删除方法（根据DeleteKey=98则删除）
        /// isDeleteVirtual=true则只虚拟删除
        /// </summary>
        /// <param name="list"></param>
        /// <param name="selectKey"></param>
        /// <param name="updateKey"></param>
        /// <param name="defDate"></param>
        /// <returns></returns>
        Task<int> AddUpdateAndDeleteAsync(List<T> list, string[] selectKey, string[] updateKey, string[] defDate, bool isDeleteVirtual);


        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idKey"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Guid[] ids, string idKey);

        /// <summary>
        /// 业务删除
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="idKey"></param>
        /// <returns></returns>
        Task<int> DeleteVirtualAsync(Guid[] ids, string idKey);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);

        DataTable QueryByTablde(string sql);
    }
}
