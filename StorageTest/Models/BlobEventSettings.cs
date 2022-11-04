namespace StorageTest.Models
{
    public class BlobEventSettings
    {
        public string connectionString { get; set; } = null!;

        public string blobContainerName { get; set; } = null!;

        public string blobName { get; set; } = null!;
    }
}
