using Mapster;

using CoreWebAPI.CoreDtos.McsAuth;

using CoreServiceLib.Bases;
using CoreServiceLib.Core.Attributes;
using CoreServiceLib.Models.UserAuth;
using CoreServiceLib.Paras;
using CoreServiceLib.Tools;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NLog;

namespace CoreWebAPI.Controllers
{
    /// <summary>
    /// 系统权限
    /// </summary>
    [McsApiGroup("系统权限")]
    public class McsAuthController : McsControlerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///  用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public McsWebApiResult<LoginResultDto> McsLogin(UserLoginPara user)
        {
            var result = new LoginResultDto();
            try
            {
                var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();
                var mUser = userService.AsQueryable().Where(a => a.UserID == user.UserID).First();
                if (mUser == null)
                {
                    result.CheckResult = false;
                    result.ErrorMessage = "用户不存在";
                    return SuccessResult(result);
                }

                if (mUser.Password != MD5Encrypt.Get(user.Password))
                {
                    result.CheckResult = false;
                    result.ErrorMessage = "用户密码错误";
                    return SuccessResult(result);
                }

                result.CheckResult = true;
                result.UserName=mUser.UserName;
                result.LoginToken = McsTokenTool.GetUserToken(user.UserID, 0, true);
                result.RoleId = mUser.AuthLevelId;
                
                return SuccessResult(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<LoginResultDto>();
            }
        }

        /// <summary>
        ///  获取菜单资源
        /// </summary>
        /// <param name="mRoleId"></param>
        /// <returns></returns>
        [HttpGet]
        public McsWebApiResult<AuthResDto[]> GetAuthResList(int mRoleId)
        {
            try
            {
                var datas = GetRoleRes(mRoleId);
                return SuccessResult(datas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<AuthResDto[]>();
            }
        }



        /// <summary>
        /// 用户密码修改
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult ChangeUserPassword(ChangePasswordPara para)
        {
            try
            {
                var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();
                var mUser = userService.AsQueryable().Where(a => a.UserID == para.UserID).First();
                if (mUser == null)
                {
                    return ErrorResult("用户不存在");
                }

                if (mUser.Password != MD5Encrypt.Get(para.OldPassword))
                {
                    return ErrorResult("用户密码错误");
                }

                if (para.OldPassword == para.NewPassword)
                {
                    return ErrorResult("旧密码与新密码一致，不允许修改");
                }

                if (para.NewPassword != para.ConfirmPassword)
                {
                    return ErrorResult("两次输入密码不一致");
                }

                mUser.Password = MD5Encrypt.Get(para.NewPassword);
                userService.Update(mUser);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult DeleteUser(string userId)
        {
            try
            {
                var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();
                userService.Delete(a => a.UserID == userId);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 用户获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public McsWebApiResult<List<UserDto>> GetUserList()
        {
            try
            {
                var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();
                var users = userService.AsQueryable().OrderBy(a => a.UserID).ToList().Adapt<List<UserDto>>();
                return SuccessResult(users);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<List<UserDto>>();
            }
        }

        /// <summary>
        ///  用户保存
        /// </summary>
        /// <param name="userpara"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult SaveUser(UserDto userpara)
        {
            try
            {
                var user = userpara.Adapt<SystemUser>();
                var userService = McsDbTool.GetDBRepositoryRef<SystemUser>();
                if (string.IsNullOrEmpty(user.UserID))
                {
                    return ErrorResult("用户编号不能为空");
                }
                var mUser = userService.AsQueryable().Where(a => a.UserID == user.UserID).First();
                if (mUser == null)
                {
                    user.Password = MD5Encrypt.Get("123456");
                    userService.Insert(user);
                }
                else
                {
                    mUser.UserName = user.UserName;
                    mUser.AuthLevelId = user.AuthLevelId;
                    mUser.Disabled = user.Disabled;
                    userService.Update(mUser);
                }
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 获取所有权限资源
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public McsWebApiResult<List<AuthResDto>> GetAllAuthRes()
        {
            try
            {
                var authService = McsDbTool.GetDBRepositoryRef<AuthResource>();
                var resList = authService.AsQueryable().OrderBy(a => a.OrderNo).ToTree(a => a.Children, a => a.ParentID, 0).Adapt<List<AuthResDto>>();
                return SuccessResult(resList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<List<AuthResDto>>();
            }
        }

        /// <summary>
        /// 删除菜单资源
        /// </summary>
        /// <param name="menuId">菜单编号</param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult DeleteMenu(long menuId)
        {
            try
            {
                var authService = McsDbTool.GetDBRepositoryRef<AuthResource>();
                authService.Delete(a => a.MenuID == menuId || a.ParentID == menuId);

                //删除角色中的资源
                var authSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
                var datas = authSerice.AsQueryable().ToList();
                foreach (var item in datas)
                {
                    var resList = item.Resources;
                    if (resList.Any(a => a.MenuID == menuId))
                    {
                        item.Resources = [.. resList.Where(a => a.MenuID != menuId)];
                        authSerice.Update(item);
                    }
                }
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 菜单保存
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult SaveMenu(List<AuthResDto> datas)
        {
            try
            {
                List<AuthResource> imps = [];
                var authService = McsDbTool.GetDBRepositoryRef<AuthResource>();
                foreach (var item in datas)
                {
                    imps.AddRange(GetMenuList(item));
                }
                authService.InsertOrUpdate(imps);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public McsWebApiResult<List<AuthLevelDto>> GetRoleList()
        {
            try
            {
                var authSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
                var datas = authSerice.AsQueryable().OrderBy(a => a.AuthLevelId).ToList().Adapt<List<AuthLevelDto>>();
                return SuccessResult(datas);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult<List<AuthLevelDto>>();
            }
        }

        /// <summary>
        /// 权限数据保存
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult SaveRole(List<AuthLevelDto> datas)
        {
            try
            {
                var authSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
                var d = datas.Adapt<List<AuthLevel>>();
                authSerice.InsertOrUpdate(d);
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        ///  删除角色
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        [HttpPost]
        public McsWebApiResult DeleteRole(int mid)
        {
            try
            {
                var authSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
                authSerice.AsDeleteable().Where(a => a.AuthLevelId == mid).ExecuteCommand();
                return SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return ErrorResult();
            }
        }

        /// <summary>
        /// 菜单数据转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static AuthResource[] GetMenuList(AuthResDto data)
        {
            List<AuthResource> datas = [];

            datas.Add(data.Adapt<AuthResource>());
            foreach (var item in data.Children)
            {
                datas.AddRange(GetMenuList(item));
            }
            return [.. datas];
        }

        /// <summary>
        /// 获取角色资源
        /// </summary>
        /// <param name="mRoleId">角色编号</param>
        /// <returns></returns>
        private static AuthResDto[] GetRoleRes(int mRoleId)
        {
            var authService = McsDbTool.GetDBRepositoryRef<AuthResource>();
            var authLeveSerice = McsDbTool.GetDBRepositoryRef<AuthLevel>();
            var mRoleRes = authLeveSerice.AsQueryable().Where(a => a.AuthLevelId == mRoleId).First()?.Resources;

            List<long> resIds = [];
            if (mRoleRes == null) return [];

            foreach (var item in mRoleRes)
            {
                resIds.Add(item.MenuID);
            }

            var datas = authService.AsQueryable()
                .Where(a => resIds.Contains(a.MenuID))
                .OrderBy(a => new { a.OrderNo, a.MenuID })
                .ToTree(a => a.Children, a => a.ParentID, 0);

            var resList = datas.Adapt<List<AuthResDto>>();
            return [.. resList];
        }
    }
}