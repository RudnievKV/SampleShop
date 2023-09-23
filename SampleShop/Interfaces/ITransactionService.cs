using System;
using SampleShop.Events;

namespace SampleShop.Interfaces
{
    public interface ITransactionService
    {
        event EventHandler<TransactionProcessedEventArgs> OnTransactionProcessed;

        void MakeDeposit(decimal amount);

        void MakeWithdrawal(decimal amount);
    }
}
