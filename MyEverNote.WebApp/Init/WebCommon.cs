using MyEvernote.Common;
using MyEvernote.Entities;
using MyEvernote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {

            EvernoteUser user = CurrentSession.User;
            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}