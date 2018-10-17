using Dafujian.Common;
using Dafujian.Core.Interface;
using Dafujian.Data.Interface;
using Dafujian.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dafujian.Core
{
    public class D_UserCore : ID_UserCore
    {
        private readonly IRepository<D_User> Repository;

        public D_UserCore(IRepository<D_User> _Repository)
        {
            Repository = _Repository;
        }

        /// <summary>
        /// 获取盐值
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public async Task<string> GetSalt(string mobile)
        {
            var sql = "select Salt from [dbo].[EM_User] where Mobile = '" + mobile + "'";
            var db = Repository.QueryByTablde(sql);
            if (db == null || db.Rows.Count <= 0)
                return null;

            var list = await Task.Run(() =>
                ModelConvertHelper<D_User>.ConvertToModel(db)
            );

            return list[0].Salt;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task<D_User> Login(string mobile, string password)
        {
            string salt = await GetSalt(mobile);
            if (string.IsNullOrEmpty(salt))
                return null;

            string pwd = Utils.PasswordHashing(password, salt);

            var sql = "select * from [dbo].[EM_User] where Mobile = '" + mobile + "' and Pwd = '" + pwd + "' and IsDelete != 99";
            var db = Repository.QueryByTablde(sql);
            if (db == null || db.Rows.Count <= 0)
                return null;

            var list = await Task.Run(() =>
                ModelConvertHelper<D_User>.ConvertToModel(db)
            );

            return list[0];
        }

        ///// <summary>
        ///// 获取用户列表
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<UserRecord>> GetList()
        //{
        //    var sql = "select a.UserId,a.RoleId,a.Mobile,a.Nickname,a.Real,a.Email,a.MaxAccess,a.CreateTime,a.UpdateTime,b.Name as RoleName,b.[Type] as RoleType,d.Name as TeamName,d.TeamId from EM_User a inner join EM_Role b on a.RoleId = b.RoleId inner join (select b.UserId, c.Name,c.TeamId from EM_Team_User b inner join EM_Team c on b.TeamId = c.TeamId) d on a.UserId = d.UserId where a.IsDelete != 99";
        //    var db = Repository.QueryByTablde(sql);
        //    if (db == null || db.Rows.Count <= 0)
        //        return null;

        //    var list = await Task.Run(() =>
        //        ModelConvertHelper<UserRecord>.ConvertToModel(db)
        //    );

        //    return list;
        //}

        ///// <summary>
        ///// 获取分页用户列表
        ///// </summary>
        ///// <returns></returns>
        //public async Task<PagedResult<UserRecord>> GetList(Paging paging)
        //{
        //    var sql = "select a.UserId,a.RoleId,a.Mobile,a.Nickname,a.Real,a.Email,a.MaxAccess,a.CreateTime,a.UpdateTime,b.Name as RoleName,b.[Type] as RoleType,d.Name as TeamName,d.TeamId from EM_User a inner join EM_Role b on a.RoleId = b.RoleId inner join (select b.UserId, c.Name,c.TeamId from EM_Team_User b inner join EM_Team c on b.TeamId = c.TeamId) d on a.UserId = d.UserId where a.IsDelete != 99";
        //    var db = Repository.QueryByTablde(sql);
        //    if (db == null || db.Rows.Count <= 0)
        //        return null;

        //    var list = await Task.Run(() =>
        //        ModelConvertHelper<UserRecord>.ConvertToModel(db)
        //    );

        //    var queryPageResult = new PagedResult<UserRecord>
        //    {
        //        PageIndex = paging.PageIndex,
        //        PageSize = paging.PageSize,
        //    };

        //    List<UserRecord> pageUser = new List<UserRecord>();
        //    for (int index = paging.PageIndex * paging.PageSize; index < (paging.PageIndex + 1) * paging.PageSize && index < list.Count; index++)
        //    {
        //        pageUser.Add(list[index]);
        //    }

        //    queryPageResult.SizeCount = list.Count;
        //    queryPageResult.Result = pageUser;

        //    return queryPageResult;
        //}

        ///// <summary>
        ///// 通过userId获取用户记录
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public async Task<UserRecord> GetRecordByUserId(Guid userId)
        //{
        //    var sql = "select a.UserId,a.RoleId,a.Mobile,a.Nickname,a.Real,a.Email,a.MaxAccess,a.CreateTime,a.UpdateTime,b.Name as RoleName,b.[Type] as RoleType,d.Name as TeamName,d.TeamId from EM_User a inner join EM_Role b on a.RoleId = b.RoleId inner join (select b.UserId, c.Name,c.TeamId from EM_Team_User b inner join EM_Team c on b.TeamId = c.TeamId) d on a.UserId = d.UserId where a.IsDelete != 99 and a.UserId = '" + userId + "'";
        //    var db = Repository.QueryByTablde(sql);
        //    if (db == null || db.Rows.Count <= 0)
        //        return null;

        //    var list = await Task.Run(() =>
        //        ModelConvertHelper<UserRecord>.ConvertToModel(db)
        //    );

        //    return list[0];
        //}

        ///// <summary>
        ///// 通过mobile获取用户记录
        ///// </summary>
        ///// <param name="mobile"></param>
        ///// <returns></returns>
        //public async Task<UserRecord> GetRecordByMobile(string mobile)
        //{
        //    var sql = "select a.UserId,a.RoleId,a.Mobile,a.Nickname,a.Real,a.Email,a.MaxAccess,a.CreateTime,a.UpdateTime,b.Name as RoleName,b.[Type] as RoleType,d.Name as TeamName,d.TeamId from EM_User a inner join EM_Role b on a.RoleId = b.RoleId inner join (select b.UserId, c.Name,c.TeamId from EM_Team_User b inner join EM_Team c on b.TeamId = c.TeamId) d on a.UserId = d.UserId where a.IsDelete != 99 and a.Mobile = '" + mobile + "'";
        //    var db = Repository.QueryByTablde(sql);
        //    if (db == null || db.Rows.Count <= 0)
        //        return null;

        //    var list = await Task.Run(() =>
        //        ModelConvertHelper<UserRecord>.ConvertToModel(db)
        //    );

        //    return list[0];
        //}

        /// <summary>
        /// 插入用户数据
        /// </summary>
        /// <param name="list">数据集</param>
        /// <returns></returns>
        public async Task<int> Insert(List<D_User> list)
        {
            return await Repository.AddUpdateAsync(list, new string[] { "UserId" }, null, new string[] { "CreateTime", "UpdateTime" });
        }

        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> Update(List<D_User> list)
        {
            return await Repository.AddUpdateAsync(list, new string[] { "UserId" }, new string[] { "RoleId", "Mobile", "Nickname", "Real", "Email", "MaxAccess", "UpdateTime" }, null);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> ChangePassWord(List<D_User> list)
        {
            return await Repository.AddUpdateAsync(list, new string[] { "UserId" }, new string[] { "Pwd", "Salt", "UpdateTime" }, null);
        }

        /// <summary>
        /// 删除用户数据(业务删除 IsDelete=99)
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<int> Delete(Guid[] list)
        {
            return await Repository.DeleteVirtualAsync(list, "UserId");
        }
    }
}
