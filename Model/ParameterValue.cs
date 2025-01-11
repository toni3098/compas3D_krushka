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
            Validate(minValue, maxValue, value);

            _minValue = minValue;
            _maxValue = maxValue;
            _value = value;
        }

        /// <summary>
        /// Максимальное допустимое значение параметра.
        /// </summary>
        public double MaxValue
        {
            get => _maxValue;
            set
            {
                //TODO: validation?
                Validate(_minValue, value, _value);
                _maxValue = value;
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
                //TODO: validation?
                Validate(value, _maxValue, _value);
                _minValue = value;
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
                Validate(_minValue, _maxValue, value);
                _value = value;
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