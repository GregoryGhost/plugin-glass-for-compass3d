using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Гранёный стакан.
    /// </summary>
    public class FacetedGlass : CleanGlass
    {
        /// <summary>
        /// Процент высоты узора от высоты стакана.
        /// </summary>
        private readonly double _percentForHeightFaceted = 90;

        /// <summary>
        /// Угол наклона высоты стакана.
        /// </summary>
        protected BorderConditions<double> _angleHeight;

        /// <summary>
        /// Толщина стенки стакана (в процентах).
        /// </summary>
        protected BorderConditions<double> _depthSide;

        /// <summary>
        /// Толщина дна стакана (в процентах).
        /// </summary>
        protected BorderConditions<double> _depthBottom;

        /// <summary>
        /// Количество граней стакана.
        /// </summary>
        protected BorderConditions<int> _countFaceted;

        private const string _labelAngleHeight = "AngleHeight";
        private const string _labelCountFaceted = "CountFaceted";
        private const string _labelDepthSide = "DepthSide";
        private const string _labelDepthBottom = "DepthBottom";

        /// <summary>
        /// Устанавливает задаваемые параметры для граненого стакана.
        /// </summary>
        /// <param name="height">Высота стакана.</param>
        /// <param name="diameterBottom">Диаметр дна стакана.</param>
        /// <param name="angleHeight">Угол наклона высоты стакана.</param>
        /// <param name="depthSide">Толщина стенки стакана.</param>
        /// <param name="depthBottom">Толщина дна стакана.</param>
        /// <param name="countFaceted">Количество граней стакана.</param>
        public FacetedGlass(BorderConditions<double> height,
            BorderConditions<double> diameterBottom,
            BorderConditions<double> angleHeight,
            BorderConditions<double> depthSide,
            BorderConditions<double> depthBottom,
            BorderConditions<int> countFaceted)
            : base(diameterBottom, height)
        {
            //Зависимые автовычисляемые параметры - height faceted.
            //Задаваемые параметры - height, diameter bottom,
            //  angle height, count faceted, depth side, depth bottom.
            _dependencies = new DependenciesParams(false, false,
                false, false, false, true, false);

            _angleHeight = angleHeight;
            _depthSide = depthSide;
            _depthBottom = depthBottom;
            _countFaceted = countFaceted;

            _isValidParams.Add(_labelAngleHeight, true);
            _isValidParams.Add(_labelCountFaceted, true);
            _isValidParams.Add(_labelDepthSide, true);
            _isValidParams.Add(_labelDepthBottom, true);
        }

        /// <summary>
        /// Угол наклона высоты.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public override double AngleHeight
        {
            get
            {
                return _angleHeight.Value;
            }
            set
            {
                try
                {
                    _angleHeight.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[_labelAngleHeight] = false;
                    throw ex;
                }
                _isValidParams[_labelAngleHeight] = true;
            }
        }

        /// <summary>
        /// Толщина стенки стакана (в процентах).
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public override double DepthSide
        {
            get
            {
                return _depthSide.Value;
            }
            set
            {
                try
                {
                    _depthSide.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[_labelDepthSide] = false;
                    throw ex;
                }
                _isValidParams[_labelDepthSide] = true;
            }
        }

        /// <summary>
        /// Толщина дна стакана (в процентах).
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public override double DepthBottom
        {
            get
            {
                return _depthBottom.Value;
            }
            set
            {
                try
                {
                    _depthBottom.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[_labelDepthBottom] = false;
                    throw ex;
                }
                _isValidParams[_labelDepthBottom] = true;
            }
        }

        /// <summary>
        /// Зависимый параметр - высота узора стакана.
        /// </summary>
        public override double HeightFaceted
        {
            get
            {
                var heightFaceted = Height * _percentForHeightFaceted 
                    / 100;
                return heightFaceted;
            }
            set
            {
            }
        }

        /// <summary>
        /// Количество граней стакана.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public override int CountFaceted
        {
            get
            {
                return _countFaceted.Value;
            }
            set
            {
                try
                {
                    _countFaceted.Value = value;
                }
                catch (ArgumentException ex)
                {
                    _isValidParams[_labelCountFaceted] = false;
                    throw ex;
                }
                _isValidParams[_labelCountFaceted] = true;
            }
        }
    }
}
