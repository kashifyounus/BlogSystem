﻿using AutoMapper;
using BlogSystem.Web.Infrastructure.Mapping;

namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;
    using Data.Models;
    using Data.UnitOfWork;

    public abstract class BaseController : Controller
    {
        protected BaseController(IBlogSystemData data)
        {
            this.Data = data;
        }
 
        protected BaseController(IBlogSystemData data, ApplicationUser userProfile) 
            : this(data)
        {
            this.UserProfile = userProfile;
        }

        protected IBlogSystemData Data { get; }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected ApplicationUser UserProfile { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var username = requestContext
                    .HttpContext
                    .User
                    .Identity
                    .GetUserName();

                var user = this.Data.Users
                    .All()
                    .FirstOrDefault(u => u.UserName == username);

                this.UserProfile = user;
                this.ViewBag.UserProfile = user;
            }

            return base.BeginExecute(requestContext, callback, state);
        }
    }
}