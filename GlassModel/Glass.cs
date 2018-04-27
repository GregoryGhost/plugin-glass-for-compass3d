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
}
