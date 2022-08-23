using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace LeaveManagmentSystem.Pages.MyPages
{
    public class CreateModel : PageModel
    {
        public List<EmployeeInfo> listEmployee = new List<EmployeeInfo>();
        public void OnGet()
        {
            try // error handling
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=LeaveManagementDatabase;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Employee";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo info = new EmployeeInfo();
                                info.employeeID = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.surname = reader.GetString(2);
                                info.leaveDate = reader.GetDateTime(3).ToString();
                                info.endDate = reader.GetDateTime(4).ToString();
                                info.typeOfLeave = reader.GetString(5);
                                info.reason = reader.GetString(6);

                                listEmployee.Add(info);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) // provding the error
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
    public class EmployeeInfo
    {
        public String employeeID;
        public String name;
        public String surname;
        public String leaveDate;
        public String endDate;
        public String typeOfLeave;
        public String reason;

    }
}
