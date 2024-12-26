using System;
using System.Runtime.InteropServices;
using Kompas6API5;
using Kompas6Constants3D;

namespace Kompas
{
    /// <summary>
    /// Класс для работы с API Компас.
    /// </summary>
    public class Wrapper
    {
        /// <summary>
        /// Поле для хранения приложения Компас.
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Поле для хранения выбранной 3d детали.
        /// </summary>
        private Kompas6API5.ksPart _part;

        /// <summary>
        /// Поле для хранения выбранного эскиза.
        /// </summary>
        private Kompas6API5.ksEntity _sketchEntity;

        /// <summary>
        /// Поле для хранения выбранной плоскости.
        /// </summary>
        private Kompas6API5.ksEntity _plane;

        /// <summary>
        /// Создание эскиза в компасе.
        /// </summary>
        /// <param name="perspective">Выбранная плоскость.</param>
        public void CreateSketch(int perspective)
        {
            if (this._part == null)
            {
                throw new Exception("Деталь (_part) не была инициализирована. Вызовите CreateFile() перед созданием эскиза.");
            }

            ksSketchDefinition sketchDef;
            this._sketchEntity = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_sketch);
            sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();

            // Выбираем плоскость
            if (perspective == 1)
            {
                this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeXOY);
            }
            else if (perspective == 2)
            {
                this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeXOZ);
            }
            else if (perspective == 3)
            {
                this._plane = (ksEntity)this._part.GetDefaultEntity((short)Obj3dType.o3d_planeYOZ);
            }
            else
            {
                throw new ArgumentException("Некорректная плоскость. Допустимые значения: 1, 2, 3.");
            }

            // Устанавливаем плоскость для эскиза
            sketchDef.SetPlane(this._plane);

            // Создаем эскиз
            this._sketchEntity.Create();

            // Выходим из режима редактирования
            ksDocument2D sketchEdit = (ksDocument2D)sketchDef.BeginEdit();
            sketchDef.EndEdit();
        }

        /// <summary>
        /// Создание линии в компасе.
        /// </summary>
        /// <param name="pointsArray">Массив точек по которым строятся линии.</param>
        /// <param name="start">Стартовый индекс массива.</param>
        /// <param name="count">Количество считываемых строк из массива.</param>
        public void CreateLine(double[,] pointsArray, int start, int count)
        {
            ksDocument2D document2D;
            ksSketchDefinition sketchDef;
            sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();
            document2D = (ksDocument2D)sketchDef.BeginEdit();
            if (document2D != null)
            {
                for (int i = start; i < start + count; i++)
                {
                    document2D.ksLineSeg(
                        pointsArray[i, 0],
                        pointsArray[i, 1],
                        pointsArray[i, 2],
                        pointsArray[i, 3],
                        (int)pointsArray[i, 4]);
                }

                sketchDef.EndEdit();
            }
        }

        /// <summary>
        /// Создание дуги в компасе.
        /// </summary>
        /// <param name="x1">x координата начальной точки.</param>
        /// <param name="y1">y координата начальной точки.</param>
        /// <param name="x2">x координата промежуточной точки.</param>
        /// <param name="y2">y координата промежуточной точки.</param>
        /// <param name="x3">x координата конечной точки.</param>
        /// <param name="y3">y координата конечной точки.</param>
        public void CreateArc(double[,] pointsArray, int start, int count)
        {
            // Ensure the array has the correct structure
            if (pointsArray.GetLength(1) != 6)
            {
                throw new ArgumentException("The pointsArray must have exactly 6 columns: {x1, y1, x2, y2, x3, y3}");
            }

            ksSketchDefinition sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();
            ksDocument2D document2D = (ksDocument2D)sketchDef.BeginEdit();

            if (document2D != null)
            {
                for (int i = start; i < start + count; i++)
                {
                    // Ensure the row index is within bounds
                    if (i >= pointsArray.GetLength(0))
                    {
                        throw new IndexOutOfRangeException($"Index {i} is outside the bounds of the points array.");
                    }

                    // Draw the arc
                    document2D.ksArcBy3Points(
                        pointsArray[i, 0], // x1
                        pointsArray[i, 1], // y1
                        pointsArray[i, 2], // x2
                        pointsArray[i, 3], // y2
                        pointsArray[i, 4], // x3
                        pointsArray[i, 5], // y3
                        1); // Direction of the arc
                }

                sketchDef.EndEdit();
            }
        }




        /// <summary>
        /// Задание вращения в компасе.
        /// </summary>
        public void Spin()
        {
            ksEntity entityRotate = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossRotated);
            if (entityRotate != null)
            {
                ksBossRotatedDefinition rotateDef =
                    (ksBossRotatedDefinition)entityRotate.GetDefinition();
                if (rotateDef != null)
                {
                    rotateDef.directionType = (short)Direction_Type.dtNormal;
                    rotateDef.SetSideParam(false, 360);
                    rotateDef.SetSketch(this._sketchEntity);  // эскиз операции вращения
                    entityRotate.Create();              // создать операцию
                }
            }
        }

        /// <summary>
        /// Выдавливание в компасе.
        /// </summary>
        /// <param name="parameter">Метод выдавливания.</param>
        /// <param name="length">Глубина выдавливания.</param>
        public void Extrusion(int parameter, double length)
        {
            if (parameter == 1)
            {
                ksEntity entityExtrusion =
                    (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
                if (entityExtrusion != null)
                {
                    ksEntity entityCutExtrusion =
                        (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
                    if (entityCutExtrusion != null)
                    {
                        ksCutExtrusionDefinition cutExtrusionDef =
                            (ksCutExtrusionDefinition)entityCutExtrusion.GetDefinition();
                        if (cutExtrusionDef != null)
                        {
                            cutExtrusionDef.SetSketch(this._sketchEntity);
                            cutExtrusionDef.directionType = (short)Direction_Type.dtBoth;
                            cutExtrusionDef.SetSideParam(
                                true,
                                (short)End_Type.etBlind,
                                length,
                                0,
                                false);
                            cutExtrusionDef.SetThinParam(false, 0, 0, 0);
                        }

                        entityCutExtrusion.Create(); // создадим операцию вырезание выдавливанием
                    }
                }
            }
            else if (parameter == 2)
            {
                ksEntity entityExtrusion =
                    (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
                if (entityExtrusion != null)
                {
                    // интерфейс свойств базовой операции выдавливания
                    ksBossExtrusionDefinition extrusionDef =
                        (ksBossExtrusionDefinition)entityExtrusion.GetDefinition();
                    if (extrusionDef != null)
                    {
                        extrusionDef.directionType = (short)Direction_Type.dtNormal;
                        extrusionDef.SetSideParam(
                            true, // прямое направление
                            (short)End_Type.etBlind,    // строго на глубину
                            length,
                            0,
                            false);
                        extrusionDef.SetThinParam(true, (short)Direction_Type.dtBoth, 0.25, 0.25);
                        extrusionDef.SetSketch(this._sketchEntity);   // эскиз операции выдавливания
                        entityExtrusion.Create();                    // создать операцию
                    }
                }
            }
            else if (parameter == 3)
            {
                ksEntity entityExtrusion =
                    (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_bossExtrusion);
                if (entityExtrusion != null)
                {
                    // интерфейс свойств базовой операции выдавливания
                    ksBossExtrusionDefinition extrusionDef =
                        (ksBossExtrusionDefinition)entityExtrusion.GetDefinition();
                    if (extrusionDef != null)
                    {
                        ksExtrusionParam extrusionProp =
                            (ksExtrusionParam)extrusionDef.ExtrusionParam();
                        ksThinParam thinProp = (ksThinParam)extrusionDef.ThinParam();
                        if (extrusionProp != null && thinProp != null)
                        {
                            extrusionDef.SetSketch(this._sketchEntity);

                            extrusionProp.direction = (short)Direction_Type.dtNormal;
                            extrusionProp.typeNormal = (short)End_Type.etBlind;
                            extrusionProp.depthNormal = length;

                            thinProp.thin = false;

                            entityExtrusion.Create();
                        }
                    }
                }
            }
            else if (parameter == 4) // 360-degree cut
            {
                ksEntity entityCutRotate = (ksEntity)this._part.NewEntity((short)Obj3dType.o3d_cutRotated);
                if (entityCutRotate != null)
                {
                    ksCutRotatedDefinition cutRotateDef = (ksCutRotatedDefinition)entityCutRotate.GetDefinition();
                    if (cutRotateDef != null)
                    {
                        cutRotateDef.directionType = (short)Direction_Type.dtNormal;
                        cutRotateDef.SetSideParam(false, 360); // Set full rotation
                        cutRotateDef.SetSketch(this._sketchEntity); // Link the sketch for rotation
                        entityCutRotate.Create(); // Create the cut operation
                    }
                }
            }
        }

        /// <summary>
        /// Открытие компаса.
        /// </summary>
        public void OpenCAD()
        {
            try
            {
                // Попытка подключения к уже запущенному процессу Kompas3D
                this._kompas = (KompasObject)Marshal.GetActiveObject("KOMPAS.Application.5");
                Console.WriteLine("Подключено к запущенному экземпляру Kompas3D.");
            }
            catch
            {
                // Если процесс не найден, создается новый экземпляр
                Type kompasType = Type.GetTypeFromProgID("KOMPAS.Application.5");
                if (kompasType != null)
                {
                    this._kompas = (KompasObject)Activator.CreateInstance(kompasType);
                    Console.WriteLine("Запущен новый экземпляр Kompas3D.");
                }
                else
                {
                    Console.WriteLine("Ошибка: Не удалось найти тип Kompas3D.");
                }
            }

            if (this._kompas != null)
            {
                this._kompas.Visible = true;
                this._kompas.ActivateControllerAPI();
                Console.WriteLine("Kompas3D готов к работе.");
            }
            else
            {
                throw new Exception("Не удалось запустить или подключиться к Kompas3D.");
            }
        }

        /// <summary>
        /// Рисует окружность в эскизе.
        /// </summary>
        /// <param name="x">Координата центра X.</param>
        /// <param name="y">Координата центра Y.</param>
        /// <param name="radius">Радиус окружности.</param>
        public void CreateCircle(double radius, int x, int y)
        {
            ksDocument2D document2D;
            ksSketchDefinition sketchDef;
            sketchDef = (ksSketchDefinition)this._sketchEntity.GetDefinition();
            document2D = (ksDocument2D)sketchDef.BeginEdit();
            if (document2D != null)
            {
                document2D.ksCircle(x, y, radius, 1);
            }
            sketchDef.EndEdit();
        }

        /// <summary>
        /// Создание документа в компасе.
        /// </summary>
        public void CreateFile()
        {
            ksDocument3D document3D;
            document3D = (ksDocument3D)this._kompas.Document3D();
            document3D.Create();
            this._part = (ksPart)document3D.GetPart((short)Part_Type.pTop_Part);
        }
    }
}
