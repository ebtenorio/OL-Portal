using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
namespace OrderApplication.Reports
{
    public partial class Report : System.Web.UI.Page
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
            if (!IsPostBack) { 
            
            
            }
        }


        private void ExportToCSVFile(DataTable mDT,string mFilename)
        {
            try
            {
                StringBuilder sbldr = new StringBuilder();
                string mOutput = "";
                sbldr.Append(mFilename);
                sbldr.Append("\r\n");
              

             

                sbldr.Append("\r\n");
                sbldr.Append("\r\n");
                if (mDT.Columns.Count != 0)
                {
                    foreach (DataColumn col in mDT.Columns)
                    {
                        sbldr.Append(col.ColumnName + ',');
                    }
                    sbldr.Append("\r\n");
                    foreach (DataRow row in mDT.Rows)
                    {
                        foreach (DataColumn column in mDT.Columns)
                        {
                            if (column.ToString() == "ProjectedLossVSNational")
                            {

                                sbldr.Append((string.Format("{0:P}", double.Parse(row[column].ToString()))) + ',');
                            }
                            else
                            {
                                sbldr.Append((row[column].ToString().Replace(",", "")) + ',');
                            }
                        }
                        sbldr.Append("\r\n");
                    }
                }



                mOutput = sbldr.ToString();




                Response.Clear();
                Response.ContentType = "application/CSV";
                Response.AddHeader("content-disposition", "attachment;filename=" + mFilename.Replace(" ", "_") + ".csv");
                Response.Write(mOutput);
                Response.End();



            }

            catch (Exception ex)
            {


              
            }
        }

        protected void lbProducts_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();

                mDT = GlobalVariables.OrderAppLib.ReportService.ProductListBySalesOrgID(int.Parse(Session["RefID"].ToString()));

                ExportToCSVFile(mDT, lbProducts.Text);
            }
            catch { 
            
            }
        }

        protected void lbUser_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();
                mDT = GlobalVariables.OrderAppLib.ReportService.UserListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
                ExportToCSVFile(mDT, lbUser.Text);
            }
            catch { 
            
            }
        }

        protected void lbStores_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();
                mDT = GlobalVariables.OrderAppLib.ReportService.StoreListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
                ExportToCSVFile(mDT, lbStores.Text);
            }
            catch { 
            
            }
        }

        protected void lbReleaseOrder_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();
                mDT = GlobalVariables.OrderAppLib.ReportService.OrderReleaseListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
                ExportToCSVFile(mDT, lbReleaseOrder.Text);
            }
            catch { 
            
            }
        }

        protected void lbProvider_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();
                mDT = GlobalVariables.OrderAppLib.ReportService.ProvidersListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
                ExportToCSVFile(mDT, lbProvider.Text);
            }
            catch { 
            
            }
        }

        protected void lbWarehouse_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable mDT = new DataTable();
                mDT = GlobalVariables.OrderAppLib.ReportService.WareHouseListBySalesOrgID(int.Parse(Session["RefID"].ToString()));
                ExportToCSVFile(mDT, lbWarehouse.Text);
            }
            catch { 
            
            }
        }

    }
}