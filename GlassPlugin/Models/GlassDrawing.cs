using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassPlugin.Models
{
    public enum TypeGlass
    {
        /// <summary>
        /// Гофрированный тип стакана
        /// </summary>
        Crimp,
        /// <summary>
        /// Гладкий тип стакана
        /// </summary>
        Clean,
        /// <summary>
        /// Гранёный тип стакана
        /// </summary>
        Faceted
    }

    /// <summary>
    /// Помогает построить чертёж стакана в САПР
    /// </summary>
    interface BuilderOfGlass
    {
        /// <summary>
        /// Построить стакан в САПР
        /// </summary>
        /// <param name="glassDrawing">Чертёж стакана</param>
        void Build(IGlassDrawing glassDrawing);
    }


    /// <summary>
    /// Предоставляет возможность задать основные параметрые стакана
    /// </summary>
    interface IGlassDrawing
    {
        /// <summary>
        /// Количество граней граненого стакана
        /// </summary>
        double CountOfFaceGlass { get; set; }

        /// <summary>
        /// Толщина стенки стакана
        /// </summary>
        double SideDepthOfGlass { get; set; }

        /// <summary>
        /// Толщина дна стакана
        /// </summary>
        double DepthBottomOfGlass { get; set; }

        /// <summary>
        /// Высота граней стакана
        /// </summary>
        double HeightFaceOfGlass { get; set; }

        /// <summary>
        /// Высота стакана
        /// </summary>
        double HeightGlass { get; set; }

        /// <summary>
        /// Диаметр верхней части(горлышка) стакана
        /// </summary>
        double DiameterTopOfGlass { get; set; }

        /// <summary>
        /// Диаметр дна стакана
        /// </summary>
        double DiameterBottomOfGlass { get; set; }

        /// <summary>
        /// Тип стакана: граненый, гофрированный, гладкий
        /// </summary>
        TypeGlass TypeOfGlass { get; set; }
    }
}
