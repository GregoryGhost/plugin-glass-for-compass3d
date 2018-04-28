using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Шаблон стакана
    /// </summary>
    public interface IGlass
    {
        /// <summary>
        /// Высота стакана
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// Диаметр основания
        /// </summary>
        double DiameterBottom { get; set; }

        /// <summary>
        /// Угол наклона высоты стакана
        /// </summary>
        double AngleHeight { get; set; }

        /// <summary>
        /// Толщина стенок стакана
        /// </summary>
        double DepthSide { get; set; }

        /// <summary>
        /// Толщина дна стакана
        /// </summary>
        double DepthBottom { get; set; }

        /// <summary>
        /// Высота граней стакана
        /// </summary>
        double HeightFaceted { get; set; }

        /// <summary>
        /// Количество граней стакана
        /// </summary>
        int CountFaceted { get; set; }
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
    /// Маркер зависимых(автовычислимых) параметров стакана
    /// </summary>
    public interface IAutoCalcParams
    {
        /// <summary>
        /// Зависимый параметр - высота стакана
        /// </summary>
        bool Height { get; set; }

        /// <summary>
        /// Зависимый параметр - диаметр основания
        /// </summary>
        bool DiameterBottom { get; set; }

        /// <summary>
        /// Зависимый параметр - угол наклона высоты стакана
        /// </summary>
        bool AngleHeight { get; set; }

        /// <summary>
        /// Зависимый параметр - толщина стенок стакана
        /// </summary>
        bool DepthSide { get; set; }

        /// <summary>
        /// Зависимый параметр - толщина дна стакана
        /// </summary>
        bool DepthBottom { get; set; }

        /// <summary>
        /// Зависимый параметр - высота граней стакана
        /// </summary>
        bool HeightFaceted { get; set; }

        /// <summary>
        /// Зависимый параметр - количество граней стакана
        /// </summary>
        bool CountFaceted { get; set; }
    }

    /// <summary>
    /// Диапазон допустимых значений параметра.
    /// </summary>
    public class BorderConditions<T> where T: IComparable<T>
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
                if(value.CompareTo(Max) > 0)
                {
                    var msg = String.Format("Заданное значение = {0}" +
                        "больше, чем максимальное значение = {1}",
                            value, Max);
                    throw new ArgumentException(msg);
                }

                if (value.CompareTo(Min) < 0)
                {
                    var msg = String.Format("Заданное значение = {0}" +
                        "меньше, чем минимальное значение = {1}",
                            value, Min);
                    throw new ArgumentException(msg);
                }

                _value = value;
            }
        }
    }

    /// <summary>
    /// Зависимые параметры стакана.
    /// </summary>
    public class DependenciesParams : IAutoCalcParams
    {

        public bool Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DiameterBottom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Зависимый параметр - угол наклона высоты стакана.
        /// </summary>
        public bool AngleHeight
        {
            get;
            set;
        }

        /// <summary>
        /// Зависимый параметр - толщина стенки стакана.
        /// </summary>
        public bool DepthSide
        {
            get;
            set;
        }

        /// <summary>
        /// Зависимый параметр - толщина дна стакана.
        /// </summary>
        public bool DepthBottom
        {
            get;
            set;
        }

        /// <summary>
        /// Зависимый параметр - высота узора.
        /// </summary>
        public bool HeightFaceted
        {
            get;
            set;
        }

        /// <summary>
        /// Зависимый параметр - количество граней.
        /// </summary>
        public bool CountFaceted
        {
            get;
            set;
        }
    }


    /// <summary>
    /// Гранёный стакан
    /// </summary>
    public class FacetedGlass : IGlass, IChecker
    {

        public double Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DiameterBottom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double AngleHeight
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DepthSide
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DepthBottom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double HeightFaceted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CountFaceted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }
    }


    /// <summary>
    /// Гофрированный стакан
    /// </summary>
    public class CrimpGlass : IGlass, IChecker
    {

        public double Height
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DiameterBottom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double AngleHeight
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DepthSide
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double DepthBottom
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public double HeightFaceted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int CountFaceted
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }
    }
}
