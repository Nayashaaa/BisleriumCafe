using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BisleriumCafe.Models.Services
{
    public static class UserService
    {
        // Seed values for the admin user.
        public const string seedUsername = "admin";
        public const string seedPassword = "password";

        // Saves the list of users to the JSON file.
        private static void SaveUser(List<User> users)
        {
            string appDataDirectory = Utils.GetAppDirectoryPath();
            string userFilePath = Utils.GetAppUsersFilePath();

            // Create the app data directory if it doesn't exist.
            if (!Directory.Exists(appDataDirectory))
            {
                Directory.CreateDirectory(appDataDirectory);
            }

            // Serialize and save the users to the file.
            var json = JsonSerializer.Serialize(users);
            File.WriteAllText(userFilePath, json);
        }

        // Retrieves the list of users from the JSON file.
        public static List<User> GetUser()
        {
            string userFilePath = Utils.GetAppUsersFilePath();

            // If the file doesn't exist, return an empty list.
            if (!File.Exists(userFilePath))
            {
                return new List<User>();
            }

            // Read the JSON from the file and deserialize it to a list of users.
            var json = File.ReadAllText(userFilePath);
            var users = JsonSerializer.Deserialize<List<User>>(json);
            return users;
        }

        // Creates a new user and adds it to the list.
        public static List<User> CreateUser(Guid userId, string username, string password, Role role)
        {
            List<User> users = GetUser();

            // Check if a user with the same username already exists.
            bool usernameExists = users.Any(x => x.Username == username);

            // If the username already exists, throw an exception.
            if (usernameExists)
            {
                throw new Exception("This username already exists!");
            }

            // Add the new user to the list.
            users.Add(
                new User
                {
                    Username = username,
                    PasswordHash = Utils.HashSecret(password),
                    Role = role
                }
            );

            // Save the updated list to the file.
            SaveUser(users);
            return users;
        }

        // Seeds the users with an admin user if none exists.
        public static void SeedUsers()
        {
            var users = GetUser().FirstOrDefault(x => x.Role == Role.Admin);

            // If there is no admin user, create one.
            if (users == null)
            {
                CreateUser(Guid.Empty, seedUsername, seedPassword, Role.Admin);
            }
        }

        // Deletes a user by their ID.
        public static List<User> DeleteUser(Guid ID)
        {
            List<User> users = GetUser();
            User user = users.FirstOrDefault(x => x.Id == ID);

            // If the user doesn't exist, throw an exception.
            if (user == null)
            {
                throw new Exception("No users to delete!");
            }

            // Remove the user from the list.
            users.Remove(user);

            // Save the updated list to the file.
            SaveUser(users);
            return users;
        }

        // Logs in a user based on the provided username and password.
        public static User LogIn(string username, string password)
        {
            List<User> users = GetUser();
            User user = users.FirstOrDefault(x => x.Username == username);

            // Error message for invalid username or password.
            var errorDialog = "Invalid username or password";

            // If the user doesn't exist, throw an exception.
            if (user == null)
            {
                throw new Exception(errorDialog);
            }

            // Check if the provided password matches the stored hash.
            bool pwMatched = Utils.VerifyHash(password, user.PasswordHash);

            // If the password doesn't match, throw an exception.
            if (!pwMatched)
            {
                throw new Exception(errorDialog);
            }

            return user;
        }
    }
}
