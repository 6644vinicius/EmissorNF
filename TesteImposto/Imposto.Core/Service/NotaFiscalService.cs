using Imposto.Core.Data;
using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Imposto.Core.Service
{
    public class NotaFiscalService
    {
        public const string _EstadosSudeste = "ES,MG,RJ,SP";

        public string GerarNotaFiscal(Domain.Pedido pedido)
        {
            try
            {
                NotaFiscal notaFiscal = new NotaFiscal();
                var result = EmitirNotaFiscal(notaFiscal, pedido);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }

                var provider = new SQLServerProvider();
                var notaFiscalRepository = new NotaFiscalRepository(provider);

                if (!notaFiscalRepository.InserirNotaFiscal(notaFiscal))
                {
                    return "Erro ao inserir Nota Fiscal";
                }

                GerarXml(provider, notaFiscal);
            }
            catch (Exception e)
            {
                throw e;
            }

            return string.Empty;
        }

        public void GerarXml(SQLServerProvider provider, NotaFiscal notaFiscal)
        {
            var configParametrosRepository = new ConfigParametrosRepository(provider);
            var diretorioNotasFiscais = configParametrosRepository.ObterValorDoParametro("Path_notas_fiscais");

            if (!System.IO.Directory.Exists(diretorioNotasFiscais))
            {
                System.IO.Directory.CreateDirectory(diretorioNotasFiscais);
            }

            var serializer = new XmlSerializer(typeof(NotaFiscal));
            using (var writer = new StreamWriter(diretorioNotasFiscais + notaFiscal.Id))
            {
                serializer.Serialize(writer, notaFiscal);
            }
        }

        public bool DefinirCfop(NotaFiscal notaFiscal, NotaFiscalItem notaFiscalItem)
        {
            if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "RJ"))
            {
                notaFiscalItem.Cfop = "6.000";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "PE"))
            {
                notaFiscalItem.Cfop = "6.001";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "MG"))
            {
                notaFiscalItem.Cfop = "6.002";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "PB"))
            {
                notaFiscalItem.Cfop = "6.003";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "PR"))
            {
                notaFiscalItem.Cfop = "6.004";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "PI"))
            {
                notaFiscalItem.Cfop = "6.005";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "RO"))
            {
                notaFiscalItem.Cfop = "6.006";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "SE"))
            {
                notaFiscalItem.Cfop = "6.007";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "TO"))
            {
                notaFiscalItem.Cfop = "6.008";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "SE"))
            {
                notaFiscalItem.Cfop = "6.009";
            }
            else if ((notaFiscal.EstadoOrigem == "SP") && (notaFiscal.EstadoDestino == "PA"))
            {
                notaFiscalItem.Cfop = "6.010";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "RJ"))
            {
                notaFiscalItem.Cfop = "6.000";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "PE"))
            {
                notaFiscalItem.Cfop = "6.001";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "MG"))
            {
                notaFiscalItem.Cfop = "6.002";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "PB"))
            {
                notaFiscalItem.Cfop = "6.003";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "PR"))
            {
                notaFiscalItem.Cfop = "6.004";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "PI"))
            {
                notaFiscalItem.Cfop = "6.005";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "RO"))
            {
                notaFiscalItem.Cfop = "6.006";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "SE"))
            {
                notaFiscalItem.Cfop = "6.007";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "TO"))
            {
                notaFiscalItem.Cfop = "6.008";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "SE"))
            {
                notaFiscalItem.Cfop = "6.009";
            }
            else if ((notaFiscal.EstadoOrigem == "MG") && (notaFiscal.EstadoDestino == "PA"))
            {
                notaFiscalItem.Cfop = "6.010";
            }

            return !string.IsNullOrWhiteSpace(notaFiscalItem.Cfop);
        }

        public void DefinirTipoIcms(NotaFiscal notaFiscal, NotaFiscalItem notaFiscalItem)
        {
            if (notaFiscal.EstadoDestino == notaFiscal.EstadoOrigem)
            {
                notaFiscalItem.TipoIcms = "60";
                notaFiscalItem.AliquotaIcms = 0.18;
            }
            else
            {
                notaFiscalItem.TipoIcms = "10";
                notaFiscalItem.AliquotaIcms = 0.17;
            }
        }

        public void DefinirBaseIcms(NotaFiscalItem notaFiscalItem, PedidoItem itemPedido)
        {
            if (notaFiscalItem.Cfop == "6.009")
            {
                notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido * 0.90; //redução de base
            }
            else
            {
                notaFiscalItem.BaseIcms = itemPedido.ValorItemPedido;
            }
        }

        public void DefinirBaseIPI(NotaFiscalItem notaFiscalItem, bool brinde)
        {
            notaFiscalItem.BaseIPI = notaFiscalItem.BaseIcms;
            notaFiscalItem.AliquotaIPI = brinde ? 0 : 10;
            notaFiscalItem.ValorIPI = notaFiscalItem.BaseIPI * (notaFiscalItem.AliquotaIPI / 100);
        }

        public void ProcessarDesconto(NotaFiscal notaFiscal, NotaFiscalItem notaFiscalItem, PedidoItem pedidoItem)
        {
            if (_EstadosSudeste.Contains(notaFiscal.EstadoDestino))
            {
                notaFiscalItem.Desconto = pedidoItem.ValorItemPedido * 0.1;
            }
            else
            {
                notaFiscalItem.Desconto = 0;
            }
        }

        public void DefinirValorIcms(NotaFiscalItem notaFiscalItem)
        {
            notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
        }

        public void TratarItemDeBrinde(NotaFiscalItem notaFiscalItem)
        {
            notaFiscalItem.TipoIcms = "60";
            notaFiscalItem.AliquotaIcms = 0.18;
            notaFiscalItem.ValorIcms = notaFiscalItem.BaseIcms * notaFiscalItem.AliquotaIcms;
        }

        public string ProcessarItemPedido(Pedido pedido, NotaFiscal notaFiscal, List<NotaFiscalItem> notaFiscalItemList)
        {
            foreach (PedidoItem itemPedido in pedido.ItensDoPedido)
            {
                var notaFiscalItem = new NotaFiscalItem();

                if (!DefinirCfop(notaFiscal, notaFiscalItem))
                {
                    return "Erro ao definir CFOP da Nota Fiscal";
                }

                DefinirTipoIcms(notaFiscal, notaFiscalItem);
                DefinirBaseIcms(notaFiscalItem, itemPedido);

                if (itemPedido.Brinde) TratarItemDeBrinde(notaFiscalItem);

                DefinirBaseIPI(notaFiscalItem, itemPedido.Brinde);

                ProcessarDesconto(notaFiscal, notaFiscalItem, itemPedido);

                notaFiscalItem.NomeProduto = itemPedido.NomeProduto;
                notaFiscalItem.CodigoProduto = itemPedido.CodigoProduto;

                notaFiscalItemList.Add(notaFiscalItem);
            }

            return string.Empty;
        }

        public string EmitirNotaFiscal(NotaFiscal notaFiscal, Pedido pedido)
        {
            var provider = new SQLServerProvider();
            var notaFiscalRepository = new NotaFiscalRepository(provider);

            notaFiscal.NumeroNotaFiscal = notaFiscalRepository.BuscarUltimaNotaFiscal();
            notaFiscal.Serie = new Random().Next(Int32.MaxValue);
            notaFiscal.NomeCliente = pedido.NomeCliente;
            notaFiscal.EstadoDestino = pedido.EstadoDestino;
            notaFiscal.EstadoOrigem = pedido.EstadoOrigem;

            var notaFiscalItemList = new List<NotaFiscalItem>();
            var result = ProcessarItemPedido(pedido, notaFiscal, notaFiscalItemList);

            if (!string.IsNullOrWhiteSpace(result))
            {
                return result;
            }

            notaFiscal.ItensDaNotaFiscal = notaFiscalItemList;
            return string.Empty;
        }
    }
}
