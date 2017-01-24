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
    [RoutePrefix("api/event")]
    public class EventsController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creatorId">CreatorId is a user ID, its used to link the created campaign with the user (creator)</param>
        /// <param name="title">Event title</param>
        /// <param name="location">Place where the event will take place</param>
        /// <param name="startDateTime">start date and time of ther event</param>
        /// <param name="endDateTime">end date and time of the event</param>
        /// <returns></returns>
        [Route("add")]
        public Event createEvent(string creatorId, string title, string location, DateTime startDateTime, DateTime endDateTime)
        {
            return obj.CreateEvent( creatorId, title, location, startDateTime, endDateTime);
        }
    }
}
