using L.PosV1.DataAccess.Common;
using L.PosV1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.PosV1.DataAccess.Repo
{
    public interface IUserRepo : IBaseRepo<User>
    {
    }

    public class UserRepo : BaseRepo<User>, IUserRepo
    {
        public UserRepo(ISessionProvider _SessionProvider) : base(_SessionProvider)
        {
            SessionProvider = _SessionProvider;
            SessionFactory = SessionProvider.GetSessionFactory();
            Session = SessionFactory.OpenSession();
        }
    }
}
