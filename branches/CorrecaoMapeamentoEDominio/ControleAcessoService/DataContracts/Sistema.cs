using System;
using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
    [Serializable]
    [DataContract(Namespace = "http://inmetro.gov.br/ControleAcesso/DataContracts")]
    public class Sistema
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string Codigo { get; set; }
        [DataMember(Order = 3)]
        public string Nome { get; set; }

        public override int GetHashCode()
        {
            int hashCode = base.GetHashCode();
            unchecked
            {
                if (Codigo != null)
                    hashCode += 1000000007 * Codigo.GetHashCode();
            }
            return hashCode;
        }

        public override string ToString()
        {
            return string.Format("[Sistema Nome={0}, Codigo={1}]", Nome, Codigo);
        }
    }
}