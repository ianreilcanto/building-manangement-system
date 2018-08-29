using MSD.SlattoFS.Attributes;
using MSD.SlattoFS.Controllers.Base;
using MSD.SlattoFS.Interfaces.Repositories;
using MSD.SlattoFS.Repositories;
using MSD.SlattoFS.Models.Pocos;
using System;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Models;
using MSD.SlattoFS.Web.Models;
using MSD.SlattoFS.Helpers;
using MSD.SlattoFS.Shared;

namespace MSD.SlattoFS.Controllers
{
    public class BMAccountController : SurfaceRenderMvcController
    {
        private readonly IPocoRepository<Account> _accountRepo;
        public BMAccountController()
        {
            _accountRepo = new AccountRepository();
        }

        [CMSAuthorizedMember]
        [HttpPost]
        public ActionResult BMAccountEditPage(RenderModel model)
        {
            var accountModel = new AccountViewModel(CurrentPage);
            return CurrentTemplate(accountModel);
        }

        [CMSAuthorizedMember]
        public ActionResult BMAccountPage(RenderModel model)
        {
            return CurrentTemplate(model);
        }

        [CMSAuthorizedMember]
        public ActionResult BMProfile(RenderModel model)
        {
            var accountRepo = new AccountRepository();
            var editorId = this.Members.GetCurrentMemberId();
            var accountViewModel = new AccountViewModel(CurrentPage);

            try
            {
                //check if property 'account' was set
                if (model.Content.IsPropertyValid(Constants.ACCOUNT_PROPERTY_ALIAS))
                {                   
                    int acctId = -1;
                    Int32.TryParse(model.Content.GetValidPropertyValue(Constants.ACCOUNT_PROPERTY_ALIAS).ToString(), out acctId);
                    var account = accountRepo.GetById(acctId);

                    if (account != null)
                    {
                        //prep view model for account
                        accountViewModel.Id = acctId;
                        accountViewModel.Name = account.Name;
                        accountViewModel.Email = account.Email;
                        accountViewModel.ImageId = account.ImageId;
                        accountViewModel.LastModifiedOn = DateTime.Now;
                        accountViewModel.CreatedOn = account.CreatedOn;
                        accountViewModel.CreatedBy = account.CreatedBy;
                        accountViewModel.ModifiedBy = editorId;

                        return CurrentTemplate(accountViewModel);
                    }
                }                
            }
            catch (Exception) { }

            return CurrentUmbracoPage();
        }

        [HttpPost]
        public ActionResult BMAccountLogin([Bind(Prefix = "loginModel")] LoginModel model)
        {
            if (ModelState.IsValid == false)
            {
                return CurrentUmbracoPage();
            }

            if (Members.Login(model.Username, model.Password) == false)
            {
                //don't add a field level error, just model level
                ModelState.AddModelError("loginModel", "Invalid username or password");
                return CurrentUmbracoPage();
            }

            TempData["LoginSuccess"] = true;

            //if there is a specified path to redirect to then use it
            if (!string.IsNullOrWhiteSpace(model.RedirectUrl))
            {
                return Redirect(model.RedirectUrl);
            }

            return RedirectToCurrentUmbracoUrl();
        }

        [HttpPost]
        public ActionResult BMAccountLogout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult UpdateAccount(Account account)
        {
            _accountRepo.Update(account.Id, account);
            return CurrentUmbracoPage();
        }
    }
}