using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using WebserviceRESTFull.Models;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Text;

namespace WebserviceRESTFull.Controllers
{
    public class RESTFullController : ApiController
    {

        ConnectionProvider connectionProvider = new ConnectionProvider();

        Modelname modelName = new Modelname();
 

        private string encrypt(String text)
        {
            String retorno = "";
            String stext = text;
            if (stext == "")
            {
                return stext;
            }
            while (true)
            {
                String letter = stext.Substring(0, 1);
                int number = char.ConvertToUtf32(letter, 0);
                number += 166;
                string snumber = number.ToString();
                if (snumber.Length < 3)
                {
                    snumber = "0" + snumber;
                }
                if (snumber.Length < 3)
                {
                    snumber = "0" + snumber;
                }
                retorno += snumber;
                stext = stext.Substring(1);
                if (stext == "")
                {
                    break;
                }
            }
            return retorno;
        }
        private string decrypt(String text)
        {
            String retorno = "";
            String stext = text;
            if (stext == "")
            {
                return stext;
            }
            try
            {
                while (true)
                {
                    String letter = stext.Substring(0, 3);
                    int snumero = int.Parse(letter);
                    snumero -= 166;
                    retorno += char.ConvertFromUtf32(snumero);
                    stext = stext.Substring(3);
                    if (stext == "")
                    {
                        break;
                    }
                }
            }
            catch (Exception ex) { }
            return retorno;
        }

        private string ExecuteQuery(string sSql)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = sSql;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "ok";
        }

        public string ReturnString(string query)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;
            string retorno = "";
            try
            {
                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;
                rdr = cmd.ExecuteReader();
                try
                {
                    rdr.Read();
                    retorno = rdr.GetString(0);
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    try
                    {
                        rdr.Close();
                    }
                    catch (Exception ex2) { }
                }
            }
            catch (Exception ex3) { }

            return retorno;
        }
        public string ReturnNumber(string query) {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader rdr = null;
            string retorno = "";
            try
            {
                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = query;
                rdr = cmd.ExecuteReader();
                try
                {
                    rdr.Read();
                    retorno = rdr.GetInt32(0).ToString();
                    rdr.Close();
                }
                catch (Exception ex)
                {
                    try
                    {
                        rdr.Close();
                    }
                    catch (Exception ex2) { }
                }
            }
            catch (Exception ex3) { }

            return retorno;
        }
        
        
        //get Method 
        [Route("api/App_name/get_fuction")]
        [HttpGet]
        [ActionName("get_fuction")]
        public DataTable get_fuction(string param1, string param2 )
        {
            SqlCommand cmd = new SqlCommand();
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                connectionProvider.myConnection.Open();

                /// enter your procedure paramesters and their types in the sqlParameter() 
                /// Use encrypt and decrypt functions to ecrypt data sending it to the database , 
                /// or decrypt before sending it to the databse and also getting data from database if it has been saved encrypted

                System.Data.SqlClient.SqlParameter wParam = new System.Data.SqlClient.SqlParameter("@param1", System.Data.SqlDbType.VarChar, 50);
                wParam.Value = encrypt(param1);
                cmd.Parameters.Add(wParam);

                System.Data.SqlClient.SqlParameter wParam1 = new System.Data.SqlClient.SqlParameter("@param2", System.Data.SqlDbType.VarChar, 50);
                wParam1.Value = encrypt(param2);
                cmd.Parameters.Add(wParam1);

                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "get_procedurename";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds, "Table");

            }
            finally
            {
                connectionProvider.myConnection.Close();
            }

            DataTableCollection dc = ds.Tables;
            DataTable dTable = dc[0];
            return dTable;

        }


        //delete method
        [Route("api/App_name/delete_function")]
        [HttpDelete]
        [ActionName("delete_function")]
        public string delete_function(int param1)
        {
            string returno = "";
            SqlCommand cmd = new SqlCommand();
            System.Data.DataSet ds = new System.Data.DataSet();
            try
            {
                connectionProvider.myConnection.Open();

                System.Data.SqlClient.SqlParameter wParam = new System.Data.SqlClient.SqlParameter("@param1", System.Data.SqlDbType.Int, 5);
                wParam.Value = param1;
                cmd.Parameters.Add(wParam);

                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "delete_Procedurename";
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                returno = "Error - " + ex.Message;
            }
            finally
            {
                connectionProvider.myConnection.Close();
            }
            return returno;

        }


        // Post method 
        [Route("api/OrderApp/post_function")]
        [HttpPost]
        [ActionName("post_function")] 
        public string post_function(Modelname model1) {

            string returno = "";
            SqlCommand cmd = new SqlCommand();

            try {
                connectionProvider.myConnection.Open();

                System.Data.SqlClient.SqlParameter wParam = new System.Data.SqlClient.SqlParameter("@id", System.Data.SqlDbType.Int, 5);
                wParam.Value = model1.model_id;
                cmd.Parameters.Add(wParam);

                System.Data.SqlClient.SqlParameter wParam1 = new System.Data.SqlClient.SqlParameter("@name", System.Data.SqlDbType.VarChar, 50);
                wParam1.Value = model1.model_name;
                cmd.Parameters.Add(wParam1);

                System.Data.SqlClient.SqlParameter wParam2 = new System.Data.SqlClient.SqlParameter("@description", System.Data.SqlDbType.VarChar, 500);
                wParam2.Value = model1.model_Description;
                cmd.Parameters.Add(wParam2);
                 
                System.Data.SqlClient.SqlParameter wParam3 = new System.Data.SqlClient.SqlParameter("@photo", System.Data.SqlDbType.Image);
                wParam3.Value = model1.model_photo;
                cmd.Parameters.Add(wParam3);

                cmd.Connection = connectionProvider.myConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "Post_procedurename";
                cmd.ExecuteNonQuery();

                returno = model1.model_id+"";

            }
            catch (Exception ex)
            {
                returno = "Falha - " + ex.Message;
            }
            finally
            {
                connectionProvider.myConnection.Close();
            }
            return returno;
        }
        

      
    }
}
