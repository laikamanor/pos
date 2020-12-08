using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AB.UI_Class
{

    class utility_class
    {
        //public string URL = "http://192.168.9.246:5000";
        public string URL = System.IO.File.ReadAllText("URL.txt");
    }
}
