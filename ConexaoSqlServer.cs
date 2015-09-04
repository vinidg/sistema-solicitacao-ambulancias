using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solicitacao_de_Ambulancias
{
    class ConexaoSqlServer
    {

   //    string conectando = "Server=DIV-HOS-AD03\\SQLSERVERDON;Database=dahue;User Id=fu14855;Password=dahue123;";
        public static SqlConnection GetConexao()
        {
            string strCon = "data source = srv-mssql-01;initial catalog = DAHUE;user id = dahue; password = DT1_D@huE_1438_DtI";
            // string strCon = "data source = dti-webhmg-01;initial catalog = DAHUE;user id = dahue; password = DT1_HmG_2005_DtI";
            //string strCon = "data source = .\\SQLEXPRESS;initial catalog = DAHUE;user id = sa; password = 123456";


            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }


    }
}
