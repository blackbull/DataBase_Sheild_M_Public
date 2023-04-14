using System;
using System.Collections.Generic;
using System.Text;

namespace TestingCalculationData
{
    /// <summary>
    /// используется для вывода измеренных данных в файл для проверки расчётов
    /// </summary>
    public class ResultSpesialClass
    {
       public List<TableString> AllTables = new List<TableString>();

       public void CreateReport()
       {
           System.Windows.Forms.RichTextBox rtb = new System.Windows.Forms.RichTextBox();

           foreach (TableString tsList in AllTables)
           {
               rtb.AppendText(string.Format("\n\n{0}\n", tsList.TableName));

               foreach (string str in tsList.StrTable)
               {
                   rtb.AppendText(string.Format("{0}\n", str));
               }
           }

           rtb.SaveFile("TEST_Result.txt", System.Windows.Forms.RichTextBoxStreamType.UnicodePlainText);
       }
    }

    public class TableString
    {
        public String TableName = "Имя не задано";

       public List<string> StrTable = new List<string>();
    }
}
