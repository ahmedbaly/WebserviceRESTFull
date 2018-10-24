using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;

namespace WebserviceRESTFull.Models
{
    public class ConnectionProvider
    {
        public SqlConnection myConnection = new SqlConnection();

        public ConnectionProvider()
        {
            
            this.myConnection.ConnectionString = @"Server=srv02\MSSQLSERVEREXPRE;Initial Catalog = orderapp; Integrated Security = False; User Id = sa; Password = 2z1wqt4t; ";


        }

    }
}


