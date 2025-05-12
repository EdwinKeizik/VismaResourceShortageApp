using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VismaResourceShortageManagement.UI;

namespace VismaResourceShortageManagement.Tests
{
    [TestClass]
    public class InputValidatorTests
    {
        private TextReader? _originalConsoleIn;
        private TextWriter? _originalConsoleOut;
        private StringWriter? _consoleOutputCapture;

        [TestInitialize]
        public void InitializeConsoleRedirection()
        {
            _originalConsoleIn = Console.In;
            _originalConsoleOut = Console.Out;
            _consoleOutputCapture = new StringWriter();
            Console.SetOut(_consoleOutputCapture);
        }

        [TestCleanup]
        public void CleanupConsoleRedirection()
        {
            if (_originalConsoleIn != null)
            {
                Console.SetIn(_originalConsoleIn);
            }
            if (_originalConsoleOut != null)
            {
                Console.SetOut(_originalConsoleOut);
            }
            _consoleOutputCapture?.Dispose();
            _consoleOutputCapture = null;
        }

        [TestMethod]
        public void GetValidString_ValidInputFirstTry_ReturnsInput()
        {
            string simulatedInput = "Valid Name\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                string result = InputValidator.GetValidString("Prompt: ", "Error: ");
                Assert.AreEqual("Valid Name", result, "Test Case: GetValidString_ValidInputFirstTry_ReturnsInput");
            }
        }

        [TestMethod]
        public void GetValidString_EmptyThenValidInput_ReturnsValidInput()
        {
            string simulatedInput = "\n   \nValidAfterEmpty\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                string result = InputValidator.GetValidString("Prompt: ", "Error: ");
                Assert.AreEqual("ValidAfterEmpty", result, "Test Case: GetValidString_EmptyThenValidInput_ReturnsValidInput");
            }
        }

        [TestMethod]
        public void GetValidMenuOption_ValidInputInRangeFirstTry_ReturnsInput()
        {
            string simulatedInput = "2\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                int result = InputValidator.GetValidMenuOption("Error: ", 1, 3);
                Assert.AreEqual(2, result, "Test Case: GetValidMenuOption_ValidInputInRangeFirstTry_ReturnsInput");
            }
        }

        [TestMethod]
        public void GetValidMenuOption_OutOfRangeThenValidInput_ReturnsValidInput()
        {
            string simulatedInput = "0\n4\n1\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                int result = InputValidator.GetValidMenuOption("Error: ", 1, 3);
                Assert.AreEqual(1, result, "Test Case: GetValidMenuOption_OutOfRangeThenValidInput_ReturnsValidInput");
            }
        }

        [TestMethod]
        public void GetValidMenuOption_NonNumericThenValidInput_ReturnsValidInput()
        {
            string simulatedInput = "abc\n3\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                int result = InputValidator.GetValidMenuOption("Error: ", 1, 3);
                Assert.AreEqual(3, result, "Test Case: GetValidMenuOption_NonNumericThenValidInput_ReturnsValidInput");
            }
        }

        [TestMethod]
        public void GetValidDate_ValidDateInput_ReturnsDate()
        {
            string simulatedInput = "2024-05-10\n";
            DateTime expectedDate = new DateTime(2024, 5, 10);
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                DateTime result = InputValidator.GetValidDate("Prompt: ", "Error: ");
                Assert.AreEqual(expectedDate, result, "Test Case: GetValidDate_ValidDateInput_ReturnsDate");
            }
        }

        [TestMethod]
        public void GetValidDate_InvalidThenValidDateInput_ReturnsValidDate()
        {
            string simulatedInput = "not-a-date\n2023-01-15\n";
            DateTime expectedDate = new DateTime(2023, 1, 15);
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                DateTime result = InputValidator.GetValidDate("Prompt: ", "Error: ");
                Assert.AreEqual(expectedDate, result, "Test Case: GetValidDate_InvalidThenValidDateInput_ReturnsValidDate");
            }
        }

        [TestMethod]
        public void GetValidPriority_ValidInputInRangeFirstTry_ReturnsInput()
        {
            string simulatedInput = "7\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                int result = InputValidator.GetValidPriority("Prompt: ", "Error: ", 1, 10);
                Assert.AreEqual(7, result, "Test Case: GetValidPriority_ValidInputInRangeFirstTry_ReturnsInput");
            }
        }

        [TestMethod]
        public void GetOptionByNumberFromArray_ValidNumber_ReturnsCorrectOption()
        {
            string[] options = { "Apple", "Banana", "Cherry" };
            string simulatedInput = "2\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                string result = InputValidator.GetOptionByNumberFromArray("Prompt: ", "Error.", options);
                Assert.AreEqual("Banana", result, "Test Case: GetOptionByNumberFromArray_ValidNumber_ReturnsCorrectOption");
            }
        }

        [TestMethod]
        public void GetOptionByNumberFromArray_InvalidInputsThenValid_ReturnsCorrectOption()
        {
            string[] options = { "Apple", "Banana", "Cherry" };
            string simulatedInput = "0\n4\ntext\n\n   \n1\n";
            using (var stringReader = new StringReader(simulatedInput))
            {
                Console.SetIn(stringReader);
                string result = InputValidator.GetOptionByNumberFromArray("Prompt: ", "Error.", options);
                Assert.AreEqual("Apple", result, "Test Case: GetOptionByNumberFromArray_InvalidInputsThenValid_ReturnsCorrectOption");
            }
        }
    }
}
