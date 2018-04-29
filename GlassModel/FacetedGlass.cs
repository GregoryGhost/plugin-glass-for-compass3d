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
        /// Толщина стенки стакана.
        /// </summary>
        protected BorderConditions<double> _depthSide;

        /// <summary>
        /// Толщина дна стакана.
        /// </summary>
        protected BorderConditions<double> _depthBottom;

        /// <summary>
        /// Количество граней стакана.
        /// </summary>
        protected BorderConditions<int> _countFaceted;

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
            BorderConditions<int> countFaceted) : 
                base(diameterBottom, height)
        {
            //Задаваемые параметры
            base._dependencies.Height = false;
            base._dependencies.DiameterBottom = false;
            base._dependencies.AngleHeight = false;
            base._dependencies.CountFaceted = false;
            base._dependencies.DepthSide = false;
            base._dependencies.DepthBottom = false;
            //Вычисляемый параметр
            base._dependencies.HeightFaceted = true;

            _angleHeight = angleHeight;
            _depthSide = depthSide;
            _depthBottom = depthBottom;
            _countFaceted = countFaceted;
        }

        /// <summary>
        /// Угол наклона высоты.
        /// </summary>
        public override double AngleHeight
        {
            get
            {
                return _angleHeight.Value;
            }
            set
            {
                _angleHeight.Value = value;
            }
        }

        public override double DepthSide
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

        public override double DepthBottom
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
        /// Зависимый параметр - высота узора стакана.
        /// </summary>
        public override double HeightFaceted
        {
            get
            {
                return this.Height * _percentForHeightFaceted / 100;
            }
            set
            {
            }
        }

        public override int CountFaceted
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
}
