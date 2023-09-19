// Ignore Spelling: APICUSTOMERS

using APICUSTOMERS.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace APICUSTOMERS.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CustomersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /* Code for Get request of any new customer to the server and update the necessary   */
        [HttpGet]
        [Route("GetCustomerDetails")]
        [EnableCors("AllowAnyOrigin")]
        public ActionResult<List<Customers>> GetCustomerDetails()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM GetCustomerInfo();";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    List<Customers> customersList = new List<Customers>();

                    foreach (DataRow row in dt.Rows)
                    {
                        Customers customer = new Customers
                        {
                            Customer_ID = row["Customer_ID"].ToString(),
                            F_Name = row["First_Name"].ToString(),
                            L_Name = row["Last_Name"].ToString(),
                            C_Add = row["Address"].ToString(),
                            City = row["City"].ToString()
                        };

                        // Add valid customers to the list
                        if (!string.IsNullOrEmpty(customer.Customer_ID))
                        {
                            customersList.Add(customer);
                        }
                    }

                    return Ok(customersList);
                }
                else
                {
                    return NotFound("No customer data found.");
                }
            }
        }
       


        /* Code for Get request for the search of the customer in the server    */
        [HttpGet]
        [Route("SearchCustomer")]
        [EnableCors("AllowAnyOrigin")]
        public ActionResult<List<Customers>> SearchCustomer(string searchTerm)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string query = "SELECT * FROM CUSTOMER WHERE First_Name LIKE @SearchTerm OR Last_Name LIKE @SearchTerm;";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    da.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        List<Customers> customersList = new List<Customers>();

                        foreach (DataRow row in dt.Rows)
                        {
                            Customers customer = new Customers
                            {
                                Customer_ID = row["Customer_ID"].ToString(),
                                F_Name = row["First_Name"].ToString(),
                                L_Name = row["Last_Name"].ToString(),
                                C_Add = row["Address"].ToString(),
                                City = row["City"].ToString()
                            };

                            customersList.Add(customer);
                        }

                        return Ok(customersList);
                    }
                    else
                    {
                        return NotFound("No matching customers found.");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        /* Code for Post request of any new customer to the server and update the necessary   */
        [HttpPost]
        [Route("AddCustomer")]
        [EnableCors("AllowAnyOrigin")]
        public async Task<IActionResult> AddCustomer([FromBody] Customers newCustomer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    await con.OpenAsync();

                    string query = "EXEC InsertCustomerWithOutput @Coupon_ID, @FirstName, @LastName, @Address, @City";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", newCustomer.F_Name);
                        cmd.Parameters.AddWithValue("@LastName", newCustomer.L_Name);
                        cmd.Parameters.AddWithValue("@Address", newCustomer.C_Add);
                        cmd.Parameters.AddWithValue("@City", newCustomer.City);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            return Ok("Customer added successfully.");
                        }
                        else
                        {
                            return BadRequest("Failed to add customer.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
