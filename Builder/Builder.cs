using System;
using Kompas;

namespace MugPlugin
{
    /// <summary>
    /// Класс для построения модели отвёртки в Компас.
    /// </summary>
    public class Builder
    {
        /// <summary>
        /// Экземпляр класс Wrapper.
        /// </summary>
        private Wrapper _wrapper = new Wrapper();

        /// <summary>
        /// Основной метод для построения кружки.
        /// </summary>
        public void Build(Parameters parameters)
        {
            // Проверяем наличие необходимых параметров
            ValidateParameters(parameters);

            // Открываем CAD и создаём новый файл
            _wrapper.OpenCAD();
            _wrapper.CreateFile();

            // Построение тела чашки
            BuildBody(parameters);

            BuildInterior(parameters);
            // Построение ручки
            BuildHandle(parameters);
        }

        /// <summary>
        /// Построение тела кружки.
        /// </summary>
        private void BuildBody(Parameters parameters)
        {
            // Получаем параметры
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value; //D1
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value; //D4
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value; //D2
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value; // L

            // Определяем точки для тела кружки
            double halfBodyWidth = bodyWidth / 2; // D1/2
            double halfBaseWidth = baseWidth / 2; // D4 / 2
            double halfBodyLenght = halfBodyWidth / 2; // L/2
            double halfInteriorUpWidth = interiorUpWidth / 2; // D2/2
            double epaisseur = halfBodyWidth - halfInteriorUpWidth; // Толщина

            double[,] pointsArray = {
                { 0, 0, -halfBaseWidth, 0, 1 }, // база 1-2
                { 0, -bodyLength, -halfBodyWidth, -bodyLength, 1 }, // верх 4-5
                { 0, -bodyLength, 0, 0, 3 }, // высота 5-1
            };

            double[,] pointsArcArray = {
                {-halfBaseWidth, 0, -halfBodyWidth+2, -halfBodyLenght, -halfBodyWidth, -bodyLength} // 2-4
            };

            _wrapper.CreateSketch(2); // Создание эскиза на плоскости
            _wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            _wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0)); // Рисуем стороны
            _wrapper.Spin();
            _wrapper.Extrusion(3, 1); // Выдавливание эскиза на глубину 10 мм
        }

        /// <summary>
        /// Построение внутренней части кружки.
        /// </summary>
        private void BuildInterior(Parameters parameters)
        {
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value; //D2
            double interiorBaseWidth = parameters.AllParameters[ParameterType.InteriorBaseWidth].Value; //D3
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value; //D1
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value; // L

            double halfInteriorUpWidth = interiorUpWidth / 2; // D2/2
            double halfInteriorBaseWidth = interiorBaseWidth / 2; // D4 / 2
            double halfBodyWidth = bodyWidth / 2; // D1/2
            double lenghtInterior = bodyLength - (halfBodyWidth - halfInteriorBaseWidth);
            double epaisseur = 2 + (halfBodyWidth - halfInteriorUpWidth); // Толщина
            double halfBodyLenght = halfBodyWidth / 2; // L/2

            double[,] pointsArray = {
                { 0, -epaisseur, -halfInteriorBaseWidth, -epaisseur, 1 }, // внутреннее основание 6-7
                { -(halfBodyWidth - epaisseur),-bodyLength, 0, -bodyLength, 1 }, // верх внутренний 8-5
                { 0, -bodyLength, 0, -epaisseur, 3 }, // высота 5-6
            };

            double[,] pointsArcArray = {
                {-halfInteriorBaseWidth, -epaisseur, -(halfBodyWidth - epaisseur - 3)+2, -(halfBodyLenght-epaisseur), -(halfBodyWidth - epaisseur), -bodyLength} // 7-8
            };

            this._wrapper.CreateSketch(2);
            this._wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0));
            this._wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            this._wrapper.Spin();
            this._wrapper.Extrusion(4, 0); // Экструзия по длине тела
        }

        /// <summary>
        /// Построение ручки кружки.
        /// </summary>
        private void BuildHandle(Parameters parameters)
        {
            // Создание эскиза для ручки
            this._wrapper.CreateSketch(2);

            // Извлечение параметров
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value; // D1
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value; // D4
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value; // L

            // Расчёт размеров и позиций ручки
            double halfBodyWidth = bodyWidth / 2; // Половина ширины тела
            double handleHeight = bodyLength * 1.2; // Высота ручки (очень длинная) (120 % от высоты тела)
            double handleWidth = bodyWidth * 0.7; // Ширина ручки (70 % от ширины тела)
            double handleThickness = bodyWidth * 0.08; // Толщина ручки
            double handleOffset = bodyLength / 3.2; // Вертикальный сдвиг, чтобы опустить ручку еще ниже

            // Определение точек для большой кривой ручки
            double[,] handleArcArray = {
                { -halfBodyWidth + 4 , -(bodyLength / 2 - handleOffset + 4), // Точка начала (подключена к телу, еще ниже) // внизу
                  -halfBodyWidth - handleWidth / 2, -(handleHeight / 2 - handleOffset), // Вершина кривой (большая кривизна)
                  -halfBodyWidth, -(handleHeight - handleOffset) }, // Точка конца (подключена к телу, еще ниже)
                
                { -halfBodyWidth+1, -(bodyLength / 2 - handleOffset + handleThickness+1), // Внутренняя точка начала
                  -halfBodyWidth - handleWidth / 2 + handleThickness, -(handleHeight / 2 - handleOffset+2), // Вершина внутренняя
                  -halfBodyWidth, -(handleHeight - handleOffset - handleThickness) } // Точка конца внутренняя
            };

            // Определение линий для замыкания ручки
            double[,] handleLines = {
                { -halfBodyWidth + 4 , -(bodyLength / 2 - handleOffset + 4),
                  -halfBodyWidth+1, -(bodyLength / 2 - handleOffset + handleThickness+1), 1 }, // Подключение внизу
                { -halfBodyWidth, -(handleHeight - handleOffset),
                  -halfBodyWidth, -(handleHeight - handleOffset - handleThickness), 1 } // Подключение сверху
            };

            // Рисование дуг
            this._wrapper.CreateArc(handleArcArray, 0, handleArcArray.GetLength(0));

            // Рисование линий для соединения дуг
            this._wrapper.CreateLine(handleLines, 0, handleLines.GetLength(0));

            //this._wrapper.Spin();
            // Экструзия для придания объема ручке
            this._wrapper.Extrusion(3, handleThickness +1);

            Console.WriteLine("Ручка с большой кривизной успешно создана.");
        }

        /// <summary>
        /// Валидация параметров перед построением.
        /// </summary>
        private void ValidateParameters(Parameters parameters)
        {
            if (!parameters.AllParameters.ContainsKey(ParameterType.BodyWidth) ||
                !parameters.AllParameters.ContainsKey(ParameterType.InteriorUpWidth) ||
                !parameters.AllParameters.ContainsKey(ParameterType.InteriorBaseWidth) ||
                !parameters.AllParameters.ContainsKey(ParameterType.BaseWidth) ||
                !parameters.AllParameters.ContainsKey(ParameterType.BodyLength) ||
                !parameters.AllParameters.ContainsKey(ParameterType.HandleLength))
            {
                throw new ArgumentException("Все необходимые параметры должны быть предоставлены.");
            }
        }
    }
}
