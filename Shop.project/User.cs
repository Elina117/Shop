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

        public int rec_id;
        public string rec_name;
        public string rec_characteristic;
        public string rec_description;

        public void Clear()
        {
            id = 0;
            login = string.Empty;
            password = string.Empty;
            email = string.Empty;
            name = string.Empty;
            code = 0;

            rec_id = 0;
            rec_name = string.Empty;
            rec_characteristic = string.Empty;
            rec_description = string.Empty;

        }
       
    }


}
