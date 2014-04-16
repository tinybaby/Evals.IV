using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Evals.IV.Infrastructure;
using Evals.IV.Infrastructure.Identity;

namespace Evals.IV.Store.Areas.Administration.Controllers
{
    [Authorize(Roles="Admin")]
    public class HomeController : Controller
    {
        protected string _orderNo;
        protected string _orderAmount;
        protected string _orderTime;
        protected string _signData;
        IUserRepository _x { get; set; }
        //
        // GET: /Administration/Home/
        [AllowAnonymous]
        public ActionResult Index()
        {

            
            

            return View();
        }

        public string GetTraf()
        {
            string url = "http://tongji.cnzz.com/main.php?c=traf&a=domain&ajax=module=refererDomainList_orderBy=pv_orderType=-1_currentPage=1_pageType=30&siteid=3354650&st=2012-01-01&et=" + DateTime.Now.ToString("yyyy-MM-dd") + "&domainCondType=&itemName=&itemNameType=&itemVal=&siteType=";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string result;
            CookieCollection cookies = WebHttpUtil.funGetCookie("http://cnzz.com/login.php", encoding.GetBytes("username=iceyq@vip.qq.com&passwd=dajiahaoo&verifyCode=&productType=0&remeber=0"), out result,
                new RequestPPT()
                {
                    Accept = "/",
                    ContentType = "application/x-www-form-urlencoded; charset=UTF-8",
                    Method = "POST",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36"
                }, "");

            var data = WebHttpUtil.funGetHtmlByCookies(url, cookies,
                new RequestPPT()
                {
                    Accept = "/",
                    ContentType = "application/json; charset=UTF-8",
                    Method = "GET",
                    UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36"

                });
            return data;
        }

    }
}