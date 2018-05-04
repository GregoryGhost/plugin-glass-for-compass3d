using Kompas6LTAPI5;
using Kompas6Constants3D;
using System;
using System.Runtime.InteropServices;

namespace GlassModel
{
    /// <summary>
    /// Мастер по созданию болванок стакана в САПР Компас 3D
    /// </summary>
    public class BuilderOfBlank : IBuilder
    {
        /// <summary>
        /// Объект для связи с САПР Компас 3D
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// ID Компаса 3D в COM реестре
        /// </summary>
        private const string _progId = "KOMPASLT.Application.5";

        /// <summary>
        /// Начальная координата по OX отрисовки для эскиза
        /// </summary>
        private double _startX;

        /// <summary>
        /// Начальная координата по OY отрисовки для эскиза
        /// </summary>
        private double _startY;

        /// <summary>
        /// Инициализация необходимых параметров для работы с Компас 3D
        /// </summary>
        public BuilderOfBlank()
        {
            ShowCAD();
        }

        /// <summary>
        /// Запуск Компаса 3D или подключение к запущенной версии
        /// </summary>
        private void RunCAD()
        {
            var kompasType =
               Type.GetTypeFromProgID(_progId);

            try
            {
                //Получение ссылки на запущенную копию Компас 3д
                _kompas = (KompasObject)Marshal.
                    GetActiveObject(_progId);
            }
            catch (COMException)
            {
                _kompas = (KompasObject)Activator.
                    CreateInstance(kompasType);
            }
        }

        /// <summary>
        /// Показывает окно Компаса 3D
        /// </summary>
        private void ShowCAD()
        {
            var maxCount = 3;
            for (var i = 0; i < maxCount; i++)
            {
                try
                {
                    _kompas.Visible = true;
                }
                catch (COMException)
                {
                    RunCAD();
                }
                catch(NullReferenceException)
                {
                    RunCAD();
                }
            }

            if (_kompas != null)
            {
                _kompas.ActivateControllerAPI();
            }
        }

        /// <summary>
        /// Построить модель стакана в САПР Компас 3D
        /// </summary>
        /// <param name="photoFrame">Шаблон стакана</param>
        /// <param name="checker">Проверяющий параметры стакана</param>
        /// <exception cref="InvalidOperationException">
        ///     Вызывается тогда, когда параметры стакана
        ///     имеют недопустимые значения.</exception>
        public void Build(IGlass glass, IChecker checker)
        {
            if (checker.IsValid == false)
            {
                var msg = String.Format("Шаблон стакана имеет" +
                    " недопустимые параметры для построения.");
                throw new InvalidOperationException(msg);
            }

            ShowCAD();

            _startX = 0;
            _startY = _startX;

            var doc = (ksDocument3D)_kompas.Document3D();
            doc.Create();

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);
            if (part == null)
            {
                return;
            }

            //построение стакана
        }
    }
}
