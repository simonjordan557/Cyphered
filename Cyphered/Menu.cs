using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;

namespace Cyphered
{
    class Menu
    {
        public Action currentMenu;
        public  Action previousMenu;
        public Personnel personnelReference;


        public void SelectUserMenu()
        {
            previousMenu = currentMenu;
            currentMenu = SelectUserMenu;
            Console.Clear();
            Console.WriteLine("PRESS THE APPROPRIATE NUMBER ON YOUR KEYBOARD: ");
            Console.WriteLine($"\n1: LOGIN EXISTING USER\n2: CREATE NEW USER\n3: LOGOUT\n4: EXIT TO DESKTOP");
            ConsoleKey keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.NumPad1:
                case ConsoleKey.D1:
                    {
                        if (personnelReference.currentUser != null)
                        {
                            Console.WriteLine("Logging in a new user will log the current user out.");
                            if (!Helper.AreYouSure())
                            {
                                currentMenu();
                            }
                        }
                        personnelReference.Login();
                        break;
                    }

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        if (personnelReference.currentUser != null)
                        {
                            Console.WriteLine("Creating a new user will log the current user out.");
                            if (!Helper.AreYouSure())
                            {
                                currentMenu();
                            }
                        }
                        personnelReference.CreateAndSaveNewUser();
                        break;
                    }

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        if (personnelReference.currentUser == null)
                        {
                            Console.WriteLine("No users are currently logged in.");
                            Thread.Sleep(Helper.threadSleep);
                            currentMenu();
                        }

