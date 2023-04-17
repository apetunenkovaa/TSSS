using Microsoft.VisualStudio.TestTools.UnitTesting;
using Technical_Software_Service;
using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Diagnostics.Eventing.Reader;

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

        [TestMethod]
        public void IsPass_WithValidPassword_ReturnsTrue() // Проверка на корректность пароля
        {
            string password = "Password11";
            bool result = Window_UpdatePassword.IsPass(password);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPass_WithValidPassword_ReturnsFalse() // Проверка некорректного пароля
        {
            string password = "Pass1";
            bool result = Window_UpdatePassword.IsPass(password);
            Assert.IsFalse(result);
        }

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

        [TestMethod]
        public void IsCheckEmail_WithValidEmail_ReturnsTrue() // Проверка на корректность почты
        {
            string email = "petr@yandex.ru";
            bool result = Window_Users.IsCheckEmail(email);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsCheckEmail_WithValidEmail_ReturnsFalse() // Проверка на некорректность почты
        {
            string email = "petr.yandex.ru";
            bool result = Window_Users.IsCheckEmail(email);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsCheckEmail_TypeOfEmail_ResultCorrectly() // Проверка на тип сохранения данных
        {
            string email = "email@gmail.ru";
            bool actual = Window_Users.IsCheckEmail(email);
            Assert.IsInstanceOfType(actual, typeof(bool));
        }

        [TestMethod]
        public void IsCheckEmail_TypeToStringOfEmail_ResultNoCorrectly() 
        {
            string email = "email@gmail.ru";
            bool actual = Window_Users.IsCheckEmail(email);
            Assert.IsNotInstanceOfType(actual, typeof(string));
        }

        [TestMethod]
        public void CheckData_NotNullCheck()
        {
            string login = "4854789";
            string password = "Password11";
            bool actual = Page_Authorization.checkData(login, password);
            Assert.IsNotNull(actual);
        }
    }
}
