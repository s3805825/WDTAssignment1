using System;
namespace s3805825_a1.Model
{
    public class Transactions
    {
        public String TransactionTimeUtc { get; set; }
        public int TransactionID { get; set; }
        public String TransactionType { get; set; }
        public String TransactionFrom { get; set; }
        public String TransactionTo { get; set; }
        public double Amount { get; set; }
        public String Comment { get; set; }


    }
}
