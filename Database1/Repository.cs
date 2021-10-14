using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Database
{
    public class Repository
    {
        public readonly SqlConnection Conn = null;

        public Repository()
        {
            string connStr = ConfigurationManager.ConnectionStrings["UWPriorityRedirect"].ConnectionString;
            this.Conn = new SqlConnection(connStr);        
        }

        /// <summary>  
        /// This method will get the list of Customer from database table CustomerRawInfo.  
        /// </summary>  
        /// <returns>List of customers in datatable</returns>  
        public DataTable GetLoans()
        {
            string Query = string.Empty;
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = null;

            string userName = HttpContext.Current.User.Identity.Name;
            //For testing purposes
            //string userName = @"EPHMC\npatel";

            try
            {
                Query = "SELECT * FROM edw.UnderwritingPriorityRedirect(@UID) order by DateSubmitted";
                SqlCommand sqlCommand = new SqlCommand(Query, Conn);
                sqlCommand.Parameters.AddWithValue("@UID", userName);
                Conn.Open();

                // create data adapter  
                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // this will query your database and return the result to your datatable  
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
                sqlDataAdapter.Dispose();
            }

            return dataTable;
        }
    }
}
