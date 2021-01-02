using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Users
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Management { get; set; }
        public override string ToString() => this.ToStringProperty();
    }
}
