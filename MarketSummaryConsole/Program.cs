using System;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace MarketSummaryConsole
{

    public class Program
    {
        static void Main(string[] args)
        {
            //Console.OutputEncoding = Encoding.UTF8;
            //CosmosRepository<ProspectData>.Initialize();
            AutoMapper.Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());

            Console.WriteLine("Searching the Web for: " + "Prospects");
            ProcessData processdata = new ProcessData();
            //processdata.ProcessBingSearchData().Wait();
            if (args != null && args.Length > 0)
            {
                if (Convert.ToString(args[0]).ToUpper() == "BINGSEARCH")
                {
                    processdata.ProcessBingSearchData().Wait();
                }
            }
            Console.Write("\nPress Enter to exit ");
            Console.ReadLine();

        }
    }
}