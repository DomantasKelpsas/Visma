using System;
using System.IO;
using VismaProject;
using VismaProject.Services;
using Xunit;

namespace VismaProjectTest
{
    public class StockTesting
    {
        [Fact]
        public void ItemIdsAreValid()
        {
            string directoryPath = Path.GetFullPath(@"..\..\..\..\VismaProject\");
            string pathToStockCSV = Path.Combine(directoryPath, @"Data\stock.csv");
            var _StockItemService = new StockItemService();
            var resultDataOfStock = _StockItemService.ReadCSVFile(pathToStockCSV);

            string ids = "1 2 3";

            Assert.True(Program.checkValidItemIds<StockItem>(resultDataOfStock, ids));
        }

        [Fact]
        public void ItemIdsAreInvalid()
        {
            string directoryPath = Path.GetFullPath(@"..\..\..\..\VismaProject\");
            string pathToStockCSV = Path.Combine(directoryPath, @"Data\stock.csv");
            var _StockItemService = new StockItemService();
            var resultDataOfStock = _StockItemService.ReadCSVFile(pathToStockCSV);

            string ids = "1 5 3";

            Assert.False(Program.checkValidItemIds<StockItem>(resultDataOfStock, ids));
        }

        [Fact]
        public void StockReducedSuccessfully()
        {
            string directoryPath = Path.GetFullPath(@"..\..\..\..\VismaProject\");
            string pathToStockCSV = Path.Combine(directoryPath, @"Data\stock.csv");
            string pathToMenuCSV = Path.Combine(directoryPath, @"Data\menu.csv");
            string pathToOrdersCSV = Path.Combine(directoryPath, @"Data\orders.csv");
            string[] pathsToCSV = { pathToStockCSV, pathToMenuCSV, pathToOrdersCSV };
            

            string ids = "1 3";

            Assert.True(Program.reduceFromStock(ids, pathsToCSV));
        }

        [Fact]
        public void StockReducedUnsuccessfullyWhileNotEnough()
        {
            string directoryPath = Path.GetFullPath(@"..\..\..\..\VismaProject\");
            string pathToStockCSV = Path.Combine(directoryPath, @"Data\stock.csv");
            string pathToMenuCSV = Path.Combine(directoryPath, @"Data\menu.csv");
            string pathToOrdersCSV = Path.Combine(directoryPath, @"Data\orders.csv");
            string[] pathsToCSV = { pathToStockCSV, pathToMenuCSV, pathToOrdersCSV };


            string ids = "2 3";

            Assert.False(Program.reduceFromStock(ids,pathsToCSV));
        }

        [Fact]
     



    }
}
