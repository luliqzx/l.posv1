using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.PosV1.Model
{
    public class BaseTID<TID>
    {
        public virtual TID tID { get; set; }
    }
}
