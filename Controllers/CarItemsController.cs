using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using RealCarAPI.Models;

namespace RealCarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarItemsController : ControllerBase
    {
        private readonly RealCarAPIContext _context;
        private IConfiguration _configuration;

        public CarItemsController(RealCarAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/CarItems
        [HttpGet]
        public IEnumerable<CarItem> GetCarItem()
        {
            return _context.CarItem;
        }

        // GET: api/CarItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carItem = await _context.CarItem.FindAsync(id);

            if (carItem == null)
            {
                return NotFound();
            }

            return Ok(carItem);
        }
   

        // PUT: api/CarItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarItem([FromRoute] int id, [FromBody] CarItem carItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(carItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CarItems
        [HttpPost]
        public async Task<IActionResult> PostCarItem([FromBody] CarItem carItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CarItem.Add(carItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarItem", new { id = carItem.Id }, carItem);
        }

        // DELETE: api/CarItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carItem = await _context.CarItem.FindAsync(id);
            if (carItem == null)
            {
                return NotFound();
            }

            _context.CarItem.Remove(carItem);
            await _context.SaveChangesAsync();

            return Ok(carItem);
        }

        private bool CarItemExists(int id)
        {
            return _context.CarItem.Any(e => e.Id == id);
        }

        // GET: api/Meme/Tags
        [Route("tags")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var CarItem = (from c in _context.CarItem
                         select c.Tags).Distinct();

            var returned = await CarItem.ToListAsync();

            return returned;
        }
        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]CarImageItems car)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = car.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(car.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }

                    CarItem carItem = new CarItem();
                    carItem.Title = car.Title;
                    carItem.Tags = car.Tags;

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    carItem.Height = image.Height.ToString();
                    carItem.Width = image.Width.ToString();
                    carItem.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;


                    _context.CarItem.Add(carItem);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {car.Title} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("images");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
        }

    }
}