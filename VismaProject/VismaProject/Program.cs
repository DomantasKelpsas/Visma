using System;
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



            Console.WriteLine("Start CSV File Reading...");
            var _StockItemService = new StockItemService();
            string stockCSV = "stock.csv";
            string path = Path.Combine(directoryPath, @"Data\", stockCSV);

            Console.WriteLine(path);

            //var path = @"Data\stock.csv";
            //Here We are calling function to read CSV file  
            var resultData = _StockItemService.ReadCSVFile(path);
            //Create an object of the StockItem class  
            //StockItem StockItem = new StockItem();
            //StockItem.RollNo = 5;
            //StockItem.Name = "Lucy";
            //StockItem.Course = "B.Tech";
            //StockItem.Fees = 75000;
            //StockItem.Mobile = "7788990099";
            //StockItem.City = "Pune";
            //resultData.Add(StockItem);
            //Here We are calling function to write file  
            //_StockItemService.WriteCSVFile(@"D:\Tutorials\NewStockItemFile.csv", resultData);
            //Here D: Drive and Tutorials is the Folder name, and CSV File name will be "NewStockItemFile.csv"  
            //Console.WriteLine("New File Created Successfully.");

           
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
    }
}
