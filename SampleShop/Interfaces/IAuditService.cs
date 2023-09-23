using System;
using SampleShop.Events;

namespace SampleShop.Interfaces
{
    public interface IAuditService
    {
        void Subscribe(ITransactionService transactionService);
    }
}
