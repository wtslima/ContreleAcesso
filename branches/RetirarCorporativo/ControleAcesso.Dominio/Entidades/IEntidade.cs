using System;

namespace ControleAcesso.Dominio.Entidades
{
    public interface IEntidade : IEquatable<IEntidade>
    {
        int Id { get; set; }
    }
}
