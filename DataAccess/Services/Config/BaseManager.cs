using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Dapper;
using Dapper.Contrib.Extensions;
using DataAccess.Helpers;
using System.Threading;

namespace DataAccess.Services
{
    public class BaseManager<T> : IDisposable where T : class
    {
        protected readonly IDbConnection con = new SqlConnection(ConfigurationManager.AppSettings["localhost"]);

        public void Dispose()
        {
            con.Close();
            con.Dispose();
        }
        public T Get(string id, bool nullable = false)
        {
            try
            {
                T result = con.Get<T>(id);
                if (result == null && !nullable) throw new ArgumentException("ID '" + id + "' could'nt be found");

                return result;
            }
            catch { throw; }
        }
        public List<T> GetAll()
        {
            try
            {
                string sql = "select * from " + typeof(T).Name + "s where deletedAt is null";
                return con.Query<T>(sql, buffered: false).ToList();
            }
            catch { throw; }
        }
        public T Save(T entity)
        {
            try
            {
                if (entity.GetType().GetProperty("id")?.GetValue(entity) == null)
                {
                    entity = Insert(entity);
                }
                else
                {
                    entity = Update(entity);
                }
                return entity;
            }
            catch { throw; }
        }
        public T Insert(T entity)
        {
            try
            {
                string id = DateTime.Now.Ticks.ToString();
                FileHelper.UploadFileInfo fileInfo = null;
                entity.GetType().GetProperty("id")?.SetValue(entity, id);
                entity.GetType().GetProperty("createdAt")?.SetValue(entity, DateTime.Now);
                entity.GetType().GetProperty("updatedAt")?.SetValue(entity, null);
                entity.GetType().GetProperty("deletedAt")?.SetValue(entity, null);
                if (entity.GetType().GetProperty("file")?.GetValue(entity) != null)
                {
                    fileInfo = FileHelper.setFilePath(
                        (IFormFile)entity.GetType().GetProperty("file")?.GetValue(entity),
                        (string)entity.GetType().GetProperty("filePath")?.GetValue(entity),
                        (string)entity.GetType().GetProperty("id")?.GetValue(entity)
                    );
                    entity.GetType().GetProperty("filePath").SetValue(entity, fileInfo.filePath);
                }
                con.Insert(entity);
                if (fileInfo != null) FileHelper.uploadFileAsync(fileInfo);
                return entity;
            }
            catch { throw; }
        }
        public List<T> BulkInsert(List<T> entity)
        {
            try
            {
                List<FileHelper.UploadFileInfo> fileInfo = new List<FileHelper.UploadFileInfo>();
                entity.ForEach(x =>
                {
                    string id = DateTime.Now.Ticks.ToString();
                    x.GetType().GetProperty("id")?.SetValue(x, id);
                    x.GetType().GetProperty("createdAt")?.SetValue(x, DateTime.Now);
                    x.GetType().GetProperty("updatedAt")?.SetValue(x, null);
                    x.GetType().GetProperty("deletedAt")?.SetValue(x, null);
                    if (x.GetType().GetProperty("file")?.GetValue(x) != null)
                    {
                        var info = FileHelper.setFilePath(
                            (IFormFile)x.GetType().GetProperty("file")?.GetValue(x),
                            (string)x.GetType().GetProperty("filePath")?.GetValue(x),
                            (string)x.GetType().GetProperty("id")?.GetValue(x)
                        );
                        fileInfo.Add(info);
                        x.GetType().GetProperty("filePath").SetValue(x, info.filePath);
                    }
                });
                con.Insert(entity);
                fileInfo.ForEach(x =>
                    {
                        FileHelper.uploadFileAsync(x);
                    }
                );
                return entity;
            }
            catch { throw; }
        }
        public List<T> ExcelBulkInsert(IFormFile file, CancellationToken cancellationToken)
        {
            List<T> data = FileHelper.uploadExcelAsync<T>(file, cancellationToken);
            return BulkInsert(data);
        }
        public T Update(T entity)
        {
            try
            {
                string id = (string)entity.GetType().GetProperty("id")?.GetValue(entity);
                FileHelper.UploadFileInfo fileInfo = null;
                T data = Get(id);
                foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType == typeof(IFormFile) || propertyInfo.Name == "createdAt")
                    {
                        continue;
                    }
                    var source = entity.GetType().GetProperty(propertyInfo.Name)?.GetValue(entity);
                    var target = propertyInfo.GetValue(data);
                    if (source != null && (source != target))
                    {
                        propertyInfo?.SetValue(data, source);
                    }
                }
                data.GetType().GetProperty("updatedAt")?.SetValue(data, DateTime.Now);
                data.GetType().GetProperty("deletedAt")?.SetValue(data, null);
                if (entity.GetType().GetProperty("file")?.GetValue(entity) != null)
                {
                    fileInfo = FileHelper.setFilePath(
                        (IFormFile)entity.GetType().GetProperty("file")?.GetValue(entity),
                        (string)entity.GetType().GetProperty("filePath")?.GetValue(entity),
                        (string)entity.GetType().GetProperty("id")?.GetValue(entity)
                    );
                    data.GetType().GetProperty("filePath").SetValue(data, fileInfo.filePath);
                }
                con.Update(data);
                if (fileInfo != null) FileHelper.uploadFileAsync(fileInfo);
                return data;
            }
            catch { throw; }
        }
        public bool Delete(string id)
        {
            try
            {
                var entity = Get(id);
                entity.GetType().GetProperty("deletedAt")?.SetValue(entity, DateTime.Now);
                return con.Update(entity);
            }
            catch { throw; }
        }
    }
}
