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


            SqlConnection conexao = new SqlConnection(strCon);
            conexao.Open();
            return conexao;
        }


    }
}
