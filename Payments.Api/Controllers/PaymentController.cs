using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payments.Api.Data;
using Payments.Api.Models;
using Payments.Api.DTOs;

namespace Payments.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentsDbContext _context;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(PaymentsDbContext context, ILogger<PaymentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region دفعيات العملاء

        [HttpGet("customer-payments")]
        public async Task<ActionResult<IEnumerable<CustomerPaymentResponseDto>>> GetCustomerPayments(
            [FromQuery] int? customerId = null,
            [FromQuery] int? orderId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? paymentMethodId = null,
            [FromQuery] int? currencyId = null)
        {
            try
            {
                var query = _context.CustomerPayments
                    .Include(cp => cp.PaymentMethod)
                    .Include(cp => cp.PaymentState)
                    .Include(cp => cp.Currency)
                    .AsQueryable();

                if (customerId.HasValue)
                    query = query.Where(cp => cp.CustomerId == customerId.Value);

                if (orderId.HasValue)
                    query = query.Where(cp => cp.OrderId == orderId.Value);

                if (fromDate.HasValue)
                    query = query.Where(cp => cp.PaymentDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(cp => cp.PaymentDate <= toDate.Value);

                if (paymentMethodId.HasValue)
                    query = query.Where(cp => cp.PaymentMethodId == paymentMethodId.Value);

                if (currencyId.HasValue)
                    query = query.Where(cp => cp.CurrencyId == currencyId.Value);

                var payments = await query
                    .OrderByDescending(cp => cp.PaymentDate)
                    .Select(cp => new CustomerPaymentResponseDto
                    {
                        PaymentId = cp.PaymentId,
                        OrderId = cp.OrderId,
                        CustomerId = cp.CustomerId,
                        Amount = cp.Amount,
                        PaymentMethodName = cp.PaymentMethod.PaymentMethodName,
                        PaymentDate = cp.PaymentDate,
                        PaymentStateName = cp.PaymentState.PaymentStatesName,
                        CurrencyName = cp.Currency.CurrencyName,
                        Notes = cp.Notes,
                        CreatedDate = cp.CreatedDate
                    })
                    .ToListAsync();

                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع دفعيات العملاء");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpGet("customer-payments/{id}")]
        public async Task<ActionResult<CustomerPaymentResponseDto>> GetCustomerPayment(int id)
        {
            try
            {
                var payment = await _context.CustomerPayments
                    .Include(cp => cp.PaymentMethod)
                    .Include(cp => cp.PaymentState)
                    .Include(cp => cp.Currency)
                    .Where(cp => cp.PaymentId == id)
                    .Select(cp => new CustomerPaymentResponseDto
                    {
                        PaymentId = cp.PaymentId,
                        OrderId = cp.OrderId,
                        CustomerId = cp.CustomerId,
                        Amount = cp.Amount,
                        PaymentMethodName = cp.PaymentMethod.PaymentMethodName,
                        PaymentDate = cp.PaymentDate,
                        PaymentStateName = cp.PaymentState.PaymentStatesName,
                        CurrencyName = cp.Currency.CurrencyName,
                        Notes = cp.Notes,
                        CreatedDate = cp.CreatedDate
                    })
                    .FirstOrDefaultAsync();

                if (payment == null)
                {
                    return NotFound("دفعية العميل غير موجودة");
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع دفعية العميل {PaymentId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("customer-payments")]
        public async Task<ActionResult<CustomerPayment>> CreateCustomerPayment([FromBody] CreateCustomerPaymentDto dto)
        {
            try
            {
                var payment = new CustomerPayment
                {
                    OrderId = dto.OrderId,
                    CustomerId = dto.CustomerId,
                    Amount = dto.Amount,
                    PaymentMethodId = dto.PaymentMethodId,
                    PaymentDate = dto.PaymentDate,
                    PaymentStatesId = dto.PaymentStatesId,
                    CurrencyId = dto.CurrencyId,
                    Notes = dto.Notes,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.CustomerPayments.Add(payment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء دفعية عميل {PaymentId} للمبلغ {Amount}", 
                    payment.PaymentId, payment.Amount);

                return CreatedAtAction(nameof(GetCustomerPayment), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء دفعية العميل");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPut("customer-payments/{id}")]
        public async Task<IActionResult> UpdateCustomerPayment(int id, [FromBody] UpdateCustomerPaymentDto dto)
        {
            try
            {
                var payment = await _context.CustomerPayments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound("دفعية العميل غير موجودة");
                }

                if (dto.Amount.HasValue)
                    payment.Amount = dto.Amount.Value;

                if (dto.PaymentMethodId.HasValue)
                    payment.PaymentMethodId = dto.PaymentMethodId.Value;

                if (dto.PaymentDate.HasValue)
                    payment.PaymentDate = dto.PaymentDate.Value;

                if (dto.PaymentStatesId.HasValue)
                    payment.PaymentStatesId = dto.PaymentStatesId.Value;

                if (dto.CurrencyId.HasValue)
                    payment.CurrencyId = dto.CurrencyId.Value;

                if (dto.Notes != null)
                    payment.Notes = dto.Notes;

                await _context.SaveChangesAsync();

                _logger.LogInformation("تم تحديث دفعية العميل {PaymentId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث دفعية العميل {PaymentId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region دفعيات الموردين

        [HttpGet("supplier-payments")]
        public async Task<ActionResult<IEnumerable<SupplierPaymentResponseDto>>> GetSupplierPayments(
            [FromQuery] int? supplierId = null,
            [FromQuery] int? orderId = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? paymentMethodId = null,
            [FromQuery] int? currencyId = null)
        {
            try
            {
                var query = _context.SupplierPayments
                    .Include(sp => sp.PaymentMethod)
                    .Include(sp => sp.PaymentState)
                    .Include(sp => sp.Currency)
                    .Include(sp => sp.Supplier)
                    .AsQueryable();

                if (supplierId.HasValue)
                    query = query.Where(sp => sp.SupplierId == supplierId.Value);

                if (orderId.HasValue)
                    query = query.Where(sp => sp.OrderId == orderId.Value);

                if (fromDate.HasValue)
                    query = query.Where(sp => sp.PaymentDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(sp => sp.PaymentDate <= toDate.Value);

                if (paymentMethodId.HasValue)
                    query = query.Where(sp => sp.PaymentMethodId == paymentMethodId.Value);

                if (currencyId.HasValue)
                    query = query.Where(sp => sp.CurrencyId == currencyId.Value);

                var payments = await query
                    .OrderByDescending(sp => sp.PaymentDate)
                    .Select(sp => new SupplierPaymentResponseDto
                    {
                        PaymentId = sp.PaymentId,
                        OrderId = sp.OrderId,
                        SupplierId = sp.SupplierId,
                        SupplierName = sp.Supplier.SupplierName ?? "",
                        Amount = sp.Amount,
                        PaymentMethodName = sp.PaymentMethod.PaymentMethodName,
                        PaymentDate = sp.PaymentDate,
                        PaymentStateName = sp.PaymentState.PaymentStatesName,
                        CurrencyName = sp.Currency.CurrencyName,
                        Notes = sp.Notes,
                        CreatedDate = sp.CreatedDate
                    })
                    .ToListAsync();

                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع دفعيات الموردين");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpGet("supplier-payments/{id}")]
        public async Task<ActionResult<SupplierPaymentResponseDto>> GetSupplierPayment(int id)
        {
            try
            {
                var payment = await _context.SupplierPayments
                    .Include(sp => sp.PaymentMethod)
                    .Include(sp => sp.PaymentState)
                    .Include(sp => sp.Currency)
                    .Include(sp => sp.Supplier)
                    .Where(sp => sp.PaymentId == id)
                    .Select(sp => new SupplierPaymentResponseDto
                    {
                        PaymentId = sp.PaymentId,
                        OrderId = sp.OrderId,
                        SupplierId = sp.SupplierId,
                        SupplierName = sp.Supplier.SupplierName ?? "",
                        Amount = sp.Amount,
                        PaymentMethodName = sp.PaymentMethod.PaymentMethodName,
                        PaymentDate = sp.PaymentDate,
                        PaymentStateName = sp.PaymentState.PaymentStatesName,
                        CurrencyName = sp.Currency.CurrencyName,
                        Notes = sp.Notes,
                        CreatedDate = sp.CreatedDate
                    })
                    .FirstOrDefaultAsync();

                if (payment == null)
                {
                    return NotFound("دفعية المورد غير موجودة");
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع دفعية المورد {PaymentId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("supplier-payments")]
        public async Task<ActionResult<SupplierPayment>> CreateSupplierPayment([FromBody] CreateSupplierPaymentDto dto)
        {
            try
            {
                var payment = new SupplierPayment
                {
                    OrderId = dto.OrderId,
                    SupplierId = dto.SupplierId,
                    Amount = dto.Amount,
                    PaymentMethodId = dto.PaymentMethodId,
                    PaymentDate = dto.PaymentDate,
                    PaymentStatesId = dto.PaymentStatesId,
                    CurrencyId = dto.CurrencyId,
                    Notes = dto.Notes,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.UtcNow
                };

                _context.SupplierPayments.Add(payment);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء دفعية مورد {PaymentId} للمبلغ {Amount}", 
                    payment.PaymentId, payment.Amount);

                return CreatedAtAction(nameof(GetSupplierPayment), new { id = payment.PaymentId }, payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء دفعية المورد");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPut("supplier-payments/{id}")]
        public async Task<IActionResult> UpdateSupplierPayment(int id, [FromBody] UpdateSupplierPaymentDto dto)
        {
            try
            {
                var payment = await _context.SupplierPayments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound("دفعية المورد غير موجودة");
                }

                if (dto.Amount.HasValue)
                    payment.Amount = dto.Amount.Value;

                if (dto.PaymentMethodId.HasValue)
                    payment.PaymentMethodId = dto.PaymentMethodId.Value;

                if (dto.PaymentDate.HasValue)
                    payment.PaymentDate = dto.PaymentDate.Value;

                if (dto.PaymentStatesId.HasValue)
                    payment.PaymentStatesId = dto.PaymentStatesId.Value;

                if (dto.CurrencyId.HasValue)
                    payment.CurrencyId = dto.CurrencyId.Value;

                if (dto.Notes != null)
                    payment.Notes = dto.Notes;

                await _context.SaveChangesAsync();

                _logger.LogInformation("تم تحديث دفعية المورد {PaymentId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في تحديث دفعية المورد {PaymentId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region طرق الدفع

        [HttpGet("payment-methods")]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethods()
        {
            try
            {
                var paymentMethods = await _context.PaymentMethods
                    .OrderBy(pm => pm.PaymentMethodName)
                    .ToListAsync();

                return Ok(paymentMethods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع طرق الدفع");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("payment-methods")]
        public async Task<ActionResult<PaymentMethod>> CreatePaymentMethod([FromBody] CreatePaymentMethodDto dto)
        {
            try
            {
                var paymentMethod = new PaymentMethod
                {
                    PaymentMethodName = dto.PaymentMethodName
                };

                _context.PaymentMethods.Add(paymentMethod);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء طريقة دفع {PaymentMethodName}", dto.PaymentMethodName);

                return CreatedAtAction(nameof(GetPaymentMethods), paymentMethod);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء طريقة الدفع");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region العملات

        [HttpGet("currencies")]
        public async Task<ActionResult<IEnumerable<Currency>>> GetCurrencies()
        {
            try
            {
                var currencies = await _context.Currencies
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.CurrencyName)
                    .ToListAsync();

                return Ok(currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع العملات");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("currencies")]
        public async Task<ActionResult<Currency>> CreateCurrency([FromBody] CreateCurrencyDto dto)
        {
            try
            {
                var currency = new Currency
                {
                    CurrencyName = dto.CurrencyName,
                    CurrencyCode = dto.CurrencyCode,
                    CurrencySymbol = dto.CurrencySymbol,
                    ExchangeRate = dto.ExchangeRate,
                    IsActive = true
                };

                _context.Currencies.Add(currency);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء عملة {CurrencyName}", dto.CurrencyName);

                return CreatedAtAction(nameof(GetCurrencies), currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء العملة");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region التقارير

        [HttpGet("statistics")]
        public async Task<ActionResult<PaymentStatisticsDto>> GetPaymentStatistics(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var from = fromDate ?? DateTime.UtcNow.AddMonths(-1);
                var to = toDate ?? DateTime.UtcNow;

                var customerPayments = await _context.CustomerPayments
                    .Where(cp => cp.PaymentDate >= from && cp.PaymentDate <= to)
                    .ToListAsync();

                var supplierPayments = await _context.SupplierPayments
                    .Where(sp => sp.PaymentDate >= from && sp.PaymentDate <= to)
                    .ToListAsync();

                var statistics = new PaymentStatisticsDto
                {
                    TotalCustomerPayments = customerPayments.Sum(cp => cp.Amount),
                    TotalSupplierPayments = supplierPayments.Sum(sp => sp.Amount),
                    TotalCustomerPaymentCount = customerPayments.Count,
                    TotalSupplierPaymentCount = supplierPayments.Count
                };

                statistics.NetAmount = statistics.TotalCustomerPayments - statistics.TotalSupplierPayments;
                statistics.AverageCustomerPayment = statistics.TotalCustomerPaymentCount > 0 
                    ? statistics.TotalCustomerPayments / statistics.TotalCustomerPaymentCount 
                    : 0;
                statistics.AverageSupplierPayment = statistics.TotalSupplierPaymentCount > 0 
                    ? statistics.TotalSupplierPayments / statistics.TotalSupplierPaymentCount 
                    : 0;

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع إحصائيات المدفوعات");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion
    }
}
