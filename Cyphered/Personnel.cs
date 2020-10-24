using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Cyphered
{
    class Personnel
    {
        private List<User> userList;
        private string usersDirectory;
        private string userFileFormat;
        private string userFileSuffix;
        private BinaryFormatter bf;
        private FileStream fs;
        public User currentUser;
        private User loginAttempt;
        public Menu menuReference;

        public Personnel()
        {
            userList = new List<User>();
            userFileFormat = "*.usr";
            userFileSuffix = ".usr";
            usersDirectory = Directory.GetCurrentDirectory() + @"\users\";
            bf = new BinaryFormatter();
        }

        public void LoadAllExistingUsers()
        {
            string[] userFilesArray = Directory.GetFiles(usersDirectory, userFileFormat);
            if (userFilesArray.Length <= 0)
            {
                return;
            }

            User userToLoad;

            foreach (string userFile in userFilesArray)
            {
                fs = File.Open(userFile, FileMode.Open);
                userToLoad = (User)bf.Deserialize(fs);
                userList.Add(userToLoad);
                fs.Close();
            }
        }

        public void CreateAndSaveNewUser()
        {
            string inputUsername;
            string inputPassword;
            string confirmInputPassword;
            List<string> usernames = new List<string>();
            foreach (User user in userList)
            {
                usernames.Add(user.UserName);
            }

            Console.Clear();
            Console.WriteLine($"Enter a username: ");

            do
            {
                inputUsername = Console.ReadLine();

                if (usernames.Contains(inputUsername))
                {
                    Console.WriteLine($"{inputUsername} is already taken. Choose a different user name: ");
                }

                if (inputUsername == "" || inputUsername == null)
                {
                    Console.WriteLine($"User name cannot be empty. Enter a valid user name: ");
                }
            }
            while (usernames.Contains(inputUsername) || inputUsername == "" || inputUsername == null);

            Console.WriteLine($"Enter a password: ");

            do
            {
                inputPassword = Console.ReadLine();
                Console.WriteLine($"Re-enter your password to confirm: ");
                confirmInputPassword = Console.ReadLine();

                if (inputPassword != confirmInputPassword)
                {
                    Console.WriteLine($"Passwords do not match. Enter your password: ");
                }

                if (inputPassword == "" || inputPassword == null)
                {
                    Console.WriteLine($"Password cannot be empty. Enter your password: ");
                }

            }
            while (inputPassword != confirmInputPassword || inputPassword == "" || inputPassword == null);

            currentUser = new User(inputUsername, inputPassword);
            userList.Add(currentUser);

            bf = new BinaryFormatter();
            string fileToSave = usersDirectory + currentUser.UserName + userFileSuffix;
            fs = File.Create(fileToSave);
            bf.Serialize(fs, currentUser);
            fs.Close();

            Console.WriteLine($"New user created and logged in:\n{currentUser}\n");
            Thread.Sleep(Helper.threadSleep);
            menuReference.SelectKeyMenu();
        }

        public void Login()
        {
            loginAttempt = Helper.GetAndValidateLogin(userList);
            if (loginAttempt == null)
            {
                Console.WriteLine("Login attempt failed.");
                Thread.Sleep(Helper.threadSleep);
                menuReference.SelectUserMenu();
            }
            currentUser = loginAttempt;
            Console.WriteLine($"\n{currentUser.UserName} successfully logged in.");
            Thread.Sleep(Helper.threadSleep);
            menuReference.SelectKeyMenu();
        }

        public void Logout()
        {
            currentUser = null;
            menuReference.SelectUserMenu();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (User user in userList)
            {
                sb.Append(user.UserName + "\n");
            }

            return sb.ToString();
        }
    }
}
