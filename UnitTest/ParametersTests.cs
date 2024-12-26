using Microsoft.VisualStudio.TestTools.UnitTesting;
using MugPlugin;
using System;

namespace UnitTest
{
    /// <summary>
    /// Тестовый класс для проверки параметров.
    /// </summary>
    [TestClass]
    public class ParametersTests
    {
        private Parameters _parameters;

        /// <summary>
        /// Метод инициализации для настройки объекта параметров перед тестами.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _parameters = new Parameters();
        }

        /// <summary>
        /// Проверка ширины корпуса, если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_BodyWidth_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 120, 100, 150);
            Assert.AreEqual(120, _parameters.AllParameters[ParameterType.BodyWidth].Value);
        }

        /// <summary>
        /// Проверка ширины корпуса, если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_BodyWidth_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 160, 100, 150);
        }

        /// <summary>
        /// Проверка ширины основания, если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_BaseWidth_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 85, 70, 100);
            Assert.AreEqual(85, _parameters.AllParameters[ParameterType.BaseWidth].Value);
        }

        /// <summary>
        /// Проверка ширины основания, если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_BaseWidth_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 60, 70, 100);
        }

        /// <summary>
        /// Проверка радиуса корпуса (BodyRadius1), если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_BodyRadius1_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyRadius1, 325, 300, 350);
            Assert.AreEqual(325, _parameters.AllParameters[ParameterType.BodyRadius1].Value);
        }

        /// <summary>
        /// Проверка радиуса корпуса (BodyRadius1), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_BodyRadius1_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.BodyRadius1, 250, 300, 350);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius3), если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius3_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.HandleRadius3, 15, 10, 20);
            Assert.AreEqual(15, _parameters.AllParameters[ParameterType.HandleRadius3].Value);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius3), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_HandleRadius3_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.HandleRadius3, 25, 10, 20);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius5), если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius5_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.HandleRadius5, 80, 75, 85);
            Assert.AreEqual(80, _parameters.AllParameters[ParameterType.HandleRadius5].Value);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius5), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_HandleRadius5_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.HandleRadius5, 90, 75, 85);
        }

        /// <summary>
        /// Проверка длины корпуса (BodyLength), если значение находится в пределах допустимого диапазона.
        /// </summary>
        [TestMethod]
        public void Validate_BodyLength_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyLength, 125, 100, 150);
            Assert.AreEqual(125, _parameters.AllParameters[ParameterType.BodyLength].Value);
        }

        /// <summary>
        /// Проверка длины корпуса (BodyLength), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_BodyLength_OutOfRange_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.BodyLength, 160, 100, 150);
        }

        /// <summary>
        /// Проверка зависимости ширины внутреннего основания.
        /// </summary>
        [TestMethod]
        public void Validate_InteriorBaseWidth_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 100, 70, 100);
            _parameters.Validate();
            Assert.AreEqual(90, _parameters.AllParameters[ParameterType.InteriorBaseWidth].Value);
        }

        /// <summary>
        /// Проверка зависимости ширины внутреннего основания, если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_InteriorBaseWidth_Dependency_ShouldThrowException()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 110, 70, 100);
            _parameters.Validate();
        }

        /// <summary>
        /// Проверка всех параметров с корректными значениями и зависимостями.
        /// </summary>
        [TestMethod]
        public void Validate_AllParameters_ValidValuesAndDependencies_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 120, 100, 150);
            _parameters.SetParameter(ParameterType.BaseWidth, 90, 70, 100);
            _parameters.SetParameter(ParameterType.BodyRadius1, 320, 300, 350);
            _parameters.SetParameter(ParameterType.HandleRadius3, 15, 10, 20);
            _parameters.SetParameter(ParameterType.HandleRadius5, 80, 75, 85);
            _parameters.SetParameter(ParameterType.BodyLength, 120, 100);
        }
    }
}
