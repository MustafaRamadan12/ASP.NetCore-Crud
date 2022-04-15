using System;
using System.Threading.Tasks;
using Demo.BL.Helper;
using Demo.BL.Models;
using Demo.DAL.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    
    public class AccountController : Controller 
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        #region Index
        public IActionResult Index()
        {
            var users = userManager.Users;
            return View(users);
        }
        #endregion 

        #region Details
        public IActionResult Details()
        { 
            return View();
        }

        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var data = await userManager.FindByIdAsync(id);


            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            user.UserName = model.UserName;

            var result = await userManager.UpdateAsync(user);

            try
            {
                if (ModelState.IsValid)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }


                }
                return View(model);

            }
            catch (Exception ex)
            {
                return View(model);
            }

        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string id)
        {
            var data = await userManager.FindByIdAsync(id);
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            var result = await userManager.DeleteAsync(user);

            try
            {
                if (ModelState.IsValid)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }


                }
                return View(model);

            }
            catch (Exception ex)
            {
                return View(model);
            }

        }
        #endregion

        #region Sign Up
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM model)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var user = new ApplicationUser() 
                    {
                        Email = model.Email,
                        IsAgree = model.IsAgree,
                        UserName = model.Email
                    };

                    var Result = await userManager.CreateAsync(user , model.Password);
            
                    if(Result.Succeeded)
                    {
                        return RedirectToAction("Login","Account");
                    }
                    else
                    {
                        foreach (var item in Result.Errors)
                        {
                            ModelState.AddModelError("",item.Description);
                        }
                    }
                }   
                return View(model);
            }
            catch(Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var user = await userManager.FindByEmailAsync(model.Email);
                    var Result = await signInManager.PasswordSignInAsync(user,model.Password,model.RememberMe,false);
                    if(Result.Succeeded)
                    {
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                            ModelState.AddModelError("","Invalid User name or password");
                       
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        #region LogOut
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        #endregion

        #region ForgetPassowrd
        public IActionResult ForgetPassowrd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassowrd(ForgetPasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                       var Token = await userManager.GeneratePasswordResetTokenAsync(user);
                        var PasswordResetLink = Url.Action("ResetPassword", "Account", new { Email=model.Email , Token=Token });
                        MailSender.SendMail(new MailVM() { Mail = model.Email , Title="Reset password" , Message="Click lik to reset"});
                        return RedirectToAction("ConfirmForgetPassword");
                    }
                   
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        #region ConfirmForgetPassowrd
        public IActionResult ConfirmForgetPassowrd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmForgetPassowrd(SignUpVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        #region ResetPassowrd
        public IActionResult ResetPassowrd( string Email , string Token )
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassowrd(ResetPasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var Result = await userManager.ResetPasswordAsync(user,model.Token,model.Password);

                        if (Result.Succeeded)
                        {
                            return RedirectToAction("ConfirmResetPassword");
                        }
                        foreach (var item in Result.Errors)
                        {
                            ModelState.AddModelError("",item.Description);
                        }   
                    }
                    
                   
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        #region ConfirmResetPassowrd
        public IActionResult ConfirmResetPassowrd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmResetPassowrd(SignUpVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(model);

            }
        }
        #endregion

        

    }



}
