using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Repository
    {
        public readonly SqlConnection Conn = null;

        public Repository()
        {
            this.Conn = new SqlConnection(ConfigurationManager.AppSettings["SQlConnectionString"].ToString());
        }

        /// <summary>  
        /// This method will get the list of Customer from database table CustomerRawInfo.  
        /// </summary>  
        /// <returns>List of customers in datatable</returns>  
        public DataTable GetCustomers()
        {
            string Query = string.Empty;
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = null;

            try
            {
                Query = "SELECT ROW_NUMBER() OVER (ORDER BY CUSTOMERNAME) ROW_NUM,CUSTOMERNAME,EMAIL,COMPANY,STREET,CITY,REGION,COUNTRY FROM CUSTOMERRAWINFO WITH(NOLOCK)";
                SqlCommand sqlCommand = new SqlCommand(Query, Conn);
                Conn.Open();

                // create data adapter  
                sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                // this will query your database and return the result to your datatable  
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                dataTable = null;
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
