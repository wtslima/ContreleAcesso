
namespace ControleAcesso.Dominio.Entidades
{
    public abstract class Entidade : IEntidade
    {
        private int _id;
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public virtual bool Equals(IEntidade entidade)
        {
            return Id == entidade.Id;
        }

        public static bool operator ==(Entidade entidade1, Entidade entidade2)
        {
            if (Equals(entidade1, null))
            {
                return Equals(entidade2, null) ? true : false;
            }
            if (Equals(entidade2, null))
            {
                return false;
            }
            return entidade1.Equals(entidade2);
        }
        public static bool operator !=(Entidade entidade1, Entidade entidade2)
        {
            return !(entidade1 == entidade2);
        }

        public abstract override int GetHashCode();
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }
        public abstract override string ToString();

    }
}
