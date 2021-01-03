using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using Microsoft.Extensions.Configuration;
using VismaProject.Mappers;
using VismaProject.Services;

namespace VismaProject
{
    public class Program
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
            var _MenuItemService = new MenuItemService();
            var _OrderItemService = new OrderItemService();
              

            var resultDataOfStock = _StockItemService.ReadCSVFile(pathsToCSV[0]);
            var resultDataOfMenu = _MenuItemService.ReadCSVFile(pathsToCSV[1]);
            var resultDataOfOrder = _OrderItemService.ReadCSVFile(pathsToCSV[2]);

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
                        menuItemAcions(ref _MenuItemService, ref resultDataOfMenu, pathsToCSV);
                        break;
                case "o":
                        printData<OrderItem>(resultDataOfOrder);
                        orderItemAcions(ref _OrderItemService, ref resultDataOfOrder, pathsToCSV);
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

            Console.Write("LAST -> " + resultData.Last().ToString());
            Console.Write("Insert item name: ");
            string name = Console.ReadLine().Trim();
            Console.Write("Insert item Portion Count: ");
            int count = Convert.ToInt32(Console.ReadLine().Trim());
            Console.Write("Insert item Unit: ");
            string unit = Console.ReadLine().Trim();
            Console.Write("Insert item Portion Size: ");
            float size = Single.Parse(Console.ReadLine().Trim());


            StockItem StockItem = new StockItem();
           
