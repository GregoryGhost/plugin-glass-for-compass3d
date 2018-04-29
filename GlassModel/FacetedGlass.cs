using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Гранёный стакан
    /// </summary>
    public class FacetedGlass : IGlass, IChecker
    {
        /// <summary>
        /// Высота стакана
        /// </summary>
        private BorderConditions<double> _height;

        /// <summary>
        /// Процент высоты узора от высоты стаканаы
        /// </summary>
        private readonly double _percentForHeightFaceted = 90;

        public FacetedGlass(BorderConditions<double> height)
        {
            _height = height;
        }

        /// <summary>
        /// Высота стакана
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     Возникает, если устанавливаемое значение
        ///     выходит за рамки заданного интервала.</exception>
        public double Height
        {
            get
            {
                return _height.Value;
            }
            set
            {
                _height.Value = value;
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

        /// <summary>
        /// Зависимый параметр - высота узора стакана
        /// </summary>
        public double HeightFaceted
        {
            get
            {
                return this.Height * _percentForHeightFaceted / 100;
            }
            set
            {
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
