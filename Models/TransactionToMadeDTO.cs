namespace Coinbot.Bitbay.Models
{
    public class TransactionToMadeDTO
    {
        public string offerType { get; set; }
        public string amount { get; set; }
        public object price { get; set; }
        public string rate { get; set; }
        public bool postOnly { get; set; }
        public string mode { get; set; }
        public bool fillOrKill { get; set; }
    }
}