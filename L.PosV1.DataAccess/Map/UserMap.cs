using FluentNHibernate.Mapping;
using L.PosV1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.PosV1.DataAccess.Map
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.tID).GeneratedBy.Assigned();
            Map(x => x.Username).Unique().Length(25).Not.Nullable();
            Map(x => x.Password);
            Map(x => x.Name).Length(100);

            #region Audit Trail
            Map(x => x.CreateBy);
            Map(x => x.CreateDate);
            Map(x => x.CreateTerminal);
            Map(x => x.UpdateBy);
            Map(x => x.UpdateDate);
            Map(x => x.UpdateTerminal);
            Version(x => x.TimeVersion);
            #endregion
        }
    }
}
