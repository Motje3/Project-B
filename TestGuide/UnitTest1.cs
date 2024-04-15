using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestGuide;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestOverride()
    {
        // Arrange
        Guide guide = new Guide("Bora", "111");

        // Act
        string result = guide.ToString();

        // Assert
        Assert.AreEqual(result, "Name: Bora\nTicket code: 111");
    }
}
