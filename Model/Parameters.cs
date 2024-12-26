using System;
using System.Collections.Generic;

namespace MugPlugin
{
    /// <summary>
    /// Класс для управления параметрами кружки.
    /// </summary>
    public class Parameters
    {
        // Поле для хранения допустимой погрешности.
        private const double Tolerance = 8.0; // Погрешность в мм.

        /// <summary>
        /// Словарь для хранения значений параметров.
        /// </summary>
        private Dictionary<ParameterType, ParameterValue> _parameters;

        /// <summary>
        /// Словарь для управления зависимостями между параметрами.
        /// </summary>
        private Dictionary<ParameterType, (ParameterType, double)> _dependencies;

        /// <summary>
        /// Свойство для доступа ко всем параметрам.
        /// </summary>
        /// <summary>
        /// Свойство для доступа ко всем параметрам.
        /// </summary>
        public Dictionary<ParameterType, ParameterValue> AllParameters
        {
            get => _parameters;
            set
            {
                if (value != null)
                {
                    // Проверяем каждый параметр в словаре.
                    foreach (var param in value)
                    {
                        // Проверяем, что значение параметра валидно.
                        ValidateSingleParameter(param.Key);
                    }
                }
                _parameters = value;
                // После установки обновляем зависимые значения.
                UpdateDependentValues();
            }
        }

        /// <summary>
        /// Конструктор класса Parameters.
        /// </summary>
        public Parameters()
        {
            _parameters = new Dictionary<ParameterType, ParameterValue>();
            _dependencies = new Dictionary<ParameterType, (ParameterType, double)>();

            // Настройка зависимостей между параметрами.
            ConfigureDependencies();
        }

        /// <summary>
        /// Метод для настройки зависимостей между параметрами.
        /// </summary>
        private void ConfigureDependencies()
        {
            // Внутренняя верхняя ширина = 9/10 * Ширина тела
            _dependencies[ParameterType.InteriorUpWidth] = (ParameterType.BodyWidth, 0.9);

            // Внутренняя ширина основания = 9/10 * Ширина основания
            _dependencies[ParameterType.InteriorBaseWidth] = (ParameterType.BaseWidth, 0.9);

            // Радиус тела 2 = 5/8 * Радиус тела 1
            _dependencies[ParameterType.BodyRadius2] = (ParameterType.BodyRadius1, 5.0 / 8.0);

            // Радиус ручки 4 = 2 * Радиус ручки 3
            _dependencies[ParameterType.HandleRadius4] = (ParameterType.HandleRadius3, 2.0);

            // Длина ручки = 3/4 * Длина тела
            _dependencies[ParameterType.HandleLength] = (ParameterType.BodyLength, 0.75);
        }

        /// <summary>
        /// Проверка всех параметров.
        /// </summary>
        public void Validate()
        {
            foreach (var param in _parameters)
            {
                ValidateSingleParameter(param.Key);
            }

            ValidateDependencies();
        }

        /// <summary>
        /// Метод для проверки отдельного параметра.
        /// </summary>
        private void ValidateSingleParameter(ParameterType type)
        {
            if (!_parameters.ContainsKey(type))
            {
                throw new ArgumentException($"Параметр {type} не задан.");
            }

            double value = _parameters[type].Value;

            switch (type)
            {
                case ParameterType.BodyWidth:
                    if (value < 100 - Tolerance || value > 150 + Tolerance)
                    {
                        throw new ArgumentException($"Ширина тела должна быть в диапазоне от 100 до 150 мм (+- {Tolerance} мм).");
                    }
                    break;

                case ParameterType.BaseWidth:
                    if (value < 70 - Tolerance || value > 100 + Tolerance)
                    {
                        throw new ArgumentException($"Ширина основания должна быть в диапазоне от 70 до 100 мм (+- {Tolerance} мм).");
                    }
                    break;

                case ParameterType.BodyRadius1:
                    if (value < 300 - Tolerance || value > 350 + Tolerance)
                    {
                        throw new ArgumentException($"Радиус тела 1 должен быть в диапазоне от 300 до 350 мм (+- {Tolerance} мм).");
                    }
                    break;

                case ParameterType.HandleRadius3:
                    if (value < 10 - Tolerance || value > 20 + Tolerance)
                    {
                        throw new ArgumentException($"Радиус ручки 3 должен быть в диапазоне от 10 до 20 мм (+- {Tolerance} мм).");
                    }
                    break;

                case ParameterType.HandleRadius5:
                    if (value < 75 - Tolerance || value > 85 + Tolerance)
                    {
                        throw new ArgumentException($"Радиус ручки 5 должен быть в диапазоне от 75 до 85 мм (+- {Tolerance} мм).");
                    }
                    break;

                case ParameterType.BodyLength:
                    if (value < 100 - Tolerance || value > 150 + Tolerance)
                    {
                        throw new ArgumentException($"Длина тела должна быть в диапазоне от 100 до 150 мм (+- {Tolerance} мм).");
                    }
                    break;
            }
        }

        /// <summary>
        /// Проверка зависимостей между параметрами.
        /// </summary>
        private void ValidateDependencies()
        {
            foreach (var dependency in _dependencies)
            {
                var dependentParam = dependency.Key;
                var (baseParam, factor) = dependency.Value;

                if (!_parameters.ContainsKey(baseParam) || !_parameters.ContainsKey(dependentParam))
                {
                    continue;
                }

                double expectedValue = _parameters[baseParam].Value * factor;
                if (Math.Abs(_parameters[dependentParam].Value - expectedValue) > Tolerance)
                {
                    throw new ArgumentException($"Значение {dependentParam} должно быть в пределах ±{Tolerance} мм от {factor} * {baseParam}, что составляет {expectedValue}.");
                }
            }
        }

        /// <summary>
        /// Обновление зависимых параметров.
        /// </summary>
        private void UpdateDependentValues()
        {
            foreach (var dependency in _dependencies)
            {
                var dependentParam = dependency.Key;
                var (baseParam, factor) = dependency.Value;

                if (_parameters.ContainsKey(baseParam))
                {
                    double newValue = _parameters[baseParam].Value * factor;
                    if (_parameters.ContainsKey(dependentParam))
                    {
                        _parameters[dependentParam].Value = newValue;
                    }
                    else
                    {
                        _parameters[dependentParam] = new ParameterValue(0, double.MaxValue, newValue);
                    }
                }
            }
        }

        /// <summary>
        /// Метод для добавления или обновления параметра.
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="value">Значение параметра.</param>
        /// <param name="minValue">Минимально допустимое значение.</param>
        /// <param name="maxValue">Максимально допустимое значение.</param>
        public void SetParameter(ParameterType parameterType, double value, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            if (_parameters.ContainsKey(parameterType))
            {
                _parameters[parameterType].Value = value;
            }
            else
            {
                _parameters[parameterType] = new ParameterValue(minValue, maxValue, value);
            }

            ValidateSingleParameter(parameterType);
            UpdateDependentValues();
        }
    }
}
