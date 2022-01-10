using ngo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ngo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@QueryType", "getActiveEvents"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return View(ds);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Services()
        {
            return View();
        }
        public ActionResult EventDetail(int EventId)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@EventId", EventId));
                SqlParameters.Add(new SqlParameter("@QueryType", "getEventById"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return View(ds);
        }
        public ActionResult Events()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@QueryType", "getActiveEvents"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return View(ds);
        }
        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Contact()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> SqlParameters = new List<SqlParameter>();
            SqlParameters.Add(new SqlParameter("@LoginId", Convert.ToInt32(ConfigurationManager.AppSettings["userid"])));
            SqlParameters.Add(new SqlParameter("@QueryType", "getUser"));
            ds = DBManager.ExecuteDataSetWithParamiter("Proc_User_Authentication", CommandType.StoredProcedure, SqlParameters);
            return View(ds);
        }
        [HttpPost]
        public ActionResult ContactSave()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@Name", Request.Form["name"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address", Request.Form["Address"].ToString()));
                SqlParameters.Add(new SqlParameter("@MobileNumber", Request.Form["mobilenumber"].ToString()));
                SqlParameters.Add(new SqlParameter("@Message", Request.Form["message"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "insert"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_CntactInfo", CommandType.StoredProcedure, SqlParameters);
                ViewBag.Msg = ds.Tables[0].Rows[0]["msg"].ToString();

            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return View("Contact");
        }
        [HttpPost]
        public JsonResult Authenticate(string loginid = "", string password = "")
        {
            string result = "Fail";
            DataSet dt = new DataSet();
            DataTable loginAuth = new DataTable();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@LoginId", loginid));
                SqlParameters.Add(new SqlParameter("@Password", password));
                SqlParameters.Add(new SqlParameter("@QueryType", "login"));
                loginAuth = DBManager.ExecuteDataTableWithParamiter("Proc_User_Authentication", CommandType.StoredProcedure, SqlParameters);

                if (loginAuth.Rows.Count > 0)
                {
                    DataRow drUser = loginAuth.Rows[0];
                    clsUser obj = new clsUser();
                    Session["UserId"] = obj.UserId = Convert.ToInt32(drUser["UserId"]);
                    Session["LoginId"] = obj.LoginId = Convert.ToString(drUser["LoginId"]);
                    Session["UserName"] = obj.UserName = Convert.ToString(drUser["FirstName"]);
                    Session["UserType"] = obj.UserType = Convert.ToInt32(drUser["UserType"]);
                    result = "Success";
                }
                else
                {
                    ViewBag.Msg = "username and password is wrong..!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            if (Session["UserName"] != null)
            {
                //if (Session["UserType"].ToString() == "1")
                //{
                return View("~/Views/Admin/DashBoard.cshtml");
                //}
                //else
                //{
                //    return View("~/Views/Customer/DashBoard.cshtml");
                //}
            }
            else
            {
                return View("~/Views/Home/Login.cshtml");
            }
        }
        public ActionResult DashBoard()
        {
            DataSet dt = new DataSet();
            try
            {

            }
            catch (Exception)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return View("~/Views/Admin/DashBoard.cshtml");
        }
        [HttpPost]
        public ActionResult Authentic()
        {
            if (Request.Form["Email"].ToString() == "" && Request.Form["Email"].ToString() == "")
            {
                Session["UserName"] = "Ravi Shanker Pandey";
                Session["UserId"] = 1;
                Session["UserType"] = "Admin";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Msg = "Check Email Id And Password";
                return View("~/Views/Home/Login.cshtml");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("index", "home");
        }
    }
}