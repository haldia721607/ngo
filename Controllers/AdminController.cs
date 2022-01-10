using Newtonsoft.Json;
using ngo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ngo.Controllers
{
    [AuthFilter]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult DashBoard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Event(bool status=false)
        {
            if (status)
            {
                ViewBag.Msg = "Event updated successfully.";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Events()
        {
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@QueryType", "get"));
                DataSet ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Content(JsonConvert.SerializeObject(ds.Tables[0]));
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(null));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
                return Content(JsonConvert.SerializeObject(new { Status = "Error", Msg = "Something went wronge !" }));
            }
        }
        [HttpGet]
        public ActionResult AddEvent()
        {
            return View();

        }
        [HttpPost]
        public ActionResult AddEvent(HttpPostedFileBase[] httpPostedFileBases)
        {
            DataSet ds = new DataSet();
            bool isSaved = false;
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@Title", Request.Form["LongDescription"].ToString()));
                SqlParameters.Add(new SqlParameter("@Description", Request.Form["LongDescription"].ToString()));
                SqlParameters.Add(new SqlParameter("@EventDate", Request.Form["EventDate"].ToString()));
                SqlParameters.Add(new SqlParameter("@IsActive", Request.Form["IsActive"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "insert"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
                ViewBag.Msg = ds.Tables[0].Rows[0]["msg"].ToString();
                int eventId = Convert.ToInt32(ds.Tables[0].Rows[0]["EventId"].ToString());
                if (eventId > 0)
                {
                    if (httpPostedFileBases.Length > 0)
                    {
                        string path = Server.MapPath("~/uplodedimage/");
                        foreach (var item in httpPostedFileBases)
                        {
                            ds = new DataSet();
                            SqlParameters.Clear();
                            string imageUrl = string.Empty;
                            string imageName = string.Empty;
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            imageName = Convert.ToString(eventId) + "-" + item.FileName;
                            imageUrl = Path.Combine(path, imageName);
                            item.SaveAs(imageUrl);
                            SqlParameters.Add(new SqlParameter("@EventId", eventId));
                            SqlParameters.Add(new SqlParameter("@ImageName", item.FileName));
                            SqlParameters.Add(new SqlParameter("@ImageUrl", imageUrl));
                            SqlParameters.Add(new SqlParameter("@QueryType", "insertImage"));
                            ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
                            if (Convert.ToInt32(ds.Tables[0].Rows[0]["ImageId"].ToString()) > 0)
                            {
                                isSaved = true;
                            }
                            else
                            {
                                isSaved = false;
                                break;
                            }
                        }
                    }
                    for (int i = 1; i <= 4; i++)
                    {
                        string key = "VideoUrl_" + i;
                        if (Request.Form[key].Length > 0)
                        {
                            ds = new DataSet();
                            SqlParameters.Clear();

                            SqlParameters.Add(new SqlParameter("@EventId", eventId));
                            SqlParameters.Add(new SqlParameter("@VideoUrl", Request.Form[key].ToString()));
                            SqlParameters.Add(new SqlParameter("@QueryType", "insertVideo"));
                            ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
                            if (Convert.ToInt32(ds.Tables[0].Rows[0]["VideoId"].ToString()) > 0)
                            {
                                isSaved = true;
                            }
                            else
                            {
                                isSaved = false;
                                break;
                            }
                        }
                    }
                }
                if (isSaved)
                {
                    ViewBag.Msg = "Event add successfully.";
                }
                else
                {
                    ViewBag.Error = "some error occurred, please try again..!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateEvent(HttpPostedFileBase[] httpPostedFileBases)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@EventId", Request.Form["EventId"].ToString()));
                SqlParameters.Add(new SqlParameter("@Title", Request.Form["ShortDescription"].ToString()));
                SqlParameters.Add(new SqlParameter("@Description", Request.Form["LongDescription"].ToString()));
                SqlParameters.Add(new SqlParameter("@EventDate", Request.Form["EventDate"].ToString()));
                SqlParameters.Add(new SqlParameter("@IsActive", Request.Form["IsActive"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "updateEvent"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_EventMaster", CommandType.StoredProcedure, SqlParameters);
                if (ds.Tables[0].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Msg = "Event updated successfully.";
                }
                else
                {
                    ViewBag.Error = "some error occurred, please try again..!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return RedirectToAction("Event", new { status = true });
        }
        [HttpGet]
        public ActionResult UpcomingEvent(bool status=false)
        {
            if (status)
            {
                ViewBag.Msg = "Upcoming Event updated successfully.";
            }
            return View();
        }
        [HttpGet]
        public ActionResult UpcomingEvents()
        {
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@QueryType", "get"));
                DataSet ds = DBManager.ExecuteDataSetWithParamiter("usp_UpCommingEventMaster", CommandType.StoredProcedure, SqlParameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Content(JsonConvert.SerializeObject(ds.Tables[0]));
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(null));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
                return Content(JsonConvert.SerializeObject(new { Status = "Error", Msg = "Something went wronge !" }));
            }
        }
        [HttpGet]
        public ActionResult AddUpcomingEvents()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveUpcomingEvents()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@Title", Request.Form["Title"].ToString()));
                SqlParameters.Add(new SqlParameter("@SubTitle", Request.Form["SubTitle"].ToString()));
                SqlParameters.Add(new SqlParameter("@EventDate", Request.Form["EventDate"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address", Request.Form["Address"].ToString()));
                SqlParameters.Add(new SqlParameter("@Status", Request.Form["Status"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "insert"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_UpCommingEventMaster", CommandType.StoredProcedure, SqlParameters);
                ViewBag.Msg = ds.Tables[0].Rows[0]["msg"].ToString();
                if (ds.Tables[0].Rows[0]["msg"].ToString() =="success")
                {
                    ViewBag.Msg = "Upcomming Event add successfully.";
                }
                else
                {
                    ViewBag.Error = "some error occurred, please try again..!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return RedirectToAction("UpcomingEvent", new { status = true });
        }
        [HttpPost]
        public ActionResult UpdateUpcomingEvents()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@Title", Request.Form["Title"].ToString()));
                SqlParameters.Add(new SqlParameter("@SubTitle", Request.Form["SubTitle"].ToString()));
                SqlParameters.Add(new SqlParameter("@EventDate", Request.Form["EventDate"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address", Request.Form["Address"].ToString()));
                SqlParameters.Add(new SqlParameter("@Status", Request.Form["Status"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "updateEvent"));
                ds = DBManager.ExecuteDataSetWithParamiter("usp_UpCommingEventMaster", CommandType.StoredProcedure, SqlParameters);
                ViewBag.Msg = ds.Tables[0].Rows[0]["msg"].ToString();
                if (ds.Tables[0].Rows[0]["msg"].ToString() == "success")
                {
                    ViewBag.Msg = "Upcomming Event update successfully.";
                }
                else
                {
                    ViewBag.Error = "some error occurred, please try again..!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return RedirectToAction("UpcomingEvent", new { status = true });
        }
        [HttpGet]
        public ActionResult AddressInfo()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = BindAddressInfo();
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
            }
            return View(ds);
        }
        [HttpPost]
        public ActionResult UpdateAddressInfo()
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@UserId", Convert.ToInt32(Session["UserId"])));
                SqlParameters.Add(new SqlParameter("@OrganizationName", Request.Form["ImageUrl_2"].ToString()));
                SqlParameters.Add(new SqlParameter("@AccountNumber", Request.Form["AccountNumber"].ToString()));
                SqlParameters.Add(new SqlParameter("@IFSC", Request.Form["IFSC"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address", Request.Form["Address"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address2", Request.Form["Address2"].ToString()));
                SqlParameters.Add(new SqlParameter("@Address3", Request.Form["Address3"].ToString()));
                SqlParameters.Add(new SqlParameter("@MobileNumber", Request.Form["MobileNumber"].ToString()));
                SqlParameters.Add(new SqlParameter("@Email", Request.Form["Email"].ToString()));
                SqlParameters.Add(new SqlParameter("@QueryType", "UpdateUserAddressInfo"));
                ds = DBManager.ExecuteDataSetWithParamiter("Proc_User_Authentication", CommandType.StoredProcedure, SqlParameters);
                ViewBag.Msg = ds.Tables[0].Rows[0]["status"].ToString();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds = BindAddressInfo();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "some error occurred, please try again..!";
            }
            return View("AddressInfo", ds);
        }
        public DataSet BindAddressInfo()
        {
            DataSet ds = new DataSet();
            List<SqlParameter> SqlParameters = new List<SqlParameter>();
            SqlParameters.Add(new SqlParameter("@LoginId", Convert.ToInt32(Session["UserId"])));
            SqlParameters.Add(new SqlParameter("@QueryType", "getUser"));
            ds = DBManager.ExecuteDataSetWithParamiter("Proc_User_Authentication", CommandType.StoredProcedure, SqlParameters);
            return ds;
        }
        [HttpGet]
        public ActionResult ContactInfo()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ListContactInfo()
        {
            try
            {
                List<SqlParameter> SqlParameters = new List<SqlParameter>();
                SqlParameters.Add(new SqlParameter("@QueryType", "get"));
                DataSet ds = DBManager.ExecuteDataSetWithParamiter("usp_CntactInfo", CommandType.StoredProcedure, SqlParameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Content(JsonConvert.SerializeObject(ds.Tables[0]));
                }
                else
                {
                    return Content(JsonConvert.SerializeObject(null));
                }
            }
            catch (Exception ex)
            {
                ViewBag.Msg = "some error occurred, please try again..!";
                return Content(JsonConvert.SerializeObject(new { Status = "Error", Msg = "Something went wronge !" }));
            }
        }
    }
}