using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace dz
{
    public partial class Form1 : Form
    {
        // connection string to the database
        string conString = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        SqlConnection conn = null;
        SqlDataAdapter ad  = null;
        DataSet set        = null;
        DataTable table    = null;

        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = null;

            // Initialize classes

            conn = new SqlConnection(conString);
            ad = new SqlDataAdapter("select * from Books", conn);
            set = new DataSet();
            ad.Fill(set, "MyTable");
            dataGridView1.DataSource = set.Tables["MyTable"]; // view query result

            // adding update command with a parameters

            SqlCommand com = new SqlCommand("update Books set NameBook = @nb where ID_Book = @id", conn);
            com.Parameters.Add(new SqlParameter("@nb", SqlDbType.VarChar));
            com.Parameters["@nb"].SourceVersion = DataRowVersion.Current;
            com.Parameters["@nb"].SourceColumn = "NameBook";

            com.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
            com.Parameters["@id"].SourceVersion = DataRowVersion.Current;
            com.Parameters["@id"].SourceColumn = "ID_Book";

            ad.UpdateCommand = com;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // view changed rows
            dataGridView2.DataSource = set.Tables["MyTable"].GetChanges(DataRowState.Modified);
        }

        private void button2_Click(object sender, EventArgs e)
        { 
            // update table in the database
            table = set.Tables["MyTable"];
            ad.Update(table.Select(null, null, DataViewRowState.Deleted));
            ad.Update(table.Select(null, null, DataViewRowState.ModifiedCurrent));
            ad.Update(table.Select(null, null, DataViewRowState.Added));
        }
    }
}
