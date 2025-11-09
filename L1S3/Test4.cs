using L1S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appliance.Test
{
    [TestClass]
    public class RealizeTests
    {
        [TestMethod]
        public void AddObject_AddsToCollection() 
        {
            var realize = new Realize();
            var appliance = new L1S3.Appliance("Samsung", "QLED", "Black");
            realize.AddObject(appliance);
            Assert.AreEqual(1, realize.appliances.Count);
            Assert.AreEqual(appliance, realize.appliances[0]);
        }

        [TestMethod]
        public void RemoveObject_ValidIndex_RemovesItem() 
        {
            var realize = new Realize();
            var appliance1 = new L1S3.Appliance("Brand1", "Model1", "Color1");
            var appliance2 = new L1S3.Appliance("Brand2", "Model2", "Color2");
            realize.AddObject(appliance1);
            realize.AddObject(appliance2);
            int initialCount = realize.appliances.Count;
            realize.RemoveObject(0);
            Assert.AreEqual(initialCount - 1, realize.appliances.Count);
            Assert.AreEqual(appliance2, realize.appliances[0]);
        }

        [TestMethod]
        public void RemoveObject_InvalidIndex_ThrowsException() 
        {
            var realize = new Realize();
            realize.AddObject(new L1S3.Appliance("Brand", "Model", "Color"));
            Assert.ThrowsException<ArgumentException>(() => realize.RemoveObject(-1));
            Assert.ThrowsException<ArgumentException>(() => realize.RemoveObject(5));
        }
        // метод Output (он же ListObject )Использует консоль через методы SetCursorPosition(), нельзя протеститровать напрямую
    }
}
