using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Suppliers.Api.Data;
using Suppliers.Api.Models;

namespace Suppliers.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly SuppliersDbContext _context;
        private readonly ILogger<LookupController> _logger;

        public LookupController(SuppliersDbContext context, ILogger<LookupController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/lookup/countries
        [HttpGet("countries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            try
            {
                var countries = await _context.Countries.ToListAsync();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving countries");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/currencies
        [HttpGet("currencies")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            try
            {
                var currencies = await _context.Currencies.ToListAsync();
                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving currencies");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/offer-statuses
        [HttpGet("offer-statuses")]
        public async Task<ActionResult<IEnumerable<OfferStatus>>> GetOfferStatuses()
        {
            try
            {
                var statuses = await _context.OfferStatuses.ToListAsync();
                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving offer statuses");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/payment-methods
        [HttpGet("payment-methods")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethods()
        {
            try
            {
                var methods = await _context.PaymentMethods.ToListAsync();
                return Ok(methods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment methods");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/payment-states
        [HttpGet("payment-states")]
        public async Task<ActionResult<IEnumerable<PaymentState>>> GetPaymentStates()
        {
            try
            {
                var states = await _context.PaymentStates.ToListAsync();
                return Ok(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment states");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/carriers
        [HttpGet("carriers")]
        public async Task<ActionResult<IEnumerable<Carrier>>> GetCarriers()
        {
            try
            {
                var carriers = await _context.Carriers.ToListAsync();
                return Ok(carriers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving carriers");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/shipment-types
        [HttpGet("shipment-types")]
        public async Task<ActionResult<IEnumerable<ShipmentType>>> GetShipmentTypes()
        {
            try
            {
                var types = await _context.ShipmentTypes.ToListAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving shipment types");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/installments-types
        [HttpGet("installments-types")]
        public async Task<ActionResult<IEnumerable<InstallmentsType>>> GetInstallmentsTypes()
        {
            try
            {
                var types = await _context.InstallmentsTypes.ToListAsync();
                return Ok(types);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving installments types");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/lookup/items
        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            try
            {
                var items = await _context.Items.ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving items");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
