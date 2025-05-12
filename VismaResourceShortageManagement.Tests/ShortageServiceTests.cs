using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using VismaResourceShortageManagement.Models;
using VismaResourceShortageManagement.Services;

namespace VismaResourceShortageManagement.Tests
{
    [TestClass]
    public class ShortageServiceTests
    {
        private UserContext _userContext = null!;
        private FakeFileStorageService _fakeFileStorage = null!;
        private ShortageService _shortageService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _userContext = new UserContext();
            _fakeFileStorage = new FakeFileStorageService();
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);
        }

        [TestMethod]
        public void RegisterShortage_WhenNewUniqueShortage_ReturnsSuccessAndAddsToList()
        {
            _userContext.SetCurrentUser("TestUser");
            var newShortage = new Shortage { Title = "Projector", Name = "TestUser", Room = "Meeting room", Category = "Electronics", Priority = 5, CreatedOn = DateTime.Now };

            var result = _shortageService.RegisterShortage(newShortage);

            Assert.AreEqual(ShortageService.RegistrationResult.Success, result);
            Assert.AreEqual(1, _fakeFileStorage.ShortagesStorage.Count);
            Assert.IsTrue(_fakeFileStorage.ShortagesStorage.Contains(newShortage));
        }

        [TestMethod]
        public void RegisterShortage_WhenNullShortage_ThrowsArgumentNullException()
        {
            _userContext.SetCurrentUser("TestUser");

            Assert.ThrowsException<ArgumentNullException>(() => _shortageService.RegisterShortage(null!));
        }

        [TestMethod]
        public void RegisterShortage_DuplicateWithLowerNewPriority_ReturnsDuplicateNotOverridden()
        {
            _userContext.SetCurrentUser("TestUser");
            var initialDate = DateTime.Now.AddDays(-1);
            var existingShortage = new Shortage { Title = "Projector", Name = "OldUser", Room = "Meeting room", Category = "Electronics", Priority = 7, CreatedOn = initialDate };
            _fakeFileStorage.AddShortageDirectly(existingShortage);
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var newShortageAttempt = new Shortage { Title = "Projector", Name = "TestUser", Room = "Meeting room", Category = "Electronics", Priority = 5, CreatedOn = DateTime.Now };

            var result = _shortageService.RegisterShortage(newShortageAttempt);

            Assert.AreEqual(ShortageService.RegistrationResult.DuplicateNotOverridden, result);
            Assert.AreEqual(1, _fakeFileStorage.ShortagesStorage.Count);
            var shortageInStorage = _fakeFileStorage.ShortagesStorage.First();
            Assert.AreEqual(7, shortageInStorage.Priority);
            Assert.AreEqual("OldUser", shortageInStorage.Name);
        }

        [TestMethod]
        public void RegisterShortage_DuplicateWithHigherNewPriority_ReturnsUpdatedAndUpdatesItem()
        {
            _userContext.SetCurrentUser("TestUserUpdater");
            var initialDate = DateTime.Now.AddDays(-1);
            var existingShortage = new Shortage { Title = "Projector", Name = "OldUser", Room = "Meeting room", Category = "Electronics", Priority = 5, CreatedOn = initialDate };
            _fakeFileStorage.AddShortageDirectly(existingShortage);
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var newDateForUpdate = DateTime.Now;
            var overridingShortage = new Shortage { Title = "Projector", Name = "TestUserUpdater", Room = "Meeting room", Category = "Electronics", Priority = 7, CreatedOn = newDateForUpdate };

            var result = _shortageService.RegisterShortage(overridingShortage);

            Assert.AreEqual(ShortageService.RegistrationResult.Updated, result);
            Assert.AreEqual(1, _fakeFileStorage.ShortagesStorage.Count);
            var updatedShortageInStorage = _fakeFileStorage.ShortagesStorage.First();
            Assert.AreEqual(7, updatedShortageInStorage.Priority);
            Assert.AreEqual("TestUserUpdater", updatedShortageInStorage.Name);
            Assert.AreEqual(newDateForUpdate, updatedShortageInStorage.CreatedOn);
        }

        [TestMethod]
        public void DeleteShortage_AsAdmin_ReturnsSuccessAndRemovesItem()
        {
            _userContext.SetCurrentUser("admin");
            var shortage = new Shortage { Title = "Coffee", Name = "OtherUser", Room = "Kitchen", Category = "Food", Priority = 10, CreatedOn = DateTime.Now };
            _fakeFileStorage.AddShortageDirectly(shortage);
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.DeleteShortage("Coffee", "Kitchen");

            Assert.AreEqual(ShortageService.DeletionResult.Success, result);
            Assert.AreEqual(0, _fakeFileStorage.ShortagesStorage.Count);
        }

        [TestMethod]
        public void DeleteShortage_AsCreator_ReturnsSuccessAndRemovesItem()
        {
            _userContext.SetCurrentUser("CreatorUser");
            var shortage = new Shortage { Title = "Milk", Name = "CreatorUser", Room = "Kitchen", Category = "Food", Priority = 8, CreatedOn = DateTime.Now };
            _fakeFileStorage.AddShortageDirectly(shortage);
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.DeleteShortage("Milk", "Kitchen");

            Assert.AreEqual(ShortageService.DeletionResult.Success, result);
            Assert.AreEqual(0, _fakeFileStorage.ShortagesStorage.Count);
        }

        [TestMethod]
        public void DeleteShortage_NotAdminNotCreator_ReturnsNoPermission()
        {
            _userContext.SetCurrentUser("OtherUser");
            var shortage = new Shortage { Title = "Sugar", Name = "CreatorUser", Room = "Kitchen", Category = "Food", Priority = 7, CreatedOn = DateTime.Now };
            _fakeFileStorage.AddShortageDirectly(shortage);
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.DeleteShortage("Sugar", "Kitchen");

            Assert.AreEqual(ShortageService.DeletionResult.NoPermission, result);
            Assert.AreEqual(1, _fakeFileStorage.ShortagesStorage.Count);
        }

        [TestMethod]
        public void DeleteShortage_ItemNotFound_ReturnsNotFound()
        {
            _userContext.SetCurrentUser("TestUser");

            var result = _shortageService.DeleteShortage("NonExistent", "Kitchen");

            Assert.AreEqual(ShortageService.DeletionResult.NotFound, result);
        }

        [TestMethod]
        public void DeleteShortage_NullOrEmptyTitleOrRoom_ReturnsNotFound()
        {
            _userContext.SetCurrentUser("TestUser");

            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage(null!, "Kitchen"));
            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage("", "Kitchen"));
            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage("   ", "Kitchen"));
            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage("ValidTitle", null!));
            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage("ValidTitle", ""));
            Assert.AreEqual(ShortageService.DeletionResult.NotFound, _shortageService.DeleteShortage("ValidTitle", "   "));
        }

        [TestMethod]
        public void GetShortagesToDisplay_AsAdmin_ReturnsAllShortagesSortedByPriority()
        {
            _userContext.SetCurrentUser("admin");
            var s1 = new Shortage { Title = "S1", Name = "UserA", Room = "R1", Category = "C1", Priority = 5, CreatedOn = DateTime.Now.AddHours(-3) };
            var s2 = new Shortage { Title = "S2", Name = "UserB", Room = "R2", Category = "C2", Priority = 10, CreatedOn = DateTime.Now.AddHours(-1) };
            var s3 = new Shortage { Title = "S3", Name = "UserA", Room = "R1", Category = "C1", Priority = 1, CreatedOn = DateTime.Now.AddHours(-2) };
            _fakeFileStorage.ShortagesStorage.AddRange(new List<Shortage> { s1, s2, s3 });
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.GetShortagesToDisplay();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("S2", result[0].Title);
            Assert.AreEqual("S1", result[1].Title);
            Assert.AreEqual("S3", result[2].Title);
        }

        [TestMethod]
        public void GetShortagesToDisplay_AsRegularUser_ReturnsOnlyOwnShortagesSortedByPriority()
        {
            _userContext.SetCurrentUser("UserA");
            var s1 = new Shortage { Title = "S1_UserA", Name = "UserA", Room = "R1", Category = "C1", Priority = 7, CreatedOn = DateTime.Now.AddHours(-2) };
            var s2 = new Shortage { Title = "S2_UserB", Name = "UserB", Room = "R2", Category = "C2", Priority = 10, CreatedOn = DateTime.Now.AddHours(-1) };
            var s3 = new Shortage { Title = "S3_UserA", Name = "UserA", Room = "R1", Category = "C1", Priority = 3, CreatedOn = DateTime.Now.AddHours(-3) };
            _fakeFileStorage.ShortagesStorage.AddRange(new List<Shortage> { s1, s2, s3 });
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.GetShortagesToDisplay();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("S1_UserA", result[0].Title);
            Assert.AreEqual("S3_UserA", result[1].Title);
            Assert.IsFalse(result.Any(s => s.Name == "UserB"));
        }

        [TestMethod]
        public void GetShortagesToDisplay_FilterByTitle_ReturnsMatchingShortages()
        {
            _userContext.SetCurrentUser("admin");
            var s1 = new Shortage { Title = "Wireless Mouse", Name = "UserA", Room = "R1", Category = "Electronics", Priority = 5, CreatedOn = DateTime.Now };
            var s2 = new Shortage { Title = "Keyboard", Name = "UserB", Room = "R2", Category = "Electronics", Priority = 10, CreatedOn = DateTime.Now };
            var s3 = new Shortage { Title = "Mouse Pad", Name = "UserA", Room = "R1", Category = "Other", Priority = 1, CreatedOn = DateTime.Now };
            _fakeFileStorage.ShortagesStorage.AddRange(new List<Shortage> { s1, s2, s3 });
            _shortageService = new ShortageService(_userContext, _fakeFileStorage);

            var result = _shortageService.GetShortagesToDisplay(filterTitle: "Mouse");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(s => s.Title == "Wireless Mouse"));
            Assert.IsTrue(result.Any(s => s.Title == "Mouse Pad"));
        }

        [TestMethod]
        public void GetShortagesToDisplay_WhenNoShortages_ReturnsEmptyList()
        {
            _userContext.SetCurrentUser("admin");

            var result = _shortageService.GetShortagesToDisplay();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}