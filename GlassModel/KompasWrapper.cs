using Kompas6Constants3D;
using Kompas6LTAPI5;
using System;
using System.Runtime.InteropServices;

namespace GlassModel
{
    /// <summary>
    /// Обертка по работе с САПР Компас 3D.
    /// </summary>
    public class KompasWrapper
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
        /// Экземпляр обертки над Компасом
        /// </summary>
        private static KompasWrapper _kompasWrapper;

        /// <summary>
        /// Инициализация обертки для работы с САПР Компас 3D.
        /// </summary>
        private KompasWrapper()
        {
            RunCAD();
        }

        /// <summary>
        /// Экземпляр обертки для работы с САПР Компас 3D.
        /// </summary>
        public static KompasWrapper Instance
        {
            get
            {
                if (_kompasWrapper == null) 
                {
                    _kompasWrapper = new KompasWrapper();
                }

                return _kompasWrapper;
            }
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
        public void ShowCAD()
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
                catch (NullReferenceException)
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
        /// Текущий документ сборки в Компас 3D.
        /// </summary>
        public ksDocument3D Document3D
        {
            get
            {
                return _kompas.Document3D();
            }
        }

        /// <summary>
        /// Создание смещенной плоскости
        /// </summary>
        /// <param name="part">Компонент сборки</param>
        /// <param name="basePlane">Исходная плоскость</param>
        /// <param name="offset">Смещение</param>
        /// <returns>Возвращает смещенную плоскость</returns>
        public ksEntity CreateOffsetPlane(ksPart part, ksEntity basePlane,
            double offset)
        {
            ksEntity planeFormSurface =
                part.NewEntity((short)Obj3dType.o3d_planeOffset);
            ksPlaneOffsetDefinition planeDefinition =
                planeFormSurface.GetDefinition();

            planeDefinition.SetPlane(basePlane);
            planeDefinition.offset = offset;

            planeFormSurface.Create();

            return planeFormSurface;
        }

        public ksRegularPolygonParam GetParamStruct(short param)
        {
            return _kompas.GetParamStruct(param);
        }
    }
}
