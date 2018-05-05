using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    public class BuilderCleanGlass : IBuilder
    {
        private IBuilder _builderBlank;

        private KompasWrapper _kompas;

        public BuilderCleanGlass()
        {
            _kompas = KompasWrapper.Instance;

            _builderBlank = new BuilderOfBlank();
        }

        public void Build(IGlass glass, IChecker checker)
        {
            _builderBlank.Build(glass, checker);
        }
    }
}
