﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utils;
using Utils.CommonModel;

namespace c.com.Controllers
{
    public class HomeController : Controller
    {
        //需要登录的页面-TODO
        public ActionResult Index(string token = null)
        {
            //C
            var v = "";//页面返回状态
            var systemNo = "c";//系统识别代码
            var serURL = System.Configuration.ConfigurationManager.AppSettings["ServerURL"];
            var ssoURL = System.Configuration.ConfigurationManager.AppSettings["SSOAddress"];
            ViewBag.ser = serURL;
            ViewBag.sso = ssoURL;
            var requestCookies = Request.Cookies["currentUser"];
            HttpCookie cookie = new HttpCookie("currentUser");
            cookie.HttpOnly = true;
            cookie.Expires = DateTime.Now.AddYears(100);
            if (requestCookies != null)
            {
                ViewBag.token = requestCookies.Value;
            }
            if (token != null)
            {
                ViewBag.token = token;
            }
            cookie.Value = ViewBag.token;
            Response.Cookies.Add(cookie);

            //验证权限
            v = RoleHelper.CheckRole(systemNo, ViewBag.token);

            //获取租户信息
            if (v != null && v != "error" && v != "roleError")
            {
                var res = HttpHelper.OpenReadWithHttps(ssoURL + "/Login/GetTenantInfo", "name=" + v);
                if (res != null && res != "")
                {
                    var data = JsonConvert.DeserializeObject<TenantsVM>(res);
                    ViewBag.TenantId = data.Tenant_id;
                    ViewBag.Name = data.Name;
                }
            }

            ViewBag.v = v;
            return View();
        }



    }
}