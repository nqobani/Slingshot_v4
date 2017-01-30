using SendGrid;
using SendGrid.Helpers.Mail;
using Slingshot.Data.MediaManager;
using Slingshot.Data.Models;
using Slingshot.LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace Slingshot.Data.Services
{
    public class UserService
    {
        DbConnection dbCon = new DbConnection();
        ValidationHandler _validationHandler = new ValidationHandler();
        AttachmentUpload fileUpload = new AttachmentUpload();
        public UserModel_forDisplayingData createUser(string userName, string firstName, string lastName, string email, string password, string phone, string type)
        {
            return dbCon.createUser(userName, firstName, lastName, email, password, phone, type);
        }
        public UserModel[] GetAllUsers(string userName)
        {
            var user = dbCon.GetAllUsers(userName);

            return user;
        }
        public IEnumerable<Slingshot.Data.Models.VCard> GetUserVCards(string userID)
        {
            Slingshot.Data.Models.VCard[] _vCard = new Slingshot.Data.Models.VCard[1];
            if (dbCon.UserExists(userID))
            {
                return dbCon.GetVCards(userID);
            }
            else
            {
                /*Needs Attention*/
                _vCard[0] = new Slingshot.Data.Models.VCard { };
                return _vCard;
            }
        }

        public Slingshot.Data.Models.VCard CreateVCard(string userId, string firstName, string lastName, string company, string jobTitle, string email, string webPageAddress, string twitter, string businessPhoneNumber, string mobilePhone, string country, string city, string cityCode, string imageLink)
        {
            Boolean userExists = dbCon.UserExists(userId);
            if (userExists)
            {
                return dbCon.createVCard(userId, firstName, lastName, company, jobTitle, email, webPageAddress, twitter, businessPhoneNumber, mobilePhone, country, city, cityCode, imageLink);
            }
            else
            {
                return new Slingshot.Data.Models.VCard { };
            }
        }
        public async Task<Campaign> createCampaign(string creatorId, string campaignName, string desciption, string thumbnail, string subject, string HTML, AttachmentUploadModel fUpload, string status = "public")
        {
            string destinationFilePath = "";
            //if (!(thumbnail.Equals("") || thumbnail.Equals(" ")))
            //{
            //    var thumbnailPath = HttpContext.Current.Server.MapPath("~/uploads/thumbnails");
            //    Directory.CreateDirectory(thumbnailPath);
            //    string fileName = Path.GetFileName(thumbnail);
            //    destinationFilePath = thumbnailPath + "\\" + fileName;

            //    System.IO.File.Copy(thumbnail, destinationFilePath, true);
            //}

            var campaign = dbCon.createCampaign(creatorId, campaignName, desciption, destinationFilePath, subject);

            long campID = campaign.Id;

            dbCon.userCampaign(creatorId, campID);

            var email = dbCon.createEmail(campID, subject, HTML);
            long eID = email.Id;


            string file = await fileUpload.SaveAttachment(eID, fUpload);
            string fileName = Path.GetFileName(file);
            dbCon.createAttachment(eID, fileName, file);


            //var path = HttpContext.Current.Server.MapPath("~/uploads/attachments");
            //Directory.CreateDirectory(path);
            //if (attechmentObjs != null)
            //{
            //    for (int i = 0; i < attechmentObjs.Length; i++)
            //    {
            //        if (attechmentObjs[i].filePath != null)
            //        {
            //string fileName = Path.GetFileName(attechmentObjs[i].filePath);
            //string destinationFilePaths = path + "/" + fileName;
            //System.IO.File.Copy(attechmentObjs[i].filePath, destinationFilePaths, true);
            //        }
            //    }
            //}

            return campaign;
        }
        public Boolean ShareCampaigns(string userId, long campId)
        {
            Boolean shared = false;
            if (dbCon.UserExists(userId))
            {
                if (_validationHandler.CanUserShare(userId, campId))
                {
                    dbCon.userCampaign(userId, campId);
                    shared = true;
                }
            }
            else
            {
                shared = false;
            }

            return shared;
        }
        public void ShareCampaignMultusers(string userId, string campId)
        {
            string[] campIds = campId.Split(',');
            UserCampaign[] userCamp = new UserCampaign[campIds.Length];
            if (dbCon.UserExists(userId))
            {
                for (int i = 0; i < campIds.Length; i++)
                {
                    long cId = (long)campId[i];
                    if (_validationHandler.CanUserShare(userId, cId))
                    {
                        userCamp[i] = new UserCampaign
                        {
                            userId = userId,
                            campaignId = cId
                        };
                    }
                }
            }
        }

        public History sendCampaign(string userId, long vcardId, long campId, string toEmail)
        {

            Boolean hasAccess = _validationHandler.UserCampaignValidation(userId, campId);
            if (hasAccess)
            {
                string fromEmail = dbCon.GetUserEmail(userId);
                Data.Models.Email email = dbCon.GetEmail(campId);

                string subject = email.subject;
                string html = email.html;

                Data.Models.Attachment[] attechments = dbCon.GetAttachmentByEmailId(email.Id).ToArray();
                SendEmail(fromEmail, toEmail, subject, vcardId, html, attechments).Wait();
                return dbCon.createHistory(userId, campId, toEmail, 0);
            }
            else
            {
                return new History { };
            }
        }


        public IEnumerable<Campaign> getCampaigns(string userId, string campName)
        {
            return dbCon.getAllCampaigns(userId, campName);
        }

        public async Task SendEmail(string fromEmail, string toEmail, string subj, long vCardId, string HTML, Data.Models.Attachment[] emailAttechments)
        {
            string[] apikey = new string[] { "SG.yqB-5hb1TQ6JENKhm", "b9LKA._ZJLzzL1FMi", "sNtBdE8bboX9c945RWgg", "_G5E-3fwMYuF" };
            string apiKey = apikey[0] + apikey[1] + apikey[3] + apikey[2] ;//Environment.GetEnvironmentVariable("sendgrid_api_key", EnvironmentVariableTarget.User);
            dynamic sg = new SendGridAPIClient(apiKey);

            SendGrid.Helpers.Mail.Email from = new SendGrid.Helpers.Mail.Email(fromEmail);
            string subject = subj;

            string[] toEmails = toEmail.Split(',');
            var recipientsEmailsArray = LoadMails(toEmails);
            SendGrid.Helpers.Mail.Content content = new SendGrid.Helpers.Mail.Content("text/html", HTML);
            Mail mail = new Mail(from, subject, recipientsEmailsArray[0], content);

            if (toEmails.Length > 0)
            {
                var mailList = recipientsEmailsArray.ToList();
                var personalise = new Personalization();
                personalise.Tos = mailList;
                mail.Personalization = new List<Personalization>() { personalise };
            }

            if (vCardId != 0)
            {
                Boolean hasVCard = VCardManager.LoadVCardData(dbCon.GetVCard(vCardId));

                if (hasVCard)
                {
                    var thumbnailPath = HttpContext.Current.Server.MapPath("~/uploads/vCard");
                    Directory.CreateDirectory(thumbnailPath);
                    string destinationFilePath = Path.Combine(thumbnailPath, "vCard.vcf");

                    var attachment = _validationHandler.GetAttechmentData(destinationFilePath);
                    var att = new SendGrid.Helpers.Mail.Attachment
                    {
                        Filename = attachment.Filename,
                        Type = attachment.Type,
                        Disposition = attachment.Disposition,
                        ContentId = "kjhlknmnjhjkk",
                        Content = attachment.Content
                    };
                    mail.AddAttachment(att);
                }
            }

            if (emailAttechments != null)
            {
                for (int i = 0; i < emailAttechments.Length; i++)
                {
                    var eAttechment = _validationHandler.GetAttechmentData(emailAttechments[i].file);
                    eAttechment.Filename = emailAttechments[i].name;

                    var eAtt = new SendGrid.Helpers.Mail.Attachment
                    {
                        Filename = eAttechment.Filename,
                        Type = eAttechment.Type,
                        Disposition = eAttechment.Disposition,
                        ContentId = "kjhlknmnjhjkk",
                        Content = eAttechment.Content
                    };
                    mail.AddAttachment(eAtt);
                }
            }

            dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
        }

        public SendGrid.Helpers.Mail.Email[] LoadMails(string[] toEmails)
        {
            var recipientEmails = new SendGrid.Helpers.Mail.Email[toEmails.Length];
            for (int i = 0; i < recipientEmails.Length; i++)
            {
                recipientEmails[i] = new SendGrid.Helpers.Mail.Email(toEmails[i]);
                recipientEmails[i].Name = "";
            }
            return recipientEmails;
        }

        public IEnumerable<History> GetUserHistory(string userId)
        {
            var history = dbCon.GetUserHistory(userId);
            return history;
        }

        public Event CreateEvent(string creatorId, string title, string location, DateTime startDateTime, DateTime endDateTime)
        {
            if (dbCon.UserExists(creatorId))
            {
                return dbCon.createEvent(creatorId, title, location, startDateTime, endDateTime);
            }
            else
            {
                return new Event { };
            }
        }



        public Recipient CaptureRecipient(string userId, string fName, string lName, string email, string phone, string jobTile, string country, string province, string city, string street, string code)
        {
            if (dbCon.UserExists(userId))
            {
                var recipient = dbCon.CaptureRecipientData(userId, fName, lName, email, phone, jobTile, country, province, city, street, code);
                return recipient;
            }
            else
            {
                return new Recipient { };
            }

        }


        public IEnumerable<Recipient> GetAllUserRecipients(long userId)
        {
            var recipients = dbCon.GetAllUserRecipients(userId);
            return recipients;
        }
        public Recipient GetRecipient(long recipientId)
        {
            var recipient = dbCon.GetRecipient(recipientId);
            return recipient;
        }

        public void cleanDatabase()
        {
            dbCon.cleanDatabase();
        }

    }
}
