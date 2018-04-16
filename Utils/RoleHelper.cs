﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utils;

namespace Utils
{
    public class RoleHelper
    {

        public static List<User> AccountInfo()
        {
            var userList = new List<User>
            {   new User{UserId =1,UserName ="x" ,Pwd ="123",Role = new List<string>{}},
                new User{UserId =2,UserName ="a" ,Pwd ="123",Role = new List<string>{"a"}},
                new User{UserId =3,UserName ="b" ,Pwd ="123",Role = new List<string>{"b"}},
                new User{UserId =4,UserName ="c" ,Pwd ="123",Role = new List<string>{"c"}},
                new User{UserId =5,UserName ="ab" ,Pwd ="123",Role = new List<string>{"a","b"}},
                new User{UserId =6,UserName ="bc" ,Pwd ="123",Role = new List<string>{"b","c"}},
                new User{UserId =7,UserName ="abc" ,Pwd ="123",Role = new List<string>{"a","b","c"}}
            };
            return userList;
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string CheckRole(string systemId, string token)
        {
            var v = "";//页面返回状态，error表示验证未通过，roleError表示单点验证通过，权限不通过

            //验证是否登录--每个需要登录验证的地方都应该调用
            var ssoAddress = System.Configuration.ConfigurationManager.AppSettings["SSOAddress"];
            v = HttpHelper.OpenReadWithHttps(ssoAddress+"/login/validateLogin", "token=" + token);
            if (v != "error")
            {
                var userList = RoleHelper.AccountInfo();//TODO 获取账号信息
                var userInfo = userList.Where(x => x.UserName == v).FirstOrDefault();
                if (userInfo != null)
                {
                    if (userInfo.Role.Count() == 0 || !userInfo.Role.Exists(x => x == systemId))
                    {
                        v = "roleError";
                    }
                }
                else
                {
                    v = "roleError";
                }
                
            }


            return v;
        }

    }


    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }

        public List<string> Role { get; set; }
    }
}