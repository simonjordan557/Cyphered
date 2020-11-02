using System;
using System.IO;

namespace Cyphered
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create User directory on first run, create all necessary references.
            Personnel personnel;
            if (!Directory.Exists(Directory.GetCurrentDirectory() + @"\users"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\users");
                personnel = new Personnel();
            }

            else
            {
                personnel = new Personnel();
                personnel.LoadAllExistingUsers();
            }

            Menu menu = new Menu();
            menu.personnelReference = personnel;
            personnel.menuReference = menu;

            menu.SelectUserMenu();


        }
    }
}
