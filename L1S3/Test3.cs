using L1S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appliance.Test
{
    [TestClass]
    public class DishwasherTests
    {
        [TestMethod]
        public void ToString_IncludesAllInformation()
        {
            // Arrange
            var dishwasher = new Dishwasher("Bosch", "SMS", "Black", 9);

            // Act
            string result = dishwasher.ToString();

            // Assert
            Assert.IsTrue(result.StartsWith("Посудомойка - "));
            Assert.IsTrue(result.Contains("Производитель: Bosch"));
            Assert.IsTrue(result.Contains("Модель: SMS"));
            Assert.IsTrue(result.Contains("Цвет: Black"));
            Assert.IsTrue(result.Contains("Количсетво программ: 9"));
        }

        [TestMethod]
        public void PrintInfo_WritesCorrectFormatToConsole()
        {
            var dishwasher = new Dishwasher("Whirlpool", "WFO", "White", 6);
            var output = new StringWriter();
            Console.SetOut(output);
            dishwasher.PrintInfo();
            string result = output.ToString().Trim();
            string expected = dishwasher.ToString();
            Assert.AreEqual(expected, result);
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
        }
    }
}
