using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using MarketSummaryConsole.Model;

namespace MarketSummaryConsole
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProspectDataSearchCriteria, ProspectSearchCriteria>();
            CreateMap<ProspectData, ProspectSummaryData>();
        }
    }
}