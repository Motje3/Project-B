namespace VisitorTesting;

[TestClass]
public class VisitorTest
{
    [TestMethod]
    public void VisitorConstructor_SetsPropertiesCorrectly()
    {
        // Arrange
        string name = "John Doe";
        int rondleidingChoice = 1;
        string ticketCode = "12345678901";

        // Act
        Visitor visitor = new Visitor(name, rondleidingChoice, ticketCode);

        // Assert
        Assert.AreEqual(name, visitor.Name);
        Assert.AreEqual(rondleidingChoice, visitor.RondleidingChoice);
        Assert.AreEqual(ticketCode, visitor.TicketCode);
        Assert.AreNotEqual(Guid.Empty, visitor.VisitorId);
    }

    [TestMethod]
    public void VisitorConstructor_GeneratesUniqueId()
    {
        // Arrange
        string name = "Jane Smith";
        int rondleidingChoice = 2;
        string ticketCode = "12345678901";

        // Act
        Visitor visitor1 = new Visitor(name, rondleidingChoice, ticketCode);
        Visitor visitor2 = new Visitor(name, rondleidingChoice, ticketCode);

        // Assert
        Assert.AreNotEqual(visitor1.VisitorId, visitor2.VisitorId);
    }

    [TestMethod]
    public void RondleidingChoiceSetter_UpdatesRondleidingChoiceCorrectly()
    {
        // Arrange
        string name = "JAne";
        Visitor visitor = new Visitor(name, 1, "12345678901");

        // Act
        visitor.RondleidingChoice = 2;

        // Assert
        Assert.AreEqual(2, visitor.RondleidingChoice);
    }

    [TestMethod]
    public void TicketCodeSetter_UpdatesTicketCodeCorrectly()
    {
        // Arrange
        string name = "John Doe";
        Visitor visitor = new Visitor(name, 1, "12345678901");

        // Act
        visitor.TicketCode = "12345678901";

        // Assert
        Assert.AreEqual("12345678901", visitor.TicketCode);
    }
}
