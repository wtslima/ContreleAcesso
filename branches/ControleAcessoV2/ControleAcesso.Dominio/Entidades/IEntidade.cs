using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleAcesso.Dominio.Entidades
{
    public interface IEntidade : IEquatable<IEntidade>
    {
        int Id { get; set; }
    }
}
