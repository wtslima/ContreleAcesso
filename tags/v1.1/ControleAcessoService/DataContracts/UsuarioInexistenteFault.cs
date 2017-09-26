using System.Runtime.Serialization;

namespace ControleAcessoService.DataContracts
{
    [DataContract]
    public class UsuarioInexistenteFault
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}