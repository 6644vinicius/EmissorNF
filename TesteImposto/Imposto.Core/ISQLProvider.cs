using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core
{
    public interface ISQLProvider
    {
        public SqlConnection connection();
        public void Open();
        public void Close();
        public void AdicionarParametro(SqlCommand command, string nome, SqlDbType tipo, int tamanho, object valor, bool isInputOutput);
        public void AdicionarParametro(SqlCommand command, string nome, SqlDbType tipo, object valor, bool isInputOutput);
        public void RemoverParametro(SqlCommand command, string pNome);
        public void LimparParametros(SqlCommand command);
        public DataTable ExecutaConsulta(SqlCommand command, string sql, bool isProcedure);
        public int ExecutaAtualizacao(SqlCommand command, string sql);
    }
}
