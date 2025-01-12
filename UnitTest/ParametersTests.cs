using Microsoft.VisualStudio.TestTools.UnitTesting;
using MugPlugin;
using System;

namespace UnitTestProject
{
    [TestClass]
    public class ParametersTests
    {
        private Parameters _parameters;

        /// <summary>
        /// Метод, который выполняется перед каждым тестом для инициализации объекта Parameters.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _parameters = new Parameters();
        }

        /// <summary>
        /// Тестирует, что ширина тела (BodyWidth) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_BodyWidth_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 120, 100, 150);
            Assert.AreEqual(120, _parameters.AllParameters[ParameterType.BodyWidth].Value);
        }

        /// <summary>
        /// Тестирует, что ширина тела (BodyWidth) с недопустимым значением вызывает исключение.
        /// </summary>
        [TestMethod]
        public void Validate_BodyWidth_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => _parameters.SetParameter(ParameterType.BodyWidth, 160, 100, 150));
        }

        /// <summary>
        /// Тестирует, что внутренняя ширина верхней части (InteriorUpWidth) правильно обновляется в зависимости от ширины тела (BodyWidth).
        /// </summary>
        [TestMethod]
        public void Validate_InteriorUpWidth_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 120, 100, 150);
            double expectedInteriorUpWidth = 120 * 0.9;
            Assert.AreEqual(expectedInteriorUpWidth, _parameters.AllParameters[ParameterType.InteriorUpWidth].Value, 0.1);
        }

        /// <summary>
        /// Тестирует, что внутренняя ширина основания (InteriorBaseWidth) правильно обновляется в зависимости от ширины основания (BaseWidth).
        /// </summary>
        [TestMethod]
        public void Validate_InteriorBaseWidth_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 90, 70, 100);
            double expectedInteriorBaseWidth = 90 * 0.9;
            Assert.AreEqual(expectedInteriorBaseWidth, _parameters.AllParameters[ParameterType.InteriorBaseWidth].Value, 0.1);
        }

        /// <summary>
        /// Тестирует, что ширина основания (BaseWidth) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_BaseWidth_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BaseWidth, 90, 70, 100);
            Assert.AreEqual(90, _parameters.AllParameters[ParameterType.BaseWidth].Value);
        }

        /// <summary>
        /// Тестирует, что ширина основания (BaseWidth) с недопустимым значением вызывает исключение.
        /// </summary>
        [TestMethod]
        public void Validate_BaseWidth_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => _parameters.SetParameter(ParameterType.BaseWidth, 110, 70, 100));
        }

        /// <summary>
        /// Тестирует, что радиус тела 1 (BodyRadius1) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_BodyRadius1_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyRadius1, 320, 300, 350);
            Assert.AreEqual(320, _parameters.AllParameters[ParameterType.BodyRadius1].Value);
        }

        /// <summary>
        /// Тестирует, что радиус тела 1 (BodyRadius1) с недопустимым значением вызывает исключение.
        /// </summary>
        [TestMethod]
        public void Validate_BodyRadius1_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => _parameters.SetParameter(ParameterType.BodyRadius1, 380, 300, 350));
        }

        /// <summary>
        /// Тестирует, что радиус тела 2 (BodyRadius2) правильно вычисляется на основе BodyRadius1.
        /// </summary>
        [TestMethod]
        public void Validate_BodyRadius2_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyRadius1, 320, 300, 350);
            double expectedBodyRadius2 = 320 * (5.0 / 8.0);
            Assert.AreEqual(expectedBodyRadius2, _parameters.AllParameters[ParameterType.BodyRadius2].Value, 0.1);
        }

        /// <summary>
        /// Тестирует, что радиус ручки 3 (HandleRadius3) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius3_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.HandleRadius3, 15, 10, 20);
            Assert.AreEqual(15, _parameters.AllParameters[ParameterType.HandleRadius3].Value);
        }

        /// <summary>
        /// Тестирует, что радиус ручки 3 (HandleRadius3) с недопустимым значением вызывает исключение.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius3_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => _parameters.SetParameter(ParameterType.HandleRadius3, 25, 10, 20));
        }

        /// <summary>
        /// Тестирует, что радиус ручки 4 (HandleRadius4) правильно вычисляется как удвоенный радиус HandleRadius3.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius4_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.HandleRadius3, 15, 10, 20);
            double expectedHandleRadius4 = 15 * 2;
            Assert.AreEqual(expectedHandleRadius4, _parameters.AllParameters[ParameterType.HandleRadius4].Value, 0.1);
        }

        /// <summary>
        /// Тестирует, что радиус ручки 5 (HandleRadius5) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_HandleRadius5_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.HandleRadius5, 80, 75, 85);
            Assert.AreEqual(80, _parameters.AllParameters[ParameterType.HandleRadius5].Value);
        }

        /// <summary>
        /// Тестирует, что длина тела (BodyLength) с допустимым значением проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_BodyLength_WithinRange_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyLength, 120, 100, 150);
            Assert.AreEqual(120, _parameters.AllParameters[ParameterType.BodyLength].Value);
        }

        /// <summary>
        /// Тестирует, что длина тела (BodyLength) с недопустимым значением вызывает исключение.
        /// </summary>
        [TestMethod]
        public void Validate_BodyLength_OutOfRange_ShouldThrowException()
        {
            Assert.ThrowsException<ArgumentException>(() => _parameters.SetParameter(ParameterType.BodyLength, 160, 100, 150));
        }

        /// <summary>
        /// Тестирует, что длина ручки (HandleLength) правильно вычисляется как 3/4 длины тела.
        /// </summary>
        [TestMethod]
        public void Validate_HandleLength_Dependency_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyLength, 120, 100, 150);
            double expectedHandleLength = 120 * 0.75;
            Assert.AreEqual(expectedHandleLength, _parameters.AllParameters[ParameterType.HandleLength].Value, 0.1);
        }

        /// <summary>
        /// Тестирует, что установка всех параметров с правильными значениями проходит проверку.
        /// </summary>
        [TestMethod]
        public void Validate_AllParameters_ValidValues_ShouldPass()
        {
            _parameters.SetParameter(ParameterType.BodyWidth, 120, 100, 150);
            _parameters.SetParameter(ParameterType.BaseWidth, 90, 70, 100);
            _parameters.SetParameter(ParameterType.BodyRadius1, 320, 300, 350);
            _parameters.SetParameter(ParameterType.HandleRadius3, 15, 10, 20);
            _parameters.SetParameter(ParameterType.HandleRadius5, 80, 75, 85);
            _parameters.SetParameter(ParameterType.BodyLength, 120, 100, 150);

            _parameters.Validate();
        }
    }
}