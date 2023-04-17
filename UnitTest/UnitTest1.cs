using Microsoft.VisualStudio.TestTools.UnitTesting;
using Technical_Software_Service;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void checkingData_correctly() // Проверка на заполененные поля при авторизации
        {
            string login = "Ivanova";
            string password = "Anuta365";
            bool except = true;
            bool actual = Page_Authorization.checkData(login, password);
            Assert.AreEqual(except, actual);
        }

        [TestMethod]
        public void checkingData_nocorrectly() // Проверка на незаполененные поля при авторизации
        {
            string login = "";
            string password = "";
            bool except = false;
            bool actual = Page_Authorization.checkData(login, password);
            Assert.AreEqual(except, actual);
        }

        //[TestMethod]
        //public void IsPass_WithValidPassword_ReturnsTrue() // Проверка на корректность пароля
        //{    
        //    string password = "Password1";
        //    bool result = Window_UpdatePassword.IsPass(password);
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void IsPass_WithValidPassword_ReturnsFalse() // Проверка на корректность пароля
        //{
        //    string password = "Pass1";
        //    bool result = Window_UpdatePassword.IsPass(password);
        //    Assert.IsFalse(result);
        //}

        [TestMethod]
        public void CoincidencePass_WhenPasswordsMatch_ReturnsTrue() // Проверка на совпадение паролей
        {
            string pass = "Alekseev22";
            string newPass = "Alekseev22";
            bool result = Window_UpdatePassword.CoincidencePass(pass, newPass);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void CoincidencePass_WhenPasswordsDoNotMatch_ReturnsFalse() // Проверка на не совпадение паролей
        {
            string pass = "password";
            string newPass = "newpassword";
            bool result = Window_UpdatePassword.CoincidencePass(pass, newPass);
            Assert.IsFalse(result);
        }

        //[DataTestMethod]
        //[DataRow("ABcd1234", true)]
        //[DataRow("abcdeFGH", false)]
        //[DataRow("1234567a", false)]
        //public void IsPass_WithMultiplePasswords_ReturnsExpectedValue(string password, bool expectedValue)
        //{
        //    Window_UpdatePassword window = new Window_UpdatePassword(user);
        //    PasswordBox passwordBox = new PasswordBox(); 
        //    string passwordBox.Password = password;
        //    bool actualValue = window.IsPass(passwordBox.Password);
        //    Assert.AreEqual(expectedValue, actualValue);
        //}
    }
}
