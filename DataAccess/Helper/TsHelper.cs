using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace DataAccess.Helpers
{
    public class TsHelper
    {
        public static List<T> BulkInsert<T>(IDbConnection con, List<T> entity, IDbTransaction ts) where T : class
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
                con.Insert(entity, transaction: ts);
                fileInfo.ForEach(x =>
                    {
                        FileHelper.uploadFileAsync(x);
                    }
                );
                return entity;
            }
            catch { throw; }
        }

        public static List<T> BulkUpdate<T>(IDbConnection con, List<T> entity, IDbTransaction ts) where T : class
        {
            try
            {
                List<FileHelper.UploadFileInfo> fileInfo = new List<FileHelper.UploadFileInfo>();

                entity.ForEach(x =>
                {
                    string id = (string)x.GetType().GetProperty("id")?.GetValue(x);
                    T data = con.Get<T>(id, transaction: ts);
                    if (data == null) throw new ArgumentException("ID '" + id + "' could'nt be found");

                    foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                    {
                        if (propertyInfo.PropertyType == typeof(IFormFile) || propertyInfo.Name == "createdAt")
                        {
                            continue;
                        }
                        var source = x.GetType().GetProperty(propertyInfo.Name)?.GetValue(x);
                        var target = propertyInfo.GetValue(data);
                        if (source != null && (source != target))
                        {
                            propertyInfo?.SetValue(data, source);
                        }
                    }
                    x.GetType().GetProperty("updatedAt")?.SetValue(x, DateTime.Now);
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
                con.Update(entity, transaction: ts);
                fileInfo.ForEach(x =>
                    {
                        FileHelper.uploadFileAsync(x);
                    }
                );
                return entity;
            }
            catch { throw; }
        }

        public static T Insert<T>(IDbConnection con, T entity, IDbTransaction ts) where T : class
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
                con.Insert(entity, transaction: ts);
                if (fileInfo != null) FileHelper.uploadFileAsync(fileInfo);
                return entity;
            }
            catch { throw; }
        }
        public static T Update<T>(IDbConnection con, T entity, IDbTransaction ts) where T : class
        {
            try
            {
                string id = (string)entity.GetType().GetProperty("id")?.GetValue(entity);
                FileHelper.UploadFileInfo fileInfo = null;
                T data = con.Get<T>(id, transaction: ts);
                if (data == null) throw new ArgumentException("ID '" + id + "' could'nt be found");

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
                con.Update(data, transaction: ts);
                if (fileInfo != null) FileHelper.uploadFileAsync(fileInfo);
                return data;
            }
            catch { throw; }
        }
    }
}
