using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using testingEnvironmentApp.Models;

namespace testingEnvironmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        [HttpPost]
        [Route("Registration")]
        public string Registration(User user)
        {
            string sqlDatasource = _configuration.GetConnectionString("LoginDBCon");
            SqlCommand cmd = null;

            using (SqlConnection conn = new SqlConnection(sqlDatasource))
            {
                string msg = string.Empty;
                try
                {
                    cmd = new SqlCommand("usp_Registration", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", user.Login);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (i > 0)
                    {
                        msg = "Data inserted";
                    }
                    else
                    {
                        msg = "Error";
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                return msg;
            }
        }


        [HttpPost]
        [Route("Login")]
        public string Login(LoginRequest loginRequest)
        {
            string sqlDatasource = _configuration.GetConnectionString("LoginDBCon");
            SqlCommand cmd = null;
            SqlDataAdapter da = null;

            using (SqlConnection conn = new SqlConnection(sqlDatasource))
            {
                string msg = string.Empty;
                string stringValue = string.Empty;
                string stringValue2 = string.Empty;
                try
                {
                    cmd = new SqlCommand("usp_Login", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Login", loginRequest.Login);
                    cmd.Parameters.AddWithValue("@Password", loginRequest.Password);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.HasRows)
                    {
                        
                        // Tutaj możesz odczytać dane zwrócone przez procedurę
                        while (reader.Read())
                        {
                            // Przykład: odczytanie wartości z pierwszej kolumny
                            int intValue = reader.GetInt32(0);  // Dla kolumny typu int
                            stringValue2 = reader.GetString(1);
                            stringValue = reader.GetString(3); // Dla kolumny typu string
                            //Debug.WriteLine(stringValue);
                            // Możesz dodać logikę obsługującą odczytane dane
                        }
                        msg = "User is valid and this is " + stringValue + "_" + stringValue2;
                    }
                    else
                    {
                        msg = "User is Invalid";
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }

                return msg;
            }
        }

    }
}
