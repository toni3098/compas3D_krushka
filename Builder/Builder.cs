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

            //  Построение крышки
            if (Flags.krishka == true)
            {
                BuildKrishka(parameters);
            }

            // Построение подставки и подподставки
            if (Flags.flag == 0)
            {
                BuildTarelka(parameters);
            }
            else if (Flags.flag == 1)
            {
                BuildTarelka(parameters);
                BuildPodTarelka(parameters);
            }
            
        }

        /// <summary>
        /// Построение тела кружки.
        /// </summary>
        private void BuildBody(Parameters parameters)
        {
            // Получаем параметры

            //D1
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;

            //D4
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value;

            //D2
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value;

            // L
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value;

            // Определяем точки для тела кружки

            // D1/2
            double halfBodyWidth = bodyWidth / 2;

            // D4 / 2
            double halfBaseWidth = baseWidth / 2;

            // L/2
            double halfBodyLenght = halfBodyWidth / 2;

            // D2/2
            double halfInteriorUpWidth = interiorUpWidth / 2;

            // Толщина
            double epaisseur = halfBodyWidth - halfInteriorUpWidth; 

            double[,] pointsArray = {
                { 
                    0, 0, 
                    -halfBaseWidth, 0, 
                    1 
                },

                { 
                    0, -bodyLength, 
                    -halfBodyWidth, -bodyLength, 
                    1 
                },

                { 
                    0, -bodyLength, 
                    0, 0, 
                    3 
                },
            };

            double[,] pointsArcArray = {
                {
                    -halfBaseWidth, 0, 
                    -halfBodyWidth+2, -halfBodyLenght, 
                    -halfBodyWidth, -bodyLength
                }
            };

            // Создание эскиза на плоскости
            _wrapper.CreateSketch(2); 
            _wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            _wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0)); 
            _wrapper.Spin();
            _wrapper.Extrusion(3, 1);
        }

        /// <summary>
        /// Построение внутренней части кружки.
        /// </summary>
        private void BuildInterior(Parameters parameters)
        {
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value;
            double interiorBaseWidth = parameters.AllParameters[ParameterType.InteriorBaseWidth].Value;
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value;

            double halfInteriorUpWidth = interiorUpWidth / 2;
            double halfInteriorBaseWidth = interiorBaseWidth / 2;
            double halfBodyWidth = bodyWidth / 2;
            double lenghtInterior = bodyLength - (halfBodyWidth - halfInteriorBaseWidth);
            double epaisseur = 2 + (halfBodyWidth - halfInteriorUpWidth);
            double halfBodyLenght = halfBodyWidth / 2;

            double[,] pointsArray = {
                { 
                    0, -epaisseur, 
                    -halfInteriorBaseWidth, -epaisseur, 
                    1 
                },

                { 
                    -(halfBodyWidth - epaisseur),-bodyLength, 
                    0, -bodyLength, 
                    1 
                },

                { 
                    0, -bodyLength, 
                    0, -epaisseur, 
                    3 
                },
            };

            double[,] pointsArcArray = 
            {
                {
                    -halfInteriorBaseWidth, -epaisseur, 
                    -(halfBodyWidth - epaisseur - 3) + 2, -(halfBodyLenght-epaisseur), 
                    -(halfBodyWidth - epaisseur), -bodyLength
                }
            };

            this._wrapper.CreateSketch(2);
            this._wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0));
            this._wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            this._wrapper.Spin();
            this._wrapper.Extrusion(4, 0);
        }

        /// <summary>
        /// Построение ручки кружки.
        /// </summary>
        private void BuildHandle(Parameters parameters)
        {
            // Создание эскиза для ручки
            this._wrapper.CreateSketch(2);

            // Извлечение параметров

            // D1
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;

            // D4
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value;

            // L
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value; 

            // Расчёт размеров и позиций ручки

            // Половина ширины тела
            double halfBodyWidth = bodyWidth / 2;

            // Высота ручки (очень длинная) (120 % от высоты тела)
            double handleHeight = bodyLength * 1.2;

            // Ширина ручки (70 % от ширины тела)
            double handleWidth = bodyWidth * 0.7;

            // Толщина ручки
            double handleThickness = bodyWidth * 0.08;

            // Вертикальный сдвиг, чтобы опустить ручку еще ниже
            double handleOffset = bodyLength / 3.2;

            // Определение точек для большой кривой ручки
            double[,] handleArcArray = 
            {
                { 
                  -halfBodyWidth + 4 , -(bodyLength / 2 - handleOffset + 4),
                  -halfBodyWidth - handleWidth / 2, -(handleHeight / 2 - handleOffset),
                  -halfBodyWidth, -(handleHeight - handleOffset) 
                },
                
                { 
                  -halfBodyWidth+1, -(bodyLength / 2 - handleOffset + handleThickness+1),
                  -halfBodyWidth - handleWidth / 2 + handleThickness, -(handleHeight / 2 - handleOffset+2),
                  -halfBodyWidth, -(handleHeight - handleOffset - handleThickness) 
                }
            };

            // Определение линий для замыкания ручки
            double[,] handleLines = 
            {
                { 
                    -halfBodyWidth + 4 , -(bodyLength / 2 - handleOffset + 4),
                    -halfBodyWidth+1, -(bodyLength / 2 - handleOffset + handleThickness+1), 1 
                },

                { 
                    -halfBodyWidth, -(handleHeight - handleOffset),
                    -halfBodyWidth, -(handleHeight - handleOffset - handleThickness), 1
                }
            };

            // Рисование дуг
            this._wrapper.CreateArc(handleArcArray, 0, handleArcArray.GetLength(0));

            // Рисование линий для соединения дуг
            this._wrapper.CreateLine(handleLines, 0, handleLines.GetLength(0));

            // Экструзия для придания объема ручке
            this._wrapper.Extrusion(3, handleThickness +1);

            Console.WriteLine("Ручка с большой кривизной успешно создана.");
        }

        /// <summary>
        /// Построение ручки кружки.
        /// </summary>
        private void BuildKrishka(Parameters parameters)
        {
            // Получаем параметры
            //D1
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;
            //D2
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value;
            // L
            double bodyLength = parameters.AllParameters[ParameterType.BodyLength].Value;

            // D1/2
            double halfBodyWidth = bodyWidth / 2;

            // D2/2
            double halfInteriorUpWidth = interiorUpWidth / 2;

            // Толщина
            double epaisseur = halfBodyWidth - halfInteriorUpWidth; 

            double[,] pointsArray = {
                { 
                    0, -bodyLength, 
                    -halfBodyWidth, -bodyLength, 
                    1 },

                { 
                    -halfInteriorUpWidth, -bodyLength-epaisseur, 
                    -epaisseur, -bodyLength-epaisseur, 
                    1 
                },

                {
                    0, -bodyLength-(2*epaisseur), 
                    -2*epaisseur, -bodyLength-(2*epaisseur), 
                    1
                },

                { 
                    0, -bodyLength, 
                    0, -bodyLength-(2*epaisseur), 
                    3
                },

                {
                    -epaisseur, -bodyLength-epaisseur, 
                    -epaisseur, -bodyLength-epaisseur-(epaisseur/2), 
                    1
                },

                {
                    -epaisseur, -bodyLength-epaisseur-(epaisseur/2), 
                    -2*epaisseur, -bodyLength-epaisseur-(epaisseur/2), 
                    1
                },

                {
                    -2*epaisseur, -bodyLength-epaisseur-(epaisseur/2), 
                    -2*epaisseur, -bodyLength-(2*epaisseur), 
                    1
                },
            };
            
            double[,] pointsArcArray = {
                {-halfBodyWidth, -bodyLength, 
                    -halfBodyWidth, -bodyLength-(epaisseur/4),
                    -halfInteriorUpWidth, -bodyLength-epaisseur},
            };

            // Создание эскиза на плоскости
            _wrapper.CreateSketch(2); 

            _wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));

            // Рисуем стороны
            _wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0)); 

            _wrapper.Spin();

            // Выдавливание эскиза на глубину 10 мм
            _wrapper.Extrusion(3, 1); 

        }

        /// <summary>
        /// Построение тарелки кружки.
        /// </summary>
        private void BuildTarelka(Parameters parameters)
        {
            // Получаем параметры

            //D1
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;

            //D4
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value;

            //D2
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value;

            //D3
            double interiorBaseWidth = parameters.AllParameters[ParameterType.InteriorBaseWidth].Value;


            // D1/2
            double halfBodyWidth = bodyWidth / 2;

            // D4 / 2
            double halfBaseWidth = baseWidth / 2;

            // D2/2
            double halfInteriorUpWidth = interiorUpWidth / 2;

            // Толщина
            double epaisseur = halfBodyWidth - halfInteriorUpWidth;

            // D3/2
            double halfInteriorBase = interiorBaseWidth / 2; 

            double[,] pointsArray = 
            {
                { 
                    0, 0, 
                    -halfBaseWidth, 0, 
                    1 
                },

                { 
                    0, 0, 
                    0, epaisseur, 
                    3 
                },

                { 
                    0, epaisseur, 
                    -halfInteriorBase, epaisseur, 
                    1 
                },

                { 
                    -halfInteriorBase-(epaisseur/2), epaisseur,
                    -halfBaseWidth-epaisseur, epaisseur, 
                    1
                },

                { 
                    -halfBaseWidth-epaisseur, 0, 
                    -halfBodyWidth, 0, 
                    1
                },
            };
            
            double[,] pointsArcArray = {
                {
                    -halfBaseWidth, 0, 
                    -halfBaseWidth-(epaisseur/2), -epaisseur/4,
                    -halfBaseWidth-epaisseur, 0
                },

                {
                    -halfBaseWidth-epaisseur, epaisseur, 
                    -halfBaseWidth-epaisseur-(epaisseur/2), 1+epaisseur/2,
                    -halfBodyWidth, 0
                },

                {
                    -halfInteriorBase, epaisseur,
                    -halfInteriorBase-(epaisseur/4), epaisseur+1,
                    -halfInteriorBase-(epaisseur/2), epaisseur
                },
            };
            
            _wrapper.CreateSketch(2);
            _wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            _wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0));
            _wrapper.Spin();
            _wrapper.Extrusion(3, 1);

        }

        /// <summary>
        /// Построение обьекта под тарелкой.
        /// </summary>
        private void BuildPodTarelka(Parameters parameters)
        {
            // Получаем параметры
            double bodyWidth = parameters.AllParameters[ParameterType.BodyWidth].Value;
            double baseWidth = parameters.AllParameters[ParameterType.BaseWidth].Value;
            double interiorUpWidth = parameters.AllParameters[ParameterType.InteriorUpWidth].Value;
            double interiorBaseWidth = parameters.AllParameters[ParameterType.InteriorBaseWidth].Value;

            double halfBodyWidth = bodyWidth / 2;
            double halfBaseWidth = baseWidth / 2;
            double halfInteriorUpWidth = interiorUpWidth / 2;
            double epaisseur = halfBodyWidth - halfInteriorUpWidth;
            double halfInteriorBase = interiorBaseWidth / 2;

            double[,] pointsArray = {
                { 
                    0, epaisseur, 
                    -halfInteriorBase, epaisseur, 
                    1 
                },

                { 
                    -halfInteriorBase-(epaisseur/2), epaisseur,
                    -halfBaseWidth-2*epaisseur, epaisseur, 
                    1
                },

                {
                    -halfBaseWidth-2*epaisseur, epaisseur, 
                    -halfBaseWidth-2*epaisseur, 3*epaisseur, 
                    1
                },

                {
                    -halfBaseWidth-2*epaisseur, 3*epaisseur, 
                    0, 3*epaisseur, 
                    1
                },

                { 
                    0, epaisseur, 
                    0, 3*epaisseur, 
                    3
                },
            };

            double[,] pointsArcArray = {
                {
                    -halfInteriorBase, epaisseur, 
                    -halfInteriorBase-(epaisseur/4), epaisseur+1,
                    -halfInteriorBase-(epaisseur/2), epaisseur
                }
            };

            _wrapper.CreateSketch(2);
            _wrapper.CreateArc(pointsArcArray, 0, pointsArcArray.GetLength(0));
            _wrapper.CreateLine(pointsArray, 0, pointsArray.GetLength(0));
            _wrapper.Spin();
            _wrapper.Extrusion(3, 1);
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
