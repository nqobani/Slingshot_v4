using Slingshot.Data.Models;
using Slingshot.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Slingshot.Controllers
{
    [RoutePrefix("api/history")]
    public class HistoryController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// Used to get all the campaigns that the user sent over.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("getuserhistory")]
        public IEnumerable<History> GetUserHistory(string userId)
        {
            return obj.GetUserHistory(userId);
        }
    }
}
