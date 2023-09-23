using System;
using Xunit;
using SampleShop.Services;
using SampleShop.Events;
using SampleShop.Tests.Mocks;
using SampleShop.Utilities;

namespace SampleShop.Tests
{
    public class TransactionServiceTests
    {
        private TransactionService CreateTransactionService()
        {
            return new TransactionService();
        }

        private AuditService CreateAuditService()
        {
            var logger = new Logger();
            return new AuditService(logger);
        }


        [Fact]
        public void When_MakeDepositRunsWithValidAmount_TheRightEventIsCalledAndRightMessageIsLogged()
        {
            // Arrange
            decimal amount = 100;
            var service = CreateTransactionService();
            string logtoCheck = $"AUDIT LOG: {TransactionType.Deposit} for ${amount} processed";
            var auditService = CreateAuditService();
            auditService.Subscribe(service);


            // Act
            // Assert
            var receivedEvent = Assert.Raises<TransactionProcessedEventArgs>(
                                         a => service.OnTransactionProcessed += a,
                                         a => service.OnTransactionProcessed -= a,
                                         () => service.MakeDeposit(amount));
            Assert.NotNull(receivedEvent);
            Assert.Equal(amount, receivedEvent.Arguments.Amount);
            Assert.Equal(TransactionType.Deposit, receivedEvent.Arguments.TransactionType);


        }


        [Fact]
        public void When_MakeWithdrawalRunsWithValidAmount_TheRightEventIsCalledAndRightMessageIsLogged()
        {
            // Arrange
            decimal amount = 120;
            var service = CreateTransactionService();
            string logtoCheck = $"AUDIT LOG: {TransactionType.Withdrawal} for ${amount} processed";
            var auditService = CreateAuditService();
            auditService.Subscribe(service);


            // Act
            // Assert
            var receivedEvent = Assert.Raises<TransactionProcessedEventArgs>(
                                         a => service.OnTransactionProcessed += a,
                                         a => service.OnTransactionProcessed -= a,
                                         () => service.MakeWithdrawal(amount));
            Assert.NotNull(receivedEvent);
            Assert.Equal(amount, receivedEvent.Arguments.Amount);
            Assert.Equal(TransactionType.Withdrawal, receivedEvent.Arguments.TransactionType);


        }

    }
}
