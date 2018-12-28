using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LowesLinkTemplates.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace LowesLinkTemplates.Controllers
{
    /// <summary>
    /// Home Page Controller for All the Pages
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Index Action Result for LowesLink Content Section
        /// </summary>        
        public ActionResult Index(string id)
        {
            try
            {
                List<LLMain> requ = new List<LLMain>();
                //getting all wikipages and respective content
                using (GetContents con = new GetContents())
                {
                    //when it's a new session 
                    if (System.Web.HttpContext.Current.Session.IsNewSession)
                    {
                        //remove existing session objects
                        System.Web.HttpContext.Current.Session.Contents.RemoveAll();
                        //saving all data into session object 'AllContent'
                        System.Web.HttpContext.Current.Session["AllContent"] = con.ContentList;
                        requ = getAllContent(id, requ, con.ContentList);
                    }
                    //when session already exists
                    else
                    {
                        //getting data back from session object 'AllContent'
                        var allContent = Session["AllContent"] as List<LLMain>;
                        requ = getAllContent(id, requ, allContent);
                    }
                    //requ = getAllContent(id, requ, con.ContentList);
                }
                // return content back to View
                return View(requ);
            }
            // catching exceptions
            catch (Exception ex)
            {
                var err = new LLMainErr();
                err.Error = ex.Message;
                return View(err.Error);
            }
        }
        /// <summary>
        /// constructing and structuring page's content based on Page name passed as query string parameter named 'id'
        /// </summary>
        public List<LLMain> getAllContent(string id, List<LLMain> requ, List<LLMain> allContent)
        {
            // Content structuring for all the pages except for Home Page
            if (id != null)
            {
                foreach (var i in allContent)
                {
                    if (i.PageName.ToLower() == id.ToLower())
                    {
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(i.Content);
                        if (System.Web.HttpContext.Current.Session.IsNewSession)
                        {
                            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='ms-rte-layoutszone-inner']");
                            string res = "";
                            foreach (HtmlNode item in htmlNodes)
                            {
                                res += item.InnerHtml;
                            }
                            i.Content = res;
                            requ.Add(i);
                        }
                        else
                        {
                            string res = "";
                            res = htmlDocument.DocumentNode.InnerHtml;
                            i.Content = res;
                            requ.Add(i);
                        }
                                                                        
                    }
                }
            }
            // Content structuring for Home Page
            if (id == null)
            {
                foreach (var i in allContent)
                {
                    if (i.PageName.ToLower() == "Home.aspx".ToLower())
                    {
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(i.Content);
                        if (System.Web.HttpContext.Current.Session.IsNewSession)
                        {
                            var htmlNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='ms-rte-layoutszone-inner']");
                            string res = "";
                            foreach (HtmlNode item in htmlNodes)
                            {
                                res += item.InnerHtml;
                            }
                            i.Content = res;
                            requ.Add(i);
                        }
                        else
                        {
                            string res = "";
                            res = htmlDocument.DocumentNode.InnerHtml;
                            i.Content = res;
                            requ.Add(i);
                        }
                    }
                }
            }
            //when page not found
            if (requ.Count() == 0)
            {
                List<LLMain> ifModelIsBlankList = new List<LLMain>();
                LLMain ifModelIsBlankObj = new LLMain();
                ifModelIsBlankObj.PageName = "Page Not Found";
                ifModelIsBlankObj.Content = "<div align='center' class='mt-4'><img src='/Content/img/WorkingOnIt.gif'></img><br/></div>";
                ifModelIsBlankList.Add(ifModelIsBlankObj);
                requ = ifModelIsBlankList;
            }
            // returning structured content back to Index Action Result
            return requ;
        }

        public ActionResult About(string id)
        {
            ViewBag.Message = "LowesLink Application Description.";
            ViewBag.id = id;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "LowesLink Application Contact Page.";

            return View();
        }
    }
}