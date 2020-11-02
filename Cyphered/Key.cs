using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace Cyphered
{
    class Key
    {
        public static int GenerateRandomKey()
        {
            // Create a new randomly generated integer, to be used as a key.
            Random seed = new Random();
            return seed.Next();
        }

        public static int GenerateKeyFromConsole()
        {
            // Get and validate an integer from the user, to be used as a key.
            Func<string, bool> func = Helper.ValidateKey;
            string message = $"Enter your Key: ";
            string input = Helper.PromptForStringInput(message, func);
            return int.Parse(input);
        }

        public static int GenerateKeyFromFile()
        {
            // Get and validate a file from the user, decrypt it to reveal an integer to validate, to be used as a key.
            Func<string, bool> func = Helper.ValidateFilePathAsEncryptedKey;
            string message = $"Enter the full path of your encrypted key file, or leave blank to check default path: ";
            string input = Helper.PromptForStringInput(message, func);
            string filePath;
            if (input == "" || input == null)
            {
                // Use default path (This has already been checked in the Helper method).
                filePath = Directory.GetCurrentDirectory() + @"\seed.bin";
            }

            else
            {
                filePath = input;
            }

            // Read in data from the file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(filePath, FileMode.Open);
            EncryptedKey readKey = (EncryptedKey)bf.Deserialize(fs);
            string rawFile = readKey.keyToEncrypt;
            StringBuilder seedBuilder = new StringBuilder();
            // Decrypt the data to find the key candidate. (Garbage for 20 characters, every 21st character is part of the key).
            for (int i = 20; i < rawFile.Length; i += 21)
            {
                seedBuilder.Append(rawFile[i]);
            }
            // If the key candidate is validated, return it.
            string seedString = seedBuilder.ToString();
            if (Helper.ValidateKey(seedString))
            {
                return int.Parse(seedString);
            }

            else
            {
                Console.WriteLine("Could not obtain a valid key from the supplied file.\nPlease check your file path or try a new key file.");
                return GenerateKeyFromFile();
            }
        }

        public static (bool, string) SaveKeyToFile(int key, string folder)
        {
            Random rng = new Random();
            string filePath = folder + @"\seed.bin";
            string keyString = key.ToString();
            StringBuilder sb = new StringBuilder();

            // Encrypt the key. (Garbage for 20 characters, every 21st character is part of the key).
            for (int i = 0; i < keyString.Length; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    sb.Append(rng.Next(0, 10));
                }
                sb.Append(keyString[i]);
            }
            string encryptedKeyString = sb.ToString();
            EncryptedKey encryptedKey = new EncryptedKey(encryptedKeyString);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(filePath, FileMode.OpenOrCreate);
            bf.Serialize(fs, encryptedKey);
            return (true, filePath);
        }
    }

    [Serializable]

    public class EncryptedKey
    {
        public string keyToEncrypt;
        public EncryptedKey(string input)
        {
            keyToEncrypt = input;
        }
    }
        
}
