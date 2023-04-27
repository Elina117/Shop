using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.project
{
    internal class User
    {
        public int id;
        public string login;
        public string password;
        public string email;
        public string name;
        public int code;
        public string birthday;
        public int char_id;
        public int wish_id;

        public void Clear()
        {
            id = 0;
            login = string.Empty;
            password = string.Empty;
            email = string.Empty;
            name = string.Empty;
            code = 0;
            birthday = string.Empty;

            char_id = 0;
            wish_id = 0;

        }
       
    }


}
