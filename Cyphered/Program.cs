using System;
using System.IO;

namespace Cyphered
{
    class Program
    {
        static void Main(string[] args)
        {
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
