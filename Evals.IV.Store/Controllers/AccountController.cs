using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Evals.IV.Store.Models;
using Evals.IV.Infrastructure.Identity;
using System.ComponentModel.Composition;

namespace Evals.IV.Store.Controllers
{
    [Export]
    [Authorize]
    public class AccountController : Controller
    {
        [Import]
        IUserRepository _repository { get; set; }

        [AllowAnonymous]
        public ActionResult RegisterUser()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterUser(Customer user)
        {
            if (ModelState.IsValid)
            {
                IdentityResult i = await _repository.CreateAsync(user, user.PasswordHash);
                if (i.Succeeded)
                {

                    return Redirect("Administration/home/index");
                }
                else
                {
                    foreach (var item in i.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                    return View(user);
                }

            }
            return View(user);
        }


        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(string UserName, string PasswordHash)
        {
            _repository.AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            var user = await _repository.FindAsync(UserName, PasswordHash);
            if (user != null)
            {
                await _repository.SignInAsync(user, false);
                return Redirect("/Administration/home/index");
            }
            else
            {
                ModelState.AddModelError("", "用户名或密码错误");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            _repository = null;
            base.Dispose(disposing);
        }

    }
}