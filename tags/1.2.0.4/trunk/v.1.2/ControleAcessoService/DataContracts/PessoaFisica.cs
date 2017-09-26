using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
    [DataContract]
    public class PessoaFisica
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }
        [DataMember(Order = 2)]
        public string CPF { get; set; }
        [DataMember(Order = 3)]
        public string Sexo { get; set; }
        [DataMember(Order = 4)]
        public System.DateTime Nascimento { get; set; }
    }
}