using Slingshot.Data.Models;
using Slingshot.Data.Services;
using Slingshot.LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Slingshot.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="phone"></param>
        /// <param name="type">'member' or 'admin': Nothing can hiden from an admin (administrator).</param>
        /// <returns></returns>
        [Route("registerUser")]
        public UserModel_forDisplayingData register(string userName, string firstName, string lastName, string email, string password, string phone, string type = "member")
        {
            return obj.createUser( userName, firstName, lastName, email, password, phone,   type);
        }
        /// <summary>
        /// Returns all the users in the database.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("get")]
        public UserModel[] GetAllUsers(string userName="")
        {
            return obj.GetAllUsers(userName);
        }
        /// <summary>
        /// The data provided on this end-point will be used to create a vCard that will be attached to the campaign being sent.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="company"></param>
        /// <param name="jobTitle"></param>
        /// <param name="email"></param>
        /// <param name="webPageAddress"></param>
        /// <param name="twitter"></param>
        /// <param name="businessPhoneNumber"></param>
        /// <param name="mobilePhone"></param>
        /// <param name="country"></param>
        /// <param name="city"></param>
        /// <param name="cityCode"></param>
        /// <param name="imageLink"></param>
        /// <returns></returns>
        [Route("createVCard")]
        public Data.Models.VCard createVCard(string userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            return obj.CreateVCard(userId, firstName, lastName, company, jobTitle, email, webPageAddress, twitter, businessPhoneNumber, mobilePhone, country, city, cityCode, imageLink);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("getvcards")]
        public IEnumerable<Data.Models.VCard> GetUserVCards(string userId)
        {
            return obj.GetUserVCards(userId);
        }
    }
}
