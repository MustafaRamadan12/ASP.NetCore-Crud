using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.BL.Models;
using Demo.DAL.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        #region Index
        public IActionResult Index()
        {
            var Roles = roleManager.Roles;
            return View(Roles);
        }
        #endregion

        #region Creat
        public IActionResult Creat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Creat(IdentityRole model)
        {
            var role = new IdentityRole()
            {
                Name = model.Name,
                NormalizedName = model.Name.ToUpper(),
            };
            var result = await roleManager.CreateAsync(role);
            try
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
                return View(role);
            }
            catch (Exception ex)
            {
                return View(model);
            }

        }
        #endregion

        #region Edit
        public async Task<IActionResult> Edit(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
            var result = await roleManager.UpdateAsync(role);
            try
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
                return View(role);
            }
            catch (Exception ex)
            {
                return View(model);
            }

        }
        #endregion

     
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            ViewBag.RoleId = RoleId;

            var role = await roleManager.FindByIdAsync(RoleId);

            var model = new List<UserInRoleVM>();

            foreach (var user in userManager.Users)
            {
                var UserInRole = new UserInRoleVM();
                UserInRole.UserId = user.Id;
                UserInRole.UserName = user.UserName;

                if (await userManager.IsInRoleAsync(user,role.Name))
                {
                    UserInRole.IsSelected = true;
                }
                else
                {
                    UserInRole.IsSelected = false;
                }
                model.Add(UserInRole);
            }
           

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleVM> model, string RoleId)
        {

            var role = await roleManager.FindByIdAsync(RoleId);

            for (int i = 0; i < model.Count; i++)
            {

                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {

                    result = await userManager.AddToRoleAsync(user, role.Name);

                }
                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                //else
                //{
                //    continue;
                //}

                //if (i < model.Count)
                //    continue;
            }

            return RedirectToAction("Edit", new { id = RoleId });
        }

    }
}
