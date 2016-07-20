using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OrderApplication
{
    public partial class WebForm5 : System.Web.UI.Page
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (Session["AccountID"].ToString() != "")
                {

                }
                else
                {
           
                }
            }
            catch
            {
                ViewState.Clear();
                Session.Clear();
                Response.Redirect("~/LogIn.aspx");
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddCatalog_Click(object sender, EventArgs e)
        {
            mpeProductCatalog.Show();
        }
    }
}