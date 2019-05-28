using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Animals.Controllers
{
    public class BaseController : Controller
    {

        // alkalmazás szintű állapot
        protected readonly ApplicationState _applicationState;

        public BaseController(ApplicationState applicationState)
        {
            _applicationState = applicationState;
        }

        /// <summary>
        /// Egy akció meghívása után végrehajtandó metódus.
        /// </summary>
        /// <param name="context">Az akció kontextus argumentuma.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            
            ViewBag.UserCount = _applicationState.UserCount;
            ViewBag.CurrentGuestName = String.IsNullOrEmpty(User.Identity.Name) ? null : User.Identity.Name;
        }
    }
}