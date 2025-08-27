using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suppliers.Api.Data;
using Suppliers.Api.Models;

namespace Suppliers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly SuppliersDbContext _context;
        private readonly ILogger<SuppliersController> _logger;

        public SuppliersController(SuppliersDbContext context, ILogger<SuppliersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            try
            {
                var suppliers = await _context.Suppliers
                    .Include(s => s.Country)
                    .Include(s => s.Attachment)
                    .Include(s => s.SupplierContacts)
                    .Where(s => s.IsActive == true)
                    .ToListAsync();

                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suppliers");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _context.Suppliers
                    .Include(s => s.Country)
                    .Include(s => s.Attachment)
                    .Include(s => s.SupplierContacts)
                    .Include(s => s.Offers)
                    .FirstOrDefaultAsync(s => s.SupplierId == id);

                if (supplier == null)
                {
                    return NotFound($"Supplier with ID {id} not found");
                }

                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supplier with ID {SupplierId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST: api/suppliers
        [HttpPost]
        public async Task<ActionResult<Supplier>> CreateSupplier([FromBody] CreateSupplierRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var supplier = new Supplier
                {
                    SupplierName = request.SupplierName,
                    Phone = request.Phone,
                    Email = request.Email,
                    Address = request.Address,
                    Websit = request.Websit,
                    CountryId = request.CountryId,
                    IsActive = true,
                    SupplierNote = request.SupplierNote,
                    AttachmentId = request.AttachmentId
                };

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Supplier created with ID {SupplierId}", supplier.SupplierId);

                return CreatedAtAction(nameof(GetSupplier), new { id = supplier.SupplierId }, supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating supplier");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT: api/suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] UpdateSupplierRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    return NotFound($"Supplier with ID {id} not found");
                }

                supplier.SupplierName = request.SupplierName;
                supplier.Phone = request.Phone;
                supplier.Email = request.Email;
                supplier.Address = request.Address;
                supplier.Websit = request.Websit;
                supplier.CountryId = request.CountryId;
                supplier.SupplierNote = request.SupplierNote;
                supplier.AttachmentId = request.AttachmentId;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Supplier updated with ID {SupplierId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating supplier with ID {SupplierId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE: api/suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    return NotFound($"Supplier with ID {id} not found");
                }

                // Soft delete - set IsActive to false
                supplier.IsActive = false;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Supplier soft deleted with ID {SupplierId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting supplier with ID {SupplierId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/suppliers/search?name=value
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Supplier>>> SearchSuppliers([FromQuery] string? name, [FromQuery] string? email)
        {
            try
            {
                var query = _context.Suppliers
                    .Include(s => s.Country)
                    .Include(s => s.Attachment)
                    .Where(s => s.IsActive == true);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(s => s.SupplierName!.Contains(name));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    query = query.Where(s => s.Email!.Contains(email));
                }

                var suppliers = await query.ToListAsync();
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching suppliers");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/suppliers/5/offers
        [HttpGet("{id}/offers")]
        public async Task<ActionResult<IEnumerable<Offer>>> GetSupplierOffers(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FindAsync(id);
                if (supplier == null)
                {
                    return NotFound($"Supplier with ID {id} not found");
                }

                var offers = await _context.Offers
                    .Include(o => o.OfferStatus)
                    .Include(o => o.Attachment)
                    .Where(o => o.SupplierID == id)
                    .ToListAsync();

                return Ok(offers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offers for supplier {SupplierId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }

    // DTOs
    public class CreateSupplierRequest
    {
        public string? SupplierName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Websit { get; set; }
        public int? CountryId { get; set; }
        public string? SupplierNote { get; set; }
        public int? AttachmentId { get; set; }
    }

    public class UpdateSupplierRequest
    {
        public string? SupplierName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Websit { get; set; }
        public int? CountryId { get; set; }
        public string? SupplierNote { get; set; }
        public int? AttachmentId { get; set; }
    }
}
