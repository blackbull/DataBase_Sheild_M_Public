using System;
using System.Windows.Forms;
using DataBase_Sheild_M;
using DB_Loader;

namespace Main_DB_Part_Loader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new LoaderForm());

            //DB_LoaderClass DBLoader = new DB_LoaderClass("Data Source=Bykov;Initial Catalog=MAIN_Sheild_M;Persist Security Info=True;User ID=sa;Password=sa", "Data Source=Bykov;Initial Catalog=Sheild_M_measurement;Persist Security Info=True;User ID=sa;Password=sa");
            //DBLoader.InitializeDB();

            //DataBaseForm databaseform = new DataBaseForm(DBLoader);

            //Application.Run(databaseform);
        }
    }
}
