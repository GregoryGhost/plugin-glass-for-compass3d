using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassPlugin.Models.ExceptionsOfGlassParameters;

namespace GlassPlugin.Models
{
    enum TypeGlass
    {
        Crimp,
        Clean,
        Faceted
    }

    interface IGlass
    {
        void CreateModel(Parameters model);
    }

    public class GlassProxy : IGlass
    {
        Glass _glass = new Glass();

        readonly int maxCountOfFaceGlass = 20;

        readonly int minCountOfFaceGlass = 4;

        public void CheckInput(Parameters model)
        {
            //TODO: как-то отрефакторить этот ужас из if'ов
            //
            if (model.diameterBottomOfGlass <= model.diameterTopOfGlass)
            {
                if (model.diameterTopOfGlass <= model.heightGlass)
                {
                    if ((model.diameterBottomOfGlass <= model.heightGlass))
                    {
                        //считаем угол наклона в 5 градусов между R1, R2
                        const int tiltAngleBetweenDiameterTopAndBottomOfGlass = 5;
                        var b = model.diameterTopOfGlass - model.diameterBottomOfGlass;
                        var a = model.heightGlass;
                        var calcTiltAngle = Math.Atan(a / b) * 180 / Math.PI;

                        if ((calcTiltAngle <= tiltAngleBetweenDiameterTopAndBottomOfGlass) &&
                            calcTiltAngle >= 0)
                        {
                            if (model.heightFaceOfGlass <= model.heightGlass)
                            {
                                if (model.sideDepthOfGlass <= (model.diameterTopOfGlass * 5 / 100))
                                {
                                    if ((model.depthBottomOfGlass <= (model.heightGlass * 7 / 100)) &&
                                        (model.depthBottomOfGlass >= (model.heightGlass * 2 / 100)))
                                    {
                                        if ((model.countOfFaceGlass <= maxCountOfFaceGlass)
                                            && (model.countOfFaceGlass >= minCountOfFaceGlass))
                                        {
                                            switch (model.typeOfGlass)
                                            {
                                                case TypeGlass.Faceted:
                                                    _glass.CreateModel(model);
                                                    break;
                                                case TypeGlass.Crimp:
                                                    _glass.CreateModel(model);
                                                    break;
                                                case TypeGlass.Clean:
                                                    if (model.diameterTopOfGlass == model.diameterBottomOfGlass)
                                                    {
                                                        _glass.CreateModel(model);
                                                    }
                                                    else
                                                    {
                                                        throw new DifferentTopAndBottomDiameters("test");
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            throw new UnacceptableNumberOfFaces();
                                        }
                                    }
                                    else
                                    {
                                        throw new OutOfRangeDepthBottom();
                                    }

                                }
                                else
                                {
                                    throw new OutOfRangeSideDepth();
                                }
                            }
                            else
                            {
                                throw new OutOfRangeHeightFace();
                            }
                        }
                        else
                        {
                            throw new OutOfRangeTitleAngle();
                        }
                    }
                    else
                    {
                        throw new DiameterBottomAboveHeightGlass();
                    }
                }
                else
                {
                    throw new DiameterTopAboveHeightGlass();
                }
            }
            else
            {
                throw new DiameterBottomOutOfRangeDiameterTop();
            }
        }

        /// <summary>
        /// Создает модель Стакана по переданным параметрам в Компасе 3d
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when...</exception>
        public void CreateModel(Parameters model)
        {
            this.CheckInput(model);
            this._glass.CreateModel(model);
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
        public void setParameters(
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
            setParameters(
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
    }
}
