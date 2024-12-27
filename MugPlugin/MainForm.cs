using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MugPlugin
{
    // <summary>
    /// Класс MainForm, представляющий основную форму приложения.
    /// </summary>
    public partial class MainForm : Form
    {
        //TODO: RSDN
        /// <summary>
        /// Поле хранящее в себе ошибок.
        /// </summary>
        private List<string> errorMessages = new List<string>();

        /// <summary>
        /// Поле хранящее объект класса Builder.
        /// </summary>
        private Builder _builder = new Builder();

        /// <summary>
        /// Поле хранящее в себе объект класса Parameters.
        /// </summary>
        private Parameters _parameters = new Parameters();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
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

        }

        /// <summary>
        /// Отображает ошибки, если они есть.
        /// </summary>
        private void ShowErrors()
        {
            if (errorMessages.Count > 0)
            {
                error_label.Visible = true;
                error_label.Text = string.Join("\n", errorMessages);
            }
            else
            {
                HideError();
            }
        }

        // <summary>
        /// Скрывает ошибку и очищает список сообщений об ошибках.
        /// </summary>
        private void HideError()
        {
            error_label.Visible = false;
            errorMessages.Clear();
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

            //TODO: redo
            if (diameterD1_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Диаметр кружки должен быть между 100 мм - 150 мм");
            }

            if (diameterD4_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Диаметр основания кружки должен быть между 70 мм – 100 мм, и меньше чем диаметр кружки");
            }

            if (radiusR1_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Радиус кривизны до верха кружки должен быть между 300 мм – 350 мм");
            }

            if (radiusR3_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Радиус 1-й кривизны запястья кружки должен быть между 10 мм – 20 мм");
            }

            if (radiusR5_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Радиус 3-й кривизны запястья кружки должен быть между 75 мм – 85 мм");
            }

            if (L_textBox.BackColor == Color.Red)
            {
                errorMessages.Add("Высота кружки должна быть между 100 мм – 150 мм, и больше чем диаметр кружки");
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

         //TODO: duplication
        /// <summary>
        /// Обработчик события изменения текста в поле диаметра кружки (D1).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void diameterD1_textBox_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(diameterD1_textBox.Text, out value) && value >= 100 && value <= 150;
            diameterD1_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле диаметра основания кружки (D4).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void diameterD4_textBox_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(diameterD4_textBox.Text, out value) && value >= 70 && value <= 100 && value < int.Parse(diameterD1_textBox.Text);
            diameterD4_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле радиуса кривизны до верха кружки (R1).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void radiusR1_textBox_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(radiusR1_textBox.Text, out value) && value >= 300 && value <= 350;
            radiusR1_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
        }

        /// <summary>
        /// Обработчик нажатия кнопки отмены.
        /// Закрывает форму.
        /// </summary>
        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле радиуса кривизны ручки (R3).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void radiusR3_textBox_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(radiusR3_textBox.Text, out value) && value >= 10 && value <= 20;
            radiusR3_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле радиуса кривизны ручки (R5).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(radiusR5_textBox.Text, out value) && value >= 75 && value <= 85;
            radiusR5_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
        }

        /// <summary>
        /// Обработчик события изменения текста в поле высоты кружки (L).
        /// Выполняет валидацию введенного значения и обновляет цвет фона.
        /// </summary>
        private void L_textBox_TextChanged(object sender, EventArgs e)
        {
            ClearErrors();
            int value;
            bool isValid = int.TryParse(L_textBox.Text, out value) &&
                value >= 100 &&
                value <= 150 &&
                double.TryParse(diameterD1_textBox.Text, out double diameterD1) &&
                diameterD1 < value;
            L_textBox.BackColor = isValid ? Color.White : Color.Red;
            CheckTextBox();
            ShowErrors();
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
                error_label.Visible = false; // Скрыть сообщение об ошибке
            }
        }

         //TODO: duplication
        /// <summary>
        /// Обработчик события покидания поля ввода для диаметра кружки (D1).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void diameterD1_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.BodyWidth;
            this.ValidateType(this.diameterD1_textBox, parameterType);
            if (this.diameterD1_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.diameterD1_textBox, parameterType);

            }
        }

         //TODO: duplication
        // <summary>
        /// Обработчик события покидания поля ввода для диаметра основания (D4).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void diameterD4_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.BaseWidth;
            this.ValidateType(this.diameterD4_textBox, parameterType);
            if (this.diameterD1_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.diameterD4_textBox, ParameterType.BaseWidth);
            }
        }

        /// <summary>
        /// Обработчик события покидания поля ввода для радиуса тела (R1).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void radiusR1_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.BodyRadius1;
            this.ValidateType(this.radiusR1_textBox, parameterType);
            if (this.radiusR1_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.radiusR1_textBox, ParameterType.BodyRadius1);
            }
        }

        /// <summary>
        /// Обработчик события покидания поля ввода для радиуса ручки (R3).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void radiusR3_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.HandleRadius3;
            this.ValidateType(this.radiusR3_textBox, parameterType);
            if (this.radiusR1_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.radiusR3_textBox, ParameterType.HandleRadius3);
            }
        }

        /// <summary>
        /// Обработчик события покидания поля ввода для радиуса ручки (R5).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void radiusR5_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.HandleRadius5;
            this.ValidateType(this.radiusR5_textBox, parameterType);
            if (this.radiusR5_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.radiusR5_textBox, ParameterType.HandleRadius5);
            }
        }

        /// <summary>
        /// Обработчик события покидания поля ввода для высоты кружки (L).
        /// Выполняет валидацию типа и значения параметра.
        /// </summary>
        private void L_textBox_Leave(object sender, EventArgs e)
        {
            ParameterType parameterType = ParameterType.BodyLength;
            this.ValidateType(this.L_textBox, parameterType);
            if (this.L_textBox.BackColor != SystemColors.Window)
            {
                this.ValidateValue(this.L_textBox, ParameterType.BodyLength);
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
                this.BackColor(parameterType, 3, 0, textBox.Text);
            }
            catch (Exception e)
            {
                textBox.Text = string.Empty;
                this.BackColor(parameterType, 1, 0, e.Message);
            }
        }

        /// <summary>
        /// Вспомогательный метод для установки цвета для текстбокса.
        /// </summary>
        /// <param name="parameterType">Тип параметра.</param>
        /// <param name="whatColor">Устанавливаемый цвет.</param>
        /// <param name="whatReason">Причина установки цвета.</param>
        /// <param name="text">Текст устанавливаемый в подсказку.</param>
        private void BackColor(ParameterType parameterType, int whatColor, int whatReason, string text)
        {
            if (whatColor == 1) // Стандартное состояние (без ошибок)
            {
                if (parameterType == ParameterType.BodyWidth)  // Ширина тела
                {
                    this.diameterD1_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.diameterD1_textBox, "Ширина тела от 100 до 150 мм");
                }
                else if (parameterType == ParameterType.BaseWidth)  // Ширина основания
                {
                    this.diameterD4_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.diameterD4_textBox, "Ширина основания от 70 до 100 мм");
                }
                else if (parameterType == ParameterType.BodyRadius1)  // Радиус тела 1
                {
                    this.radiusR1_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.radiusR1_textBox, "Радиус тела 1 от 300 до 350 мм");
                }
                else if (parameterType == ParameterType.HandleRadius3)  // Радиус ручки 3
                {
                    this.radiusR3_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.radiusR3_textBox, "Радиус ручки 3 от 10 до 20 мм");
                }
                else if (parameterType == ParameterType.HandleRadius5)  // Радиус ручки 5
                {
                    this.radiusR5_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.radiusR5_textBox, "Радиус ручки 5 от 75 до 85 мм");
                }
                else if (parameterType == ParameterType.BodyLength)  // Длина тела
                {
                    this.L_textBox.BackColor = SystemColors.Window;
                    this.toolTip1.SetToolTip(this.L_textBox, "Длина тела от 100 до 150 мм");
                }
            }
            else if (whatColor == 2) // Ошибка (красный цвет)
            {
                if (parameterType == ParameterType.BodyWidth)  // Ширина тела
                {
                    this.diameterD1_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.diameterD1_textBox, whatReason == 1 ? "Ширина тела от 100 до 150 мм" : text);
                }
                else if (parameterType == ParameterType.BaseWidth)  // Ширина основания
                {
                    this.diameterD4_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.diameterD4_textBox, whatReason == 1 ? "Ширина основания от 70 до 100 мм" : text);
                }
                else if (parameterType == ParameterType.BodyRadius1)  // Радиус тела 1
                {
                    this.radiusR1_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.radiusR1_textBox, whatReason == 1 ? "Радиус тела от 300 до 350 мм" : text);
                }
                else if (parameterType == ParameterType.HandleRadius3)  // Радиус ручки 3
                {
                    this.radiusR3_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.radiusR3_textBox, whatReason == 1 ? "Радиус ручки от 10 до 20 мм" : text);
                }
                else if (parameterType == ParameterType.HandleRadius5)  // Радиус ручки 5
                {
                    this.radiusR5_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.radiusR5_textBox, whatReason == 1 ? "Радиус ручки от 75 до 85 мм" : text);
                }
                else if (parameterType == ParameterType.BodyLength)  // Длина тела
                {
                    this.L_textBox.BackColor = Color.Red;
                    this.toolTip1.SetToolTip(this.L_textBox, whatReason == 1 ? "Длина тела от 100 до 150 мм" : text);
                }
            }
            else if (whatColor == 3) // Успешно (зеленый цвет)
            {
                if (parameterType == ParameterType.BodyWidth)  // Ширина тела
                {
                    this.diameterD1_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.diameterD1_textBox, null);
                }
                else if (parameterType == ParameterType.BaseWidth)  // Ширина основания
                {
                    this.diameterD4_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.diameterD4_textBox, null);
                }
                else if (parameterType == ParameterType.BodyRadius1)  // Радиус тела 1
                {
                    this.radiusR1_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR1_textBox, null);
                }
                else if (parameterType == ParameterType.HandleRadius3)  // Радиус ручки 3
                {
                    this.radiusR3_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR3_textBox, null);
                }
                else if (parameterType == ParameterType.HandleRadius5)  // Радиус ручки 5
                {
                    this.radiusR5_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.radiusR5_textBox, null);
                }
                else if (parameterType == ParameterType.BodyLength)  // Длина тела
                {
                    this.L_textBox.BackColor = Color.Green;
                    this.toolTip1.SetToolTip(this.L_textBox, null);
                }
            }
        }


        /// <summary>
        /// Вторичная валидация, попытка создания параметра, попытка добавления
        /// корректного параметра в словарь.
        /// </summary>
        /// <param name="textBox">Используемый текстбокс.</param>
        /// <param name="parameterType">Тип параметра.</param>

        private void ValidateValue(System.Windows.Forms.TextBox textBox, ParameterType parameterType)
        {
            bool cached = false;
            ParameterValue parameter = new ParameterValue();

            // Инициализация объекта ParameterValue для новых параметров
            switch (parameterType)
            {
                case ParameterType.BodyWidth:
                    parameter.MaxValue = 150;
                    parameter.MinValue = 100;
                    break;

                case ParameterType.BaseWidth:
                    parameter.MaxValue = 100;
                    parameter.MinValue = 70;
                    break;

                case ParameterType.BodyRadius1:
                    parameter.MaxValue = 350;
                    parameter.MinValue = 300;
                    break;

                case ParameterType.HandleRadius3:
                    parameter.MaxValue = 20;
                    parameter.MinValue = 10;
                    break;

                case ParameterType.HandleRadius5:
                    parameter.MaxValue = 85;
                    parameter.MinValue = 75;
                    break;

                case ParameterType.BodyLength:
                    parameter.MaxValue = 150;
                    parameter.MinValue = 100;
                    break;

                default:
                    throw new ArgumentException("Неизвестный тип параметра.");
            }

            try
            {
                // Попытка преобразовать текст в число
                parameter.Value = int.Parse(textBox.Text);
            }
            catch (Exception e)
            {
                this.BackColor(parameterType, 2, 1, e.Message);
                cached = true; // Установка флага кэширования в true при ошибке
            }

            if (!cached)
            {
                double D1; // Объявление переменной для D1

                // Получение значения D1 из diameterD1_textbox
                try
                {
                    D1 = double.Parse(this.diameterD1_textBox.Text); // Преобразование текста в число
                }
                catch (Exception e)
                {
                    this.BackColor(parameterType, 2, 1, "Ошибка при чтении D1: " + e.Message);
                    return; // Выход из метода при ошибке
                }

                // Проверка BaseWidth и BodyLength относительно D1
                if (parameterType == ParameterType.BaseWidth && parameter.Value >= D1)
                {
                    this.BackColor(parameterType, 2, 1, "BaseWidth должно быть меньше D1.");
                    cached = true; // Установка флага кэширования в true при ошибке
                }
                else if (parameterType == ParameterType.BodyLength && parameter.Value <= D1)
                {
                    this.BackColor(parameterType, 2, 1, "BodyLength должно быть больше D1.");
                    cached = true; // Установка флага кэширования в true при ошибке
                }

                if (!cached)
                {
                    // Проверка на диапазон значений
                    if (parameter.Value < parameter.MinValue || parameter.Value > parameter.MaxValue)
                    {
                        this.BackColor(parameterType, 2, 1, $"Значение должно быть между {parameter.MinValue} и {parameter.MaxValue}.");
                        cached = true; // Установка флага кэширования в true при ошибке
                    }
                }

                if (!cached)
                {
                    try
                    {
                        // Передача значения параметра вместо объекта
                        this._parameters.SetParameter(parameterType, parameter.Value, parameter.MinValue, parameter.MaxValue);
                        this.BackColor(parameterType, 3, 0, string.Empty);
                    }
                    catch (Exception e)
                    {
                        this.BackColor(parameterType, 2, 0, e.Message);
                    }
                }
            }
        }




    }
}





