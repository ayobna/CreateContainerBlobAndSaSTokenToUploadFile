using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageTest.Services
{
    public class Container
    {

        readonly string connectionString;
   
        public Container(IOptions<BlobEventSettings> _blobEventSettings)
        {
            connectionString = _blobEventSettings.Value.connectionString;

        }        
        public async Task CreateContainerIfNotExistsAsync( string newContainer)
        {
            try
            {
              CloudStorageAccount storageacc = CloudStorageAccount.Parse(connectionString);

                //Create Reference to Azure Blob
                CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();
                             BlobContainerPermissions blobContainerPermissions = new BlobContainerPermissions() { PublicAccess=BlobContainerPublicAccessType.Blob};
                //The next 2 lines create if not exists a container named "democontainer"
                CloudBlobContainer container = blobClient.GetContainerReference(newContainer);
         
                await container.CreateIfNotExistsAsync();
                await container.SetPermissionsAsync(blobContainerPermissions);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
