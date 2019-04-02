using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RedditClone.Common.Constants;

namespace RedditClone.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private int errorId = 0;

        protected void AddStatusMessage(string message, string type)
        {
            this.TempData[WebConstants.StatusMessagePrefix] = message;
            this.TempData[WebConstants.StatusMessageTypeKey] = type;
        }

        protected void AddStatusMessage(ModelStateDictionary modelState, string type = WebConstants.MessageTypeDanger)
        {
            this.TempData[WebConstants.StatusMessageTypeKey] = type;

            foreach (var value in modelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    this.TempData[WebConstants.StatusMessagePrefix + this.errorId] = error.ErrorMessage;
                    this.errorId++;
                }
            }
        }
    }
}