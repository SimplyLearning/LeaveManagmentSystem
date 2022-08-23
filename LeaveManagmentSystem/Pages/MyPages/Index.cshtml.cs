using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace LeaveManagmentSystem.Pages.MyPages
{
    
    public class IndexModel : PageModel
    {
        public EmployeeInfo info = new EmployeeInfo();

        // saving error message 
        public String errorMessage = "";

        // saving success message
        public String sucMessage = "";
        public void OnGet()
        {
          
        }

        public void OnPost()
        {
            info.name = Request.Form["name"];
            info.surname = Request.Form["surname"];
            info.leaveDate = Request.Form["leaveDate"];
            info.endDate = Request.Form["endDate"];
            info.typeOfLeave = Request.Form["typeOfLeave"];
            info.reason = Request.Form["reason"];

            // Validating fields
            if (info.name.Length == 0 || info.surname.Length == 0 || info.leaveDate.Length == 0 || info.endDate.Length == 0 || info.typeOfLeave.Length == 0 || info.reason.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            // saving the data into the table
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=LeaveManagementDatabase;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Employee " + "(name, surname, leaveDate, endDate, typeOfLeave, reason) VALUES "
                                + "(@name, @surname, @leaveDate, @endDate, @typeOfLeave, @reason);";

                    // creating an sql command to execute the query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", info.name);
                        command.Parameters.AddWithValue("@surname", info.surname);
                        command.Parameters.AddWithValue("@leaveDate", info.leaveDate);
                        command.Parameters.AddWithValue("@endDate", info.endDate);
                        command.Parameters.AddWithValue("@typeOfLeave", info.typeOfLeave);
                        command.Parameters.AddWithValue("@reason", info.reason);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            info.name = ""; info.surname = ""; info.leaveDate = ""; info.endDate = ""; info.typeOfLeave = ""; info.reason = "";
            sucMessage = "The Request has been sent for review";

            Response.Redirect("/MyPages/Create");
        }
    }
}
