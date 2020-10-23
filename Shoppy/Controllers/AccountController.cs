using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shoppy.DataAccess;
using Shoppy.ViewModels;
using Microsoft.AspNetCore.Http;
using Shoppy.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Shoppy.Controllers
{
    public class AccountController : Controller
    {

        readonly UserDbHandler _userDbHandler;

        public AccountController(IConfiguration configuration)
        {
            _userDbHandler = new UserDbHandler(configuration);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult isUsernameInUse(string username)
        {
            int check = _userDbHandler.checkUsername(username);

            if (check == 1)
            {
                return Json(true);
            }
            else
            {
                return Json($"Username {username} is already in use.");
            }
        }
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public IActionResult isEmailInUse(string email)
        {
            int check = _userDbHandler.checkEmail(email);

            if (check == 1)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                int checkusername = _userDbHandler.checkUsername(registerViewModel.Username);
                int checkemail = _userDbHandler.checkEmail(registerViewModel.Email);
                if (checkusername == 0)
                {
                    ModelState.AddModelError("", "Username already used");
                }
                if (checkemail == 0)
                {
                    ModelState.AddModelError("", "Email already used");
                    return View();
                }
                registerViewModel.Role = "client";
                int insertUser = _userDbHandler.insertUser(registerViewModel);
                if (insertUser == 1)
                {
                    int uid = _userDbHandler.getUid(registerViewModel.Username);
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetInt32("uid", uid);   
                    HttpContext.Session.SetString("username", registerViewModel.Username);
                    HttpContext.Session.SetString("role", registerViewModel.Role);
                    return RedirectToAction("index", "home");
                }
                else
                {
                    ModelState.AddModelError("", "Try Again");
                }
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginModelView loginModelView)
        {
            if (ModelState.IsValid)
            {
                int checkemail = _userDbHandler.checkEmail(loginModelView.Email);
                if (checkemail == 1)
                {
                    ModelState.AddModelError("", "Email not valid");
                    return View();
                }
                User user = _userDbHandler.login(loginModelView.Email, loginModelView.EncryptedPass);
                if (user.Username == null)
                {
                    ModelState.AddModelError("", "Incorrect Email or Password"+ loginModelView.EncryptedPass);
                    return View();
                }else 
                {
                    HttpContext.Session.Clear();
                    HttpContext.Session.SetInt32("uid", user.Uid);
                    HttpContext.Session.SetString("username", user.Username);
                    HttpContext.Session.SetString("role", user.Role);
                    if (loginModelView.RememberMe == true)
                    {
                        CookieOptions cookieOptions = new CookieOptions();
                        cookieOptions.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Append("email", loginModelView.Email, cookieOptions);
                        Response.Cookies.Append("password", loginModelView.EncryptedPass, cookieOptions);
                    }
                    return RedirectToAction("index", "home");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        { 
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("role");
            HttpContext.Session.Clear();
            Response.Cookies.Delete("email");
            Response.Cookies.Delete("password");
            return RedirectToAction("index", "home");
        }

    }
}
