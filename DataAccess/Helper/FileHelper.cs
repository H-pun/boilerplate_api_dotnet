using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DataAccess.Entities;
using System.Reflection;
using Common;
using DataAccess.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace DataAccess.Helpers
{    
    public class FileHelper
    {
        public class DownloadInfo
        {
            public MemoryStream stream { get; set; }
            public string contentType { get; set; }
            public string fileName { get; set; }
        }
        public class UploadFileInfo
        {
            public IFormFile file { get; set; }
            public string filePath { get; set; }
        }
        public static DownloadInfo DownloadExcel<T>(CancellationToken cancellationToken)
        {

            // query data from database  
            // List<SchoolClass> list = new SchoolClassService().GetAll();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;  
                workSheet.DefaultRowHeight = 12;  
                //Header of table  
                //  
                workSheet.Row(1).Height = 20;  
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;  
                // workSheet.Cells.LoadFromCollection(list, true);
                workSheet.Column(1).AutoFit();  
                workSheet.Column(2).AutoFit();  
                workSheet.Column(3).AutoFit();  
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                // ws.Cells["A3"].Style.Numberformat.Format = "yyyy-mm-dd";
                // ws.Cells["A3"].Formula = "=DATE(2014,10,5)";
                package.Save();
            }
            
            stream.Position = 0;
            //return File(stream, "application/octet-stream", excelName);
            return new DownloadInfo(){
                stream = stream,
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName = $"UserList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx"
            };
        }
        public static dynamic uploadExcelAsync<T>(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length <= 0)
            {
                return "No file chosen!";
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return "File extension not supported";
            }

            List<T> list = new List<T>();

            using (var stream = new MemoryStream())
            {
                file.CopyToAsync(stream, cancellationToken);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        T data = Activator.CreateInstance<T>();
                        foreach (PropertyInfo propertyInfo in data.GetType().GetProperties())
                        {
                            if (propertyInfo.Name == "idUser")
                            {
                                continue;
                            }
                            for (int col = 2; col <= colCount; col++)
                            {
                                string source = ((string)worksheet.Cells[1, col].Value);
                                string target = propertyInfo.Name;
                                if(source.Substring(0,2) != "id"){
                                    source = source.ToLower();
                                }
                                if (source == target)
                                {
                                    var value = worksheet.Cells[row, col].Value;
                                    if (value != null)
                                    {
                                        if (propertyInfo.PropertyType == typeof(DateTime))
                                        {
                                            value = DateTime.FromOADate((double)value);
                                        }
                                        else
                                        {
                                            Convert.ChangeType(value, propertyInfo.PropertyType);
                                        }
                                        propertyInfo?.SetValue(data, value);
                                    }
                                }
                            }
                        }
                        list.Add(data);
                    }
                }
            }
            return list;
        }
        public static UploadFileInfo setFilePath(IFormFile file, string path, string fileName){
            return new UploadFileInfo {
                file = file,
                filePath = file.SetFilePath(path, fileName)
            };
        }
        public static void uploadFileAsync(UploadFileInfo upload)
        {
            if (upload.file == null || upload.file.Length == 0) { return; }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload", upload.filePath);
            (new FileInfo(path)).Directory.Create();

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.file.CopyTo(stream);
            }
        }
    }
}