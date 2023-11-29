using Flex.Core.Reflection;
using Flex.Core.Timing;
using Flex.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flex.Web.Areas.System.Controllers.APIController
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleUrlController : ApiBaseController
    {
        private IUnitOfWork _unitOfWork;
        private IdWorker _idWorker;
        public RoleUrlController(IUnitOfWork unitOfWork, IdWorker idWorker)
        {
            _unitOfWork = unitOfWork;
            _idWorker = idWorker;
        }
        [HttpGet("ChangeId")]
        public async Task<string> ChangeId()
        {
            var menulist = await _unitOfWork.GetRepository<SysMenu>().GetAllAsync();
            var menulists = new List<SysMenu>();
            foreach (var item in menulist)
            {
                menulists.Add(item);
            }
            try
            {
                using (var tran=_unitOfWork.BeginTransaction())
                {
                    _unitOfWork.GetRepository<SysMenu>().Update(menulists);
                    await _unitOfWork.SaveChangesAsync();
                    await tran.CommitAsync();
                }
                return Success("共修改" + menulists.Count + "条数据");
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
        /// <summary>
        /// 遍历所有接口地址并存入数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> InitRoleUrl()
        {
            List<ReflectMenuModel> data = ReflectionUrl.GetALLMenuByReflection();
            var menuapis = new List<SysRoleUrl>();
            DateTime dateTime = DateTime.Now;
            data.ForEach(item =>
            {
                item.ActionList.ForEach(items =>
                {
                    SysRoleUrl menuApi = new SysRoleUrl();
                    //menuApi.Id = _idWorker.NextId();
                    menuApi.Url = item.MenuLink + items.ActionCode;
                    menuApi.Name = items.ActionName;
                    menuApi.Description = items.ActionDesc;
                    menuApi.MaxErrorCount = 10;
                    menuApi.ReturnContent = "";
                    menuApi.NeedActionPermission = items.ActionPermission;
                    menuApi.RequestType = items.ActionType;
                    menuApi.Category = items.ActionId;
                    menuApi.OrderId = 1;
                    menuApi.AddUserName = "system";
                    menuApi.AddTime = Clock.Now;
                    menuApi.LastEditDate = Clock.Now;
                    menuApi.LastEditUserName = "system";
                    if (!menuapis.Contains(menuApi))
                        menuapis.Add(menuApi);
                });
            });
            try
            {
                if (Consts.Mode ==ProductMode.Dev)
                {
                    _unitOfWork.ExecuteSqlCommand("delete from tbl_core_roleurl");
                }
                await _unitOfWork.GetRepository<SysRoleUrl>().InsertAsync(menuapis);
                await _unitOfWork.SaveChangesAsync();
                return Success("共添加" + menuapis.Count + "条数据");
            }
            catch (Exception ex)
            {
                return Fail(ex.Message);
            }
        }
    }
}
