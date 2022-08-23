using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace LeaveManagmentSystem.Pages.MyPages
{
    public class EditModel : PageModel
    {
        // creating the variables again 
        public EmployeeInfo info = new EmployeeInfo();
        public String errorMessage = "";
        public String sucMessage = "";

        public void OnGet()
        {
            String employeeID = Request.Query["employeeID"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=LeaveManagementDatabase;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Employee WHERE employeeID=@employeeID";

                    // creating an sql command to execute the query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@employeeID", employeeID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.employeeID = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.surname = reader.GetString(2);
                                info.leaveDate = reader.GetString(3);
                                info.endDate = reader.GetString(4);
                                info.reason = reader.GetString(5);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            info.employeeID = Request.Form["employeeID"];
            info.name = Request.Form["name"];
            info.surname = Request.Form["surname"];
            info.leaveDate = Request.Form["leaveDate"];
            info.endDate = Request.Form["endDate"];
            info.typeOfLeave = Request.Form["typeOfLeave"];
            info.reason = Request.Form["reason"];

            // Validating fields
            if (info.employeeID.Length == 0 || info.name.Length == 0 || info.surname.Length == 0 || info.leaveDate.Length == 0 || info.endDate.Length == 0 || info.typeOfLeave.Length == 0 || info.reason.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=LeaveManagementDatabase;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Employee " + "SET name=@name, surname=@surname, leaveDate=@leaveDate, endDate=@endDate, typeOfLeave=@typeOfLeave, reason=@reason " +
                                "WHERE employeeID=@employeeID"; // *******
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", info.name);
                        command.Parameters.AddWithValue("@surname", info.surname);
                        command.Parameters.AddWithValue("@leaveDate", info.leaveDate);
                        command.Parameters.AddWithValue("@endDate", info.endDate);
                        command.Parameters.AddWithValue("@typeOfLeave", info.typeOfLeave);
                        command.Parameters.AddWithValue("@reason", info.reason);
                        command.Parameters.AddWithValue("@employeeID", info.employeeID);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/MyPages/Create");
        }
    }
}
