using L.PosV1.DataAccess.Common;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace L.PosV1.DataAccess.Repo
{
    public interface IBaseRepo<T> where T : class
    {
        IList<T> GetAll();
        T GetBy(Expression<Func<T, bool>> expfunc);
        T Save(T T);
        void Update(T T);
        void Delete(T T);
        ITransaction Transaction { get; set; }
        void BeginTransaction(IsolationLevel IsolationLevel = IsolationLevel.ReadCommitted);
        void Commit();
    }
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        protected virtual ISessionFactory SessionFactory { get; set; }
        protected virtual ISession Session { get; set; }
        protected virtual ISessionProvider SessionProvider { get; set; }

        public BaseRepo(ISessionProvider _SessionProvider)
        {
            SessionProvider = _SessionProvider;
            SessionFactory = SessionProvider.GetSessionFactory();
            Session = SessionFactory.OpenSession();
        }

        public BaseRepo()
        {
        }

        public virtual IList<T> GetAll()
        {
            IList<T> lstT = Session.QueryOver<T>().List<T>();
            return lstT;
        }

        public virtual T GetBy(Expression<Func<T, bool>> expfunc)
        {
            T T = Session.Query<T>().FirstOrDefault(expfunc);
            return T;
        }

        public T Save(T T)
        {
            Session.Save(T);
            return T;
        }

        public void Update(T T)
        {
            Session.Update(T);
        }

        public void Delete(T T)
        {
            Session.Delete(T);
        }

        public ITransaction Transaction { get; set; }

        public void BeginTransaction(IsolationLevel IsolationLevel = IsolationLevel.ReadCommitted)
        {
            Transaction = Session.BeginTransaction(IsolationLevel);
        }

        public void Commit()
        {
            Transaction.Commit();
        }
    }
}
