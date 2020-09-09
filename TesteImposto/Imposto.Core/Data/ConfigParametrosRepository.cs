using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    class ConfigParametrosRepository : IConfigParametrosRepository
    {
        readonly SQLServerProvider repository = new SQLServerProvider();

        public ConfigParametrosRepository(SQLServerProvider provider)
        {
            this.repository = provider;
        }
        public string ObterValorDoParametro(string parametro)
        {
            var command = new SqlCommand();
            var query = new StringBuilder().AppendFormat("SELECT Valor FROM ConfigParametros WHERE Parametro = '{0}'", parametro).ToString();

            var dataTableResult = repository.ExecutaConsulta(command, query);

            return dataTableResult.Rows[0]["Valor"].ToString();
        }
    }
}