/*
private void ValidateValue(System.Windows.Forms.TextBox textBox, ParameterType parameterType)
{
    bool cached = false;
    ParameterValue parameter = new ParameterValue();

    // Инициализация объекта ParameterValue для новых параметров
    switch (parameterType)
    {
        case ParameterType.BodyWidth:
            parameter.MaxValue = 150;
            parameter.MinValue = 100;
            break;

        case ParameterType.BaseWidth:
            parameter.MaxValue = 100;
            parameter.MinValue = 70;
            break;

        case ParameterType.BodyRadius1:
            parameter.MaxValue = 350;
            parameter.MinValue = 300;
            break;

        case ParameterType.HandleRadius3:
            parameter.MaxValue = 20;
            parameter.MinValue = 10;
            break;

        case ParameterType.HandleRadius5:
            parameter.MaxValue = 85;
            parameter.MinValue = 75;
            break;

        case ParameterType.BodyLength:
            parameter.MaxValue = 150;
            parameter.MinValue = 100;
            break;

        default:
            throw new ArgumentException("Неизвестный тип параметра.");
    }

    try
    {
        // Попытка преобразовать текст в число
        parameter.Value = int.Parse(textBox.Text);
    }
    catch (Exception e)
    {
        this.BackColor(parameterType, 2, 1, e.Message);
        cached = true;
    }

    if (!cached)
    {
        try
        {
            // Передача значения параметра вместо объекта
            this._parameters.SetParameter(parameterType, parameter.Value, parameter.MinValue, parameter.MaxValue);
            this.BackColor(parameterType, 3, 0, string.Empty);
        }
        catch (Exception e)
        {
            this.BackColor(parameterType, 2, 0, e.Message);
        }
    }
}
*/

/*
// Переменная для отслеживания наличия ошибок
bool hasError = false;
string errorMessage = "Пожалуйста, исправьте ошибки:\n"; // Сообщение об ошибке

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
    error_label.Visible = false; // Скрыть сообщение об ошибке
}
*/