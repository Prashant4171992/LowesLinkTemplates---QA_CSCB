using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security;
using Microsoft.SharePoint.Client;
using System.Web.Configuration;
using LowesLinkTemplates.Models;
using System.IO;
using System.Configuration;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using OfficeDevPnP.Core;

namespace LowesLinkTemplates.Models
{
    public class GetContents : IDisposable
    {
        /// <summary>
        /// Creating and Initializing variables
        /// </summary>
        private static ClientContext ctx = null;
        private static string ListName => ConfigurationManager.AppSettings.Get("SPListName").ToString();        
        public static string[] docExtensions = { ".docx", ".doc", ".pdf", ".zip", ".csv", ".xml", ".jpeg", ".jpg", ".png", ".gif", ".ico", ".ppt", ".pptx", ".xls", ".xlsx", ".xlsm", ".xlt" , ".txt", ".avi", ".flv", ".wmv", ".mov", ".mp4", ".3gp" };
        public static string[] spPageExtensions = { "/sites/", "~site/" };
        public static string[] secureContentExtensions = { "ww3.loweslink", "secure.loweslink" };
        public static string[] appPageExtensions = { "/Home/Index/" };
        public static List<LLMain> requ = new List<LLMain>();        
        public static Dictionary<string, string> urlDictProp = new Dictionary<string, string>();
        public static string lastKey = null;
        /// <summary>
        /// Get All Page's Content using LoadLowesLinkContent function
        /// </summary>
        public List<LLMain> ContentList => LoadLowesLinkContent();
        /// <summary>
        /// Return All Page's Content
        /// </summary>
        public static List<LLMain> LoadLowesLinkContent()
        {
            lastKey = null;
            LLMainErr Model = new LLMainErr();
            List<LLMain> contentListObj = new List<LLMain>();
            /// <summary>
            /// contentListObj List of type LLMain storing all the Wiki page's content and other metadata properties
            /// </summary>
            try
            {                
                using (ctx = GetContext())
                {
                    List olist = ctx.Web.Lists.GetByTitle(ListName);
                    CamlQuery qry = new CamlQuery();
                    qry.ViewXml = @"<View Scope='Recursive'>
                                     <Query>
                                     </Query>
                                </View>";

                    ListItemCollection listCol = olist.GetItems(qry);
                    ListItemCollection items = listCol;
                    ctx.Load(items, icol => icol.Include(i => i["WikiField"], i => i.File.Name));
                    ctx.ExecuteQuery();
                    urlDictProp.Clear();
                    /// <summary>
                    /// Loop through all Wiki pages
                    /// </summary>
                    foreach (ListItem item in items)
                    {
                        /// <summary>
                        /// object intialization of type LLMain, for storing each wiki page's content and other metadata properties
                        /// </summary>
                        LLMain llmainPropObj = new LLMain();
                        llmainPropObj.PageName = Convert.ToString(item.File.Name);
                        llmainPropObj.Content = Convert.ToString(item["WikiField"]);
                        /// <summary>
                        /// getting all the links from each wiki page's html content
                        /// </summary>
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(llmainPropObj.Content);
                        if (htmlDocument.DocumentNode.SelectNodes("//a") != null)
                        {
                            /// <summary>
                            /// Forming links for documents, internal page links and external page links
                            /// </summary>
                            llmainPropObj.Content = generateDocumentUrls(htmlDocument, llmainPropObj.Content);
                        }
                        /// <summary>
                        /// adding updated page's links and other content to List contentListObj of type LLMain 
                        /// </summary>
                        contentListObj.Add(llmainPropObj);
                    }
                }
            }
            catch (Exception ex)
            {
                Model.Error = ex.Message;
            }
            return contentListObj;
        }

