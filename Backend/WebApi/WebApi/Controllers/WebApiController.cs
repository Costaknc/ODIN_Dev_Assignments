using Microsoft.AspNetCore.Mvc;
using IoT_Backend_Assignment;
using Microsoft.Extensions.Caching.Memory;
using static Batches;
using System;

namespace WebApi
{
    [ApiController]
    public class WebApiController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DbEntity _context;

        public WebApiController(IMemoryCache memoryCache, DbEntity context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        [HttpGet("[controller]/GetIpDetails/{ip}")]
        public IActionResult GetMessage([FromRoute] string ip)
        {
            // does IP exist in cache?
            // yes
            if (_memoryCache.TryGetValue(ip, out var cachedData))
            {
                return Ok(new { Message = cachedData });
            }

            // no
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            // does ip exist in db?
            // yes
            // fetch data from db
            var detailsFromDb = DbHandler.FetchRow(ip);

            if (detailsFromDb != null)
            {
                var details = new IpDetails();
                details.City = detailsFromDb.City;
                details.Country = detailsFromDb.Country;
                details.Continent = detailsFromDb.Continent;
                details.Latitude = detailsFromDb.Lat;
                details.Longitude = detailsFromDb.Lon;
                // put in cache
                _memoryCache.Set(ip, details, cacheEntryOptions);

                // ok
                return Ok(new { Message = details });
            }
            else // no
            {
                // request data from library
                var provider = new IPInfoProvider();

                try
                {
                    var detailsFromApi = provider.GetDetails(ip);
                    // write data in db
                    DbHandler.InsertNewRow(ip, detailsFromApi);
                    // put in cache
                    _memoryCache.Set(ip, detailsFromApi, cacheEntryOptions);
                    // ok
                    return Ok(new { Message = detailsFromApi });
                }
                catch (Exception ex)
                {
                    return BadRequest(new
                    {
                        Error = $"Error: {ex.Message}"
                    });
                }
            }
        }

        [HttpPost("[controller]/update")]
        public IActionResult PostUpdate([FromBody] updateRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.IP) || string.IsNullOrEmpty(request.field) || string.IsNullOrEmpty(request.value))
                {
                    return BadRequest("Invalid request body.");
                }
                var batchId = BatchHandler.AddToBatch(request);
                return Ok(new { BatchId = batchId });
            }
            catch (IPServiceNotAvailableException ex)
            {
                Console.WriteLine($"IPServiceNotAvailableException: {ex.Message}");
                return StatusCode(503, new { error = "IP Service is currently unavailable. Please try again later. :)" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message} :)" });
            }
        }


        [HttpGet("[controller]/progress/{batchId}")]
        public IActionResult GetProgress([FromRoute] Guid batchId)
        {
            var batch = BatchHandler.GetBatchProgress(batchId);
            if (batch == null)
            {
                return NotFound(new { Message = "Batch not found. :(" });
            }

            return Ok(new
            {
                BatchId = batch.BatchId,
                ProcessedRequests = batch.ProcessedRequests,
                TotalRequests = batch.Updates.Count
            });
        }
    }
}
