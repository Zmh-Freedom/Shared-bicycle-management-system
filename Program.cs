using login_register_Form1;
using shareDemo3;
using System;
using System.Windows.Forms;

namespace shareDemo2
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new CustomerForm());
            Application.Run(new loginForm());
            //Application.Run(new managerForm("12345678910"));
        }
    }
}
