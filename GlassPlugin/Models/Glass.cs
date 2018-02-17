using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassPlugin.Models.ExceptionsOfGlassParameters;

namespace GlassPlugin.Models
{
    public class GlassProxy : IGlassDrawing
    {
        readonly int maxCountOfFaceGlass = 20;

        readonly int minCountOfFaceGlass = 4;

        /// <summary>
        /// Проверяет параметры модели Glass
        /// </summary>
        public void CheckInput(Parameters model)
        {
            if ((model.diameterBottomOfGlass <= model.diameterTopOfGlass) == false)
            {
                throw new DiameterBottomOutOfRangeDiameterTop("Диаметр дна должен быть меньше или равен диаметру горлышка");
            }
            if ((model.diameterTopOfGlass <= model.heightGlass) == false)
            {
                throw new DiameterTopAboveHeightGlass("Диаметр горлышка должен быть меньше либо равен высоте стакана");
            }
            if ((model.diameterBottomOfGlass <= model.heightGlass) == false)
            {
                throw new DiameterBottomAboveHeightGlass("Диаметр дна должен быть меньше либо равен высоте стакана");
            }
            //считаем угол наклона в 5 градусов между R1, R2
            const int tiltAngleBetweenDiameterTopAndBottomOfGlass = 5;
            var b = model.diameterTopOfGlass - model.diameterBottomOfGlass;
            var a = model.heightGlass;
            var calcTiltAngle = Math.Atan(a / b) * 180 / Math.PI;

            if (((calcTiltAngle <= tiltAngleBetweenDiameterTopAndBottomOfGlass) &&
                 (calcTiltAngle >= 0)) == false)
            {
                var t = String.Format("Угл наклона выходит за допустимый диапозон от 0 до {0} градусов", calcTiltAngle);
                throw new OutOfRangeTitleAngle(t);
            }
            if ((model.heightFaceOfGlass <= model.heightGlass) == false)
            {
                throw new OutOfRangeHeightFace("Высота граней должна быть меньше либо равна высоты стакана");
            }
            if ((model.sideDepthOfGlass <= (model.diameterTopOfGlass * 5 / 100)) == false)
            {
                throw new OutOfRangeSideDepth("Толщина стенки должна быть меньше либо равна 5% от высоты стакана");
            }

            var c = (model.heightGlass * 7 / 100);
            var c2 = (model.heightGlass * 2 / 100);

            if ((model.depthBottomOfGlass <= c) &&
                (model.depthBottomOfGlass >= c2) == false)
            {
                var t = String.Format("Толщина дна выходит за допустимый диапозон от {0} до {1}", c, c2);
                throw new OutOfRangeDepthBottom(t);
            }

            if (((model.countOfFaceGlass <= maxCountOfFaceGlass) &&
                (model.countOfFaceGlass >= minCountOfFaceGlass)) == false)
            {
                var t = String.Format("Количество граней стакана выходит за допустимый диапозон от {0} до {1}",
                    minCountOfFaceGlass,
                    maxCountOfFaceGlass);
                throw new UnacceptableNumberOfFaces(t);
            }

            if (model.typeOfGlass == TypeGlass.Clean)
            {
                if ((model.diameterTopOfGlass == model.diameterBottomOfGlass) == false)
                {
                    throw new DifferentTopAndBottomDiameters("Диаметры дна и горлышка должны быть равны у гладкого стакана");
                }
            }
        }

        public double CountOfFaceGlass{
            get;
            set;
        }

        public double DepthBottomOfGlass
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

        public double HeightFaceOfGlass
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

        public double HeightGlass
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

        public double DiameterTopOfGlass
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

        public double DiameterBottomOfGlass
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

        public double SideDepthOfGlass
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

        public TypeGlass TypeOfGlass
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



    class Glass : IGlass
    {
        Compass3D _kompas;
        Parameters _param;

        public void CreateModel(Parameters model)
        {
            /*надеямся, что модель имеет валидные данные и создаем 3д модел в Компасе*/
            switch (model.typeOfGlass)
            {
                case TypeGlass.Faceted:
                    _kompas.BuildGlass(model);
                    break;
                case TypeGlass.Crimp:
                    break;
                case TypeGlass.Clean:
                    break;
            }
            throw new NotImplementedException();
        }
    }

    public class Parameters
    {
        /// <summary>
        /// Количество граней граненого стакана
        /// </summary>
        public double countOfFaceGlass;
        /// <summary>
        /// Толщина дна стакана
        /// </summary>
        public double depthBottomOfGlass;
        /// <summary>
        /// Высота граней стакана
        /// </summary>
        public double heightFaceOfGlass;
        /// <summary>
        /// Высота стакана
        /// </summary>
        public double heightGlass;
        /// <summary>
        /// Диаметр верхней части(горлышка) стакана
        /// </summary>
        public double diameterTopOfGlass;
        /// <summary>
        /// Диаметр дна стакана
        /// </summary>
        public double diameterBottomOfGlass;
        /// <summary>
        /// Толщина стенки стакана
        /// </summary>
        public double sideDepthOfGlass;
        /// <summary>
        /// Тип стакана: граненый, гофрированный, гладкий
        /// </summary>
        public TypeGlass typeOfGlass;

        //public double countOfFaceGlass
        //{
        //    get { return countOfFaceGlass; }
        //}
        public Parameters() { }

        public Parameters(
            double countOfFaceGlass,
            double depthBottomOfGlass,
            double diameterTopOfGlass,
            double diameterBottomOfGlass,
            double sideDepthOfGlass,
            double heightFaceOfGlass,
            double heightGlass,
            TypeGlass typeOfGlass)
        {
            SetParameters(
                countOfFaceGlass,
                depthBottomOfGlass,
                depthBottomOfGlass,
                diameterTopOfGlass,
                sideDepthOfGlass,
                heightFaceOfGlass,
                heightGlass,
                typeOfGlass
                );
        }
        public void SetParameters(
            double countOfFaceGlass,
            double depthBottomOfGlass,
            double diameterTopOfGlass,
            double diameterBottomOfGlass,
            double sideDepthOfGlass,
            double heightFaceOfGlass,
            double heightGlass,
            TypeGlass typeOfGlass)
        {
            this.countOfFaceGlass = countOfFaceGlass;
            this.depthBottomOfGlass = depthBottomOfGlass;
            this.heightFaceOfGlass = heightFaceOfGlass;
            this.diameterTopOfGlass = diameterTopOfGlass;
            this.sideDepthOfGlass = sideDepthOfGlass;
            this.typeOfGlass = typeOfGlass;
            this.diameterBottomOfGlass = diameterBottomOfGlass;
            this.heightGlass = heightGlass;

        }
    }
}
