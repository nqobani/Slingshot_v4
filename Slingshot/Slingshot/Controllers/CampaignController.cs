﻿using Slingshot.Data.Models;
using Slingshot.Data.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Slingshot.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/campaign")]
    public class CampaignController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// send campiagn to recipient/s
        /// </summary>
        /// <param name="userId">Is a rpimary key for a user ( each user has a unique primary key). This field is used to get user the senders data for example email address</param>
        /// <param name="campId">Key the Indenify each an evey campaign in a system. With this key provided, the api will be able to get all the data associated with that campaign and prepare it to be sent to the recipient.</param>
        /// <param name="toEmail">This field takes recipients email address/es. If you want to send the campaign to multiple users, you can provide as many email addresses as you can (the list of provided email addresses must be comma seperated. e.g nqobani@gmail.com,zulu@oulook.co.za)</param>
        /// <param name="vcardId">Each an every vCard has it own Id. The Id is user to get all the other vCard data from the api. If it provided, the api will attach that vCard to the campaign being sent</param>
        /// <returns></returns>
        [Route("send")]
        public History sendCampaigns(string userId, long campId, string toEmail, long vcardId = 0)
        {
            return obj.sendCampaign(userId, vcardId, campId, toEmail);
        }
        /// <summary>
        /// This end-point is capable of creating a complete campaign( with the subject, campaign body in HTML formate and the attechments
        /// </summary>
        /// <param name="creatorId">Creator Id is foriegn key the Indentify the user that is creating a campaign</param>
        /// <param name="attechmentsJSONString">This field takes any array of object( e.g. [{"name":"images.jpg","filePath":"C:\\Users\\Nqobani Zulu\\Pictures\\cat-1285634_960_720.png"},{"name":"images.jpg","filePath":"C:\\Users\\Nqobani Zulu\\Pictures\\images.jpg"}])</param>
        /// <param name="campaignName"></param>
        /// <param name="prefared">This field if of Boolean Data type...</param>
        /// <param name="thumbnail"></param>
        /// <param name="subject"></param>
        /// <param name="HTML"></param>
        /// <param name="status">'private' or 'public' If it private, the campaign will only be visible to the creator, the administrator and all the user its shared with. Only the creator and administrator can share a campaign with other users</param>
        /// <returns></returns>
        [Route("add")]
        public Campaign addCampaign(string creatorId, string attechmentsJSONString = "[]", string campaignName = "No Name", Boolean prefared = false, string thumbnail = " ", string subject = "immedia", string HTML = " ", string status = "public")
        {
            UserService obj = new UserService();
            return obj.createCampaign(creatorId, campaignName, prefared, thumbnail, subject, HTML, attechmentsJSONString, status);
        }
        /// <summary>
        /// Allow the user to get all the campaign she/he has access to. Its also allow the user to do the search by campaign name.
        /// </summary>
        /// <param name="userId">Foreign key used to check which camplaigns does the user have access to</param>
        /// <param name="name">Name of a campaign that the user can use for searching purposes</param>
        /// <returns></returns>
        [Route("get")]
        public IEnumerable<Campaign> getCampaigns(string userId, string name = "")
        {
            UserService obj = new UserService();
            return obj.getCampaigns(userId, name);
        }
        /// <summary>
        /// Used to provided users access to campaigns
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="campaignId"></param>
        /// <returns></returns>
        [Route("share")]
        public Boolean shareCampaign(string userId, long campaignId)
        {
            return obj.ShareCampaigns(userId, campaignId);
        }
        [Route("uploadImage")]
        public string uploadImage(HttpPostedFile profileImage)
        {
            //var path = HttpContext.Current.Server.MapPath("~/uploads/attachments");
            //Directory.CreateDirectory(path);
            //string gesturefile = Path.Combine(Environment.CurrentDirectory, @"vCard\vCard.vcf");

            return profileImage.FileName;
        }
        
    }
}
