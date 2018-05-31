using GlassModel.Glasses;
using GlassModel.Helpers;

namespace GlassModel
{
    namespace Builders
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
            public void Build(IGlass glass)
            {
                _builderBlank.Build(glass);
            }
        }
    }
}
