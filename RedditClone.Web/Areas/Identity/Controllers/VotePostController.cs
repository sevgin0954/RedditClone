using Microsoft.AspNetCore.Mvc;
using RedditClone.Data.Interfaces;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class VotePostController : BaseIdentityController
    {
        private readonly IRedditCloneUnitOfWork unitOfWork;
        private readonly IUserVotePostService userVoteService;

        public VotePostController(IRedditCloneUnitOfWork unitOfWork, IUserVotePostService userVoteService)
        {
            this.unitOfWork = unitOfWork;
            this.userVoteService = userVoteService;
        }

        [HttpPost]
        public async Task<IActionResult> DownVote(string postId)
        {
            var isSuccessful = await this.userVoteService.AddDownVoteToPostAsync(postId, this.User);
            if (isSuccessful)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpVote(string postId)
        {
            var isSuccessful = await this.userVoteService.AddUpVoteToPostAsync(postId, this.User);
            if (isSuccessful)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveDownVote(string postId)
        {
            var isSuccessful = await this.userVoteService.RemoveDownVoteToPostAsync(postId, this.User);
            if (isSuccessful)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUpVote(string postId)
        {
            var isSuccessful = await this.userVoteService.RemoveUpVoteToPostAsync(postId, this.User);
            if (isSuccessful)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest();
            }
        }
    }
}