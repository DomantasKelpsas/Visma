using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using VismaProject.Services;

namespace VismaProject
{
    class Program
    {
        
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            string directoryPath = Path.GetFullPath(@"..\..\..\");           
            string pathToStockCSV = Path.Combine(directoryPath, @"Data\stock.csv");

            var _StockItemService = new StockItemService();
             
            var resultData = _StockItemService.ReadCSVFile(pathToStockCSV);

            Console.Write("stock = 1 | : ");
            switch (Convert.ToInt32(Console.ReadLine().Trim())) {

                case 1:
                    Console.Write("insert = 1 | delete = 2: ");
                    switch (Convert.ToInt32(Console.ReadLine().Trim())) {

                        case 1:
                            addStockItem(ref _StockItemService, ref resultData, pathToStockCSV);
                            break;
                        case 2:
                            Console.Write("Type Id of item to remove: ");
                            int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                            removeStockItem(ref _StockItemService, ref resultData, pathToStockCSV, removeId);
                            break;

                    }
                    break;
                default:
                    break;

            }


            //addStockItem(ref _StockItemService, ref resultData, pathToStockCSV);

            foreach (StockItem sti in resultData)
                Console.WriteLine(sti.ToString());

           

        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        static void addStockItem(ref StockItemService _StockItemService, ref List<StockItem> resultData, string path) {
            StockItem StockItem = new StockItem();
            StockItem.Id = 2;
            StockItem.Name = "Potatoes";
            StockItem.PortionCount = 10;
            StockItem.Unit = "kg";
            StockItem.PortionSize = (float)0.3;           
            resultData.Add(StockItem);         
            _StockItemService.WriteCSVFile(path, resultData);
        }

        static void removeStockItem(ref StockItemService _StockItemService, ref List<StockItem> resultData, string path, int removeId)
        {
            for (int i = 0; i < resultData.Count; i++)
            {
                if (resultData[i].Id == removeId)
                {
                    resultData.RemoveAt(i);
                    break;
                }

            }
            
            _StockItemService.WriteCSVFile(path, resultData);
        }
    }
}
