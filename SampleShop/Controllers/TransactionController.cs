using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;

using System;
using System.Collections.Generic;
using SampleShop.Interfaces;
using SampleShop.Model;

namespace SampleShop.Controllers
{
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionService;
        private IAuditService _auditService;

        public TransactionController(ITransactionService transactionService, IAuditService auditService)
        {
            _transactionService = transactionService;
            _auditService = auditService;
            _auditService.Subscribe(_transactionService);
        }

        [HttpPost("deposit")]
        public IActionResult MakeDeposit([FromQuery]decimal amount)
        {
            _transactionService.MakeDeposit(amount);
            return Ok();
        }

        [HttpPost("withdrawal")]
        public IActionResult MakeWithdrawal([FromQuery]decimal amount)
        {
            _transactionService.MakeWithdrawal(amount);
            return Ok();
        }
    }
}
