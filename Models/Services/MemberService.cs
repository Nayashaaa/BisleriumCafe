
using System.Text.Json;

namespace BisleriumCafe.Models.Services
{
    public static class MemberService
    {
        // Retrieves the list of members from the JSON file.
        public static List<Member> GetMember()
        {
            string memberFilePath = Utils.GetMembersFilePath();

            if (!File.Exists(memberFilePath))
            {
                return new List<Member>();
            }

            // Read the JSON from the file and deserialize it to a list of members.
            var json = File.ReadAllText(memberFilePath);
            var members = JsonSerializer.Deserialize<List<Member>>(json);
            return members;
        }
        // Saves the list of members to a JSON file.
        public static void SaveMember(List<Member> members)
        {
            string memberFilePath = Utils.GetMembersFilePath();

            // Serialize the list of members to JSON and write it to the file.
            var json = JsonSerializer.Serialize(members);
            File.WriteAllText(memberFilePath, json);
        }

        // Creates a new member with the specified user ID.
        public static List<Member> CreateMember(long userId)
        {


            List<Member> members = GetMember();

            // Check if a member with the same user ID already exists.
            bool userExists = members.Any(x => x.UserID == userId);

            if (!userExists)
            {  
                members.Add(new Member()
                {
                    UserID = userId,
                    UpdatedNumberOfOrders = 0    
                });
            }
            SaveMember(members);
            return members;
        }

        public static List<Member> UpdateTransactionCount(long userID)
        {
            List<Member> members = GetMember();
            Member member = members.FirstOrDefault(members => members.UserID == userID);

            if (member == null)
            {
                CreateMember(userID);
            }
            else
            {
                member.UpdatedNumberOfOrders += 1;
            }
            SaveMember(members);
            return members;
        }

        public static List<Member> UpdateBenifitStatus(long userID)
        {
            List<Member> members = GetMember();
            Member member = members.FirstOrDefault(members => members.UserID == userID);

            if (member != null)
            {

                // Get the transaction count for the user and update the benefit status.
                int x = TransactionService.GetTransactionByUserId(userID).Count;
                if (x % 10 == 0)
                {
                    member.BenefitToClaim = true;
                }
                else
                {
                    member.BenefitToClaim = false;
                }
            }
            SaveMember(members);
            return members;
        }
    }
}
