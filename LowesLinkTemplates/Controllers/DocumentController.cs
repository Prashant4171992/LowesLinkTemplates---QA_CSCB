using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LowesLinkTemplates.Models;

namespace LowesLinkTemplates.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document Index Action Result
        public ActionResult Index(string id)
        {
            Stream s = GetContents.GetStream(id);
            string[] openExtensionsForBrowser = { "pdf", "jpeg", "jpg", "png", "gif", "ico" };
            string[] downloadExtensionsForBrowser = { "zip", "doc", "docx", "xls", "xlsx", "xlsm", "xlt" ,"ppt", "pptx", "avi", "flv", "wmv", "mov", "mp4", "3gp" };
            string relativePath = GetContents.urlDictProp[Convert.ToInt32(id)];
            string fileType = relativePath.Split('.')[1].ToLower();
            string fileContentType = "";
            string fileName = "";
            if (s != null)
            {
                //returning file properties, which can't be view in the browser and should get downloaded
                if (downloadExtensionsForBrowser.Any(fileType.Contains))
                {
                    int idx = relativePath.LastIndexOf('/');
                    if (idx != -1)
                    {
                        //getting file name from relative path of respective file
                        fileName = relativePath.Substring(idx + 1);
                    }
                    //getting file content type
                    fileContentType = getFileContentType(fileType);                    
                    return File(s, fileContentType, fileName);
                }
                //returning file properties, which can be view in the browsers
                else
                {
                    //getting file content type
                    fileContentType = getFileContentType(fileType);
                    return File(s, fileContentType);
                }
            }
            //when the stream is null
            return Content("Document Not Found !!");
        }

        /// <summary>
        /// Get Content types for different document types e.g., docs,pdf,jpg,pptx,xlsx,zip etc.
        /// </summary>
        public string getFileContentType(string fileType)
        {
            string[] openExtensionsForBrowser = { "pdf", "jpeg", "jpg", "png", "gif", "ico" };
            string[] downloadDocsExtensionsForBrowser = { "zip", "doc", "docx", "xls", "xlsx", "xlsm", "xlt" ,"ppt", "pptx" };
            string[] downloadVdosExtForBrowser = { "avi", "flv", "wmv", "mov", "mp4", "3gp" };
            string contentType = "";
            if (openExtensionsForBrowser.Any(fileType.Contains))
            {
                //return content type for PDFs
                if(fileType.Contains("pdf"))
                {
                    contentType = "application/" + fileType;
                }
                //return content type for Images
                else
                {
                    contentType = "image/" + fileType;
                }                
            }
            //return content type for office documents & zip files
            if (downloadDocsExtensionsForBrowser.Any(fileType.Contains))
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }

            if (downloadVdosExtForBrowser.Any(fileType.Contains))
            {
                if(fileType.ToLower() == "flv")
                {
                    contentType = "video/x-flv";
                }
                if (fileType.ToLower() == "mp4")
                {
                    contentType = "video/mp4";
                }
                if (fileType.ToLower() == "3gp")
                {
                    contentType = "video/3gpp";
                }
                if (fileType.ToLower() == "mov")
                {
                    contentType = "video/quicktime";
                }
                if (fileType.ToLower() == "avi")
                {
                    contentType = "video/x-msvideo";
                }
                if (fileType.ToLower() == "wmv")
                {
                    contentType = "video/x-ms-wmv";
                }
            }
            //return respective file content type
            return contentType;
        }
    }
}