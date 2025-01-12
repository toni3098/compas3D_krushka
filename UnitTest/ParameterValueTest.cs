using Microsoft.VisualStudio.TestTools.UnitTesting;
using MugPlugin;  // Assurez-vous d'utiliser le bon namespace pour ParameterValue
using System;

namespace UnitTestProject
{
    /// <summary>
    /// Класс, содержащий тесты для проверки функциональности класса <see cref="MugPlugin.ParameterValue"/>.
    /// </summary>
    [TestClass]
    public class ParameterValueTests
    {
        /// <summary>
        /// Тест проверяет, что объект "ParameterValue" корректно инициализируется с заданными значениями.
        /// </summary>
        [TestMethod]
        public void ParameterValue_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(287, 420, 350);
            Assert.AreEqual(287, parameter.MinValue);
            Assert.AreEqual(420, parameter.MaxValue);
            Assert.AreEqual(350, parameter.Value);
        }

        /// <summary>
        /// Тест проверяет, что при создании объекта ParameterValue с некорректным диапазоном выбрасывается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        public void ParameterValue_InvalidMinMax_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => new ParameterValue(500, 400, 450));
        }

        /// <summary>
        /// Тест проверяет, что при создании объекта ParameterValue со значением вне допустимого диапазона выбрасывается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        public void ParameterValue_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => new ParameterValue(287, 420, 500));
        }

        /// <summary>
        /// Тест проверяет, что значение объекта ParameterValue корректно обновляется, если новое значение находится в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void ParameterValue_UpdateWithinRange_ShouldPass()
        {
            var parameter = new ParameterValue(287, 420, 350);
            parameter.Value = 400;
            Assert.AreEqual(400, parameter.Value);
        }

        /// <summary>
        /// Тест проверяет, что при попытке обновить значение объекта ParameterValue значением вне допустимого диапазона
        /// выбрасывается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        public void ParameterValue_UpdateOutOfRange_ShouldThrowException()
        {
            var parameter = new ParameterValue(287, 420, 350);
            Assert.ThrowsException<ArgumentException>(() => parameter.Value = 500);
        }

        /// <summary>
        ///  Тест, который проверяет, что обновление значения объекта ParameterValue в пределах допустимого диапазона не вызывает исключения.
        ///  Этот тест дублирует функциональность теста ParameterValue_UpdateWithinRange_ShouldPass.
        /// </summary>
        [TestMethod]
        public void ParameterValue_UpdateWithinRange_ShouldNotThrowException()
        {
            var parameter = new ParameterValue(287, 420, 350);
            parameter.Value = 400;
            Assert.AreEqual(400, parameter.Value);
        }

        /// <summary>
        /// Тест проверяет, что при попытке установить MaxValue с некорректным диапазоном выбрасывается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        public void ParameterValue_SetMaxValueInvalid_ShouldThrowException()
        {
            var parameter = new ParameterValue(287, 420, 350);
            Assert.ThrowsException<ArgumentException>(() => parameter.MaxValue = 200);
        }

        /// <summary>
        /// Тест проверяет, что при попытке установить MinValue с некорректным диапазоном выбрасывается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        public void ParameterValue_SetMinValueInvalid_ShouldThrowException()
        {
            var parameter = new ParameterValue(287, 420, 350);
            Assert.ThrowsException<ArgumentException>(() => parameter.MinValue = 500);
        }

        /// <summary>
        /// Тест проверяет, что при попытке обновить значение объекта ParameterValue с правильными диапазонами после изменения MaxValue.
        /// </summary>
        [TestMethod]
        public void ParameterValue_SetMaxValueValid_ShouldPass()
        {
            var parameter = new ParameterValue(287, 420, 350);
            parameter.MaxValue = 500;
            parameter.Value = 400; // Should pass as 400 is within the new range
            Assert.AreEqual(400, parameter.Value);
        }

        /// <summary>
        /// Тест проверяет, что при попытке обновить значение объекта ParameterValue с правильными диапазонами после изменения MinValue.
        /// </summary>
        [TestMethod]
        public void ParameterValue_SetMinValueValid_ShouldPass()
        {
            var parameter = new ParameterValue(287, 420, 350);
            parameter.MinValue = 200;
            parameter.Value = 250; // Should pass as 250 is within the new range
            Assert.AreEqual(250, parameter.Value);
        }
    }
}
