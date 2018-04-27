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
}
