using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Slingshot.LogicLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slingshot.Data.MediaManager
{
    public class AttachmentUpload
    {

        public async Task<string> SaveAttachment(long emailId, AttachmentUploadModel attachmentEntity)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container. 
            CloudBlobContainer container = blobClient.GetContainerReference("attach");

            // Create the container if it doesn't already exist.
            var containerResult = container.CreateIfNotExists();

            if (containerResult)
            {
                var permissions = await container.GetPermissionsAsync();
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(permissions);
            }
            var fileName = Path.GetFileName(attachmentEntity.File);

            CloudBlockBlob blockBlob;
            // Retrieve reference to a blob named "myblob".
            if (attachmentEntity.ContentType.StartsWith("image"))
            {
                blockBlob = container.GetBlockBlobReference($"images/{fileName}");
            }
            else if (attachmentEntity.ContentType.StartsWith("video"))
            {
                blockBlob = container.GetBlockBlobReference($"videos/{fileName}");
            }
            else if (attachmentEntity.ContentType.StartsWith("audio"))
            {
                blockBlob = container.GetBlockBlobReference($"audios/{fileName}");
            }
            else
            {
                blockBlob = container.GetBlockBlobReference($"documents/{fileName}");
            }

            string tempFile;
            //Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(attachmentEntity.File))
            {
                blockBlob.UploadFromStream(fileStream);
                tempFile = fileStream.Name;
            }
            File.Delete(tempFile);
            var filePath = blockBlob.StorageUri.PrimaryUri.AbsoluteUri.ToString();
            return filePath;
        }
    }
}
