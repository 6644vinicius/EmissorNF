using Imposto.Core.Domain;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Data
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        readonly SQLServerProvider repository = new SQLServerProvider();

        public NotaFiscalRepository(SQLServerProvider provider)
        {
            this.repository = provider;
        }

        public int BuscarUltimaNotaFiscal()
        {
            var command = new SqlCommand();
            var dtResult = repository.ExecutaConsulta(command, "SELECT MAX(NumeroNotaFiscal) AS MAX FROM NotaFiscal");

            var result = dtResult.Rows[0]["MAX"].ToString();

            return string.IsNullOrWhiteSpace(result) ? 0 : Convert.ToInt32(result) + 1;
        }

        public bool InserirNotaFiscal(NotaFiscal notaFiscal)
        {
            var provider = new SQLServerProvider();
            var command = new SqlCommand();

            //inserir nota fiscal
            provider.LimparParametros(command);

            string storedProcedure = "P_NOTA_FISCAL";
            
            provider.AdicionarParametro(command, "@pId", SqlDbType.Int, 0, true);
            provider.AdicionarParametro(command, "@pNumeroNotaFiscal", SqlDbType.Int, notaFiscal.NumeroNotaFiscal);
            provider.AdicionarParametro(command, "@pSerie", SqlDbType.Int, notaFiscal.Serie);
            provider.AdicionarParametro(command, "@pNomeCliente", SqlDbType.VarChar, notaFiscal.NomeCliente);
            provider.AdicionarParametro(command, "@pEstadoDestino", SqlDbType.VarChar, notaFiscal.EstadoDestino);
            provider.AdicionarParametro(command, "@pEstadoOrigem", SqlDbType.VarChar, notaFiscal.EstadoOrigem);
            // Retorna a quantidade de linhas afetadas
            
            if (provider.ExecutaAtualizacao(command, storedProcedure) > 0)
            {
                notaFiscal.Id = Convert.ToInt32(command.Parameters["@pId"].Value.ToString().Trim());

                foreach (var notaFiscalItem in notaFiscal.ItensDaNotaFiscal)
                {
                    if (!InserirNotaFiscalItem(notaFiscal, notaFiscalItem))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool InserirNotaFiscalItem(NotaFiscal notaFiscal, NotaFiscalItem notaFiscalItem)
        {
            var provider = new SQLServerProvider();
            var command = new SqlCommand();

            //inserir nota fiscal item
            provider.LimparParametros(command);
            string storedProcedure = "P_NOTA_FISCAL_ITEM";

            // Adiciona os parâmetros da instrução SQL
            provider.AdicionarParametro(command, "@pId", SqlDbType.Int, 0, true);
            provider.AdicionarParametro(command, "@pIdNotaFiscal", SqlDbType.Int, notaFiscal.Id);
            provider.AdicionarParametro(command, "@pCfop", SqlDbType.VarChar, notaFiscalItem.Cfop);
            provider.AdicionarParametro(command, "@pTipoIcms", SqlDbType.VarChar, notaFiscalItem.TipoIcms);
            provider.AdicionarParametro(command, "@pBaseIcms", SqlDbType.Decimal, notaFiscalItem.BaseIcms);
            provider.AdicionarParametro(command, "@pAliquotaIcms", SqlDbType.Decimal, notaFiscalItem.AliquotaIcms);
            provider.AdicionarParametro(command, "@pValorIcms", SqlDbType.Decimal, notaFiscalItem.ValorIcms);
            provider.AdicionarParametro(command, "@pNomeProduto", SqlDbType.VarChar, notaFiscalItem.NomeProduto);
            provider.AdicionarParametro(command, "@pCodigoProduto", SqlDbType.VarChar, notaFiscalItem.CodigoProduto);
            provider.AdicionarParametro(command, "@pBaseIPI", SqlDbType.Decimal, notaFiscalItem.BaseIPI);
            provider.AdicionarParametro(command, "@pAliquotaIPI", SqlDbType.Decimal, notaFiscalItem.AliquotaIPI);
            provider.AdicionarParametro(command, "@pValorIPI", SqlDbType.Decimal, notaFiscalItem.ValorIPI);
            provider.AdicionarParametro(command, "@pDesconto", SqlDbType.Decimal, notaFiscalItem.Desconto);
            // Retorna a quantidade de linhas afetadas
            if (provider.ExecutaAtualizacao(command, storedProcedure) > 0)
            {
                notaFiscalItem.Id = Convert.ToInt32(command.Parameters["@pId"].Value.ToString().Trim());

                return true;
            }

            return false;
        }
    }
}
