using System;
namespace Assignment_Manager.Model
{
    public class Transactions
    {
        public String TransactionTimeUtc { get; set; }
        public int TransactionID { get; set; }
        public String TransactionType { get; set; }
        public int TransactionFrom { get; set; }
        public int TransactionTo { get; set; }
        public double Amount { get; set; }
        public String Comment { get; set; }


    }
}
