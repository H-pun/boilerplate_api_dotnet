using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Configuration;
using System.Text;
using DataAccess.Services;
using DataAccess.Entities;
using WebAPI.Models;
using WebAPI.Helpers;
using Common.Helpers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController<User>
    {
        [HttpGet("getuserbytoken")]
        public ApiResponseModel GetUserByToken(string token)
        {
            return InvokeServiceMethod("GetUserByToken", new object[] { token });
        }
        [HttpPost("login")]
        public ApiResponseModel Login(User user)
        {
            try
            {
                User objUser = new UserService().GetByEmail(user.email);
                if (!PasswordHelper.VerifyHashedPassword(objUser.password, user.password))
                {
                    return apiResponse.Failed("Wrong password!");
                }
                else
                {
                    objUser.appToken = JwtHelper.CreateToken(new Claim[]{
                        new Claim(ClaimTypes.Name, objUser.email),
                        new Claim(ClaimTypes.Role, objUser.idRole.ToString()),
                    }, DateTime.UtcNow.AddDays(7));
                    new UserService().Update(objUser);
                    return apiResponse.Success(new UserService().Login(user.email));
                }
            }
            catch (Exception e)
            {
                return apiResponse.Failed(e.Message);
            }
        }
        [HttpPost("registeradmin")]
        public ApiResponseModel RegisterAdmin(AdminModel adminModel)
        {
            try
            {
                if (adminModel.username == null || adminModel.password == null)
                {
                    return apiResponse.Failed("Make sure required fields are not empty!");
                }
                if (adminModel.password.Length < 8 && adminModel.password != null)
                {
                    return apiResponse.Failed("Password minimum 8 character!");
                }
                User user = new User
                {
                    id = DateTime.Now.Ticks.ToString(),
                    idRole = 0,
                    email = adminModel.username + "@ischool.sch",
                    password = PasswordHelper.HashPassword(adminModel.password),
                    createdAt = DateTime.Now
                };
                Admin admin = new Admin
                {
                    id = DateTime.Now.Ticks.ToString(),
                    idUser = user.id,
                    name = adminModel.name,
                    createdAt = DateTime.Now
                };
                if (new UserService().GetByEmail(user.email, true) != null)
                {
                    return apiResponse.Failed("Email already registered!");
                }
                new UserService().RegisterAdmin(user, admin);
                return apiResponse.Success("Success");
            }
            catch (Exception e)
            {
                return apiResponse.Failed(e.Message);
            }
        }
        [HttpGet("getall")]
        public ApiResponseModel GetAll()
        {
            return InvokeServiceMethod("GetAll");
        }
        [HttpGet("getbyid")]
        public ApiResponseModel GetById(string id)
        {
            return InvokeServiceMethod("Get", new object[] { id, Type.Missing });
        }
        [HttpPost("delete")]
        public ApiResponseModel Delete(string id)
        {
            return InvokeServiceMethod("Delete", new object[] { id });
        }
    }
}