            int lastId = resultData.Last().Id;
            StockItem.Id = ++lastId;
            StockItem.Name = name;
            StockItem.PortionCount = count;
            StockItem.Unit = unit;
            StockItem.PortionSize = size;           
            resultData.Add(StockItem);         
            _ItemService.WriteCSVFile(path, resultData);
        }
        static void addMenuItem(ref MenuItemService _ItemService, ref List<MenuItem> resultData, string[] pathsToCSV)
        {



           
            Console.Write("Insert item name: ");
            string name = Console.ReadLine().Trim();
            Console.Write("Insert item Products: ");
            string products = Console.ReadLine().Trim();

            var _StockItemService = new StockItemService();
            var _resultData = _StockItemService.ReadCSVFile(pathsToCSV[0]);
            var valid = checkValidItemIds<StockItem>(_resultData, products);

            if (valid) {
                MenuItem menuItem = new MenuItem();
                int lastId = resultData.Last().Id;
                menuItem.Id = ++lastId;
                menuItem.Name = name;
                menuItem.Products = products;
                resultData.Add(menuItem);
                _ItemService.WriteCSVFile(pathsToCSV[1], resultData);
            }
            else Console.WriteLine($"Could not find such items ({products})");
        }
        static void addOrderItem(ref OrderItemService _ItemService, ref List<OrderItem> resultData, string[] pathsToCSV)
        {

           
            Console.Write("Insert menu Items: ");
            string products = Console.ReadLine().Trim();

            var _MenuItemService = new MenuItemService();
            var _resultData = _MenuItemService.ReadCSVFile(pathsToCSV[1]);
            var valid = checkValidItemIds<MenuItem>(_resultData, products);

            if (valid)
            {


                if (reduceFromStock(products, pathsToCSV))
                {
                    OrderItem OrderItem = new OrderItem();
                    int lastId = resultData.Last().Id;
                    OrderItem.Id = ++lastId;
                    OrderItem.DateTime = DateTime.Now;
                    OrderItem.MenuItems = products;
                    resultData.Add(OrderItem);
                    _ItemService.WriteCSVFile(pathsToCSV[2], resultData);
                }
                else Console.WriteLine($"Not enough stock! Order declined!");
            }
            else Console.WriteLine($"Could not find such items ({products})");
        }
        public static void removeItem<I>(ref List<I> resultData, int removeId)
        {
            for (int i = 0; i < resultData.Count; i++)
            {
                try
                {
                    var x = resultData[i].GetType().GetProperty("Id").GetValue(resultData[i], null);
                    if ((int)x == removeId)
                    {
                        resultData.RemoveAt(i);
                        break;
                    }
                }
                catch (Exception e) { Console.WriteLine("Error finding id"); }
            }
        }
     

        static void stockItemAcions(ref StockItemService _itemService, ref List<StockItem> resultData, string pathToFile) {

            Console.Write("insert = 1 \t | \t delete = 2 \t | \t view = 3 \t: ");
            switch (Convert.ToInt32(Console.ReadLine().Trim()))
            {

                case 1:
                    addStockItem(ref _itemService, ref resultData, pathToFile);
                    break;
                case 2:
                    try
                    {
                        Console.Write("Type Id of item to remove: ");
                        int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                        removeItem(ref resultData, removeId);
                        _itemService.WriteCSVFile(pathToFile, resultData);
                       
                    }
                    catch (Exception e) { Console.WriteLine("Error finding id"); }
                    break;
                case 3:
                    printData<StockItem>(resultData);
                    break;

            }
        }

        static void menuItemAcions(ref MenuItemService _itemService, ref List<MenuItem> resultData, string[] pathsToCSV)
        {

            Console.Write("insert = 1 \t | \t delete = 2 \t | \t view = 3 \t: ");
            switch (Convert.ToInt32(Console.ReadLine().Trim()))
            {

                case 1:
                    addMenuItem(ref _itemService, ref resultData, pathsToCSV);
                    break;
                case 2:
                    try
                    {
                        Console.Write("Type Id of item to remove: ");
                        int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                        removeItem(ref resultData, removeId);
                        _itemService.WriteCSVFile(pathsToCSV[1], resultData);

                    }
                    catch (Exception e) { Console.WriteLine("Error finding id"); }
                    break;
                case 3:
                    printData<MenuItem>(resultData);
                    break;

            }
        }

        static void orderItemAcions(ref OrderItemService _itemService, ref List<OrderItem> resultData, string[] pathsToCSV)
        {

            Console.Write("insert = 1 \t | \t delete = 2 \t | \t view = 3 \t: ");
            switch (Convert.ToInt32(Console.ReadLine().Trim()))
            {

                case 1:
                    addOrderItem(ref _itemService, ref resultData,pathsToCSV);
                    break;
                case 2:
                    try
                    {
                        Console.Write("Type Id of item to remove: ");
                        int removeId = Convert.ToInt32(Console.ReadLine().Trim());
                        removeItem(ref resultData, removeId);
                        _itemService.WriteCSVFile(pathsToCSV[2], resultData);

                    }
                    catch (Exception e) { Console.WriteLine("Error finding id"); }
                    break;
                case 3:
                    printData<OrderItem>(resultData);
                    break;

            }
        }
       
        public static bool checkValidItemIds<I>(List<I> resultData, string ids) {


            var idList = ids.Trim().Split(' ').Select(Int32.Parse).ToList();
            int idList_size = idList.Count;
            int validCount = 0;
            foreach (int i in idList)
            {
                foreach (var item in resultData)
                {
                   var id = item.GetType().GetProperty("Id").GetValue(item, null);
                    if (i == (int)id) {
                        validCount++;
                        break;                   
                    }
                }
            }
            if (validCount == idList_size)
                return true;
            else return false;
        }

        static void printData<I>(List<I> resultData) {

            foreach (var item in resultData)
                Console.WriteLine(item.ToString());
        }

        public static bool reduceFromStock(string ids, string[] pathsToCSV) {

            var menuIds = ids.Trim().Split(' ').Select(Int32.Parse).ToList();
            
            var _StockItemService = new StockItemService();
            var resultDataOfStock = _StockItemService.ReadCSVFile(pathsToCSV[0]);

            var _MenuItemService = new MenuItemService();
            var resultDataOfMenu = _MenuItemService.ReadCSVFile(pathsToCSV[1]);

            foreach (int mId in menuIds)
                foreach (MenuItem mItem in resultDataOfMenu) {
                    if (mItem.Id == mId) {
                        string Products = mItem.Products;
                        var stockIds = Products.Trim().Split(' ').Select(Int32.Parse).ToList();
                        foreach (int sId in stockIds)
                            foreach (StockItem sItem in resultDataOfStock)
                                if (sItem.Id == sId)
                                {
                                    if (sItem.PortionCount > 0)
                                    {
                                        sItem.PortionCount--;
                                        _StockItemService.WriteCSVFile(pathsToCSV[0], resultDataOfStock);
                                    }
                                    else return false;
                                    break;
                                }

                    }
                }
            return true;
        }
    }
}
