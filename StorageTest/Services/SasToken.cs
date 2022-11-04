using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using StorageTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTest.Services
{
    public class SasToken
    {
        readonly string connectionString;
        readonly string blobContainerName;
        public SasToken(IOptions<BlobEventSettings> _blobEventSettings)
        {
             connectionString = _blobEventSettings.Value.connectionString;
             blobContainerName = _blobEventSettings.Value.blobContainerName;
          //   blobName = _blobEventSettings.Value.blobName;
        }
        public Uri GetSasUri( string blobImageName)
        {
            try
            {              
                BlobClient blobClient1 = new BlobClient(connectionString, blobContainerName, blobImageName);
                return  GetServiceSasUriForBlob(blobClient1);

            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        private Uri GetServiceSasUriForBlob(BlobClient blobClient, string storedPolicyName = null)
        {
            //// Check whether this BlobClient object has been authorized with Shared Key.
            if (blobClient.CanGenerateSasUri)
            {
                // Create a SAS token that's valid for one hour.
                BlobSasBuilder sasBuilder = new BlobSasBuilder()
                {
                    BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                    BlobName = blobClient.Name,
                    Resource = "b"
                };

                if (storedPolicyName == null)
                {
                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
                    sasBuilder.SetPermissions(BlobSasPermissions.Read |
                        BlobSasPermissions.Write);
                }
                else
                {
                    sasBuilder.Identifier = storedPolicyName;
                }

                Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
                Console.WriteLine("SAS URI for blob is: {0}", sasUri);
                Console.WriteLine();

                return sasUri;
            }
            else
            {
                Console.WriteLine(@"BlobClient must be authorized with Shared Key 
                          credentials to create a service SAS.");
                return null;
            }
        }


    }


}
