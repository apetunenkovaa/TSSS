using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Technical_Software_Service
{
    public partial class Users
    {
        public string NameUsers
        {
            get
            {
                if (MiddleName == null)
                {
                    return LastName + " " + FirstName[0] + ".";
                }
                else
                {
                    return LastName + " " + FirstName[0] + "." + " " + MiddleName[0] + ".";
                }
            }
        }
    }
}
