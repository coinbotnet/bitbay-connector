using System.Collections.Generic;

namespace Coinbot.Bitbay.Models
{
    public class TransactionDTO
    {
        public decimal amount { get; set; }
        public decimal rate { get; set; }
    }

    public class TransactionMadeDTO
    {
        public string status { get; set; }
        public string[] errors { get; set; }
        public bool completed { get; set; }
        public string offerId { get; set; }
        public List<TransactionDTO> transactions { get; set; }
    }
}