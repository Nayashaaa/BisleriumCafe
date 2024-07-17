
using System.Text.Json;

namespace BisleriumCafe.Models.Services
{
    internal class TransactionService
    {
        // Retrieves the list of transactions from the JSON file.
        public static List<Transaction> GetTransaction()
        {
            string transactionFilePath = Utils.GetAppTransactionFilePath();

            if (!File.Exists(transactionFilePath))
            {
                return new List<Transaction>();
            }

            // Read the JSON from the file and deserialize it to a list of transactions.
            var json = File.ReadAllText(transactionFilePath);
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(json);
            return transactions;
        }

        // Saves the list of transactions to a JSON file.
        public static void SaveTransaction(List<Transaction> transactions)
        {
            string transactionFilePath = Utils.GetAppTransactionFilePath();

            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(transactionFilePath, json);
        }

        public static List<Transaction> CreateTransaction(Transaction transactionData)
        {
            List<Transaction> transactions = GetTransaction();
            bool itemExists = transactions.Any(x => x.TransactionId == transactionData.TransactionId);

            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                transactions.Add(
                              new Transaction()
                              {
                                  TransactionId = new Guid(),
                                  Items = transactionData.Items,
                                  MemberId = transactionData.MemberId,
                                  SalesDate = DateTime.Now,
                                  OrderTotal = transactionData.OrderTotal,
                                  ItemCount = transactionData.ItemCount,
                              }
                          );
            }
            SaveTransaction(transactions);
            return transactions;
        }

        public static List<UserTransactionData> GetTransactionByUserId(long userId)
        {
            string transactionFilePath = Utils.GetAppTransactionFilePath();

            if (!File.Exists(transactionFilePath))
            {
                return new List<UserTransactionData>();
            }

            var json = File.ReadAllText(transactionFilePath);
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(json);

            var userTransactionData = transactions.Where(x => x.MemberId == userId && x.SalesDate.Month == DateTime.Now.Month);

            var dataCount = userTransactionData.Count();
            var data = new List<UserTransactionData>();

            foreach (var transaction in userTransactionData)
            {
                data.Add(new UserTransactionData()
                {
                    UserId = transaction.MemberId,
                    SalesDate = transaction.SalesDate,
                });
            }
            return data;
        }
    }
}
