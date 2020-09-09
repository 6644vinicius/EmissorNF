using Imposto.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public interface INotaFiscalRepository
    {
        int BuscarUltimaNotaFiscal();
        bool InserirNotaFiscal(NotaFiscal notaFiscal);
        bool InserirNotaFiscalItem(NotaFiscal notaFiscal, NotaFiscalItem notaFiscalItem);
    }
}
