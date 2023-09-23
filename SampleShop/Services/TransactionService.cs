using System;
using SampleShop.Events;
using SampleShop.Interfaces;

namespace SampleShop.Services
{
    public class TransactionService : ITransactionService
    {
   
        public event EventHandler<TransactionProcessedEventArgs> OnTransactionProcessed;

        /// <summary>
        /// Processes a deposit and sends an event to every subsciber holding the amount and transactiontype.
        /// amount must be larger than 0.
        /// </summary>
        public void MakeDeposit(decimal amount) {
       
            if(amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            ProcessDeposit(amount);
            OnTransactionProcessed.Invoke(this, new TransactionProcessedEventArgs(amount, TransactionType.Deposit));
        }

        /// <summary>
        /// Processes a withdrawal and sends an event to every subsciber holding the amount and transactiontype.
        /// amount must be larger than 0.
        /// </summary>
        public void MakeWithdrawal(decimal amount)
        {
            
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            ProcessWithdrawal(amount);
            OnTransactionProcessed.Invoke(this, new TransactionProcessedEventArgs(amount, TransactionType.Withdrawal));
        }

        private void ProcessDeposit(decimal amount)
        {

        }

        private void ProcessWithdrawal(decimal amount)
        {

        }
    }
}
