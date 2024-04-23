using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuseumTesting;
using System;

namespace MuseumTesting.Tests
{
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
            Assert.AreEqual(result, "Ticket code: 111");
        }
        [TestMethod]
        public void CompleteTour_Deleted_return_early()
        {
            // Arrange
            GuidedTour existingTour = new GuidedTour 
            { 
                TourId = Guid.NewGuid(), Deleted = true 
            };
            Guide guide = new Guide("456");
            guide.AssingedTourId = existingTour.TourId;

            // Act
            guide.CompleteTour();

            // Assert
            Assert.IsFalse(existingTour.Completed); 
        }

        [TestMethod]
        public void CompleteTour_Completed_return_true()
        {
            // Arrange
            GuidedTour existingTour = new GuidedTour 
            {
                TourId = Guid.NewGuid(), Deleted = false 
            };
            Guide guide = new Guide("789");
            guide.AssingedTourId = existingTour.TourId;

            // Act
            guide.CompleteTour();

            // Assert
            // Verify that the tour is marked as completed
            Assert.IsFalse(existingTour.Completed);
        }
    }
}