using Slingshot.Data.Models;
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
    [RoutePrefix("api/campaign")]
    public class CampaignController : ApiController
    {
        UserService obj = new UserService();
        [Route("send")]
        public History sendCampaigns(string userId, long vcardId, long campId, string toEmail)
        {
            return obj.sendCampaign(userId, vcardId, campId, toEmail);
        }
        [Route("add")]
        public Campaign addCampaign(string creatorId, string attechmentsJSONString, string campaignName = "No Name", Boolean prefared = false, string thumbnail = "HTTPS", string subject = "TESTIING", string HTML = "<!DOCTYPE html>", string status = "public")
        {
            UserService obj = new UserService();
            return obj.createCampaign(creatorId, campaignName, prefared, thumbnail, subject, HTML, attechmentsJSONString, status);
        }
        [Route("get")]
        public IEnumerable<Campaign> getCampaigns(string userId, string name = "")
        {
            UserService obj = new UserService();
            return obj.getCampaigns(userId, name);
        }
        [Route("share")]
        public Boolean shareCampaign(string userId, long campaignId)
        {
            return obj.ShareCampaigns(userId, campaignId);
        }
        [Route("uploadImage")]
        public string uploadImage()
        {
            var path = HttpContext.Current.Server.MapPath("~/uploads/attachments");
            Directory.CreateDirectory(path);
            string gesturefile = Path.Combine(Environment.CurrentDirectory, @"vCard\vCard.vcf");
            return gesturefile;
        }
    }
}
