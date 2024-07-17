namespace BisleriumCafe.Models
{

    //This class is responsible for storing the member's details of the app
    public class Member
    {
        public long UserID { get; set; }
        public int UpdatedNumberOfOrders { get; set; }
        public bool BenefitToClaim{ get; set; }=false;
    }
}