namespace  MuseumTesting;

[TestClass]
public class GuideTest
{
    [TestMethod]
    public void TestOverride()
    {
        // Arrange
        Guide guide = new("111");

        // Act
        string result = guide.ToString();

        // Assert
        Assert.AreEqual(result, "Name: Bora\nTicket code: 111");
    }
}