        /// <summary>
        /// returns the custom generated links for documents, internal page links and external page links
        /// </summary>
        public static string generateDocumentUrls(HtmlAgilityPack.HtmlDocument htmlDoc, string content)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//a").ToArray();
            string result = content;
            foreach (HtmlNode nod in nodes)
            {
                if (nod != null)
                {
                    if (nod.Attributes["href"] != null)
                    {
                        var hrefVal = nod.Attributes["href"].Value.ToLower();
                        var fileName = "";
                        var fileExt = "";
                        var fullFileName = "";
                        /// <summary>
                        /// generate links for documents
                        /// </summary>
                        if (docExtensions.Any(hrefVal.Contains))
                        {
                            if (!secureContentExtensions.Any(hrefVal.Contains))
                            {                               
                                //Only File Name
                                fileName = System.IO.Path.GetFileNameWithoutExtension(hrefVal);
                                //Only File Extension
                                fileExt = System.IO.Path.GetExtension(hrefVal);
                                //Complete File Name
                                fullFileName = fileName + fileExt;
                                urlDictProp[fullFileName.ToString()] = hrefVal.ToString();
                                string oldHref = nod.Attributes["href"].Value;                                
                                string updatedHref = nod.Attributes["href"].Value = "/Document/Index/" + fileName + "?ext=" + fileExt.Split('.')[1];
                                result = result.Replace(oldHref, updatedHref);
                            }
                        }
                        /// <summary>
                        /// generate links for pages
                        /// </summary>
                        else if (!appPageExtensions.Any(hrefVal.Contains))
                        {
                            if (spPageExtensions.Any(hrefVal.Contains) && hrefVal.Contains(".aspx"))
                            {
                                string oldHref = nod.Attributes["href"].Value;
                                string updatedHref = "";
                                int idx = hrefVal.LastIndexOf('/');
                                if (idx != -1)
                                {
                                    updatedHref = nod.Attributes["href"].Value = "/Home/Index/" + hrefVal.Substring(idx + 1);
                                }
                                result = result.Replace(oldHref, updatedHref);
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get SharePoint site context
        /// </summary>
        public static ClientContext GetContext()
        {
            LLMainErr Model = new LLMainErr();
            ClientContext ctx = null;
            try
            {
                string url = WebConfigurationManager.AppSettings.Get("SPUrl");
                string appId = WebConfigurationManager.AppSettings.Get("AppId");
                string secretId = WebConfigurationManager.AppSettings.Get("AppSecret");
                //string userName = WebConfigurationManager.AppSettings.Get("SPUserName");
                //string pass = WebConfigurationManager.AppSettings.Get("SPPassword");
                //SecureString password = new SecureString();
                //foreach (char c in pass)
                //{
                //    password.AppendChar(c);
                //}
                AuthenticationManager authManager = new AuthenticationManager();
                ctx = authManager.GetAppOnlyAuthenticatedContext(url, appId, secretId);
                ctx.Load(ctx.Web);
                ctx.ExecuteQuery();
                Console.WriteLine(ctx.Web.Title);
                //ctx = new ClientContext(url);
                //ctx.Credentials = new SharePointOnlineCredentials(userName, password);
            }
            catch (Exception ex)
            {
                Model.Error = ex.Message;
            }
            return ctx;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GetContents() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
        /// <summary>
        /// returns the document stream using corresponding relative document path
        /// </summary>        
        public static Stream GetStream(string id, string ext)
        {
            LLMainErr Model = new LLMainErr();
            Stream fileStream = null;
            string fileName = "";
            //ClientResult<Stream> streamResult = null;
            try
            {
                using (ClientContext ctx = GetContext())
                {
                    List olist = ctx.Web.Lists.GetByTitle(ListName);
                    bool relativePathExist = urlDictProp.TryGetValue(Uri.EscapeDataString(id) + "." + ext, out string relativePath); //urlDictProp[id.ToString()];
                    if (relativePathExist)
                    {
                        Microsoft.SharePoint.Client.File file = ctx.Web.GetFileByServerRelativeUrl(relativePath);
                        ctx.Load(file);
                        ClientResult<Stream> streamX = file.OpenBinaryStream();
                        ctx.Load(file);
                        ctx.ExecuteQuery();
                        fileStream = streamX.Value;
                        fileStream.Seek(0, SeekOrigin.Begin);
                        fileName = file.Name;
                    }
                    else
                    {
                        fileStream = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Model.Error = ex.Message;
            }
            return fileStream;
        }
    }
}