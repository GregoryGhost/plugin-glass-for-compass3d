using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Построитель гладкий стаканов.
    /// </summary>
    public class BuilderCleanGlass : IBuilder
    {
        /// <summary>
        /// Построитель болванок стакана.
        /// </summary>
        private IBuilder _builderBlank;

        /// <summary>
        /// Экземпляр Компаса 3D.
        /// </summary>
        private KompasWrapper _kompas;

        /// <summary>
        /// Установление связи с Компасом 3D.
        /// </summary>
        public BuilderCleanGlass()
        {
            _kompas = KompasWrapper.Instance;

            _builderBlank = new BuilderOfBlank();
        }

        /// <summary>
        /// Построить гладкий стакан.
        /// </summary>
        /// <param name="glass">Гладкий стакан.</param>
        /// <param name="checker">Проверяющий параметры стакана
        ///     в соответствие с требованиям предметной области.</param>
        public void Build(IGlass glass, IChecker checker)
        {
            _builderBlank.Build(glass, checker);
        }
    }
}
