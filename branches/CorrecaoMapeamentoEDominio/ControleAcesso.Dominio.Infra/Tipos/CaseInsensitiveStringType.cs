using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace ControleAcesso.Dominio.Infra.Tipos
{
    public class CaseInsensitiveStringType : IUserType
    {

        public object Assemble(object cached, object owner)
        {
            return cached;
        }

        public object DeepCopy(object value)
        {
            if (value == null) return null;
            return value;
        }

        public object Disassemble(object value)
        {
            return value;
        }

        public bool Equals(object x, object y)
        {
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            return comparer.Compare(x, y) == 0;
        }

        public int GetHashCode(object x)
        {
            return x.ToString().ToUpperInvariant().GetHashCode();
        }

        public bool IsMutable
        {
            get { return false; }
        }

        public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
        {
            string value = (string)NHibernateUtil.String.NullSafeGet(rs, names[0]);
            return value;
        }

        public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
        {
            if (value == null)
            {
                NHibernateUtil.String.NullSafeSet(cmd, null, index);
                return;
            }
            value = value.ToString();
            NHibernateUtil.String.NullSafeSet(cmd, value, index);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public System.Type ReturnedType
        {
            get { return typeof(string); }
        }

        public SqlType[] SqlTypes
        {
            get
            {
                SqlType[] types = new SqlType[1];
                types[0] = new SqlType(DbType.String);
                return types;
            }
        }
    }
}
