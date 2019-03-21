using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataGridView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetCustomers_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Customers";
            DataTable dt = GetData(sql);

            dgvCustomer.DataSource = dt;

            //To change the header name
            dgvCustomer.Columns[1].HeaderCell.Value = "Company Name";

            //To resize the cell so that the info can fit in the cell
            dgvCustomer.AutoResizeColumns();

            //make the gridview read-only
            dgvCustomer.ReadOnly = true;

            //Remove the column header
            //dataGridView1.ColumnHeadersVisible = false;

            //changes where the data will be displayed. The header will not move columns.
            dgvCustomer.Columns[1].HeaderText = "City";

            //This will hide the column of your choice
            //dataGridView1.Columns[0].Visible = false;

            //ColumnHeadersDefaultCellStyle in the properties will let you be able to change the colors and a few more properties.


        }

        private DataTable GetData(string sql)
        {
            DataTable getData = new DataTable();
            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.cnnString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(getData);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return getData;

        }
    }
}
