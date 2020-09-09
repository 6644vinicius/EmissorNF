using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core
{
    public class SQLServerProvider : ISQLProvider
    {
        #region Objetos Estáticos
        public static SqlConnection sqlconnection = new SqlConnection();
        public static SqlParameter parametro = new SqlParameter();

        public SQLServerProvider()
        {
        }
        #endregion

        #region Obter SqlConnection
        public SqlConnection connection()
        {
            try
            {
                // Obtemos os dados da conexão existentes no WebConfig
                //utilizando o ConfigurationManager
                string dadosConexao = @"Server=localhost\SQLEXPRESS;Database=Teste;Trusted_Connection=True;";
                // Instanciando o objeto SqlConnection
                sqlconnection = new SqlConnection(dadosConexao);
                //Verifica se a conexão esta fechada.
                if (sqlconnection.State == ConnectionState.Closed)
                {
                    //Abre a conexão.
                    sqlconnection.Open();
                }
                //Retorna o sqlconnection.
                return sqlconnection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Abre Conexão
        public void Open()
        {
            sqlconnection.Open();
        }
        #endregion

        #region Fecha Conexão
        public void Close()
        {
            sqlconnection.Close();
        }
        #endregion

        #region Adiciona Parâmetros
        public void AdicionarParametro(SqlCommand command, string nome, SqlDbType tipo, int tamanho, object valor, bool isInputOutput = false)
        {
            // Cria a instância do Parâmetro e adiciona os valores
            parametro = new SqlParameter();
            parametro.ParameterName = nome;
            parametro.SqlDbType = tipo;
            parametro.Size = tamanho;
            parametro.Value = valor;
            // Adiciona ao comando SQL o parâmetro
            if (isInputOutput)
            {
                command.Parameters.Add(parametro).Direction = ParameterDirection.InputOutput;
            }
            else
            {
                command.Parameters.Add(parametro);
            }
        }
        #endregion

        #region Adiciona Parâmetros
        public void AdicionarParametro(SqlCommand command, string nome, SqlDbType tipo, object valor, bool isInputOutput = false)
        {
            // Cria a instância do Parâmetro e adiciona os valores
            SqlParameter parametro = new SqlParameter();
            parametro.ParameterName = nome;
            parametro.SqlDbType = tipo;
            parametro.Value = valor;
            // Adiciona ao comando SQL o parâmetro
            if (isInputOutput)
            {
                command.Parameters.Add(parametro).Direction = ParameterDirection.InputOutput;
            }
            else
            {
                command.Parameters.Add(parametro);
            }
        }
        #endregion

        #region Remove os parâmetros
        public void RemoverParametro(SqlCommand command, string pNome)
        {
            // Verifica se existe o parâmetro
            if (command.Parameters.Contains(pNome))
                // Se exite remove o mesmo
                command.Parameters.Remove(pNome);
        }
        #endregion

        #region Limpar Parâmetros
        public void LimparParametros(SqlCommand command)
        {
            command.Parameters.Clear();
        }
        #endregion

        #region Executar Consulta SQL
        public DataTable ExecutaConsulta(SqlCommand command, string sql, bool isProcedure = false)
        {
            try
            {
                command.Connection = connection();
                command.CommandText = sql;

                if (isProcedure)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }

                command.ExecuteScalar();

                IDataReader dtreader = command.ExecuteReader();
                DataTable dtresult = new DataTable();

                dtresult.Load(dtreader);

                sqlconnection.Close();

                return dtresult;
            }
            catch (Exception ex)
            {
                // Retorna uma exceção simples que pode ser tratada por parte do desenvolvedor
                // Exemplo: if (ex.Message.toString().Contains(‘Networkig’))
                // Exemplo throw new Exception(‘Problema de rede detectado’);
                throw ex;
            }
        }
        #endregion

        #region Executa uma instrução SQL: INSERT, UPDATE e DELETE
        public int ExecutaAtualizacao(SqlCommand command, string sql)
        {
            try
            {
                //Instância o sqlcommand com a query sql que será executada e a conexão.
                //comando = new SqlCommand(sql, connection());
                command.Connection = connection();
                command.CommandText = sql;
                command.CommandType = CommandType.StoredProcedure;

                //Executa a query sql.
                int result = command.ExecuteNonQuery();

                sqlconnection.Close();
                // Retorna a quantidade de linhas afetadas
                return result;
            }
            catch (Exception ex)
            {
                // Retorna uma exceção simples que pode ser tratada por parte do desenvolvedor
                throw ex;
            }
        }

        #endregion
    }
}
