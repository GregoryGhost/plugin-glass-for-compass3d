using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassModel
{
    /// <summary>
    /// Построитель стакана в САПР
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Построить модель стакана
        /// </summary>
        /// <param name="glass">Шаблон с параметрами стакана</param>
        /// <param name="checker">Проверяющий параметры стакана</param>
        void Build(IGlass glass, IChecker checker);
    }
}
