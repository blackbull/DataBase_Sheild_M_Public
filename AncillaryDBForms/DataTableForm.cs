using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultOptionsClassLibrary;

namespace AncillaryDBForms
{
    public partial class DataTableForm : Form
    {
        public DataTableForm()
        {
            InitializeComponent();
        }

        public IList<ResultElementClass> DataList = new List<ResultElementClass>();

        protected List<ResultElementClass> _EditResult=null;
        public List<ResultElementClass> EditResult
        {
            get { return _EditResult; }
        }

        public bool EditMode = false;

        private DataGridViewRow CreateRow(List<string> StrList)
        {
            DataGridViewRow ret = new DataGridViewRow();

            foreach (string temp in StrList)
            {
                ret.Cells.Add(CreateCell(temp));
            }

            return ret;
        }

        private DataGridViewTextBoxCell CreateCell(string text)
        {
            DataGridViewTextBoxCell ret = new DataGridViewTextBoxCell();
            ret.Value = text;

            return ret;
        }

        private void FillColums(int NeedCount)
        {
            while (dataGridView1.Columns.Count < NeedCount)
            {
                DataGridViewColumn Column=new DataGridViewColumn();
                Column.Name=(dataGridView1.Columns.Count+1).ToString();

                dataGridView1.Columns.Add(Column);
            }
        }

        private void DataTableForm_Load(object sender, EventArgs e)
        {
            FillColums(3);

            foreach (ResultElementClass re in DataList)
            {
                DataGridViewRow row=new DataGridViewRow();

                DataGridViewTextBoxCell cellCoord=new DataGridViewTextBoxCell();
                cellCoord.Value=re.Cordinate;
                row.Cells.Add(cellCoord);

                DataGridViewTextBoxCell cellAmpl=new DataGridViewTextBoxCell();
                cellAmpl.Value=re.Ampl_dB;
                row.Cells.Add(cellAmpl);

                DataGridViewTextBoxCell cellPhase=new DataGridViewTextBoxCell();
                cellPhase.Value=re.Phase_degree;
                row.Cells.Add(cellPhase);


                dataGridView1.Rows.Add(row);
            }

            if (EditMode)
            {
                splitContainer1.Panel1Collapsed = false;
                dataGridView1.ReadOnly = false;
                dataGridView1.AllowUserToAddRows = true;
                dataGridView1.AllowUserToDeleteRows = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            _EditResult = new List<ResultElementClass>();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                double coord = Convert.ToDouble(row.Cells[0].Value);
                double ampl = Convert.ToDouble(row.Cells[1].Value);
                double phas = Convert.ToDouble(row.Cells[2].Value);

                _EditResult.Add(new ResultElementClass(coord, ampl, phas));
            }

            DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 44 && e.KeyChar != 45)
            {
                e.Handled = true;
            }
            else
            {
                this.buttonOK.Enabled = true;
            }
        }

        private void form1_KeyUp(object sender, KeyEventArgs e)
        {
            //if user clicked Shift+Ins or Ctrl+V (paste from clipboard)
            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V))
            {
                //отображаем кнопку сохранения
                this.buttonOK.Enabled = true;

                char[] rowSplitter = { '\r', '\n' };
                char[] columnSplitter = { '\t' };

                //get the text from clipboard
                IDataObject dataInClipboard = Clipboard.GetDataObject();
                string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);

                //split it into lines
                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);

                //get the row and column of selected cell in grid
                int r = dataGridView1.SelectedCells[0].RowIndex;
                int c = dataGridView1.SelectedCells[0].ColumnIndex;

                //add rows into grid to fit clipboard lines
                if (dataGridView1.Rows.Count < (r + rowsInClipboard.Length))
                    dataGridView1.Rows.Add(r + rowsInClipboard.Length - dataGridView1.Rows.Count);

                // loop through the lines, split them into cells and place the values in the corresponding cell.
                for (int iRow = 0; iRow < rowsInClipboard.Length; iRow++)
                {
                    //split row into cell values
                    string[] valuesInRow = rowsInClipboard[iRow].Split(columnSplitter);

                    //cycle through cell values
                    for (int iCol = 0; iCol < valuesInRow.Length; iCol++)
                    {
                        //assign cell value, only if it within columns of the grid
                        if (dataGridView1.ColumnCount - 1 >= c + iCol)
                            dataGridView1.Rows[r + iRow].Cells[c + iCol].Value = valuesInRow[iCol];
                    }
                }
            }
        }

        private void buttonAddData_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = true;

            double AddData = Convert.ToDouble(numericUpDown1.Value);
            int ColumnNumber = 0;

            if (sender == this.buttonAddCoord)
                ColumnNumber = 0;
            else if (sender == this.buttonAddAmpl)
                ColumnNumber = 1;
            else if (sender == this.buttonAddPhase)
                ColumnNumber = 2;


            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                double Data = Convert.ToDouble(row.Cells[ColumnNumber].Value);
                row.Cells[ColumnNumber].Value = Data + AddData;
            }
        }

        private void buttonReversDN_Click(object sender, EventArgs e)
        {

        }

        public static IList<ResultElementClass> ReversDN(IList<ResultElementClass> Data)
        {
            IList<ResultElementClass> ret = Data;




            return ret;
        }
       
    }
}
