using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Accounting.Api.Data;
using Accounting.Api.Models;
using Accounting.Api.DTOs;

namespace Accounting.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountingController : ControllerBase
    {
        private readonly AccountingDbContext _context;
        private readonly ILogger<AccountingController> _logger;

        public AccountingController(AccountingDbContext context, ILogger<AccountingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region دليل الحسابات

        [HttpGet("chart-of-accounts")]
        public async Task<ActionResult<IEnumerable<ChartOfAccountResponseDto>>> GetChartOfAccounts(
            [FromQuery] string? accountType = null,
            [FromQuery] bool? isActive = null)
        {
            try
            {
                var query = _context.ChartOfAccounts
                    .Include(coa => coa.ParentAccount)
                    .Include(coa => coa.SubAccounts)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(accountType))
                    query = query.Where(coa => coa.AccountType == accountType);

                if (isActive.HasValue)
                    query = query.Where(coa => coa.IsActive == isActive.Value);

                var accounts = await query
                    .OrderBy(coa => coa.AccountCode)
                    .Select(coa => new ChartOfAccountResponseDto
                    {
                        AccountId = coa.AccountId,
                        AccountCode = coa.AccountCode,
                        AccountName = coa.AccountName,
                        AccountType = coa.AccountType,
                        ParentAccountId = coa.ParentAccountId,
                        ParentAccountName = coa.ParentAccount != null ? coa.ParentAccount.AccountName : null,
                        IsActive = coa.IsActive,
                        CreatedDate = coa.CreatedDate,
                        UpdatedDate = coa.UpdatedDate
                    })
                    .ToListAsync();

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع دليل الحسابات");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpGet("chart-of-accounts/{id}")]
        public async Task<ActionResult<ChartOfAccountResponseDto>> GetChartOfAccount(int id)
        {
            try
            {
                var account = await _context.ChartOfAccounts
                    .Include(coa => coa.ParentAccount)
                    .Include(coa => coa.SubAccounts)
                    .Where(coa => coa.AccountId == id)
                    .Select(coa => new ChartOfAccountResponseDto
                    {
                        AccountId = coa.AccountId,
                        AccountCode = coa.AccountCode,
                        AccountName = coa.AccountName,
                        AccountType = coa.AccountType,
                        ParentAccountId = coa.ParentAccountId,
                        ParentAccountName = coa.ParentAccount != null ? coa.ParentAccount.AccountName : null,
                        IsActive = coa.IsActive,
                        CreatedDate = coa.CreatedDate,
                        UpdatedDate = coa.UpdatedDate
                    })
                    .FirstOrDefaultAsync();

                if (account == null)
                {
                    return NotFound("الحساب غير موجود");
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع الحساب {AccountId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("chart-of-accounts")]
        public async Task<ActionResult<ChartOfAccount>> CreateChartOfAccount([FromBody] CreateChartOfAccountDto dto)
        {
            try
            {
                // Check if account code already exists
                var existingAccount = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(coa => coa.AccountCode == dto.AccountCode);

                if (existingAccount != null)
                {
                    return BadRequest("كود الحساب موجود بالفعل");
                }

                var account = new ChartOfAccount
                {
                    AccountCode = dto.AccountCode,
                    AccountName = dto.AccountName,
                    AccountType = dto.AccountType,
                    ParentAccountId = dto.ParentAccountId,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                _context.ChartOfAccounts.Add(account);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء حساب جديد {AccountCode} - {AccountName}", 
                    account.AccountCode, account.AccountName);

                return CreatedAtAction(nameof(GetChartOfAccount), new { id = account.AccountId }, account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء الحساب");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region قيود اليومية

        [HttpGet("journal-entries")]
        public async Task<ActionResult<IEnumerable<JournalEntryResponseDto>>> GetJournalEntries(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int? orderId = null,
            [FromQuery] bool? isPosted = null)
        {
            try
            {
                var query = _context.JournalEntries
                    .Include(je => je.JournalEntryDetails)
                    .ThenInclude(jed => jed.ChartOfAccount)
                    .AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(je => je.EntryDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(je => je.EntryDate <= toDate.Value);

                if (orderId.HasValue)
                    query = query.Where(je => je.OrderId == orderId.Value);

                if (isPosted.HasValue)
                    query = query.Where(je => je.IsPosted == isPosted.Value);

                var entries = await query
                    .OrderByDescending(je => je.EntryDate)
                    .Select(je => new JournalEntryResponseDto
                    {
                        EntryId = je.EntryId,
                        OrderId = je.OrderId,
                        EntryDate = je.EntryDate,
                        Description = je.Description,
                        TotalAmount = je.TotalAmount,
                        CreatedBy = je.CreatedBy,
                        CreatedDate = je.CreatedDate,
                        IsPosted = je.IsPosted,
                        PostedDate = je.PostedDate,
                        PostedBy = je.PostedBy,
                        Details = je.JournalEntryDetails.Select(jed => new JournalEntryDetailResponseDto
                        {
                            DetailId = jed.DetailId,
                            EntryId = jed.EntryId,
                            AccountId = jed.AccountId,
                            AccountCode = jed.ChartOfAccount.AccountCode,
                            AccountName = jed.ChartOfAccount.AccountName,
                            Debit = jed.Debit,
                            Credit = jed.Credit,
                            Description = jed.Description
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(entries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع قيود اليومية");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpGet("journal-entries/{id}")]
        public async Task<ActionResult<JournalEntryResponseDto>> GetJournalEntry(int id)
        {
            try
            {
                var entry = await _context.JournalEntries
                    .Include(je => je.JournalEntryDetails)
                    .ThenInclude(jed => jed.ChartOfAccount)
                    .Where(je => je.EntryId == id)
                    .Select(je => new JournalEntryResponseDto
                    {
                        EntryId = je.EntryId,
                        OrderId = je.OrderId,
                        EntryDate = je.EntryDate,
                        Description = je.Description,
                        TotalAmount = je.TotalAmount,
                        CreatedBy = je.CreatedBy,
                        CreatedDate = je.CreatedDate,
                        IsPosted = je.IsPosted,
                        PostedDate = je.PostedDate,
                        PostedBy = je.PostedBy,
                        Details = je.JournalEntryDetails.Select(jed => new JournalEntryDetailResponseDto
                        {
                            DetailId = jed.DetailId,
                            EntryId = jed.EntryId,
                            AccountId = jed.AccountId,
                            AccountCode = jed.ChartOfAccount.AccountCode,
                            AccountName = jed.ChartOfAccount.AccountName,
                            Debit = jed.Debit,
                            Credit = jed.Credit,
                            Description = jed.Description
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (entry == null)
                {
                    return NotFound("قيد اليومية غير موجود");
                }

                return Ok(entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع قيد اليومية {EntryId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("journal-entries")]
        public async Task<ActionResult<JournalEntry>> CreateJournalEntry([FromBody] CreateJournalEntryDto dto)
        {
            try
            {
                // Validate that debits equal credits
                var totalDebits = dto.Details.Sum(d => d.Debit);
                var totalCredits = dto.Details.Sum(d => d.Credit);

                if (totalDebits != totalCredits)
                {
                    return BadRequest("مجموع المدين يجب أن يساوي مجموع الدائن");
                }

                // Validate that each detail has either debit or credit, not both
                foreach (var detail in dto.Details)
                {
                    if (detail.Debit > 0 && detail.Credit > 0)
                    {
                        return BadRequest("لا يمكن أن يكون الحساب مدين ودائن في نفس الوقت");
                    }

                    if (detail.Debit == 0 && detail.Credit == 0)
                    {
                        return BadRequest("يجب أن يكون الحساب مدين أو دائن");
                    }
                }

                var entry = new JournalEntry
                {
                    OrderId = dto.OrderId,
                    EntryDate = dto.EntryDate,
                    Description = dto.Description,
                    TotalAmount = totalDebits,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsPosted = false
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                // Add details
                foreach (var detailDto in dto.Details)
                {
                    var detail = new JournalEntryDetail
                    {
                        EntryId = entry.EntryId,
                        AccountId = detailDto.AccountId,
                        Debit = detailDto.Debit,
                        Credit = detailDto.Credit,
                        Description = detailDto.Description
                    };

                    _context.JournalEntryDetails.Add(detail);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء قيد يومية {EntryId} للمبلغ {TotalAmount}", 
                    entry.EntryId, entry.TotalAmount);

                return CreatedAtAction(nameof(GetJournalEntry), new { id = entry.EntryId }, entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء قيد اليومية");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("journal-entries/{id}/post")]
        public async Task<IActionResult> PostJournalEntry(int id, [FromBody] int postedBy)
        {
            try
            {
                var entry = await _context.JournalEntries.FindAsync(id);

                if (entry == null)
                {
                    return NotFound("قيد اليومية غير موجود");
                }

                if (entry.IsPosted)
                {
                    return BadRequest("قيد اليومية مرسل بالفعل");
                }

                entry.IsPosted = true;
                entry.PostedDate = DateTime.UtcNow;
                entry.PostedBy = postedBy;

                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إرسال قيد اليومية {EntryId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إرسال قيد اليومية {EntryId}", id);
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region القيود التلقائية

        [HttpPost("customer-payment-journal-entry")]
        public async Task<ActionResult<JournalEntry>> CreateCustomerPaymentJournalEntry([FromBody] CreateCustomerPaymentJournalEntryDto dto)
        {
            try
            {
                // Get customer account (assuming account code 103 for customers)
                var customerAccount = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(coa => coa.AccountCode == "103");

                if (customerAccount == null)
                {
                    return BadRequest("حساب العملاء غير موجود في دليل الحسابات");
                }

                // Get bank account (assuming account code 101 for bank)
                var bankAccount = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(coa => coa.AccountCode == "101");

                if (bankAccount == null)
                {
                    return BadRequest("حساب البنك غير موجود في دليل الحسابات");
                }

                var entry = new JournalEntry
                {
                    OrderId = dto.OrderId,
                    EntryDate = DateTime.UtcNow,
                    Description = $"دفعية عميل - طلب رقم {dto.OrderId}",
                    TotalAmount = dto.Amount,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsPosted = true,
                    PostedDate = DateTime.UtcNow,
                    PostedBy = dto.CreatedBy
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                // Create journal entry details
                var details = new List<JournalEntryDetail>
                {
                    new JournalEntryDetail
                    {
                        EntryId = entry.EntryId,
                        AccountId = bankAccount.AccountId,
                        Debit = dto.Amount,
                        Credit = 0,
                        Description = $"استلام دفعية من العميل {dto.CustomerId}"
                    },
                    new JournalEntryDetail
                    {
                        EntryId = entry.EntryId,
                        AccountId = customerAccount.AccountId,
                        Debit = 0,
                        Credit = dto.Amount,
                        Description = $"تسوية حساب العميل {dto.CustomerId}"
                    }
                };

                _context.JournalEntryDetails.AddRange(details);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء قيد يومية تلقائي لدفعية العميل {CustomerId} للمبلغ {Amount}", 
                    dto.CustomerId, dto.Amount);

                return CreatedAtAction(nameof(GetJournalEntry), new { id = entry.EntryId }, entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء قيد يومية دفعية العميل");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpPost("supplier-payment-journal-entry")]
        public async Task<ActionResult<JournalEntry>> CreateSupplierPaymentJournalEntry([FromBody] CreateSupplierPaymentJournalEntryDto dto)
        {
            try
            {
                // Get supplier account (assuming account code 201 for suppliers)
                var supplierAccount = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(coa => coa.AccountCode == "201");

                if (supplierAccount == null)
                {
                    return BadRequest("حساب الموردين غير موجود في دليل الحسابات");
                }

                // Get bank account (assuming account code 101 for bank)
                var bankAccount = await _context.ChartOfAccounts
                    .FirstOrDefaultAsync(coa => coa.AccountCode == "101");

                if (bankAccount == null)
                {
                    return BadRequest("حساب البنك غير موجود في دليل الحسابات");
                }

                var entry = new JournalEntry
                {
                    OrderId = dto.OrderId,
                    EntryDate = DateTime.UtcNow,
                    Description = $"دفعية مورد - طلب رقم {dto.OrderId}",
                    TotalAmount = dto.Amount,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.UtcNow,
                    IsPosted = true,
                    PostedDate = DateTime.UtcNow,
                    PostedBy = dto.CreatedBy
                };

                _context.JournalEntries.Add(entry);
                await _context.SaveChangesAsync();

                // Create journal entry details
                var details = new List<JournalEntryDetail>
                {
                    new JournalEntryDetail
                    {
                        EntryId = entry.EntryId,
                        AccountId = supplierAccount.AccountId,
                        Debit = dto.Amount,
                        Credit = 0,
                        Description = $"دفعية للمورد {dto.SupplierId}"
                    },
                    new JournalEntryDetail
                    {
                        EntryId = entry.EntryId,
                        AccountId = bankAccount.AccountId,
                        Debit = 0,
                        Credit = dto.Amount,
                        Description = $"دفع من البنك للمورد {dto.SupplierId}"
                    }
                };

                _context.JournalEntryDetails.AddRange(details);
                await _context.SaveChangesAsync();

                _logger.LogInformation("تم إنشاء قيد يومية تلقائي لدفعية المورد {SupplierId} للمبلغ {Amount}", 
                    dto.SupplierId, dto.Amount);

                return CreatedAtAction(nameof(GetJournalEntry), new { id = entry.EntryId }, entry);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في إنشاء قيد يومية دفعية المورد");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion

        #region التقارير

        [HttpGet("trial-balance")]
        public async Task<ActionResult<TrialBalanceResponseDto>> GetTrialBalance([FromQuery] TrialBalanceRequestDto request)
        {
            try
            {
                var query = _context.ChartOfAccounts
                    .Where(coa => coa.IsActive)
                    .AsQueryable();

                if (request.AccountId.HasValue)
                    query = query.Where(coa => coa.AccountId == request.AccountId.Value);

                if (!string.IsNullOrEmpty(request.AccountType))
                    query = query.Where(coa => coa.AccountType == request.AccountType);

                var accounts = await query.ToListAsync();

                var accountBalances = new List<AccountBalanceResponseDto>();

                foreach (var account in accounts)
                {
                    var debitBalance = await _context.JournalEntryDetails
                        .Where(jed => jed.AccountId == account.AccountId && jed.JournalEntry.IsPosted)
                        .SumAsync(jed => jed.Debit);

                    var creditBalance = await _context.JournalEntryDetails
                        .Where(jed => jed.AccountId == account.AccountId && jed.JournalEntry.IsPosted)
                        .SumAsync(jed => jed.Credit);

                    var netBalance = debitBalance - creditBalance;
                    var isDebit = netBalance >= 0;

                    accountBalances.Add(new AccountBalanceResponseDto
                    {
                        AccountId = account.AccountId,
                        AccountCode = account.AccountCode,
                        AccountName = account.AccountName,
                        AccountType = account.AccountType,
                        DebitBalance = isDebit ? Math.Abs(netBalance) : 0,
                        CreditBalance = !isDebit ? Math.Abs(netBalance) : 0,
                        NetBalance = Math.Abs(netBalance),
                        IsDebit = isDebit
                    });
                }

                var trialBalance = new TrialBalanceResponseDto
                {
                    AsOfDate = request.AsOfDate,
                    AccountBalances = accountBalances,
                    TotalDebits = accountBalances.Sum(ab => ab.DebitBalance),
                    TotalCredits = accountBalances.Sum(ab => ab.CreditBalance)
                };

                trialBalance.IsBalanced = trialBalance.TotalDebits == trialBalance.TotalCredits;

                return Ok(trialBalance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع ميزان المراجعة");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        [HttpGet("income-statement")]
        public async Task<ActionResult<IncomeStatementResponseDto>> GetIncomeStatement([FromQuery] IncomeStatementRequestDto request)
        {
            try
            {
                var revenueAccounts = await _context.ChartOfAccounts
                    .Where(coa => coa.AccountType == "Revenue" && coa.IsActive)
                    .ToListAsync();

                var expenseAccounts = await _context.ChartOfAccounts
                    .Where(coa => coa.AccountType == "Expense" && coa.IsActive)
                    .ToListAsync();

                var revenueBalances = new List<AccountBalanceResponseDto>();
                var expenseBalances = new List<AccountBalanceResponseDto>();

                // Calculate revenue balances
                foreach (var account in revenueAccounts)
                {
                    var creditBalance = await _context.JournalEntryDetails
                        .Where(jed => jed.AccountId == account.AccountId && 
                                     jed.JournalEntry.IsPosted &&
                                     jed.JournalEntry.EntryDate >= request.FromDate &&
                                     jed.JournalEntry.EntryDate <= request.ToDate)
                        .SumAsync(jed => jed.Credit);

                    if (creditBalance > 0)
                    {
                        revenueBalances.Add(new AccountBalanceResponseDto
                        {
                            AccountId = account.AccountId,
                            AccountCode = account.AccountCode,
                            AccountName = account.AccountName,
                            AccountType = account.AccountType,
                            DebitBalance = 0,
                            CreditBalance = creditBalance,
                            NetBalance = creditBalance,
                            IsDebit = false
                        });
                    }
                }

                // Calculate expense balances
                foreach (var account in expenseAccounts)
                {
                    var debitBalance = await _context.JournalEntryDetails
                        .Where(jed => jed.AccountId == account.AccountId && 
                                     jed.JournalEntry.IsPosted &&
                                     jed.JournalEntry.EntryDate >= request.FromDate &&
                                     jed.JournalEntry.EntryDate <= request.ToDate)
                        .SumAsync(jed => jed.Debit);

                    if (debitBalance > 0)
                    {
                        expenseBalances.Add(new AccountBalanceResponseDto
                        {
                            AccountId = account.AccountId,
                            AccountCode = account.AccountCode,
                            AccountName = account.AccountName,
                            AccountType = account.AccountType,
                            DebitBalance = debitBalance,
                            CreditBalance = 0,
                            NetBalance = debitBalance,
                            IsDebit = true
                        });
                    }
                }

                var incomeStatement = new IncomeStatementResponseDto
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    TotalRevenue = revenueBalances.Sum(rb => rb.NetBalance),
                    TotalExpenses = expenseBalances.Sum(eb => eb.NetBalance),
                    RevenueAccounts = revenueBalances,
                    ExpenseAccounts = expenseBalances
                };

                incomeStatement.NetIncome = incomeStatement.TotalRevenue - incomeStatement.TotalExpenses;

                return Ok(incomeStatement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطأ في استرجاع قائمة الدخل");
                return StatusCode(500, "خطأ داخلي في الخادم");
            }
        }

        #endregion
    }
}
