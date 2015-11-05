using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace FindJob.UI
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty((string)Session["UserName"]))
            {
                Response.Redirect("~/UI/Home.aspx");
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                MembershipCreateStatus Status;
                Membership.CreateUser(UserNameText.Text, PassWordText.Text);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ex:" + ex.Message);

            }
        }
    }
}