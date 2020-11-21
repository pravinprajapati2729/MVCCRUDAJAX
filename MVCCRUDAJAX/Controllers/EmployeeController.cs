using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCCRUDAJAX.Models;


namespace MVCCRUDAJAX.Controllers
{
    //[ValidateAntiForgeryToken()]
    public class EmployeeController : Controller
    {
        // GET: Employee
        string strConstring = @"Data Source=LAPTOP-NDPOD62O\W4U;Initial Catalog=MVCCRUD;User ID=sa;Password=Deltabrav0";

        public ActionResult LoadEmp()
        {
            return View();
        }

        public ActionResult Enter()
        {
            return View("EnterEmployee", new Employee());
        }

        [HttpGet]
        public ActionResult EditEmp(string id)
        {
            

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strConstring))
            {

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from tbl_Employee_Details where  n_Emp_Id="+id+"", con);
                da.Fill(dt);
            }
           
                Employee objemp = new Employee();
                objemp.EmpId = Convert.ToInt32(dt.Rows[0]["n_Emp_Id"]);
                objemp.EmployeeName = dt.Rows[0]["s_Emp_Name"].ToString();
                objemp.City = dt.Rows[0]["s_Emp_City"].ToString();
                objemp.Salary = dt.Rows[0]["n_Emp_Salary"].ToString();
             
            return View("EnterEmployee", objemp);
        }



        [HttpPost]
        public ActionResult SaveEmp(string inXML)
        {
            if(ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(strConstring))
                {

                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_InsertEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@XMLParam", inXML);
                    cmd.ExecuteNonQuery();
                }

            }
            return View("EnterEmployee", new Employee());
        }

        [HttpPost]
        public ActionResult SaveEmpUsingModel(Employee objEmp)
        {
            string EmployeeName = objEmp.EmployeeName;
            string EmployeeCity = objEmp.City;
            string EmployeeSalary = objEmp.Salary;


            using (SqlConnection con = new SqlConnection(strConstring))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("Insert into tbl_Employee_Details Values(@EmpName,@EmpCity,@Salary)", con);
                cmd.Parameters.AddWithValue("@EmpName", EmployeeName);
                cmd.Parameters.AddWithValue("@EmpCity", EmployeeCity);
                cmd.Parameters.AddWithValue("@Salary", EmployeeSalary);
                cmd.ExecuteNonQuery();
            }

            //return RedirectToAction("LoadEmp");
            return View("EnterEmployee", new Employee());
        }
        [HttpPost]
        public ActionResult UpdateEmpUsingModel(Employee objEmp)
        {
            int EmpId = objEmp.EmpId;
            string EmployeeName = objEmp.EmployeeName;
            string EmployeeCity = objEmp.City;
            string EmployeeSalary = objEmp.Salary;


            using (SqlConnection con = new SqlConnection(strConstring))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("update tbl_Employee_Details set s_Emp_Name=@EmpName , s_Emp_City=@EmpCity, n_Emp_Salary=@Salary where n_Emp_Id=@EmpId", con);
                cmd.Parameters.AddWithValue("@EmpId", EmpId);
                cmd.Parameters.AddWithValue("@EmpName", EmployeeName);
                cmd.Parameters.AddWithValue("@EmpCity", EmployeeCity);
                cmd.Parameters.AddWithValue("@Salary", EmployeeSalary);
                cmd.ExecuteNonQuery();
            }

            //return RedirectToAction("LoadEmp");
            return View("EnterEmployee", new Employee());
        }


        public ActionResult GetCustomerData()
         {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(strConstring))
            {

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("select * from tbl_Employee_Details", con);
                da.Fill(dt);
            }
            List<Employee> listemp = new List<Employee>();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Employee objemp = new Employee();
                objemp.EmpId = Convert.ToInt32(dt.Rows[i]["n_Emp_Id"]);
                objemp.EmployeeName = dt.Rows[i]["s_Emp_Name"].ToString();
                objemp.City = dt.Rows[i]["s_Emp_City"].ToString();
                objemp.Salary = dt.Rows[i]["n_Emp_Salary"].ToString();
                listemp.Add(objemp);
            }

           // var jsonres = JsonConvert.SerializeObject(dt, Formatting.Indented);

            return Json(listemp, JsonRequestBehavior.AllowGet);
        }
    }
}