namespace BasicActivity
{
    public class LoanTransaction
    {
        public System.DateTime TransactionTimeStamp { get; set; }
        public TransactionType TypeOfTransaction { get; set; }
        public decimal Amount { get; set; }
    }

    public enum TransactionType
    {
        Repay,
        Borrow
    }
}