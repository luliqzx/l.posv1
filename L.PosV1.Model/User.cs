using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L.PosV1.Model
{
    public class User : BaseT<string>
    {
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Name { get; set; }
    }
}
