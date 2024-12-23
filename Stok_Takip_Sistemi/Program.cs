using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Stok_Takip_Sistemi
{
    static class Program
    {
       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new anaMenu());
        }
    }
}
