using System;
using System.Collections.Generic;
using System.Text;

namespace Cyphered
{
    [Serializable]
    public class User
    {
        private string userName;
        private string password;

        [NonSerialized]
        private Cypher cipher;

        public string UserName
        {
            get
            {
                return userName;
            }
        }

        
        public Cypher Cipher
        {
            get
            {
                return cipher;
            }

            set
            {
                cipher = value;
            }
        }

        public User(string u, string p)
        {
            userName = u;
            password = p;
        }

        public override string ToString()
        {
            return $"USER NAME: {userName}\nPASSWORD: ********";
        }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if (user != null)
            {
                return (this.userName == user.userName && this.password == user.password);
            }

            else return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

       


    }
}
