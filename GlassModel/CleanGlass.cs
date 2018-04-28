using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Гладкий стакан.
    /// </summary>
    public class CleanGlass : IGlass, IChecker
    {
        /// <summary>
        /// Зависимые и фиксированные параметры стакана
        /// </summary>
        private IAutoCalcParams _dependencies;

        /// <summary>
        /// Постоянный угол наклона высоты стакана
        /// </summary>
        private readonly double _angleHeight = 0.0f;

        /// <summary>
        /// Отсутствует узор стакана => высота узора равна нулю 
        /// </summary>
        private readonly double _heightFaceted = 0.0f;

        /// <summary>
        /// Отсутствуют грани стакана => количество граней равно нулю
        /// </summary>
        private readonly int _countFaceted = 0;

        /// <summary>
        /// Установление параметров гладкого стакана.
        /// </summary>
        /// <param name="diameterBottom">Диаметр дна стакана.</param>
        /// <param name="height">Высота стакана.</param>
        public CleanGlass(BorderConditions<double> diameterBottom,
            BorderConditions<double> height)
        {
            _dependencies = new DependenciesParams();
            _dependencies.AngleHeight = true;
            _dependencies.CountFaceted = true;
            _dependencies.DepthBottom = true;
            _dependencies.DepthSide = true;
            _dependencies.HeightFaceted = true;
        }

        /// <summary>
        /// Высота стакана
        /// </summary>
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

        /// <summary>
        /// Диаметр дна стакана
        /// </summary>
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

        /// <summary>
        /// Отсутствующий угол наклона высоты стакана
        /// </summary>
        public double AngleHeight
        {
            get
            {
                return _angleHeight;
            }
            set
            {
            }
        }

        /// <summary>
        /// Зависящий параметр - толщина стенки стакана
        /// </summary>
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

        /// <summary>
        /// Зависящий параметр - толщина дна
        /// </summary>
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
        /// Отсутствующий узор
        /// </summary>
        public double HeightFaceted
        {
            get
            {
                return _heightFaceted;
            }
            set
            {
            }
        }

        /// <summary>
        /// Отсутствующие грани
        /// </summary>
        public int CountFaceted
        {
            get
            {
                return _countFaceted;
            }
            set
            {
            }
        }

        public bool IsValid
        {
            get { throw new NotImplementedException(); }
        }
    }
}
