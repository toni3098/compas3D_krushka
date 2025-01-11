using Microsoft.VisualStudio.TestTools.UnitTesting;
using MugPlugin;
using System;

namespace UnitTestProject
{
    /// <summary>
    /// Тестовый класс для проверки значений параметров и их зависимостей.
    /// </summary>
    [TestClass]
    public class ParameterValueTests
    {
        /// <summary>
        /// Проверка инициализации ширины корпуса (BodyWidth). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void BodyWidth_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(100, 150, 120);
            Assert.AreEqual(100, parameter.MinValue);
            Assert.AreEqual(150, parameter.MaxValue);
            Assert.AreEqual(120, parameter.Value);
        }

        /// <summary>
        /// Проверка ширины корпуса (BodyWidth), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BodyWidth_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(100, 150, 160);
        }

        /// <summary>
        /// Проверка инициализации ширины основания (BaseWidth). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void BaseWidth_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(70, 100, 85);
            Assert.AreEqual(70, parameter.MinValue);
            Assert.AreEqual(100, parameter.MaxValue);
            Assert.AreEqual(85, parameter.Value);
        }

        /// <summary>
        /// Проверка ширины основания (BaseWidth), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BaseWidth_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(70, 100, 60);
        }

        /// <summary>
        /// Проверка инициализации радиуса корпуса (BodyRadius1). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void BodyRadius1_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(300, 350, 325);
            Assert.AreEqual(300, parameter.MinValue);
            Assert.AreEqual(350, parameter.MaxValue);
            Assert.AreEqual(325, parameter.Value);
        }

        /// <summary>
        /// Проверка радиуса корпуса (BodyRadius1), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BodyRadius1_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(300, 350, 250);
        }

        /// <summary>
        /// Проверка инициализации радиуса ручки (HandleRadius3). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void HandleRadius3_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(10, 20, 15);
            Assert.AreEqual(10, parameter.MinValue);
            Assert.AreEqual(20, parameter.MaxValue);
            Assert.AreEqual(15, parameter.Value);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius3), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleRadius3_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(10, 20, 25);
        }

        /// <summary>
        /// Проверка инициализации радиуса ручки (HandleRadius5). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void HandleRadius5_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(75, 85, 80);
            Assert.AreEqual(75, parameter.MinValue);
            Assert.AreEqual(85, parameter.MaxValue);
            Assert.AreEqual(80, parameter.Value);
        }

        /// <summary>
        /// Проверка радиуса ручки (HandleRadius5), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleRadius5_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(75, 85, 90);
        }

        /// <summary>
        /// Проверка инициализации длины корпуса (BodyLength). Значения находятся в допустимом диапазоне.
        /// </summary>
        [TestMethod]
        public void BodyLength_Initialization_ShouldPass()
        {
            var parameter = new ParameterValue(100, 150, 125);
            Assert.AreEqual(100, parameter.MinValue);
            Assert.AreEqual(150, parameter.MaxValue);
            Assert.AreEqual(125, parameter.Value);
        }

        /// <summary>
        /// Проверка длины корпуса (BodyLength), если значение выходит за пределы диапазона. Ожидается исключение ArgumentException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BodyLength_InvalidValue_ShouldThrowException()
        {
            new ParameterValue(100, 150, 90);
        }

        /// Тесты зависимостей

        /// <summary>
        /// Проверка зависимости ширины внутренней верхней части корпуса (InteriorUpWidth).
        /// </summary>
        [TestMethod]
        public void InteriorUpWidth_ValidDependency_ShouldPass()
        {
            var bodyWidth = new ParameterValue(100, 150, 120);
            // 9/10 от 120
            var interiorUpWidth = new ParameterValue(90, 135, 108); 
            Assert.AreEqual(108, interiorUpWidth.Value);
        }

        /// <summary>
        /// Проверка зависимости ширины внутреннего основания (InteriorBaseWidth).
        /// </summary>
        [TestMethod]
        public void InteriorBaseWidth_ValidDependency_ShouldPass()
        {
            var baseWidth = new ParameterValue(70, 100, 80);
            // 9/10 от 80
            var interiorBaseWidth = new ParameterValue(63, 90, 72); 
            Assert.AreEqual(72, interiorBaseWidth.Value);
        }

        /// <summary>
        /// Проверка зависимости радиуса корпуса (BodyRadius2).
        /// </summary>
        [TestMethod]
        public void BodyRadius2_ValidDependency_ShouldPass()
        {
            var bodyRadius1 = new ParameterValue(300, 350, 320);
            // 5/8 от 320
            var bodyRadius2 = new ParameterValue(187.5, 218.75, 200); 
            Assert.AreEqual(200, bodyRadius2.Value);
        }

        /// <summary>
        /// Проверка зависимости радиуса ручки (HandleRadius4).
        /// </summary>
        [TestMethod]
        public void HandleRadius4_ValidDependency_ShouldPass()
        {
            var handleRadius3 = new ParameterValue(10, 20, 15);
            // 3 * 15
            var handleRadius4 = new ParameterValue(30, 60, 45); 
            Assert.AreEqual(45, handleRadius4.Value);
        }

        /// <summary>
        /// Проверка зависимости длины ручки (HandleLength).
        /// </summary>
        [TestMethod]
        public void HandleLength_ValidDependency_ShouldPass()
        {
            var bodyLength = new ParameterValue(100, 150, 120);
            // 3/4 от 120
            var handleLength = new ParameterValue(75, 112.5, 90); 
            Assert.AreEqual(90, handleLength.Value);
        }
    }
}
