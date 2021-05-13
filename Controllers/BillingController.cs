using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicBilling.API.Model;
using BasicBilling.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BasicBilling.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillingController : ControllerBase
    {
        private readonly ILogger<BillingController> _logger;
        public readonly IBillingService _service;

        public BillingController(ILogger<BillingController> logger,
            IBillingService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("bills")]
        public async Task<IActionResult> CreateBills(CreateBillRequest request)
        {
            var result = await _service.CreateBillsForPeriod(request.Period, request.Category);

            return Ok(result);
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> GetBills([FromQuery]SearchBillFilter request)
        {
            var result = await _service.GetBillByFilter(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPendingBills([FromQuery]PendingBillsFilter request)
        {
            var searchFilter = new SearchBillFilter
            {
                Category = request.Category,
                ClientId = request.ClientId,
                Period = null,
                Status = "PENDING"
            };

            var result = await _service.GetBillByFilter(searchFilter);

            return Ok(result);
        }

        [HttpPost]
        [Route("pay")]
        public async Task<IActionResult> PayBills([FromBody] PayBillRequest request)
        {
            var result = await _service.PayBill(request.ClientId, request.Period, request.Category);

            return Ok(result);
        }

    }
}
