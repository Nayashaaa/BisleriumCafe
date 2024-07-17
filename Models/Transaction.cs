
namespace BisleriumCafe.Models
{

    //This class stores the transaction details such as date, id, items, etc. of the coffee shop
    public class Transaction
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        public required List<Item> Items { get; set; }
        public List<int> ItemCount { get; set; }

        public long MemberId { get; set; }

        public DateTime SalesDate { get; set; }

        public float OrderTotal { get; set; }
    }
}
