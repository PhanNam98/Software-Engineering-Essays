using BDS_ML.Models.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BDS_ML.Models.CustomModel
{
    public class CustomerCustom
    {
        public CustomerCustom()
        {

        }
        public CustomerCustom(Customer Customer, string NameAdmin)
        {
            this.customer = Customer;
            this.nameAdmin = NameAdmin;
        }
        public Customer customer { get; set; }
        public string nameAdmin { get; set; }
    }
}
