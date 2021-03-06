﻿using Microsoft.AspNetCore.Mvc;
using RedditClone.Common.Constants;
using RedditClone.Models.WebModels.SubredditModels.BindingModels;
using RedditClone.Services.UserServices.Interfaces;
using System.Threading.Tasks;

namespace RedditClone.Web.Areas.Identity.Controllers
{
    public class SubredditController : BaseIdentityController
    {
        private readonly IUserSubredditService userSubredditService;

        public SubredditController(IUserSubredditService userSubredditService)
        {
            this.userSubredditService = userSubredditService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SubredditCreationBindingModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubredditCreationBindingModel model)
        {
            if (ModelState.IsValid == false)
            {
                this.AddStatusMessage(ModelState);
                return this.Redirect("/");
            }

            var result = await this.userSubredditService.CreateSubredditAsync(model, this.User);

            if (result == false)
            {
                this.AddStatusMessage(AlertConstants.ErrorMessageSubredditNameTaken, AlertConstants.AlertTypeDanger);
                return this.RedirectToAction("Create");
            }

            this.AddStatusMessage(AlertConstants.MessageSubredditCreated, AlertConstants.AlertTypeSuccess);
            return this.Redirect("/");
        }
    }
}