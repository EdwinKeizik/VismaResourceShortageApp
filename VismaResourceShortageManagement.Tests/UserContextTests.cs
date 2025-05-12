using Microsoft.VisualStudio.TestTools.UnitTesting;
using VismaResourceShortageManagement.Services;
using System;

namespace VismaResourceShortageManagement.Tests
{
    [TestClass]
    public class UserContextTests
    {
        private UserContext _userContext = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            _userContext = new UserContext();
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenAdminNameLowercase_ShouldSetIsAdminToTrue()
        {
            string adminUserName = "admin";

            _userContext.SetCurrentUser(adminUserName);

            Assert.IsTrue(_userContext.IsAdmin, "User should be marked as admin for 'admin'.");
            Assert.AreEqual(adminUserName, _userContext.Name, "User name should be set correctly.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenAdminNameMixedCase_ShouldSetIsAdminToTrue()
        {
            string adminUserName = "Admin";

            _userContext.SetCurrentUser(adminUserName);

            Assert.IsTrue(_userContext.IsAdmin, "User should be marked as admin for 'Admin' (case-insensitive).");
            Assert.AreEqual(adminUserName, _userContext.Name, "User name should be set correctly.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenAdminNameUppercase_ShouldSetIsAdminToTrue()
        {
            string adminUserName = "ADMIN";

            _userContext.SetCurrentUser(adminUserName);

            Assert.IsTrue(_userContext.IsAdmin, "User should be marked as admin for 'ADMIN' (case-insensitive).");
            Assert.AreEqual(adminUserName, _userContext.Name, "User name should be set correctly.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenRegularName_ShouldSetIsAdminToFalse()
        {
            string regularUserName = "testUser";

            _userContext.SetCurrentUser(regularUserName);

            Assert.IsFalse(_userContext.IsAdmin, "User should not be marked as admin for a regular name.");
            Assert.AreEqual(regularUserName, _userContext.Name, "User name should be set correctly.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenEmptyName_ShouldSetIsAdminToFalseAndNameIsEmpty()
        {
            string emptyUserName = "";

            _userContext.SetCurrentUser(emptyUserName);

            Assert.IsFalse(_userContext.IsAdmin, "User should not be admin if name is empty.");
            Assert.AreEqual(emptyUserName, _userContext.Name, "Name should be set to empty string.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenWhitespaceName_ShouldSetIsAdminToFalseAndNameIsWhitespace()
        {
            string whitespaceUserName = "   ";

            _userContext.SetCurrentUser(whitespaceUserName);

            Assert.IsFalse(_userContext.IsAdmin, "User should not be admin if name is whitespace.");
            Assert.AreEqual(whitespaceUserName, _userContext.Name, "Name should be set to the whitespace string.");
        }

        [TestMethod]
        public void SetCurrentUser_WhenGivenNullName_ShouldThrowNullReferenceException()
        {
            string? nullUserName = null;

            Assert.ThrowsException<NullReferenceException>(() => _userContext.SetCurrentUser(nullUserName!),
                "Passing null as userName should throw NullReferenceException due to .Equals call on null.");
        }

        [TestMethod]
        public void UserContext_DefaultConstructor_InitializesPropertiesCorrectly()
        {
            Assert.AreEqual(string.Empty, _userContext.Name, "Default name should be empty string.");
            Assert.IsFalse(_userContext.IsAdmin, "Default IsAdmin should be false.");
        }
    }
}