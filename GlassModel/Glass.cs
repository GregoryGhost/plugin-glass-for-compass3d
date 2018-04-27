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
        double CountFaceted { get; set; }
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
    public class BorderConditions
    {
        /// <summary>
        /// Минимальная граница для значения параметра
        /// </summary>
        private double _minimum = 0.0f;
        private const string nameOfMin = "min";

        /// <summary>
        /// Максимальная граница для значения параметра
        /// </summary>
        private double _maximum = 0.0f;

        /// <summary>
        /// Значение параметра
        /// </summary>
        private double _value = 0.0f;

        /// <summary>
        /// Установление граничных условий для значения.
        /// </summary>
        /// <param name="min">Минимальное значение.</param>
        /// <param name="value">Текущее значение.</param>
        /// <param name="max">Максимальное значение.</param>
        /// <exception cref="ArgumentException">
        ///     Возникает, если максимальное
        ///     меньше минимального значения.</exception>
        public BorderConditions(double min, double value, double max)
        {
            if (min > max)
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
        public double Max
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
        public double Min
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
        public double Value
        {
            get { return _value; }
            set
            {
                if (value > Max)
                {
                    var msg = String.Format("Заданное значение = {0}" +
                        "больше, чем максимальное значение = {1}",
                            value, Max);
                    throw new ArgumentException(msg);
                }

                if (value < Min)
                {
                    var msg = String.Format("Заданное значение = {0}" +
                        "меньше, чем минимальное значение = {1}",
                            value, Min);
                    throw new ArgumentException(msg);
                }

                _value = value;
            }
        }


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

            public bool AngleHeight
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

            public bool DepthSide
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

            public bool DepthBottom
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

            public bool HeightFaceted
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

            public bool CountFaceted
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
        }

        /// <summary>
        /// Гладкий стакан
        /// </summary>
        public class CleanGlass : IGlass, IChecker
        {
            private IAutoCalcParams _dependencies;

            public CleanGlass()
            {
                _dependencies = new DependenciesParams();
            }

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

            public double CountFaceted
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

            public double CountFaceted
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

            public double CountFaceted
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
}
