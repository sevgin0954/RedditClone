using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    [Area(WebConstants.IdentityAreaName)]
    [Authorize]
    public abstract class BaseIdentityController : Controller
    {
    }
}