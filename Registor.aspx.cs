using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace DashBoard
{
    public partial class Registor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStatistics();
                LoadChart(); // Pass data to JavaScript
                BindGrid();  // Bind the GridView with data
                LoadGridData();
            }
            
        }
        private void LoadStatistics()
        {
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id=sa; Password=odpserver550810998@";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // Total Registrations
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Registrations", conn);
                lblTotalUsers.Text = cmd.ExecuteScalar().ToString();

                // Total Male
                cmd = new SqlCommand("SELECT COUNT(*) FROM Registrations WHERE Gender = 'male'", conn);
                lblTotalMale.Text = cmd.ExecuteScalar().ToString();

                // Total Female
                cmd = new SqlCommand("SELECT COUNT(*) FROM Registrations WHERE Gender = 'female'", conn);
                lblTotalFemale.Text = cmd.ExecuteScalar().ToString();
            }
        }

        private void LoadChart()
        {
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id=sa; Password=odpserver550810998@";
            string query = "SELECT Gender, COUNT(*) AS Total FROM Registrations GROUP BY Gender";
           

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Extract male and female registration counts
                int maleCount = dt.AsEnumerable().Where(row => row.Field<string>("Gender") == "male").Sum(row => row.Field<int>("Total"));
                int femaleCount = dt.AsEnumerable().Where(row => row.Field<string>("Gender") == "female").Sum(row => row.Field<int>("Total"));

                // Pass data to client-side using JavaScript
                ClientScript.RegisterStartupScript(this.GetType(), "chartData",
                    $"var maleRegistrations = {maleCount}; var femaleRegistrations = {femaleCount};", true);
            }
        }

        private void BindGrid()
        {
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id =sa; Password=odpserver550810998@";
            string query = "SELECT StudentId, FirstName, LastName,DateOfBirth,Email,PhoneNumber,Address,Gender, RegistrationDate FROM Registrations";  // Adjust query as needed

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
                LoadChart();
                LoadStatistics();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set the row to edit mode
            GridView1.EditIndex = e.NewEditIndex;
            BindGrid(); // Rebind the GridView to refresh data

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            // Cancel edit mode
            GridView1.EditIndex = -1;
            BindGrid(); // Rebind the GridView to refresh data

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Get the primary key of the row being edited
            int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            // Find the new values from the GridView controls
            GridViewRow row = GridView1.Rows[e.RowIndex];
            string firstName = ((TextBox)row.Cells[1].Controls[0]).Text;
            string lastName = ((TextBox)row.Cells[2].Controls[0]).Text;
            string email = ((TextBox)row.Cells[3].Controls[0]).Text;
            string gender = ((TextBox)row.Cells[4].Controls[0]).Text;

            // Update the database
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id=sa; Password=odpserver550810998@";
            string query = "UPDATE Registrations SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Gender = @Gender WHERE StudentId = @StudentId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", firstName);
                cmd.Parameters.AddWithValue("@LastName", lastName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Gender", gender);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                cmd.ExecuteNonQuery();
                BindGrid();
            }

            // Exit edit mode and rebind the GridView
            GridView1.EditIndex = -1;
            BindGrid();

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the primary key of the row being deleted
            int studentId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);

            // Delete the record from the database
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id=sa; Password=odpserver550810998@";
            string query = "DELETE FROM Registrations WHERE StudentId = @StudentId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentId", studentId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            // Rebind the GridView
            BindGrid();

        }

        private void LoadGridData(string searchQuery = null)
        {
            string connString = "Data Source=occweb05; initial catalog=TechnicalSupport; User Id =sa; Password=odpserver550810998@";
           // string query = "SELECT StudentId, FirstName, LastName,DateOfBirth,Email,PhoneNumber,Address,Gender, RegistrationDate FROM Registrations";  // Adjust query as needed

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("GetRegistrations", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameter for search query (if any)
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@SearchQuery", searchQuery);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@SearchQuery", DBNull.Value); // NULL for no search
                    }

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the data to GridView
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

        // The method to handle the search functionality
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();
            LoadGridData(searchQuery); // Load data with the search query
            LoadChart();
        }

    }
}