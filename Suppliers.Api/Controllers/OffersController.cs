using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suppliers.Api.Data;
using Suppliers.Api.Models;

namespace Suppliers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly SuppliersDbContext _context;
        private readonly ILogger<OffersController> _logger;

        public OffersController(SuppliersDbContext context, ILogger<OffersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/offers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffers()
        {
            try
            {
                var offers = await _context.Offers
                    .Include(o => o.Supplier)
                    .Include(o => o.OfferStatus)
                    .Include(o => o.Attachment)
                    .Include(o => o.OfferItems)
                    .Include(o => o.OfferContracts)
                    .ToListAsync();

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offers");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/offers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Offer>> GetOffer(int id)
        {
            try
            {
                var offer = await _context.Offers
                    .Include(o => o.Supplier)
                    .Include(o => o.OfferStatus)
                    .Include(o => o.Attachment)
                    .Include(o => o.OfferItems)
                        .ThenInclude(oi => oi.Item)
                    .Include(o => o.OfferItems)
                        .ThenInclude(oi => oi.Currency)
                    .Include(o => o.OfferContracts)
                        .ThenInclude(oc => oc.Currency)
                    .Include(o => o.OfferContracts)
                        .ThenInclude(oc => oc.InstallmentsType)
                    .Include(o => o.OfferShippingCosts)
                        .ThenInclude(osc => osc.Carrier)
                    .FirstOrDefaultAsync(o => o.OfferID == id);

                if (offer == null)
                {
                    return NotFound($"Offer with ID {id} not found");
                }

                return Ok(offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offer with ID {OfferId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/offers
        [HttpPost]
        public async Task<ActionResult<Offer>> CreateOffer([FromBody] CreateOfferRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var offer = new Offer
                {
                    OfferName = request.OfferName,
                    OfferDescription = request.OfferDescription,
                    SupplierID = request.SupplierID,
                    OfferStatusId = request.OfferStatusId,
                    AttachmentId = request.AttachmentId,
                    CreatedDate = DateTime.UtcNow,
                    ExpiryDate = request.ExpiryDate
                };

                _context.Offers.Add(offer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Offer created with ID {OfferId}", offer.OfferID);

                return CreatedAtAction(nameof(GetOffer), new { id = offer.OfferID }, offer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating offer");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/offers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] UpdateOfferRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var offer = await _context.Offers.FindAsync(id);
                if (offer == null)
                {
                    return NotFound($"Offer with ID {id} not found");
                }

                offer.OfferName = request.OfferName;
                offer.OfferDescription = request.OfferDescription;
                offer.SupplierID = request.SupplierID;
                offer.OfferStatusId = request.OfferStatusId;
                offer.AttachmentId = request.AttachmentId;
                offer.ExpiryDate = request.ExpiryDate;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Offer updated with ID {OfferId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating offer with ID {OfferId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/offers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            try
            {
                var offer = await _context.Offers.FindAsync(id);
                if (offer == null)
                {
                    return NotFound($"Offer with ID {id} not found");
                }

                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Offer deleted with ID {OfferId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting offer with ID {OfferId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/offers/supplier/5
        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffersBySupplier(int supplierId)
        {
            try
            {
                var offers = await _context.Offers
                    .Include(o => o.OfferStatus)
                    .Include(o => o.Attachment)
                    .Where(o => o.SupplierID == supplierId)
                    .ToListAsync();

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offers for supplier {SupplierId}", supplierId);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/offers/status/5
        [HttpGet("status/{statusId}")]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffersByStatus(int statusId)
        {
            try
            {
                var offers = await _context.Offers
                    .Include(o => o.Supplier)
                    .Include(o => o.OfferStatus)
                    .Where(o => o.OfferStatusId == statusId)
                    .ToListAsync();

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offers for status {StatusId}", statusId);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    // DTOs
    public class CreateOfferRequest
    {
        public string? OfferName { get; set; }
        public string? OfferDescription { get; set; }
        public int SupplierID { get; set; }
        public int? OfferStatusId { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class UpdateOfferRequest
    {
        public string? OfferName { get; set; }
        public string? OfferDescription { get; set; }
        public int SupplierID { get; set; }
        public int? OfferStatusId { get; set; }
        public int? AttachmentId { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
