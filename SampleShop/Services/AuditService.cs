using System;
using System.Diagnostics;
using SampleShop.Events;
using SampleShop.Interfaces;

namespace SampleShop.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger _logger;
        public AuditService(ILogger log)
        {
            _logger = log;
        }

        /// <summary>
        /// Subscribes to TransactionService's OnTransactionProcessed and writes to log.
        /// </summary>
        public void Subscribe(ITransactionService transactionService)
        {

            transactionService.OnTransactionProcessed += (object o, TransactionProcessedEventArgs e) =>
            {
                _logger.WriteToLog($"AUDIT LOG: {e.TransactionType} for ${e.Amount} processed");
            };
        }

       
     
    
    }
}
