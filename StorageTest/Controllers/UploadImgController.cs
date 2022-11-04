using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageTest.Services;
using System.Buffers.Text;

namespace StorageTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImgController : ControllerBase
    {
        private readonly ILogger<UploadImgController> _logger;
        private readonly SasToken  _sasToken;

        private readonly Container _container;
        public UploadImgController(ILogger<UploadImgController> logger, SasToken sasToken , Container container)
        {
            _logger = logger;
            _sasToken = sasToken;
            _container = container;
        }

        [HttpPut("/sasToken")]
        public async Task<IActionResult> GetSasToken(string blobImageName)
        {
            try
            {
                // blobImageName= <blobImageName>.png
                return Ok(_sasToken.GetSasUri(blobImageName));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
       
        }

        [HttpGet("/NewBlobContainer/{newBlobContainer}")]
        public async Task<IActionResult> NewBlobContainer(string newBlobContainer)
        {
            try
            {
             await  _container.CreateContainerIfNotExistsAsync(newBlobContainer)
                    .ConfigureAwait(false);

                return Ok(" Blob Container created successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
