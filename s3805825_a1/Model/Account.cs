using System;
using System.Collections.Generic;

namespace s3805825_a1.Model
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
