using System.Linq;
using AutoMapper;
using Coinbot.Bitbay.Models;
using Coinbot.Domain.Contracts.Models.StockApiService;

namespace Coinbot.Bitbay.Mappings
{
    public class BitbayProfile : Profile
    {
        public BitbayProfile()
        {
            CreateMap<TickDTO, Tick>()
                .ForMember(dest => dest.Ask, opt => opt.MapFrom(src => src.ticker.lowestAsk))
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(src => src.ticker.highestBid))
                .ForMember(dest => dest.Last, opt => opt.MapFrom(src => src.ticker.rate));


            CreateMap<TransactionMadeDTO, Transaction>()
                .ForMember(dest => dest.OrderRefId, opt => opt.MapFrom(src => src.offerId))
                .ForMember(dest => dest.IsOpen, opt => opt.MapFrom(src => !src.completed))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => (double)src.transactions.Sum(x=>x.amount)));
        }
    }
}