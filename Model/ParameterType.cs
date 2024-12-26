namespace MugPlugin
{

    /// <summary>
    /// Перечисление типов параметров.
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Радиус R3 1-й кривизны запястья кружки.
        /// </summary>
        HandleRadius3,

        /// <summary>
        /// Радиус R4 2-й кривизны запястья кружки.
        /// </summary>
        HandleRadius4,

        /// <summary>
        /// Радиус R5 3-й кривизны запястья кружки.
        /// </summary>
        HandleRadius5,

        /// <summary>
        /// Длина ручки.
        /// </summary>
        HandleLength,

        /// <summary>
        /// Высота кружки.
        /// </summary>
        BodyLength,

        /// <summary>
        /// Диаметр кружки.
        /// </summary>
        BodyWidth,

        /// <summary>
        /// Радиус R1 от кривизны кружки до верха кружки.
        /// </summary>
        BodyRadius1,

        /// <summary>
        /// Радиус R2 кривизны кружки от основания.
        /// </summary>
        BodyRadius2,

        /// <summary>
        /// Диаметр основания кружки. 
        /// </summary>
        BaseWidth,

        /// <summary>
        /// Диаметр основания полости кружки.
        /// </summary>
        InteriorBaseWidth,

        /// <summary>
        /// Диаметр полости кружки.
        /// </summary>
        InteriorUpWidth,
    }
}
