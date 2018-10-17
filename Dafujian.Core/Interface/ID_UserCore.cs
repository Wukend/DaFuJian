using Dafujian.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Core.Interface
{
    public interface ID_UserCore
    {
        /// <summary>
        /// 获取盐值
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<string> GetSalt(string mobile);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<D_User> Login(string mobile, string password);

        ///// <summary>
        ///// 获取用户列表
        ///// </summary>
        ///// <returns></returns>
        //Task<List<UserRecord>> GetList();

        ///// <summary>
        ///// 获取分页用户列表
        ///// </summary>
        ///// <param name="paging"></param>
        ///// <returns></returns>
        //Task<PagedResult<UserRecord>> GetList(Paging paging);

        ///// <summary>
        ///// 通过userId获取用户记录
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //Task<UserRecord> GetRecordByUserId(Guid userId);

        ///// <summary>
        ///// 通过mobile获取用户记录
        ///// </summary>
        ///// <param name="mobile"></param>
        ///// <returns></returns>
        //Task<UserRecord> GetRecordByMobile(string mobile);

        /// <summary>
        /// 插入用户数据
        /// </summary>
        /// <param name="list">数据集</param>
        /// <returns></returns>
        Task<int> Insert(List<D_User> list);

        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> Update(List<D_User> list);

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> ChangePassWord(List<D_User> list);

        /// <summary>
        /// 删除用户数据(业务删除 IsDelete=99)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> Delete(Guid[] list);
    }
}
