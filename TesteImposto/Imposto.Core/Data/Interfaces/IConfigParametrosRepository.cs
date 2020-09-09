using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imposto.Core.Data
{
    public interface IConfigParametrosRepository
    {
        string ObterValorDoParametro(string parametro);
    }
}
