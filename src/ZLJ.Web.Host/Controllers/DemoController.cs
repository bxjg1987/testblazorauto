using BXJG.Utils.Web.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZLJ.Controllers;

namespace ZLJ.Web.Host.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]/[action]")]
    public class DemoController : ZLJControllerBase
    {
        private readonly UIEventTrigger uIEventTrigger;

        public DemoController(UIEventTrigger uIEventTrigger)
        {
            this.uIEventTrigger = uIEventTrigger;
        }

        //[HttpPost]
        //public async Task TriggerUIEvent(TestEventUi input)
        //{
        //    await uIEventTrigger.Trigger(input.EventName, input.Param, input.Scope, input.ScopeId);
        //}
        [HttpPost]
        public async Task TriggerGetAllApplication()
        {
            await uIEventTrigger.TriggerApplication(BXJG.Utils.Application.Share.Consts.ETGetAll);
        }
        [HttpPost]
        public async Task TriggerGetAllTenant()
        {
            await uIEventTrigger.TriggerTenant(BXJG.Utils.Application.Share.Consts.ETGetAll);
        }
        [HttpPost]
        public async Task TriggerGetAllUser()
        {
            await uIEventTrigger.TriggerUser(BXJG.Utils.Application.Share.Consts.ETGetAll);
        }
    }

    //public class TestEventUi
    //{
    //    public string EventName { get; set; }
    //    public object Param { get; set; }
    //    public int Scope { get; set; }
    //    public long? ScopeId { get; set; }
    //}
}
