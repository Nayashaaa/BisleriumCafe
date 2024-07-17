namespace BisleriumCafe.Models
{
    //This class is responsible for storing the transaction data of the user
    internal class UserTransactionData
    {
        public long UserId { get; set; }
        public DateTime SalesDate { get; set; }
    }
}
