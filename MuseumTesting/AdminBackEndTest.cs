using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReservationSystem;
using System;
using System.Collections.Generic;
using System.IO;

[TestClass]
public class AdminBackEndTests
{
    private readonly string testFilePath = "./JSON-Files/GuideAssignments.json";

    [TestInitialize]
    public void TestInitialize()
    {
        // Clear TodaysTours before each test
        TourTools.TodaysTours.Clear();

        // Ensure the test file path exists
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Clean up after each test
        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }

    [TestMethod]
    public void AddTourForToday_ValidTime_TourAdded()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n1\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourForToday();
            }
        }

        // Assert
        Assert.AreEqual(1, TourTools.TodaysTours.Count);
        Assert.AreEqual("Guide 1", TourTools.TodaysTours[0].AssignedGuide.Name);
        Assert.AreEqual(new TimeSpan(10, 0, 0), TourTools.TodaysTours[0].StartTime.TimeOfDay);
    }

    [TestMethod]
    public void AddTourForToday_InvalidTimeFormat_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("invalid_time\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourForToday();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid time format. Please enter the time in hh:mm format."));
            Assert.AreEqual(0, TourTools.TodaysTours.Count);
        }
    }

    [TestMethod]
    public void AddTourForToday_InvalidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n99\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourForToday();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid guide choice. Please select a valid guide number."));
            Assert.AreEqual(0, TourTools.TodaysTours.Count);
        }
    }

    [TestMethod]
    public void AddTourForToday_ValidTime_InvalidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n99\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourForToday();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid guide choice. Please select a valid guide number."));
            Assert.AreEqual(0, TourTools.TodaysTours.Count);
        }
    }

    [TestMethod]
    public void AddTourForToday_InvalidTimeFormat_ValidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("invalid_time\n1\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourForToday();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid time format. Please enter the time in hh:mm format."));
            Assert.AreEqual(0, TourTools.TodaysTours.Count);
        }
    }

    [TestMethod]
    public void AddTourToStandardSchedule_ValidTime_TourAddedToStandardSchedule()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n1\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourToStandardSchedule();
            }
        }

        // Assert
        var guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(File.ReadAllText(testFilePath));
        Assert.IsNotNull(guideAssignments);
        Assert.AreEqual(1, guideAssignments.Count);
        Assert.AreEqual("Guide 1", guideAssignments[0].GuideName);
        Assert.AreEqual(1, guideAssignments[0].Tours.Count);
        Assert.AreEqual("10:00 AM", guideAssignments[0].Tours[0].StartTime);
    }

    [TestMethod]
    public void AddTourToStandardSchedule_InvalidTimeFormat_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("invalid_time\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourToStandardSchedule();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid time format. Please enter the time in hh:mm format."));
            var guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(File.ReadAllText(testFilePath));
            Assert.IsNull(guideAssignments);
        }
    }

    [TestMethod]
    public void AddTourToStandardSchedule_InvalidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n99\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourToStandardSchedule();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid guide choice. Please select a valid guide number."));
            var guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(File.ReadAllText(testFilePath));
            Assert.IsNull(guideAssignments);
        }
    }

    [TestMethod]
    public void AddTourToStandardSchedule_ValidTime_InvalidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("10:00\n99\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourToStandardSchedule();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid guide choice. Please select a valid guide number."));
            var guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(File.ReadAllText(testFilePath));
            Assert.IsNull(guideAssignments);
        }
    }

    [TestMethod]
    public void AddTourToStandardSchedule_InvalidTimeFormat_ValidGuideChoice_ShowsErrorMessage()
    {
        // Arrange
        List<Guide> guides = new List<Guide>
        {
            new Guide(Guid.NewGuid(), "Guide 1", "pass1"),
            new Guide(Guid.NewGuid(), "Guide 2", "pass2")
        };
        Guide.AllGuides = guides;

        // Act
        using (var sw = new StringWriter())
        {
            Console.SetOut(sw);
            using (var sr = new StringReader("invalid_time\n1\n"))
            {
                Console.SetIn(sr);
                AdminBackEnd.AddTourToStandardSchedule();
            }

            // Assert
            string result = sw.ToString();
            Assert.IsTrue(result.Contains("Invalid time format. Please enter the time in hh:mm format."));
            var guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(File.ReadAllText(testFilePath));
            Assert.IsNull(guideAssignments);
        }
    }
}
