namespace Coinbot.Bitbay.Models
{
    public class First
    {
        public string currency { get; set; }
        public double minOffer { get; set; }
        public double scale { get; set; }
    }

    public class Second
    {
        public string currency { get; set; }
        public double minOffer { get; set; }
        public int scale { get; set; }
    }

    public class Market
    {
        public string code { get; set; }
        public First first { get; set; }
        public Second second { get; set; }
    }

    public class Ticker
    {
        public Market market { get; set; }
        public long time { get; set; }
        public double highestBid { get; set; }
        public double lowestAsk { get; set; }
        public double rate { get; set; }
        public double previousRate { get; set; }
    }

    public class TickDTO
    {
        public string status { get; set; }
        public string[] errors { get; set; }
        public Ticker ticker { get; set; }
    }
}