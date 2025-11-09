using L1S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appliance.Test
{
    [TestClass]
    public class WashingMachineTests
    {
        [TestMethod]
        public void Constructor_SetsPropertiesAndInheritsBase()
        {
            var machine = new WashingMachine("Brand", "Model", "Color", 7.5f);
            Assert.AreEqual("Brand", machine.Brand);
            Assert.AreEqual(7.5f, machine.Capacity);
        }

        [TestMethod]
        public void ToString_IncludesAllInfo()
        {
            var machine = new WashingMachine("LG", "F4", "Black", 8.5f);
            string result = machine.ToString();

            Assert.IsTrue(result.StartsWith("Стиральная машина - "));
            Assert.IsTrue(result.Contains("Производитель: LG"));
            Assert.IsTrue(result.Contains("Модель: F4"));
            Assert.IsTrue(result.Contains("Цвет: Black"));
            Assert.IsTrue(result.Contains("Объем: 8,5 л"));
        }

        [TestMethod]
        public void PrintInfo_ShowsFormattedCapacity()
        {
            var machine = new WashingMachine("Test", "M", "C", 6.75f);
            var output = new StringWriter();
            Console.SetOut(output);

            machine.PrintInfo();

            string result = output.ToString();
            Assert.IsTrue(result.Contains("6,75") || result.Contains("6.75"));
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
        }
    }
}
