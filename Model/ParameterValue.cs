using System;

namespace MugPlugin
{
    /// <summary>
    /// Класс параметра.
    /// </summary>
    public class ParameterValue
    {
        /// <summary>
        /// Поле для максимального значения параметра.
        /// </summary>
        private double _maxValue;

        /// <summary>
        /// Поле для минимального значения параметра.
        /// </summary>
        private double _minValue;

        /// <summary>
        /// Поле для значения параметра.
        /// </summary>
        private double _value;

        /// <summary>
        /// Конструктор класса ParameterValue.
        /// </summary>
        /// <param name="minValue">Минимальное значение параметра.</param>
        /// <param name="maxValue">Максимальное значение параметра.</param>
        /// <param name="value">Текущее значение параметра.</param>
        public ParameterValue(double minValue, double maxValue, double value)
        {
            try
            {
                Validate(minValue, maxValue, value);

                _minValue = minValue;
                _maxValue = maxValue;
                _value = value;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException($"Ошибка при инициализации ParameterValue: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Максимальное допустимое значение параметра.
        /// </summary>
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                try
                {
                    Validate(_minValue, value, _value);
                    _maxValue = value;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"Ошибка при установке MaxValue: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Минимальное допустимое значение параметра.
        /// </summary>
        public double MinValue
        {
            get => _minValue;
            set
            {
                try
                {
                    Validate(value, _maxValue, _value);
                    _minValue = value;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"Ошибка при установке MinValue: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Gets or sets для поля _value (значение).
        /// </summary>
        public double Value
        {
            get => _value;
            set
            {
                try
                {
                    Validate(_minValue, _maxValue, value);
                    _value = value;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException($"Ошибка при установке Value: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Общая функция для валидации диапазона и значения.
        /// </summary>
        /// <param name="min">Минимальное значение.</param>
        /// <param name="max">Максимальное значение.</param>
        /// <param name="value">Значение для проверки.</param>
        private void Validate(double min, double max, double value)
        {
            if (min > max)
            {
                throw new ArgumentException("MinValue не может быть больше MaxValue.");
            }

            if (value < min || value > max)
            {
                throw new ArgumentException("Значение выходит за допустимые пределы.");
            }
        }
    }
}
