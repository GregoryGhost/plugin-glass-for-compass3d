using Kompas6Constants;
using Kompas6Constants3D;
using Kompas6LTAPI5;

using System;
using System.Runtime.InteropServices;
using System.Windows.Media.Media3D;

namespace GlassModel
{
    namespace Helpers
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

            public ksDocument3D ActiveDocument3D
            {
                get
                {
                    return _kompas.ActiveDocument3D();
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

            /// <summary>
            /// Инициализировать многогранник.
            /// </summary>
            /// <returns>Экземпляр многогранника.</returns>
            public ksRegularPolygonParam InitPolygon()
            {
                return _kompas.GetParamStruct(
                    (short)StructType2DEnum.ko_RegularPolygonParam);
            }

            /// <summary>
            /// Скругление по грани.
            /// <param name="part">Сборка детали.</param>
            /// <param name="numberFace">Сглаживаемая грань.</param>
            /// <param name="radius">Радиус сглаживания.</param>
            public void FilletedOnFace(
                ksPart part, object face, int radius)
            {
                var extrFillet = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_fillet);

                var filletDef = (ksFilletDefinition)extrFillet.GetDefinition();
                filletDef.radius = radius;
                //Не продолжать по касательным ребрам
                filletDef.tangent = false;

                var filletFaces = (ksEntityCollection)(filletDef.array());
                filletFaces.Clear();
                filletFaces.Add(face);

                extrFillet.Create();
            }

            /// <summary>
            /// Скругление по ребру.
            /// </summary>
            /// <param name="part">Сборка детали.</param>
            /// <param name="numberFace">Сглаживаемое ребро.</param>
            /// <param name="radius">Радиус сглаживания.</param>
            public void FilletedOnEdge(ksPart part, object edge,
                double radius)
            {
                var extrFillet = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_fillet);

                var filletDef = (ksFilletDefinition)extrFillet.GetDefinition();
                filletDef.radius = radius;
                //Не продолжать по касательным ребрам
                filletDef.tangent = false;

                var filletEdges = (ksEntityCollection)(filletDef.array());
                filletEdges.Clear();
                filletEdges.Add(edge);

                extrFillet.Create();
            }

            /// <summary>
            /// Находит первое ребро, пересекающиеся с точкой.
            /// </summary>
            /// <param name="part">Сборка детали</param>
            /// <param name="point">Точка пересечения.</param>
            /// <returns>Возвращает первое ребро,
            ///     пересекающиеся с точкой.</returns>
            public object FindIntersectionPointWithEdge(
                ksPart part, Point3D point)
            {
                var edges = (ksEntityCollection)part.EntityCollection(
                           (short)Obj3dType.o3d_edge);
                //отфильтровать ребра, проходящие через точку
                edges.SelectByPoint(point.X, point.Y, point.Z);

                return edges.First();
            }

            /// <summary>
            /// Находит первое грань, пересекающиеся с точкой.
            /// </summary>
            /// <param name="part">Сборка детали</param>
            /// <param name="point">Точка пересечения.</param>
            /// <returns>Возвращает первую грань,
            ///     пересекающуюся с точкой.</returns>
            public object FindIntersectionPointWithFace(
                ksPart part, Point3D point)
            {
                var faces = (ksEntityCollection)part.EntityCollection(
                           (short)Obj3dType.o3d_face);
                //отфильтровать грани, проходящие через точку
                faces.SelectByPoint(point.X, point.Y, point.Z);

                return faces.First();
            }
        }
    }
}