using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Web.Controllers;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    [Area(WebConstants.IdentityAreaName)]
    [Authorize]
    public abstract class BaseIdentityController : BaseController
    {
    }
}