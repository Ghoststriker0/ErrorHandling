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
    public partial class HomeWork : Form
    {
        private bool state = false;

        public HomeWork()
        {
            InitializeComponent();
        }

        private void HomeWork_Load(object sender, EventArgs e)
        {
            FillComboBox();
            cboYear.Enabled = false;
            
        }


        private DataTable GetData(string sql)
        {
            DataTable getData = new DataTable();
 
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

            return getData;

        }

        private void FillComboBox()
        {
            string sql = "SELECT CategoryID, CategoryName FROM Categories";

            DataTable dt = GetData(sql);

            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryID";
            cboCategory.DataSource = dt;

            cboYear.Items.Add("1996");
            cboYear.Items.Add("1997");
            cboYear.Items.Add("1998");
            
        }

        private void cboCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string sql = "SELECT ProductID, ProductName, CompanyName, QuantityPerUnit, UnitPrice, UnitsInStock " +
                "FROM Products INNER JOIN Suppliers " +
                "ON Products.SupplierID = Suppliers.SupplierID " +
                $"WHERE CategoryID = {cboCategory.SelectedValue}";

            DataTable dt = GetData(sql);

            dgvProduct.DataSource = dt;
            dgvProduct.Columns[0].Visible = false;
            dgvProduct.ReadOnly = true;
            dgvProduct.AutoResizeColumns();
            dgvProduct.ReadOnly = true;

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            string sql = "SELECT OrderID, CompanyName, LastName + ' ' + FirstName AS EmployeeName, OrderDate " +
                    "FROM Orders INNER JOIN Customers " +
                    "ON Orders.CustomerID = Customers.CustomerID " +
                    "INNER JOIN Employees " +
                    "ON Orders.EmployeeID = Employees.EmployeeID ";



            if (state == false)
            {
                sql += $"WHERE ShippedDate BETWEEN '{dtpStart.Value}' AND '{dtpEnd.Value}'";
                if(dtpStart.Value > dtpEnd.Value)
                {
                    MessageBox.Show("Make sure the start date is lower than the end date");
                    return;
                }
            }

            else
            {
                sql += $"WHERE ShippedDate BETWEEN '{Convert.ToInt32(cboYear.SelectedItem)}-01-01'" +
                    $" AND '{Convert.ToInt32(cboYear.SelectedItem)}-12-31'";
            }


            DataTable dt = GetData(sql);

            dgvProduct.DataSource = dt;


        }

        private void ChangeState()
        {
            cboYear.Enabled = !cboYear.Enabled;
            dtpStart.Enabled = !dtpStart.Enabled;
            dtpEnd.Enabled = !dtpEnd.Enabled;

            state = !state;
        }

        private void btnChangeState_Click(object sender, EventArgs e)
        {
            ChangeState();
        }
    }
}
