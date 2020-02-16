using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Reflection;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using DataAccess.Entities;
using DataAccess.Helpers;
using Common;

namespace WebAPI.Controllers
{
    public class BaseController<T> : Controller where T : class
    {
        protected readonly Type ServiceProvider;
        protected readonly object ServiceObject;
        protected readonly string WebAppUrl = ConfigurationManager.AppSettings["WebAppUrl"];
        protected ApiResponseModel apiResponse = new ApiResponseModel();
        protected BaseController()
        {
            try
            {
                ServiceProvider = Type.GetType("DataAccess.Services." + typeof(T).Name + "Service, DataAccess");
                ConstructorInfo ServiceConstructor = ServiceProvider.GetConstructor(Type.EmptyTypes);
                ServiceObject = ServiceConstructor.Invoke(new object[] { });
            }
            catch { throw; }
        }
        protected ApiResponseModel InvokeServiceMethod(string method, object[] param = default)
        {
            try
            {
                MethodInfo ServiceMethod = ServiceProvider.GetMethod(method);
                return apiResponse.Success(ServiceMethod.Invoke(ServiceObject, param));
            }
            catch (Exception ex)
            {
                return apiResponse.Failed(ex.InnerException.Message);
            }
        }
        protected FileStreamResult DownloadFile(FileHelper.DownloadInfo downloadInfo)
        {
            return File(downloadInfo.stream, downloadInfo.contentType, downloadInfo.fileName);
        }
        // [HttpGet("getall")]
        // public ApiResponseModel GetAll()
        // {
        //     return InvokeServiceMethod("GetAll");
        // }
        // [HttpGet("getbyid")]
        // public ApiResponseModel GetById(string id)
        // {
        //     return InvokeServiceMethod("Get", new object[]{id});
        // }        
        // [HttpPost("save")]
        // public ApiResponseModel Save(T entity)
        // {
        //     return InvokeServiceMethod("Save", new object[]{entity});
        // }        
        // [HttpPost("delete")]
        // public ApiResponseModel Save(string id)
        // {
        //     return InvokeServiceMethod("Delete", new object[]{id});
        // }        
    }
}
