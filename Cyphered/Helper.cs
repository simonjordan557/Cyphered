using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Cyphered
{
    public static class Helper
    {
        public const int threadSleep = 1000;
        public const int timeout = 30000;

        /// <summary>
        /// Displays a prompt to the user to input a string, then checks the string is valid according to a specified function.
        /// </summary>
        /// <param name="promptDisplayedToUser">the prompt displayed to the user.</param>
        /// <param name="func">The method which applies the validity check on the input string.</param>
        /// <returns>The user's input string, when it is valid.</returns>
        public static string PromptForStringInput(string promptDisplayedToUser, Func<string, bool> func)
        {
            string input;
            Console.Clear();
            // Keep prompting for input until the user supplies valid input.
            do
            {
                Console.WriteLine(promptDisplayedToUser);
                input = Console.ReadLine();
            }
            while (!func(input));

            // Return the input once it is deemed valid.
            return input;
        }
        /// <summary>
        /// Checks that the input is a positive integer between 0 and 2,147,483,647.
        /// </summary>
        /// <param name="input">The key string supplied to the method.</param>
        /// <returns>Whether the key string can be converted to a key integer.</returns>
        public static bool ValidateKey(string input)
        {
            input.Trim();
            if (!int.TryParse(input, out int result))
            // It's not an integer.
            {
                Console.WriteLine($"That is not a valid key.\n");
                return false;
            }

            else if (!(result >= 0 && result <= int.MaxValue))
            {
                // It's an integer but may be negative.
                Console.WriteLine($"'{input}' is not in range: Provide a key between 0 and {int.MaxValue}.");
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check that the file exists at the given or default path, and is likely to be a key file.
        /// </summary>
        /// <param name="input">the full path to the key file.</param>
        /// <returns>Whether the key file is found.</returns>
        public static bool ValidateFilePathAsEncryptedKey(string input)
        {
            string suffix = @"\seed.bin";
            return ValidateFilePath(input, suffix);   
        }

        public static bool ValidateFilePathAsTxtFile(string input)
        {
            string suffix = ".txt";
            return ValidateFilePath(input, suffix);
        }

        public static bool ValidateFilePath(string input, string suffix)
        {
            input = input.Trim();
            if (input == "" || input == null)
            {
                if (File.Exists(Directory.GetCurrentDirectory() + suffix))
                {
                    // Is there a matching file at the default location?
                    return true;
                }
                else return false;
            }

            if (File.Exists(input))
            {
                if (input.Substring(input.Length - suffix.Length, suffix.Length) != suffix)
                {
                    // Does the file exist at the given path? Is it the correct file type (i.e. seed.txt or *.txt)?
                    Console.WriteLine($"Incorrect file supplied. File should be named ...{suffix}");
                    return false;
                }
                return true;
            }

            else
            {
                Console.WriteLine($"File not found! check your file path, or move your file to {Directory.GetCurrentDirectory()}\\");
                return false;
            }
        }

        
        public static User GetAndValidateLogin(List<User> userList)
        {
            string inputUsername;
            string inputPassword;
            User loginAttempt;

            Console.Clear();

            do
            {
                Console.WriteLine($"Enter your username: ");
                inputUsername = Console.ReadLine();
                Console.WriteLine($"Enter your password: ");
                inputPassword = Console.ReadLine();

                loginAttempt = new User(inputUsername, inputPassword);

                if (!userList.Contains(loginAttempt))
                {
                    Console.WriteLine("Username and / or password not recognised.");
                    Console.WriteLine("Do you want to try again? (Y/N): ");
                    ConsoleKey keyPressed = Console.ReadKey().Key;
                    if (keyPressed == ConsoleKey.Y)
                    {
                        continue;
                    }

                    else return null;
                }

                if (inputUsername == "" || inputUsername == null)
                {
                    Console.WriteLine("Username cannot be blank.");
                }

                if (inputPassword == "" || inputPassword == null)
                {
                    Console.WriteLine("Password cannot be blank.");
                }
            }
            while (!userList.Contains(loginAttempt) || inputUsername == "" || inputUsername == null || inputPassword == "" || inputPassword == null);

            return loginAttempt;
        }
        public static bool AreYouSure()
        {
            Console.WriteLine("Are you sure you want to proceed? (Y/N):");
            ConsoleKey keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.Y:
                    {
                        return true;
                    }

                case ConsoleKey.N:
                    {
                        return false;
                    }

                default:
                    {
                        Console.WriteLine("Invalid response.");
                        Thread.Sleep(threadSleep);
                        return false;
                    }
            }
        }

            /// <summary>
            /// Opens a file and returns the entire contents as a single string.
            /// </summary>
            /// <param name="path">The path of the file to open.</param>
            /// <returns>The text of the file.</returns>
            public static string ReadFromFile(string path)
            {
                FileStream fs = File.Open(path, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                StringBuilder sb = new StringBuilder();

                sb.Append(sr.ReadToEnd());
                sr.Close();
                return sb.ToString();
            }
        public static bool WriteToFile(string path, string content)
        {
            FileStream fs = File.Open(path, FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(content);
            sw.Close();
            return true;
        }
    }
}
