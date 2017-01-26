using HeyRed.Mime;
using Slingshot.LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Slingshot.Data.MediaManager
{
    public class AttachmentFormatter : MediaTypeFormatter
    { 
        private static Dictionary<string, string> MimeTypeExtensions = new Dictionary<string, string>
        {
            {"audio/aac", ".aac"},
            {"audio/x-m4a", ".m4a"},
            {"audio/m4a", ".m4a"},
            {"audio/mp3", ".mp3"},
            {"image/gif", ".gif"},
            {"image/jpeg", ".jpg"},
            {"image/png", ".png"},
            {"video/mp4", ".mp4"},
            {"application/pdf", ".pdf"},
            {"text/plain", ".txt"},
            {"application/msword", ".doc"},
        };

        public AttachmentFormatter()
        {
            //Audio
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("audio/aac"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("audio/x-m4a"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("audio/m4a"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("audio/mp3"));

            //Images
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));

            //Video
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("video/mp4"));

            //text
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/msword"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/pdf"));

            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return typeof(AttachmentUploadModel) == type;
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            string fileMediaType = null;
            string filename = null;
            string thumbnaileName = null;
            HttpContent mediaContent;

            if (content.IsMimeMultipartContent())
            {
                var provider = await content.ReadAsMultipartAsync();

                foreach (var httpContent in provider.Contents)
                {
                    var partName = httpContent?.Headers?.ContentDisposition.Name.ToLower().Replace("\"", "");

                    if (partName.Equals("file", StringComparison.InvariantCultureIgnoreCase))
                    {
                        fileMediaType = httpContent?.Headers?.ContentType?.MediaType;
                        filename = GetFileName(fileMediaType);
                        if (fileMediaType != null
                            && (fileMediaType.StartsWith("audio", StringComparison.InvariantCulture)
                                || fileMediaType.StartsWith("image", StringComparison.InvariantCulture)
                                || fileMediaType.StartsWith("video", StringComparison.InvariantCulture)
                                || fileMediaType.StartsWith("application", StringComparison.InvariantCulture)
                                || fileMediaType.StartsWith("text", StringComparison.InvariantCulture)))
                        {
                            mediaContent = httpContent;
                            await WriteMediaContentToFileAsync(filename, mediaContent);
                        }
                    }
                    else if (partName.Equals("thumbnail", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var thumbnailMediaType = httpContent?.Headers?.ContentType?.MediaType;
                        thumbnaileName = GetFileName(thumbnailMediaType);
                        if (thumbnailMediaType != null &&
                            (thumbnailMediaType.StartsWith("image", StringComparison.InvariantCulture)))
                        {
                            mediaContent = httpContent;
                            await WriteMediaContentToFileAsync(thumbnaileName, mediaContent);
                        }
                    }
                }
            }
            else
            {
                fileMediaType = content?.Headers?.ContentType?.MediaType;
                filename = GetFileName(fileMediaType);
                if (fileMediaType != null
                    && (fileMediaType.StartsWith("audio", StringComparison.InvariantCulture)
                        || fileMediaType.StartsWith("image", StringComparison.InvariantCulture)
                        || fileMediaType.StartsWith("video", StringComparison.InvariantCulture)
                        || fileMediaType.StartsWith("application", StringComparison.InvariantCulture)
                        || fileMediaType.StartsWith("text", StringComparison.InvariantCulture)))
                {
                    mediaContent = content;
                    await WriteMediaContentToFileAsync(filename, mediaContent);
                }
            }

            var model = new AttachmentUploadModel
            {
                File = filename,
                Thumbnail = thumbnaileName,
                ContentType = fileMediaType
            };

            return model;
        }

        /// <summary>
        /// Gets a named part from a multipart stream
        /// </summary>
        /// <param name="provider">The <see cref="MultipartStreamProvider"/> to get the part from</param>
        /// <param name="part">The name of the part to get</param>
        /// <returns></returns>
        protected static HttpContent GetPart(MultipartStreamProvider provider, string part)
        {
            return provider.Contents.FirstOrDefault(x =>
            {
                var partName = x.Headers.ContentDisposition.Name.ToLower().Replace("\"", "");
                return (partName == part);
            });
        }

        /// <summary>
        /// Gets the filename from a part of a multipart stream
        /// </summary>
        /// <param name="mediaType">The mediatype of the part</param>
        /// <returns></returns>
        protected string GetFileName(string mediaType)
        {
            string extension = null;
            try
            {
                var trimmedType = mediaType.Trim();
                if (MimeTypeExtensions.ContainsKey(trimmedType))
                {
                    extension = MimeTypeExtensions[trimmedType];
                }
                else
                {
                    extension = MimeTypesMap.GetExtension(mediaType.Trim());
                }
            }
            catch (ArgumentException e)
            {
                extension = mediaType.Split('/').LastOrDefault();
                if (extension != null)
                {
                    extension = $".{extension}";
                }
            }
            if (extension == null)
            {
                extension = "";
            }
            return HttpContext.Current.Server.MapPath(@"~\tmp") + @"\" + Guid.NewGuid() + extension;
        }

        protected static async Task WriteMediaContentToFileAsync(string fileName, HttpContent mediaContent)
        {
            var directoryName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrWhiteSpace(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            using (var fileStream = File.Create(fileName))
            {
                await mediaContent.CopyToAsync(fileStream);
            }
        }
    }
}
