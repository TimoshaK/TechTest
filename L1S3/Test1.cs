namespace Appliance.Test;
using L1S3;
using System.Drawing;
using System.Reflection;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void Constructor_WithParameters_SetsPropertiesCorrectly()
    {
        // Arrange & Act
        var appliance = new Appliance("Samsung", "QLED", "Black");
        // Assert
        Assert.AreEqual("Samsung", appliance.Brand);
        Assert.AreEqual("QLED", appliance.Model);
        Assert.AreEqual("Black", appliance.Color);
    }
    [TestMethod]
    public void Constructor_DefaultParameters_SetsEmptyStrings()
    {
        var appliance = new Appliance();
        Assert.AreEqual("", appliance.Brand);
        Assert.AreEqual("", appliance.Model);
        Assert.AreEqual("", appliance.Color);
    }
    [TestMethod]
    public void Properties_CanBeSetAndGet()
    {
        var appliance = new Appliance();
        appliance.Brand = "LG";
        appliance.Model = "OLED";
        appliance.Color = "White";
        Assert.AreEqual("LG", appliance.Brand);
        Assert.AreEqual("OLED", appliance.Model);
        Assert.AreEqual("White", appliance.Color);
    }
    [TestMethod]
    public void ToString_WithAllProperties_ReturnsFormattedString()
    {
        var appliance = new Appliance("Bosch", "Series 6", "Silver");
        string result = appliance.ToString();
        Assert.AreEqual("Производитель: Bosch, Модель: Series 6,Цвет: Silver", result);
    }
    [TestMethod]
    public void ToString_WithEmptyProperties_ReturnsFormattedStringWithEmptyValues()
    {
        var appliance = new Appliance();
        string result = appliance.ToString();
        Assert.AreEqual("Производитель: , Модель: ,Цвет: ", result);
    }
    // тестирование printinfo aналогично предыдущему.
}
