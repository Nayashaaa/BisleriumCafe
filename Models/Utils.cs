using System.Security.Cryptography;

namespace BisleriumCafe.Models
{
    public static class Utils
    {
        // Delimiter used to separate segments in the hashed secret string.
        private const char _segmentDelimiter = ':';

        // Hashes the input string using PBKDF2 with SHA256.
        public static string HashSecret(string input)
        {
            // Parameters for PBKDF2
            var saltSize = 16;
            var iterations = 100_000;
            var keySize = 32;
            HashAlgorithmName algorithm = HashAlgorithmName.SHA256;

            // Generate a random salt
            byte[] salt = RandomNumberGenerator.GetBytes(saltSize);

            // Perform PBKDF2 to derive the hash
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

            // Combine the hash, salt, iterations, and algorithm into a single string
            return string.Join(
                _segmentDelimiter,
                Convert.ToHexString(hash),
                Convert.ToHexString(salt),
                iterations,
                algorithm
            );
        }

        // Verifies if the input matches the hashed secret string.
        public static bool VerifyHash(string input, string hashString)
        {
            // Split the hashed secret string into segments
            string[] segments = hashString.Split(_segmentDelimiter);

            // Convert segments back to byte arrays and retrieve parameters
            byte[] hash = Convert.FromHexString(segments[0]);
            byte[] salt = Convert.FromHexString(segments[1]);
            int iterations = int.Parse(segments[2]);
            HashAlgorithmName algorithm = new(segments[3]);

            // Hash the input with the retrieved parameters
            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                input,
                salt,
                iterations,
                algorithm,
                hash.Length
            );

            // Compare the input hash with the stored hash in a constant time manner
            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        // Gets the application directory path.
        public static string GetAppDirectoryPath()
        {
            return Path.Combine(
              FileSystem.AppDataDirectory,
              "Jsons"
            );
        }

        // Gets the file path for storing user data.
        public static string GetAppUsersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "users.json");
        }

        // Gets the file path for storing transaction data.
        public static string GetAppTransactionFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "sales.json");
        }

        // Gets the file path for storing item data.
        public static string GetItemsFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "items.json");
        }

        // Gets the file path for storing member data.
        public static string GetMembersFilePath()
        {
            return Path.Combine(GetAppDirectoryPath(), "members.json");
        }
    }
}
