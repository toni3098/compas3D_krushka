using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MugPlugin
{
    /// <summary>
    /// Класс MainForm, представляющий основную форму приложения.
    /// </summary>
    public partial class MugPlugin : Form
    {
        //TODO: RSDN
        /// <summary>
        /// Поле хранящее в себе ошибок.
        /// </summary>
        public List<string> errorMessages = new List<string>();

        /// <summary>
        /// Поле хранящее объект класса Builder.
        /// </summary>
        private Builder _builder = new Builder();

        /// <summary>
        /// Поле хранящее в себе объект класса Parameters.
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Initializes a new instance of the <see cref="MugPlugin"/> class.
        /// </summary>
        public MugPlugin()
        {
            this.InitializeComponent();
            error_label.Visible = false;
        }

        /// <summary>
        /// Обработчик события загрузки формы.
        /// Инициализирует словарь параметров.
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            this._parameters.AllParameters = new Dictionary<ParameterType, ParameterValue>();
            if (_parameters.AllParameters.Count == 0)
            {
                _parameters.AllParameters[ParameterType.BodyWidth] = new ParameterValue(100, 150, 120);
                _parameters.AllParameters[ParameterType.BaseWidth] = new ParameterValue(70, 100, 85);
                _parameters.AllParameters[ParameterType.BodyRadius1] = new ParameterValue(300, 350, 320);
                _parameters.AllParameters[ParameterType.HandleRadius3] = new ParameterValue(10, 20, 15);
                _parameters.AllParameters[ParameterType.HandleRadius5] = new ParameterValue(75, 85, 80);
                _parameters.AllParameters[ParameterType.BodyLength] = new ParameterValue(100, 150, 120);
            }
            
            comboBox1.SelectedIndex = 2;
            
        }
        

        /// <summary>
        /// Очищает список ошибок и текст ошибки на метке.
        /// </summary>
        private void ClearErrors()
        {
            errorMessages.Clear();
            error_label.Text = string.Empty;
        }

        /// <summary>
        /// Проверяет поля ввода на наличие ошибок, отображает соответствующие сообщения.
        /// </summary>
        /// <returns>True, если есть ошибки, иначе false.</returns>
        private bool CheckTextBox()
        {
            bool isAnyTextBoxRed = diameterD1_textBox.BackColor == Color.Red ||
                                    diameterD4_textBox.BackColor == Color.Red ||
                                    radiusR1_textBox.BackColor == Color.Red ||
                                    radiusR3_textBox.BackColor == Color.Red ||
            radiusR5_textBox.BackColor == Color.Red ||
                                    L_textBox.BackColor == Color.Red;

            bool isAnyTextBoxFilled = diameterD1_textBox.Text != null &&
                                       diameterD4_textBox.Text != null &&
                                       radiusR1_textBox.Text != null &&
                                       radiusR3_textBox.Text != null &&
            radiusR5_textBox.Text != null &&
                                       L_textBox.Text != null;

            // Получение диапазонов параметров из объекта _parameters
            var diameterD1Range = _parameters.AllParameters[ParameterType.BodyWidth];
            var diameterD4Range = _parameters.AllParameters[ParameterType.BaseWidth];
            var radiusR1Range = _parameters.AllParameters[ParameterType.BodyRadius1];
            var radiusR3Range = _parameters.AllParameters[ParameterType.HandleRadius3];
            var radiusR5Range = _parameters.AllParameters[ParameterType.HandleRadius3];
            var heightRange = _parameters.AllParameters[ParameterType.BodyLength];

            var textBoxToParameterMapping = new Dictionary<System.Windows.Forms.TextBox, (string ErrorMessage, ParameterValue Range)>
{
            { diameterD1_textBox, ($"Диаметр кружки должен быть между {{0}} мм - {{1}} мм", diameterD1Range) },
            { diameterD4_textBox, ($"Диаметр основания кружки должен быть между {{0}} мм - {{1}} мм, и меньше чем диаметр кружки", diameterD4Range) },
            { radiusR1_textBox, ($"Радиус кривизны до верха кружки должен быть между {{0}} мм - {{1}} мм", radiusR1Range) },
            { radiusR3_textBox, ($"Радиус 1-й кривизны запястья кружки должен быть между {{0}} мм - {{1}} мм", radiusR3Range) },
            { radiusR5_textBox, ($"Радиус 3-й кривизны запястья кружки должен быть между {{0}} мм - {{1}} мм", radiusR5Range) },
            { L_textBox, ($"Высота кружки должна быть между {{0}} мм - {{1}} мм, и больше чем диаметр кружки", heightRange) }
};

            foreach (var entry in textBoxToParameterMapping)
            {
                var textBox = entry.Key;
                var (errorMessageTemplate, range) = entry.Value;

                if (textBox.BackColor == Color.Red)
                {
                    switch (textBox)
                    {
                        case var _ when textBox == diameterD1_textBox:
                        case var _ when textBox == diameterD4_textBox:
                        case var _ when textBox == radiusR1_textBox:
                        case var _ when textBox == radiusR3_textBox:
                        case var _ when textBox == radiusR5_textBox:
                        case var _ when textBox == L_textBox:
                            errorMessages.Add(string.Format(errorMessageTemplate, range.MinValue, range.MaxValue));
                            break;
                    }
                }
            }

            if (errorMessages.Count > 0)
            {
                error_label.Visible = true;
                error_label.Text = string.Join("\n", errorMessages);
                return true;
            }
            else
            {
                error_label.Visible = false;
                return false;
            }
        }

        /// <summary>
        /// Обработчик события для кнопки создания.
        /// Проверяет цвета фонов текстовых полей и, если все они корректны, вызывает метод построения.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void create_button_Click(object sender, EventArgs e)
        {
            // Переменная для отслеживания наличия ошибок 
            bool hasError = false;
            string errorMessage = "Пожалуйста, исправьте ошибки:\n";

            // Проверка на пустые текстовые поля
            if (string.IsNullOrWhiteSpace(diameterD1_textBox.Text) ||
                string.IsNullOrWhiteSpace(diameterD4_textBox.Text) ||
                string.IsNullOrWhiteSpace(radiusR1_textBox.Text) ||
                string.IsNullOrWhiteSpace(radiusR3_textBox.Text) ||
                string.IsNullOrWhiteSpace(radiusR5_textBox.Text) ||
                string.IsNullOrWhiteSpace(L_textBox.Text))
            {
                hasError = true;
                errorMessage += "• Все поля должны быть заполнены.\n";
            }

            // Проверка всех полей на наличие ошибок
            if (this.diameterD4_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Диаметр основания (D4) неверен.\n";
            }
            if (this.diameterD1_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Диаметр тела (D1) неверен.\n";
            }
            if (this.radiusR1_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Радиус тела (R1) неверен.\n";
            }
            if (this.radiusR3_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Радиус ручки (R3) неверен.\n";
            }
            if (this.radiusR5_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Радиус ручки (R5) неверен.\n";
            }
            if (this.L_textBox.BackColor == Color.Red)
            {
                hasError = true;
                errorMessage += "• Высота кружки (L) неверна.\n";
            }

            // Если ошибки обнаружены, показываем сообщение с красным крестом
            if (hasError)
            {
                // Показать MessageBox с ошибками
                MessageBox.Show(errorMessage, "Ошибки при вводе данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Если все поля валидны, вызываем метод Build для создания объекта
                this._builder.Build(this._parameters);
                // Скрыть сообщение об ошибке
                error_label.Visible = false; 
            }
        }

         //TODO: duplication
        /// <summary>
        /// Обработчик события покидания поля ввода для всех текстовых полей.
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void TextBox_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox textBox)
            {
                ParameterType parameterType;

                // Определение типа параметра на основе имени текстового поля
                switch (textBox.Name)
                {
                    // Обработчик события покидания поля ввода для диаметра кружки (D1).
                    case "diameterD1_textBox":
                        parameterType = ParameterType.BodyWidth;
                        break;

                    // Обработчик события покидания поля ввода для диаметра основания (D4).
                    case "diameterD4_textBox":
                        parameterType = ParameterType.BaseWidth;
                        break;

                    // Обработчик события покидания поля ввода для радиуса тела (R1).
                    case "radiusR1_textBox":
                        parameterType = ParameterType.BodyRadius1;
                        break;

                    // Обработчик события покидания поля ввода для радиуса ручки (R3).
                    case "radiusR3_textBox":
                        parameterType = ParameterType.HandleRadius3;
                        break;

                    // Обработчик события покидания поля ввода для радиуса ручки (R5).
                    case "radiusR5_textBox":
                        parameterType = ParameterType.HandleRadius5;
                        break;

                    // Обработчик события покидания поля ввода для высоты кружки (L).
                    case "L_textBox":
                        parameterType = ParameterType.BodyLength;
                        break;
                    default:
                        // Неизвестный текстовый блок
                        return;
                }

                // Выполнение валидации типа и значения
                this.ValidateType(textBox, parameterType);
                if (textBox.BackColor != SystemColors.Window)
                {
                    this.ValidateValue(textBox, parameterType);
                }
            }
        }

        /// <summary>
        /// Вторичная валидация, попытка создания параметра, попытка добавления
        /// корректного параметра в словарь.
        /// </summary>
        /// <param name="textBox">Используемый текстБокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        private void ValidateType(
            System.Windows.Forms.TextBox textBox, ParameterType parameterType)
        {
            try
            {
                double.Parse(textBox.Text);
                this.BackColor(parameterType, 3);
            }
            catch (Exception e)
            {
                textBox.Text = string.Empty;
                this.BackColor(parameterType, 1);
            }
        }

        /// <summary>
        /// Вспомогательный метод для установки цвета для текстбокса.
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="whatColor">Устанавливаемый цвет.</param>
        /// <param name="whatReason">Причина установки цвета.</param>
        /// <param name="text">Текст устанавливаемый в подсказку.</param>
        private void BackColor(ParameterType parameterType, int whatColor)
        {
            // Стандартное состояние (без ошибок)
            if (whatColor == 1) 
            {
                // Ширина тела
                if (parameterType == ParameterType.BodyWidth)  
                {
                    this.diameterD1_textBox.BackColor = SystemColors.Window;
                }
                // Ширина основания
                else if (parameterType == ParameterType.BaseWidth)  
                {
                    this.diameterD4_textBox.BackColor = SystemColors.Window;
                }
                // Радиус тела 1
                else if (parameterType == ParameterType.BodyRadius1)  
                {
                    this.radiusR1_textBox.BackColor = SystemColors.Window;
                }
                // Радиус ручки 3
                else if (parameterType == ParameterType.HandleRadius3)  
                {
                    this.radiusR3_textBox.BackColor = SystemColors.Window;
                }
                // Радиус ручки 5
                else if (parameterType == ParameterType.HandleRadius5)  
                {
                    this.radiusR5_textBox.BackColor = SystemColors.Window;
                }
                // Длина тела
                else if (parameterType == ParameterType.BodyLength)  
                {
                    this.L_textBox.BackColor = SystemColors.Window;
                }
            }
            // Ошибка (красный цвет)
            else if (whatColor == 2) 
            {
                // Ширина тела
                if (parameterType == ParameterType.BodyWidth)  
                {
                    this.diameterD1_textBox.BackColor = Color.Red;
                }
                // Ширина основания
                else if (parameterType == ParameterType.BaseWidth)  
                {
                    this.diameterD4_textBox.BackColor = Color.Red;
                }

                // Радиус тела 1
                else if (parameterType == ParameterType.BodyRadius1)  
                {
                    this.radiusR1_textBox.BackColor = Color.Red;
                }

                // Радиус ручки 3
                else if (parameterType == ParameterType.HandleRadius3)  
                {
                    this.radiusR3_textBox.BackColor = Color.Red;
                }

                // Радиус ручки 5
                else if (parameterType == ParameterType.HandleRadius5)  
                {
                    this.radiusR5_textBox.BackColor = Color.Red;
                }

                // Длина тела
                else if (parameterType == ParameterType.BodyLength)  
                {
                    this.L_textBox.BackColor = Color.Red;
                }
            }

            // Успешно (зеленый цвет)
            else if (whatColor == 3) 
            {
                // Ширина тела
                if (parameterType == ParameterType.BodyWidth)  
                {
                    this.diameterD1_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.diameterD1_textBox, null);
                }
            
                // Ширина основания
                else if (parameterType == ParameterType.BaseWidth)  
                {
                    this.diameterD4_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.diameterD4_textBox, null);
                }
                
                // Радиус тела 1
                else if (parameterType == ParameterType.BodyRadius1)  
                {
                    this.radiusR1_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR1_textBox, null);
                }
                
                // Радиус ручки 3
                else if (parameterType == ParameterType.HandleRadius3)  
                {
                    this.radiusR3_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR3_textBox, null);
                }
                
                // Радиус ручки 5
                else if (parameterType == ParameterType.HandleRadius5)  
                {
                    this.radiusR5_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR5_textBox, null);
                }
                
                // Длина тела
                else if (parameterType == ParameterType.BodyLength)  
                {
                    this.L_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.L_textBox, null);
                }
            }
        }


        /// <summary>
        /// Валидация, попытка создания параметра, попытка добавления
        /// корректного параметра в словарь.
        /// </summary>
        /// <param name="textBox">Используемый текстбокс.</param>
        /// <param name="parameterType">Тип параметра.</param>
        private void ValidateValue(System.Windows.Forms.TextBox textBox, ParameterType parameterType)
        {
            ClearErrors();

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Если текстбокс пустой
                // Установить цвет как Window
                BackColor(parameterType, 1); 
                return;
            }

            bool isValid = true;
            double value;

            // Попытка преобразовать текст в число
            if (!double.TryParse(textBox.Text, out value))
            {
                isValid = false;
            }

            double D1 = 0;
            if (!double.TryParse(this.diameterD1_textBox.Text, out D1) && parameterType != ParameterType.BodyWidth)
            {
                // Установить красный цвет
                BackColor(parameterType, 2); 
                return;
            }

            // Проверка значений в зависимости от типа параметра
            switch (parameterType)
            {
                case ParameterType.BodyWidth:
                    isValid = value >= 100 && value <= 150;
                    break;

                case ParameterType.BaseWidth:
                    isValid = value >= 70 && value <= 100 && value < D1;
                    break;

                case ParameterType.BodyRadius1:
                    isValid = value >= 300 && value <= 350;
                    break;

                case ParameterType.HandleRadius3:
                    isValid = value >= 10 && value <= 20;
                    break;

                case ParameterType.HandleRadius5:
                    isValid = value >= 75 && value <= 85;
                    break;

                case ParameterType.BodyLength:
                    isValid = value >= 100 && value <= 150 && value > D1;
                    break;

                default:
                    throw new ArgumentException("Неизвестный тип параметра.");
            }

            // Установка цвета в зависимости от результата проверки
            if (isValid)
            {
                // Установить зеленый цвет
                BackColor(parameterType, 3); 

                // Аффектация значения параметру
                try
                {
                    // Передача значения параметра
                    _parameters.SetParameter(parameterType, value); 
                }
                catch (Exception ex)
                {
                    // Логгирование ошибки при аффектации параметра
                    Console.WriteLine($"Ошибка при установке параметра: {ex.Message}");

                    // Установить красный цвет в случае ошибки
                    BackColor(parameterType, 2); 
                }
            }
            else
            {
                // Установить красный цвет
                BackColor(parameterType, 2);
            }
            CheckTextBox();
        }

        /// <summary>
        /// Сигналы для построения подставки и подподставки.
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox1.SelectedIndex == 2)
            {
                Flags.flag = -1;
                return;
            }

            Flags.flag = comboBox1.SelectedIndex;
            MessageBox.Show($"Будет построенно: {comboBox1.SelectedItem}.");
        }

        /// <summary>
        /// Сигналы для построения крышки.
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Flags.krishka = true;
                MessageBox.Show($"Будет построенно: {checkBox1.Text}.");
            }
            else
            {
                Flags.krishka = false;
            }
        }
    }
}