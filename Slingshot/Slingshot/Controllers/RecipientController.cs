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
    [RoutePrefix("api/recipient")]
    public class RecipientController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="email"></param>
        /// <param name="cell"></param>
        /// <param name="jobTile"></param>
        /// <param name="country"></param>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="street"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [Route("addclient")]
        public Recipient AddClient(string userId, string fName, string lName, string email, string cell, string jobTile, string country, string province, string city, string street, string code)
        {
            return obj.CaptureRecipient(userId, fName, lName, email, cell,  jobTile,  country,  province,  city,  street,  code);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("getUserClients")]
        public IEnumerable<Recipient> GetAllUserClients(long userId)
        {
            return obj.GetAllUserRecipients(userId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("getClient")]
        public Recipient GetClient(long userId)
        {
            return obj.GetRecipient(userId);
        }

    }
}
