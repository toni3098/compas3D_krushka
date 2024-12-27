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
            if (minValue > maxValue)
            {
                throw new ArgumentException("MinValue не может быть больше MaxValue.");
            }

            _minValue = minValue;
            _maxValue = maxValue;

            if (!Validate(value))  // Utilisation de Validate ici
            {
                throw new ArgumentException("Значение выходит за допустимые пределы.");
            }

            _value = value;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterValue"/> с диапазоном значений по умолчанию.
        /// </summary>
        public ParameterValue()
        {
            MinValue = 0;
            MaxValue = double.MaxValue;
            Value = 0;
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
                if (value < _minValue)
                {
                    throw new ArgumentException("MaxValue не может быть меньше MinValue.");
                }
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
                if (value > _maxValue)
                {
                    throw new ArgumentException("MinValue не может быть больше MaxValue.");
                }
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
                if (!Validate(value))  // Validation via the Validate method
                {
                    throw new ArgumentException("Значение выходит за допустимые пределы.");
                }
                _value = value;
            }
        }

        /// <summary>
        /// Функция для валидации значения.
        /// </summary>
        /// <param name="value">Значение для проверки.</param>
        /// <returns>Возвращает true, если значение в допустимом диапазоне, иначе false.</returns>
        public bool Validate(double value)
        {
            return value >= _minValue && value <= _maxValue;
        }
    }
}
