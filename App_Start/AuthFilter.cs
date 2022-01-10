using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ngo.Controllers
{
    public class AuthFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AuthFilter authFilter = new AuthFilter();
            string returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
            int iUserId = Convert.ToInt32(filterContext.HttpContext.Session.Contents["UserId"]);
            int iUserType = Convert.ToInt32(filterContext.HttpContext.Session.Contents["UserType"]);
            if (iUserId == 0)
            {
                var values = new
                {
                    controller = "Home",
                    action = "Login",
                    //returnUrl= returnUrl
                };
                //filterContext.Controller.TempData.Add("login", "Please login to access.");
                filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(values));
            }
            //if (iUserId > 0 && iUserType > 0)
            //{
            //    if (!authFilter.VerifyUser(iUserId, iUserType))
            //    {
            //        throw new Exception("Unauthorized");
            //    }
            //}
            base.OnActionExecuting(filterContext);
        }

        public bool VerifyUser(int iUserId, int iUserType)
        {
            DataSet ds = new DataSet();
            bool bIsAdmin = false;
            int type = 0;
            List<SqlParameter> SqlParameters = new List<SqlParameter>();
            SqlParameters.Add(new SqlParameter("@UserId", Convert.ToInt32(iUserId)));
            SqlParameters.Add(new SqlParameter("@QueryType", "getuserbyid"));
            ds = DBManager.ExecuteDataSetWithParamiter("Proc_User_Authentication", CommandType.StoredProcedure, SqlParameters);

            if (ds != null && ds.Tables.Count > 0)
            {
                DataRow drUser = ds.Tables[0].Rows[0];
                type = Convert.ToInt32(drUser["UserType"]);
                if (type == iUserType)
                {
                    bIsAdmin = true;
                }
                else
                {
                    bIsAdmin = false;
                }
            }
            return bIsAdmin;
        }
    }
}