using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System;

namespace MarketSummaryConsole
{
    public class SQLRepository
    {
        private SqlConnection connection = null;
        private void initialiseConnection()
        {
            if (connection == null)
            {
                SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder();
                connBuilder.DataSource = "masteksql.database.windows.net";
                connBuilder.UserID = "yashpal";
                connBuilder.Password = "Welcome498$";
                connBuilder.InitialCatalog = "ProspectData";
                connection = new SqlConnection(connBuilder.ConnectionString);
            }
        }
        public List<ProspectDataSearchCriteria> GetProspectData(string WhereClause = "")
        {
            try
            {
                initialiseConnection();
                connection.Open();
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.Append("SELECT * from  ProspectDataSearchCriteria");
                if (!string.IsNullOrEmpty(WhereClause))
                {
                    strBuilder.Append(WhereClause);
                }
                string cmdText = strBuilder.ToString();
                DataTable dataTableProspects = new DataTable();
                using (SqlCommand sqlCmd = new SqlCommand(cmdText, connection))
                {
                    SqlDataAdapter sqlda = new SqlDataAdapter(sqlCmd);
                    sqlda.Fill(dataTableProspects);
                }
                List<ProspectDataSearchCriteria> prospectList = new List<ProspectDataSearchCriteria>();
                foreach (DataRow dataRow in dataTableProspects.Rows)
                {

                    prospectList.Add(
                        new ProspectDataSearchCriteria
                        {

                            Id = Convert.ToString(dataRow["Id"]),
                            ProspectName = Convert.ToString(dataRow["ProspectName"]),
                            SearchString = Convert.ToString(dataRow["SearchString"]),
                            TwitterUpdates = Convert.ToBoolean(dataRow["TwitterUpdates"]),
                            FacebookUpdates = Convert.ToBoolean(dataRow["FacebookUpdates"]),
                            LinkedinUpdates = Convert.ToBoolean(dataRow["LinkedinUpdates"]),
                            EmailUpdates = Convert.ToBoolean(dataRow["EmailUpdates"]),
                            BingSearchUpdates = Convert.ToBoolean(dataRow["BingSearchUpdates"]),
                        }
                        );


                }

                return prospectList;
            }
            catch
            {
                throw;
            }

        }

        public bool InsertProspectData(ProspectData prospectData)
        {

            initialiseConnection();
            SqlCommand com = new SqlCommand("Insert into ProspectData(ProspectName,SearchString,SearchResult,TwitterUpdates,FacebookUpdates,LinkedinUpdates,EmailUpdates,BingSearchUpdates) values(@ProspectName,@SearchString,@SearchResult,@TwitterUpdates,@FacebookUpdates,@LinkedinUpdates,@EmailUpdates,@BingSearchUpdates)", connection);
            com.CommandType = CommandType.Text;
            com.Parameters.AddWithValue("@ProspectName", prospectData.ProspectName);
            com.Parameters.AddWithValue("@SearchString", prospectData.SearchString);
            com.Parameters.AddWithValue("@SearchResult", prospectData.SearchResult);
            com.Parameters.AddWithValue("@TwitterUpdates", prospectData.TwitterUpdates);
            com.Parameters.AddWithValue("@FacebookUpdates", prospectData.FacebookUpdates);
            com.Parameters.AddWithValue("@LinkedinUpdates", prospectData.LinkedinUpdates);
            com.Parameters.AddWithValue("@EmailUpdates", prospectData.EmailUpdates);
            com.Parameters.AddWithValue("@BingSearchUpdates", prospectData.BingSearchUpdates);

            connection.Open();
            int rows = com.ExecuteNonQuery();
            connection.Close();
            return rows >= 1 ? true : false;
        }

    }
}
