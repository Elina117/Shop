using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.project
{
    internal class User
    {
        public int user_signin_id;
        public string user_signin_login;
        public string user_signin_password;
        public string user_signin_email;
        public string user_lc_name;
        public int user_signin_code;

        public int rec_id;
        public string rec_name;
        public string rec_characteristic;
        public string rec_description;

        public void Clear()
        {
            user_signin_id = 0;
            user_signin_login = string.Empty;
            user_signin_password = string.Empty;
            user_signin_email = string.Empty;
            user_lc_name = string.Empty;
            user_signin_code = 0;

            rec_id = 0;
            rec_name = string.Empty;
            rec_characteristic = string.Empty;
            rec_description = string.Empty;

        }
       
    }


}