                        else
                        {
                            if (Helper.AreYouSure())
                            {
                                personnelReference.Logout();
                            }

                            else
                            {
                                currentMenu();
                            }
                        }
                        break;
                    }

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        if (Helper.AreYouSure())
                        {
                            Environment.Exit(0);
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Invalid response.");
                        Thread.Sleep(Helper.threadSleep);
                        currentMenu();
                        break;
                    }
            }
        }

        public void SelectKeyMenu()
        {
            previousMenu = currentMenu;
            currentMenu = SelectKeyMenu;
            Console.Clear();
            Console.WriteLine("PRESS THE APPROPRIATE NUMBER ON YOUR KEYBOARD: ");
            Console.WriteLine($"\n1: ENTER KEY\n2: LOAD KEY FROM FILE\n3: GENERATE NEW KEY\n4: PREVIOUS MENU\n5: LOGOUT\n6: EXIT TO DESKTOP");
            ConsoleKey keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        int key = Key.GenerateKeyFromConsole();
                        personnelReference.currentUser.Cipher = new Cypher(key, personnelReference.currentUser);
                        CypherActionsMenu();
                        break;
                    }

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        int key = Key.GenerateKeyFromFile();
                        personnelReference.currentUser.Cipher = new Cypher(key, personnelReference.currentUser);
                        CypherActionsMenu();
                        break;
                    }

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        int key = Key.GenerateRandomKey();
                        personnelReference.currentUser.Cipher = new Cypher(key, personnelReference.currentUser);
                        CypherActionsMenu();
                        break;
                    }

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        previousMenu();
                        break;
                    }

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    {
                        if (Helper.AreYouSure())
                        {
                            personnelReference.Logout();
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    {
                        if (Helper.AreYouSure())
                        {
                            Environment.Exit(0);
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Invalid response.");
                        Thread.Sleep(Helper.threadSleep);
                        currentMenu();
                        break;
                    }
            }
        }

        public void CypherActionsMenu()
        {
            previousMenu = currentMenu;
            currentMenu = CypherActionsMenu;
            Console.Clear();
            Console.WriteLine("PRESS THE APPROPRIATE NUMBER ON YOUR KEYBOARD: ");
            Console.WriteLine($"\n1: ENCRYPT MESSAGE (KEYBOARD ENTRY)\n2: ENCRYPT MESSAGE FROM .txt FILE\n");
            Console.WriteLine($"\n3: DECRYPT MESSAGE (KEYBOARD ENTRY)\n4: DECRYPT MESSAGE FROM .txt FILE\n");
            Console.WriteLine($"\n5: SHOW CURRENT KEY ONSCREEN\n6: SAVE CURRENT KEY TO FILE\n7: USE A DIFFERENT KEY\n");
            Console.WriteLine($"\n8: LOGOUT\n9: EXIT TO DESKTOP\n");
            ConsoleKey keyPressed = Console.ReadKey().Key;

            switch (keyPressed)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    {
                        personnelReference.currentUser.Cipher.EncryptMessageFromConsole();
                        Console.ReadLine();
                        currentMenu();
                        break;
                    }

                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    {
                        personnelReference.currentUser.Cipher.EncryptMessageFromFile();
                        Console.ReadLine();
                        currentMenu();
                        break;
                    }

                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    {
                        personnelReference.currentUser.Cipher.DecryptMessageFromConsole();
                        Console.ReadLine();
                        currentMenu();
                        break;
                    }

                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    {
                        personnelReference.currentUser.Cipher.DecryptMessageFromFile();
                        Console.ReadLine();
                        currentMenu();
                        break;
                    }

                case ConsoleKey.D5:
                case ConsoleKey.NumPad5:
                    {
                        Console.WriteLine("Confirm your login details: ");
                        List<User> currentUserAsList = new List<User>() { personnelReference.currentUser };
                        Thread.Sleep(Helper.threadSleep);
                        User checkedUser = Helper.GetAndValidateLogin(currentUserAsList);
                        if (checkedUser.Equals(personnelReference.currentUser))
                        {
                            Console.WriteLine(personnelReference.currentUser.Cipher.ToString());
                            System.Timers.Timer timer = new System.Timers.Timer(Helper.timeout);
                            timer.Start();
                            timer.Elapsed += Timer_Elapsed;
                            
                            Console.WriteLine($"\nSCREEN WILL CLEAR AFTER {Helper.timeout / 1000} SECONDS.");
                            Console.ReadLine();
                            timer.Stop();
                            currentMenu();
                        }

                        else
                        {
                            Console.WriteLine("As login was not authenticated, logging out.");
                            Thread.Sleep(Helper.threadSleep);
                            personnelReference.Logout();
                        }
                        break;
                    }

                case ConsoleKey.D6:
                case ConsoleKey.NumPad6:
                    {
                        Console.WriteLine("Confirm your login details: ");
                        Thread.Sleep(Helper.threadSleep);
                        int keyToSave = personnelReference.currentUser.Cipher.Seed;

                        if (keyToSave < 0)
                        {
                            Console.WriteLine($"As login was not authenticated, logging out.");
                            Thread.Sleep(Helper.threadSleep);
                            personnelReference.Logout();
                        }

                        else
                        {
                            var didKeySave = Key.SaveKeyToFile(keyToSave, personnelReference.currentUser.Cipher.outputDirectory);
                            if (didKeySave.Item1)
                            {
                                Console.WriteLine($"Key saved successfully to {didKeySave.Item2}.");
                                Thread.Sleep(Helper.threadSleep);
                                currentMenu();
                            }

                            else
                            {
                                Console.WriteLine($"There was a problem, please try again.");
                                Thread.Sleep(Helper.threadSleep);
                                currentMenu();
                            }
                        }
                        break;
                    }

                case ConsoleKey.D7:
                case ConsoleKey.NumPad7:
                    {
                        Console.WriteLine("This will cancel the current key, please ensure you have written it down or saved it to a file.");
                        if (Helper.AreYouSure())
                        {
                            personnelReference.currentUser.Cipher = null;
                            SelectKeyMenu();
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                case ConsoleKey.D8:
                case ConsoleKey.NumPad8:
                    {
                        if (Helper.AreYouSure())
                        {
                            personnelReference.Logout();
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                case ConsoleKey.D9:
                case ConsoleKey.NumPad9:
                    {
                        if (Helper.AreYouSure())
                        {
                            Environment.Exit(0);
                        }

                        else
                        {
                            currentMenu();
                        }
                        break;
                    }

                default:
                    {
                        Console.WriteLine("Invalid response.");
                        Thread.Sleep(Helper.threadSleep);
                        currentMenu();
                        break;
                    }
            }

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer t = sender as System.Timers.Timer;
            if (t != null)
            {
                t.Stop();
            }
            sender = null;
            Console.Clear();
            Console.WriteLine("TIMED OUT: SCREEN CLEARED FOR YOUR SECURITY. PRESS ENTER TO CONTINUE.");
        }
    }
}
