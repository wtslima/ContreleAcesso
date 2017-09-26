using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
    [DataContract]
    public class TokenInvalidoFault
    {
        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}