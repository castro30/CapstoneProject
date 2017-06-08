﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CapstoneProject.Models;
using CapstoneProject.Models.MyFamilyDbModel;
using CapstoneProject.Controllers;

namespace AccountController.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController(IDbModel dbmodel)
            : this(dbmodel, new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        private AccountController(IDbModel dbmodel, UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            this.dbmodel = dbmodel;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        private IDbModel dbmodel { get; set; }


        #region Login
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Register
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //// GET: /Account/ConfirmEmail
        /* [AllowAnonymous]
         public async Task<ActionResult> ConfirmEmail(string Token, string Email)
         {
             ApplicationUser user = this.UserManager.FindById(Token);
             if (user != null)
             {
                 if (user.Email == Email)
                 {
                     user.ConfirmedEmail = true;
                     await UserManager.UpdateAsync(user);
                     await SignInAsync(user, isPersistent: false);
                     return RedirectToAction("Index", "Home", new { ConfirmedEmail = user.Email });
                 }
                 else
                 {
                     return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                 }
             }
             else
             {
                 return RedirectToAction("Confirm", "Account", new { Email = "" });
             }
         }*/

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);
                    //var person = new RegisterViewModel();

                    var person = new Person()
                    {
                        PersonName = model.PersonName,
                        ExpDate = model.ExpDate
                    };

                    var registeredUsers = new tbl_RegisteredUsers()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        BirthDate = model.BirthDate,
                        MarriedDate = model.MarriedDate,
                        CurrentLocation = model.CurrentLocation
                    };
                    /*System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                    new System.Net.Mail.MailAddress("sender@mydomain.com", "Web Registration"),
                    new System.Net.Mail.MailAddress(user.Email));
                    m.Subject = "Email confirmation";
                    m.Body = string.Format("Dear {0}" +
                        "< BR /> Thank you for your registration, " +
                        "please click on the below link to complete your " +
                        "registration: < a href =\"{1}\"title =\"User Email Confirm\">{1}</a>",
                       user.UserName, Url.Action("ConfirmEmail", "Account",
                       new { Token = user.Id, Email = user.Email }, Request.Url.Scheme)) ;
                    m.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mydomain.com");
                    smtp.Credentials = new System.Net.NetworkCredential("sender@mydomain.com", "password");
                    smtp.EnableSsl = true;
                    smtp.Send(m);*/

                    dbmodel.AddPerson(person, registeredUsers);

                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        /* public ActionResult RegisterFamilyMember()
         {
             return View();
         }

         // POST: /Account/Register a Family Member
         [HttpPost]
         [AllowAnonymous]
         [ValidateAntiForgeryToken]
         public async Task<ActionResult> RegisterFamily(RegisterFamilyViewModel model)
         {
             if (ModelState.IsValid)
             {
                 v

             }*/



        #endregion

        #region Disassociate
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }
        #endregion

        #region Manage
        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion



        #region Profile

        public ActionResult Profile(int? id)
        {
            if (id.HasValue)
            {
                var person = dbmodel.PersonProfile(id.Value);
                return View(person);
            }
            else
            {
                var user = User.Identity.GetUserName();
                var getRegUser = dbmodel.GetRegisterUser(user);
                //var people = dbmodel.GetImmediateFamily(User.Identity.GetUserName());
                var person = dbmodel.GetImmediateFamily(getRegUser.PersonID);
                return View(person);
            }
        }
        public ActionResult RegisteredProfile(RegisterViewModel model)
        {
            var registereduser = dbmodel.RegisteredProfile(User.Identity.GetUserName());
            return View(registereduser);
        }



        public ActionResult EditProfile()
        {
            var person = dbmodel.RegisteredProfile(User.Identity.GetUserName());
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(RegisterViewModel personmodel)
        {
            if (ModelState.IsValid)
            {

                RegisterViewModel LoggedinPerson = dbmodel.RegisteredProfile(User.Identity.GetUserName());
                //Person person = personmodel.AsPerson(LoggedinPerson.PersonID, LoggedinPerson.Username);
                dbmodel.EditPerson(personmodel);
                return RedirectToAction("Person");
            }
            return View(personmodel);
        }
        #endregion
        /* public ActionResult ImmediateFamily(SP_PullingRelatives_Result person, RegisterViewModel personmodel)
         {
          var yourid = dbmodel.GetI
             return View(family);
         }*/

        public ActionResult AddFamilyMember(int? id)
        {
            if (id.HasValue)
            {
                //var people = dbmodel.GetImmediateFamily(User.Identity.GetUserName());
                var people = dbmodel.GetImmediateFamily(id.Value);
                return View(people);
            }
            else
            {
                var user = User.Identity.GetUserName();
                var getRegUser = dbmodel.GetRegisterUser(user);
                //var people = dbmodel.GetImmediateFamily(User.Identity.GetUserName());
                var people = dbmodel.GetImmediateFamily(getRegUser.PersonID);
                return View(people);
            }
        }
        /* public ActionResult ImmediateFamilyDetails(RegisterViewModel model)
         {
             var person = dbmodel.RegisteredProfile(User.Identity.GetUserName());
                 return View(person);
         }*/


        public ActionResult CreateFamilyMember()
        {
            FamilyPersonModel model = new FamilyPersonModel
            {
                People = dbmodel.GetAllPeople().Select(x => new SelectListItem
                {
                    Text = x.PersonName,
                    Value = x.PersonID.ToString()
                }).ToList(),

                Relation = dbmodel.GetRelations().Select(x => new SelectListItem
                {
                    Text = x.Relationship,
                    Value = x.RID.ToString()
                }).ToList()
            };

            return View(model);
        }

        // POST: /Account/New Imediate Problem

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFamilyMember(FamilyMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
               /* var person = new Person()
                {
                    PersonName = model.PersonName,
                    ExpDate = model.ExpDate,

                };*/
                var family = new Family()
                {
                    PersonID = dbmodel.GetCurrentUserId(User.Identity.Name),
                    SecondPersonID = model.PersonId,
                    RID = model.RID,
                    ID = dbmodel.GetNextFamilyId(),
                };

                dbmodel.AddPersonToFamily(null, family);

                //Mail.SendEmail("charwell1234@gmail.com", "Hello", "Body!!!!!");

                return RedirectToAction("AddFamilyMember");
            }
            return View(model);
        }

        #region ExternalLogin
        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }
        #endregion

        #region LinkLogin
        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }
        #endregion

        #region ExternalLoginConfirmation
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        #endregion

        #region LogOff
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region ExternalLoginFailure
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion
        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;

            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}