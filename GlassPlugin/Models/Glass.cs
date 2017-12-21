using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassPlugin.Models
{
    enum TypeGlass
    {
        crimp,
        clean,
        faceted
    }

    interface IGlass
    {
        void CreateModel(Parameters model);
    }

    public class GlassProxy: IGlass
    {
        Glass _glass = new Glass();
        
        public void CheckInput(Parameters model)
        {
            /*Проверка переданных параметров*/
            /*
             * Параметры:
             * диаметр дна (R1);
             * диаметр верхней части (либо угол наклона стенки)[R2];
             * тип узора стенки (изображены на рисунке 1);
             * высота узора стенки (от середины стакана)[H2];
             * верхний и нижний ободок стакана;
             * количество граней;
             * толщина дна (T1);
             * толщина стенки (T2);
             * высота стакана (H1);

             * Описанные выше параметры взаимосвязаны следующим образом:
             * если стакан граненый, то:
             * R1 меньше, либо равно R2;
             * R1, R2 меньше, либо равны H1;
             * R1, R2 угол наклона между ними не больше пяти градусов;
             * H2 меньше, либо равно H1;
             * T2 меньше, либо равно пяти процентам от R2;
             * T1 в интервале от двух до семи процентов от H1;
             * количество граней в интервале от четырех до двадцати;
             * для нижнего обода пять процентов от H1.
             * 
             * если стакан гофрированный, то:
             * для верхнего обода, узора, толщины стенки и дна - два процента и один процент соответственно от H1;
             * остальные параметры как и в граненном стакане.
             *
             * если стакан гладкий, то для него R1 равно R2 и толщина стенки и дна выбирается автоматически в зависимости от H1.
            */
            switch (model.typeOfGlass)
            {
                case TypeGlass.faceted:
                    break;
                case TypeGlass.crimp:
                    break;
                case TypeGlass.clean:
                    break;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Создает модель Стакана по переданным параметрам в Компасе 3d
        /// </summary>
        /// <exception cref="System.Exception">Thrown when...</exception>
        public void CreateModel(Parameters model)
        {
            this.CheckInput(model);
            this._glass.CreateModel(model);
        }
    }

    /*Sample of create user exception*/
    [Serializable()]
    public class InvalidDepartmentException : System.Exception
    {
        public InvalidDepartmentException() : base() { }
        public InvalidDepartmentException(string message) : base(message) { }
        public InvalidDepartmentException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected InvalidDepartmentException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

    class Glass:IGlass
    {
        //Kompas3DObject _kompas;
        Parameters _param;

        public void CreateModel(Parameters model)
        {
            /*надеямся, что модель имеет валидные данные и создаем 3д модел в Компасе*/
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
