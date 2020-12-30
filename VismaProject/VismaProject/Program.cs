using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using VismaProject.Mappers;
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
            string pathToMenuCSV = Path.Combine(directoryPath, @"Data\menu.csv");
            string pathToOrdersCSV = Path.Combine(directoryPath, @"Data\orders.csv");
            string[] pathsToCSV = { pathToStockCSV, pathToMenuCSV, pathToOrdersCSV };

            InitConsoleUI(pathsToCSV);

            //addStockItem(ref _StockItemService, ref resultData, pathToStockCSV);

            //foreach (StockItem sti in resultData)
            //    Console.WriteLine(sti.ToString());



        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }

        static void InitConsoleUI(string[] pathsToCSV) {

            var _StockItemService = new StockItemService();
            var resultDataOfStock = _StockItemService.ReadCSVFile(pathsToCSV[0]);

            var _MenuItemService = new MenuItemService();
            var resultDataOfMenu = _MenuItemService.ReadCSVFile(pathsToCSV[1]);

            bool refresh = true;

            while (refresh) { 

            Console.Write("Select which file to open (stock = s | menu = m | orders = o) press X to close : ");
            switch (Console.ReadLine().Trim().ToLower())
            {

                case "s":
                        printData<StockItem>(resultDataOfStock);
                        stockItemAcions(ref _StockItemService, ref resultDataOfStock, pathsToCSV[0]);
                    break;
                case "m":
                        printData<MenuItem>(resultDataOfMenu);
                        menuItemAcions(ref _MenuItemService, ref resultDataOfMenu, pathsToCSV[1]);
                        break;
                case "o":
                    break;
                    case "x":
                        refresh = false;
                        break;

                    default:
                    break;

            }
        }
    }
        static void addStockItem(ref StockItemService _ItemService, ref List<StockItem> resultData, string path) {

            Console.Write("Insert item id: ");
            int id = Convert.ToInt32(Console.ReadLine().Trim());
            Console.Write("Insert item name: ");
            string name = Console.ReadLine().Trim();
            Console.Write("Insert item Portion Count: ");
            int count = Convert.ToInt32(Console.ReadLine().Trim());
            Console.Write("Insert item Unit: ");
            string unit = Console.ReadLine().Trim();
            Console.Write("Insert item Portion Size: ");
            float size = Single.Parse(Console.ReadLine().Trim());


            StockItem StockItem = new StockItem();
            StockItem.Id = id;
            StockItem.Name = name;
            StockItem.PortionCount = count;
            StockItem.Unit = unit;
            StockItem.PortionSize = size;           
            resultData.Add(StockItem);         
            _ItemService.WriteCSVFile(path, resultData);
        }

        static void addMenuItem(ref MenuItemService _ItemService, ref List<MenuItem> resultData, string path)
        {

            Console.Write("Insert item id: ");
            int id = Convert.ToInt32(Console.ReadLine().Trim());
            Console.Write("Insert item name: ");
            string name = Console.ReadLine().Trim();
            Console.Write("Insert item Products: ");
            string products = Console.ReadLine().Trim();




            MenuItem menuItem = new MenuItem();
            menuItem.Id = id;
            menuItem.Name = name;
            menuItem.Products = products;      
            resultData.Add(menuItem);
            _ItemService.WriteCSVFile(path, resultData);
        }

        static void removeStockItem(ref StockItemService _ItemService, ref List<StockItem> resultData, string path, int removeId)
        {
            for (int i = 0; i < resultData.Count; i++)
            {
                if (resultData[i].Id == removeId)
                {
                    resultData.RemoveAt(i);
                    break;
                }

            }

            _ItemService.WriteCSVFile(path, resultData);
        }

        static void removeMenuItem(ref MenuItemService _ItemService, ref List<MenuItem> resultData, string path, int removeId)
        {
            for (int i = 0; i < resultData.Count; i++)
            {
                if (resultData[i].Id == removeId)
                {
                    resultData.RemoveAt(i);
                    break;
                }

            }

            _ItemService.WriteCSVFile(path, resultData);
        }

        static void stockItemAcions(ref StockItemService _itemService, ref List<StockItem> resultData, string pathToFile) {

            Console.Write("insert = 1 \t | \t delete = 2 \t | \t view = 3 \t: ");
            switch (Convert.ToInt32(Console.ReadLine().Trim()))
            {

                case 1:
                    addStockItem(ref _itemService, ref resultData, pathToFile);
                    break;
                case 2:
                    Console.Write("Type Id of item to remove: ");
                    int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                    removeStockItem(ref _itemService, ref resultData, pathToFile, removeId);
                    break;
                case 3:
                    printData<StockItem>(resultData);
                    break;

            }
        }

        static void menuItemAcions(ref MenuItemService _itemService, ref List<MenuItem> resultData, string pathToFile)
        {

            Console.Write("insert = 1 \t | \t delete = 2 \t | \t view = 3 \t: ");
            switch (Convert.ToInt32(Console.ReadLine().Trim()))
            {

                case 1:
                    addMenuItem(ref _itemService, ref resultData, pathToFile);
                    break;
                case 2:
                    Console.Write("Type Id of item to remove: ");
                    int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                    removeMenuItem(ref _itemService, ref resultData, pathToFile, removeId);
                    break;
                case 3:
                    printData<MenuItem>(resultData);
                    break;

            }
        }
       
        static void printData<I>(List<I> resultData) {

            foreach (var item in resultData)
                Console.WriteLine(item.ToString());
        }
    }
}
