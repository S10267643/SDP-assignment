using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_assignment
{
    public class CustomerCreator : UserFactory
    {
        protected override User CreateUser()
        {
            return new Customer();
        }
    }

}
