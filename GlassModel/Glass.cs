using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Шаблон стакана.
    /// </summary>
    public interface IGlass
    {
        /// <summary>
        /// Высота стакана.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Диаметр основания.
        /// </summary>
        double DiameterBottom { get; set; }

        /// <summary>
        /// Угол наклона высоты стакана.
        /// </summary>
        double AngleHeight { get; set; }

        /// <summary>
        /// Толщина стенок стакана (в процентах).
        /// </summary>
        double DepthSide { get; set; }

        /// <summary>
        /// Толщина дна стакана (в процентах).
        /// </summary>
        double DepthBottom { get; set; }

        /// <summary>
        /// Высота граней стакана.
        /// </summary>
        double HeightFaceted { get; set; }

        /// <summary>
        /// Количество граней стакана.
        /// </summary>
        int CountFaceted { get; set; }

        /// <summary>
        /// Узнать автовычислимые параметры стакана.
        /// </summary>
        IAutoCalcParams Properties { get; }

        /// <summary>
        /// Скругление дна и горлышка стакана.
        /// </summary>
        bool Filleted { get; set; }
    }


    /// <summary>
    /// Интерфейс для проверки параметров стакана 
    ///     на требования предметной области.
    /// </summary>
    public interface IChecker
    {
        /// <summary>
        /// Проверяет удовлетворяют ли параметры стакана
        ///     требованиям предметной области.
        /// </summary>
        bool IsValid { get; }
    }


    /// <summary>
    /// Маркер зависимых(автовычислимых) параметров стакана.
    /// </summary>
    public interface IAutoCalcParams
    {
        /// <summary>
        /// Зависимый параметр - высота стакана.
        /// </summary>
        bool Height { get; }

        /// <summary>
        /// Зависимый параметр - диаметр основания.
        /// </summary>
        bool DiameterBottom { get; }

        /// <summary>
        /// Зависимый параметр - угол наклона высоты стакана.
        /// </summary>
        bool AngleHeight { get; }

        /// <summary>
        /// Зависимый параметр - толщина стенок стакана.
        /// </summary>
        bool DepthSide { get; }

        /// <summary>
        /// Зависимый параметр - толщина дна стакана.
        /// </summary>
        bool DepthBottom { get; }

        /// <summary>
        /// Зависимый параметр - высота граней стакана.
        /// </summary>
        bool HeightFaceted { get; }

        /// <summary>
        /// Зависимый параметр - количество граней стакана.
        /// </summary>
        bool CountFaceted { get; }
    }

    /// <summary>
    /// Диапазон допустимых значений параметра.
    /// </summary>
    public class BorderConditions<T> where T : IComparable<T>
    {
        /// <summary>
        /// Минимальная граница для значения параметра.
        /// </summary>
        private T _minimum;
        private const string nameOfMin = "min";

        /// <summary>
        /// Максимальная граница для значения параметра.
        /// </summary>
        private T _maximum;

        /// <summary>
        /// Значение параметра.
        /// </summary>
        private T _value;

        /// <summary>
        /// Установление граничных условий для значения.
        /// </summary>
        /// <param name="min">Минимальное значение.</param>
        /// <param name="value">Текущее значение.</param>
        /// <param name="max">Максимальное значение.</param>
        /// <exception cref="ArgumentException">
        ///     Возникает, если максимальное
        ///     меньше минимального значения.</exception>
        public BorderConditions(T min, T value, T max)
        {
            if (min.CompareTo(max) > 0)
            {
                var msg = String.Format("Минимальное значение = {0} " +
                    "больше максимального = {1}", min, max);
                throw new ArgumentException(msg, nameOfMin);
            }

            Min = min;
            Max = max;
            Value = value;
        }

        /// <summary>
        /// Максимальное значение диапазона.
        /// </summary>
        public T Max
        {
            get { return _maximum; }
            private set
            {
                _maximum = value;
            }
        }

        /// <summary>
        /// Минимальное значение диапазона.
        /// </summary>
        public T Min
        {
            get { return _minimum; }
            private set
            {
                _minimum = value;
            }
        }

        /// <summary>
        /// Значение, входящее в диапазон.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public T Value
        {
            get { return _value; }
            set
            {
                //if (value > Max)
                if (value.CompareTo(Max) > 0)
                {
                    var msg = String.Format("Заданное значение = {0} " +
                        "больше, чем максимальное значение = {1}",
                            value, Max);
                    throw new ArgumentException(msg);
                }

                if (value.CompareTo(Min) < 0)
                {
                    var msg = String.Format("Заданное значение = {0} " +
                        "меньше, чем минимальное значение = {1}",
                            value, Min);
                    throw new ArgumentException(msg);
                }

                _value = value;
            }
        }
    }

    /// <summary>
    /// Интерфейс для зависимых, фиксированных и задаваемых
    ///     параметров стакана.
    /// </summary>
    public class DependenciesParams : IAutoCalcParams
    {
        /// <summary>
        /// Инициализация вычислимых параметров стакана.
        /// </summary>
        /// <param name="height">Высота стакана 
        ///     - вычислимый параметр.</param>
        /// <param name="diameterBottom">Диаметр дна стакана
        ///     - вычислимый параметр.</param>
        /// <param name="angleHeight">Угол наклона высоты стакана
        ///     - вычислимый параметр.</param>
        /// <param name="depthSide">Толщина стенки стакана 
        ///     - вычислимый параметр.</param>
        /// <param name="depthBottom">Толщина дна стакана
        ///     - вычислимый параметр.</param>
        /// <param name="heightFaceted">Высота узора стакана
        ///     - вычислимый параметр.</param>
        /// <param name="countFaceted">Количество граней стакана
        ///     - вычислимый параметр.</param>
        public DependenciesParams(bool height, bool diameterBottom,
            bool angleHeight, bool depthSide, bool depthBottom,
                bool heightFaceted, bool countFaceted)
        {
            Height = height;
            DiameterBottom = diameterBottom;
            AngleHeight = angleHeight;
            DepthSide = depthSide;
            DepthBottom = depthBottom;
            HeightFaceted = heightFaceted;
            CountFaceted = countFaceted;
        }

        /// <summary>
        /// Высота стакана.
        /// </summary>
        public bool Height
        {
            get;
            private set;
        }

        /// <summary>
        /// Диаметр дна стакана.
        /// </summary>
        public bool DiameterBottom
        {
            get;
            private set;
        }

        /// <summary>
        /// Угол наклона высоты стакана.
        /// </summary>
        public bool AngleHeight
        {
            get;
            private set;
        }

        /// <summary>
        /// Толщина стенки стакана.
        /// </summary>
        public bool DepthSide
        {
            get;
            private set;
        }

        /// <summary>
        /// Толщина дна стакана.
        /// </summary>
        public bool DepthBottom
        {
            get;
            private set;
        }

        /// <summary>
        /// Высота узора.
        /// </summary>
        public bool HeightFaceted
        {
            get;
            private set;
        }

        /// <summary>
        /// Количество граней.
        /// </summary>
        public bool CountFaceted
        {
            get;
            private set;
        }
    }
}
