using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{


    /// <summary>
    /// Гофрированный стакан
    /// </summary>
    public class CrimpGlass : FacetedGlass
    {
        /// <summary>
        /// Устанавливает задаваемые параметры для гофрированного стакана.
        /// </summary>
        /// <param name="height">Высота стакана.</param>
        /// <param name="diameterBottom">Диаметр дна стакана.</param>
        /// <param name="countFaceted">Количество граней стакана.</param>
        public CrimpGlass(BorderConditions<double> height,
            BorderConditions<double> diameterBottom,
            BorderConditions<int> countFaceted)
            : base(height, diameterBottom, null, null, null,
                countFaceted)
        {
            //Зависимые автовычисляемые параметры - height faceted.
            //Фиксированные параметры - depth side, depth bottom,
            //  angle height.
            //Задаваемые параметры - height, diameter bottom,
            //  count faceted.
            _dependencies = new DependenciesParams(false, false,
                true, true, true, true, false);

            this._angleHeight = new BorderConditions<double>(5, 5, 5);
            this._depthSide = new BorderConditions<double>(3, 3, 3);
            this._depthBottom = new BorderConditions<double>(4, 4, 4);
        }

        /// <summary>
        /// Угол наклона высоты.
        /// </summary>
        public override double AngleHeight
        {
            get
            {
                return base.AngleHeight;
            }
            set
            {
            }
        }

        /// <summary>
        /// Толщина стенки стакана (в процентах).
        /// </summary>
        public override double DepthSide
        {
            get
            {
                return base.DepthSide;
            }
            set
            {
            }
        }

        /// <summary>
        /// Толщина дна стакана (в процентах).
        /// </summary>
        public override double DepthBottom
        {
            get
            {
                return base.DepthBottom;
            }
            set
            {
            }
        }
    }
}
