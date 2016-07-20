using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PL.PersistenceServices.DTOS;
using System.Data;
using OrderLinc;


namespace OrderApplication
{
    public class GlobalVariables
    {
        static OrderLinc.OrderLincServices Order;
        // static byte[] imageByte;
        static string DateFormat;

        public static OrderLinc.OrderLincServices OrderAppLib
        {
            get
            {
                if (Order == null)
                {
                    DTOConnectionConfiguration mConfig = new DTOConnectionConfiguration();

                    mConfig.ServerName = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                    mConfig.ServerUri = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                    mConfig.DatabaseName = System.Configuration.ConfigurationManager.AppSettings["Database"];
                    mConfig.UserName = System.Configuration.ConfigurationManager.AppSettings["Username"];
                    mConfig.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
                    bool AuthenticationType = false;
                    if (System.Configuration.ConfigurationManager.AppSettings["AuthenticationType"].ToString() == "1")
                    {
                        //mConfig.AuthenticationType = PL.PersistenceServices.Enumerations.DatabaseAuthenticationTypes.ServerAuthentication;
                        AuthenticationType = false;
                    }
                    else
                    {
                        AuthenticationType = true;
                      
                    }


                    Order = new OrderLinc.OrderLincServices(mConfig.ServerName.ToString(), mConfig.DatabaseName.ToString(), AuthenticationType, mConfig.UserName.ToString(), mConfig.Password.ToString());

                  var Config =  Order.ConfigurationService.SYSConfigListByKey("DateFormat");

                  if (Config != null && Config.SYSConfigID != 0)
                  {

                      DateFormat = Config.ConfigValue;
                  }
                  else {
                      DateFormat = "dd/MM/yyyy";
                  }
                    
                }
                return Order;
            }


        }


        public static String GetDateFormat {

            get { return DateFormat; }
        
        }

    }
}