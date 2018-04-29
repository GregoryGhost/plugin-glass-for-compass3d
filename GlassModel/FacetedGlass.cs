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
    public class FacetedGlass : CleanGlass
    {
        /// <summary>
        /// Процент высоты узора от высоты стакана
        /// </summary>
        private readonly double _percentForHeightFaceted = 90;

        public FacetedGlass(BorderConditions<double> height,
            BorderConditions<double> diameterBottom)
            : base(diameterBottom, height) { }

        public override double AngleHeight
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
        /// Зависимый параметр - высота узора стакана
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
