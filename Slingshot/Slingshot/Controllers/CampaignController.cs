using Slingshot.Data.MediaManager;
using Slingshot.Data.Models;
using Slingshot.Data.Services;
using Slingshot.LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.UI.WebControls;

namespace Slingshot.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AttachmentFormatter : Attribute, IControllerConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="descriptor"></param>
        public void Initialize(HttpControllerSettings settings, HttpControllerDescriptor descriptor)
        {
            // Add a custom media-type formatter.
            settings.Formatters.Add(new Data.MediaManager.AttachmentFormatter());
        }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttachmentFormatter]
    [RoutePrefix("api/campaign")]
    public class CampaignController : ApiController
    {
        UserService obj = new UserService();
        /// <summary>
        /// send campiagn to recipient/s
        /// </summary>
        /// <param name="userId">Is a primary key for a user (each user has a unique primary key). This field is used to get user's/sender's data for example email address</param>
        /// <param name="campId">Key used to Indentify each an evey campaign in a system. With this key provided, the api will be able to get all the data associated with that campaign and prepare it to be sent to the recipient.</param>
        /// <param name="toEmail">This field takes recipients email address/es. If you want to send the campaign to multiple users, you can provide as many email addresses as you want (the list of provided email addresses must be comma seperated. e.g nqobani@gmail.com,zulu@oulook.co.za)</param>
        /// <param name="vcardId">Each an every vCard has it own Id. The Id is used to get all the other vCard data from the api. If its provided, the api will attach that vCard to the campaign being sent and send it with it</param>
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
        /// <param name="fUpload"></param>
        /// <param name="campaignName"></param>
        /// <param name="description"></param>
        /// <param name="thumbnail"></param>
        /// <param name="subject"></param>
        /// <param name="HTML"></param>
        /// <param name="status">'private' or 'public' If it private, the campaign will only be visible to the creator, the administrator and all the user its shared with. Only the creator and administrator can share a campaign with other users</param>
        /// <returns></returns>
        [Route("add")]
        public async Task<Campaign> addCampaign(string creatorId, AttachmentUploadModel fUpload, string campaignName = "No Name",string description="No Description", string thumbnail = " ", string subject = "immedia", string HTML = " ", string status = "public")
        {
            if(string.IsNullOrWhiteSpace(creatorId))
            {
                throw new ArgumentException("Parameter cannot be null", "original");
            }
            UserService obj = new UserService();
            return await obj.createCampaign(creatorId, campaignName, description, thumbnail, subject, HTML, fUpload, status);
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
        /// <param name="shareWith_userId"></param>
        /// <returns></returns>
        [Route("share")]
        public Boolean shareCampaign(string userId, long campaignId, string shareWith_userId)
        {
            return obj.ShareCampaigns(userId, campaignId, shareWith_userId);
        }
        /// <summary>
        /// Code Testing...
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("uploadImage")]
        public string uploadImage(AttachmentUploadModel fUpload)
        {
            AttachmentUpload kk = new AttachmentUpload();
            //Directory.CreateDirectory(path);
            //string gesturefile = Path.Combine(Environment.CurrentDirectory, @"vCard\vCard.vcf");
            var path = HttpContext.Current.Server.MapPath("~/uploads/attachments");

            var fdfd=kk.SaveAttachment(12, fUpload);


            return path;

            //obj.SaveAttachmentAsync(userId, fUpload);
            //return fUpload.FileName;
        }
        
    }
}
