using System;
using System.Collections.Generic;

namespace Assignment_Manager.Model
{
    public class Account
    {

        public int AccountNumber { get; set; }
        public Double Balance { get; set; }
        public String AccountType { get; set; }
        public String CustomerID { get; set; }
        public List<Transactions> Transactions { get; set; }


    }
}
