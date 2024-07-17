
namespace BisleriumCafe.Models
{
    //This class contains the details of the products and Add-Ins in the shop
    public class Item
    {
        public required string itemName { get; set; }
        public required float itemPrice { get; set; }
        public ItemType itemType { get; set; }


    }
}
