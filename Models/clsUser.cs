using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ngo.Models
{
    public class clsUser
    {
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public int UserType { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string AadharNumber { get; set; }
        private string _LoginId;
        private bool _Islogin;
        public bool Islogin { get { return _Islogin; } }

        public clsUser()
                   : this("0")
        {

        }
        public clsUser(string PLoginId)
        {
            _LoginId = PLoginId;
            _Islogin = false;
        }
    }

    public class EventMaster
    {
        public EventMaster()
        {
            this.ImageUrl = new List<HttpPostedFileBase>();
            this.Status = new List<SelectList>();
        }
        public int EventId { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Date { get; set; }
        public string VideoUrl { get; set; }
        public List<HttpPostedFileBase> ImageUrl { get; set; }
        public int IsActive { get; set; }
        public List<SelectList> Status { get; set; }
    }
}