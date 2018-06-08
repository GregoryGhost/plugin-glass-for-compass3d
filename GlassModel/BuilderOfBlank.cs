using Kompas6LTAPI5;
using Kompas6Constants3D;

using System;
using System.Windows.Media.Media3D;

using GlassModel.Glasses;
using GlassModel.Helpers;

namespace GlassModel
{
    namespace Builders
    {
        /// <summary>
        /// Мастер по созданию болванок стакана в САПР Компас 3D.
        /// </summary>
        public class BuilderOfBlank : IBuilder
        {
            /// <summary>
            /// Начальная координата по OX отрисовки для эскиза.
            /// </summary>
            private double _startX;

            /// <summary>
            /// Начальная координата по OY отрисовки для эскиза.
            /// </summary>
            private double _startY;

            /// <summary>
            /// Обертка над САПР Компас 3D.
            /// </summary>
            private KompasWrapper _kompas;

            /// <summary>
            /// Шаблон стакана для построения.
            /// </summary>
            private IGlass _glass;

            /// <summary>
            /// Посчитанные необходимые параметры
            ///     для построения стакана в САПР Компас 3D.
            /// </summary>
            private CalcParams _calcParams;

            /// <summary>
            /// Инициализация необходимых параметров для работы с Компас 3D.
            /// </summary>
            public BuilderOfBlank()
            {
                _kompas = KompasWrapper.Instance;
                _kompas.ShowCAD();

                _startX = 0;
                _startY = _startX;
            }

            /// <summary>
            /// Построить модель стакана в САПР Компас 3D.
            /// </summary>
            /// <param name="photoFrame">Шаблон стакана.</param>
            /// <exception cref="InvalidOperationException">
            ///     Вызывается тогда, когда параметры стакана
            ///     имеют недопустимые значения.</exception>
            public void Build(IGlass glass)
            {
                if (glass.IsValid == false)
                {
                    var msg = String.Format("Шаблон стакана имеет" +
                        " недопустимые параметры для построения.");
                    throw new InvalidOperationException(msg);
                }

                _kompas.ShowCAD();

                _startX = 0;
                _startY = 0;

                _glass = glass;
                _calcParams = new CalcParams(glass);

                var doc = _kompas.Document3D;
                doc.Create();

                var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);

                var sketchBase = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_sketch);
                sketchBase.name = "Эскиз основания";

                var basePlane = (ksEntity)part.GetDefaultEntity(
                    (short)Obj3dType.o3d_planeXOY);

                var sketchDefBase =
                    (ksSketchDefinition)sketchBase.GetDefinition();
                sketchDefBase.SetPlane(basePlane);

                sketchBase.Create();

                var sketchCutSide = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_sketch);
                sketchCutSide.name = "Эскиз вырезанных внутренностей";

                var offsetCutPlane = _kompas.CreateOffsetPlane(
                    part, basePlane, glass.Height);

                var sketchDefCutSide =
                    (ksSketchDefinition)sketchCutSide.GetDefinition();
                sketchDefCutSide.SetPlane(offsetCutPlane);

                sketchCutSide.Create();

                GenerateBlank2d(sketchDefBase);
                GenerateBlank3d(sketchBase, part);
                GenerateCutSide2d(sketchDefCutSide);
                GenerateCutSide3d(sketchCutSide, part);

                if (glass.Filleted)
                {
                    FilletedBottomAndTop(glass, part);
                }
            }

            /// <summary>
            /// Сгладить дно и горлышко болванки стакана.
            /// </summary>
            /// <param name="glass">Параметры стакана.</param>
            /// <param name="part">Сборка детали.</param>
            private void FilletedBottomAndTop(IGlass glass, ksPart part)
            {
                //дно стакана
                var rBottom = glass.DiameterBottom / 2;
                var pointBottom = new Point3D(_startX, rBottom, 0);
                var edge0 = _kompas.FindIntersectionPointWithEdge(
                    part, pointBottom);
                _kompas.FilletedOnEdge(part, edge0,
                    _calcParams.RadiusBottomFilleted);

                //с внешней стороны диаметр горлышка
                var pointTop = new Point3D(_startX,
                    _calcParams.DiameterTop / 2, glass.Height);
                var edge = _kompas.FindIntersectionPointWithEdge(
                    part, pointTop);
                _kompas.FilletedOnEdge(part, edge,
                    _calcParams.RadiusTopFilleted);

                //с внутренней стороны диаметр горлышка
                //  (вырезаемых внутренностей)
                var pointTop2 = new Point3D(_startX,
                    _calcParams.DiameterSideCutting / 2, glass.Height);
                var edge2 = _kompas.FindIntersectionPointWithEdge(
                    part, pointTop2);
                _kompas.FilletedOnEdge(part, edge2,
                    _calcParams.RadiusTopFilleted);
            }

            /// <summary>
            /// Генерация эскиза основания стакана.
            /// </summary>
            /// <param name="sketchDef">
            ///     Описание эскиза основания стакана.</param>
            private void GenerateBlank2d(ksSketchDefinition sketchDef)
            {
                var draw = (ksDocument2D)sketchDef.BeginEdit();

                draw.ksCircle(_startX, _startY,
                    _glass.DiameterBottom / 2.0, 1);

                sketchDef.EndEdit();
            }

            /// <summary>
            /// Генерация модели стакана.
            /// </summary>
            /// <param name="sketch">Эскиз основания стакана.</param>
            /// <param name="part">Сборка детали.</param>
            private void GenerateBlank3d(ksEntity sketch, ksPart part)
            {
                var extr = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_bossExtrusion);
                extr.name = "Выдавливание основания";

                var extrDef =
                    (ksBossExtrusionDefinition)extr.GetDefinition();
                extrDef.directionType =
                    (short)Direction_Type.dtNormal;

                extrDef.SetSideParam(true,
                    (short)End_Type.etBlind,
                    _glass.Height,
                    _glass.AngleHeight,
                    false);
                extrDef.SetSketch(sketch);

                extr.Create();
            }

            /// <summary>
            /// Генерация эскиза вырезанных внутренностей стакана.
            /// </summary>
            /// <param name="sketchDef">Описание эскиза вырезаемых
            ///     внутренностей стакана.</param>
            private void GenerateCutSide2d(ksSketchDefinition sketchDef)
            {
                var draw =
                    (ksDocument2D)sketchDef.BeginEdit();

                draw.ksCircle(_startX, _startY,
                    _calcParams.DiameterSideCutting / 2, 1);

                sketchDef.EndEdit();
            }

            /// <summary>
            /// Вырезание внутренностей стакана из болванки стакана.
            /// </summary>
            /// <param name="sketch">Эскиз вырезаемых внутренностей.</param>
            /// <param name="part">Сборка детали.</param>
            private void GenerateCutSide3d(ksEntity sketch, ksPart part)
            {
                var extr = (ksEntity)part.NewEntity(
                    (short)Obj3dType.o3d_cutExtrusion);
                extr.name = "Вырезание внутренностей";

                var extrDef =
                    (ksCutExtrusionDefinition)extr.GetDefinition();
                extrDef.directionType = (short)Direction_Type.dtNormal;

                var angle = _glass.AngleHeight;
                var depthCut = _calcParams.HeightCutting;
                var draftOutward = true;//вырезание угла направлено внутрь

                extrDef.SetSideParam(true, (short)End_Type.etBlind,
                    depthCut, angle, draftOutward);
                extrDef.SetSketch(sketch);

                extr.Create();
            }
        }
    }


    namespace Helpers
    {
        /// <summary>
        /// Вычисляемые параметры стакана, необходимые для
        ///     работы алгоритмов построения стакана.
        /// </summary>
        public class CalcParams
        {
            /// <summary>
            /// Процент сглаживания для дна стакана.
            /// </summary>
            private readonly double _percentFilletedBottom = 90;

            /// <summary>
            /// Диаметр начала отрисовки граней.
            /// </summary>
            private double _diameterFacetedStart;

            /// <summary>
            /// Диаметр вырезаемых внутренностей стакана.
            /// </summary>
            private double _diameterSideCutting;

            /// <summary>
            /// Высота вырезаемых внутренностей стакана.
            /// </summary>
            private double _heightCutting;

            /// <summary>
            /// Смещенная плоскость отрисовки узора стакана.
            /// </summary>
            private double _offsetFacetedPlane;

            /// <summary>
            /// Диаметр полоски, используется для построения граней
            ///     гофрированного стакана.
            /// </summary>
            private double _diameterStripsCrimp;

            /// <summary>
            /// Инициализация параметров для построения стакана.
            /// </summary>
            /// <param name="glass">Целевой стакан.</param>
            public CalcParams(IGlass glass)
            {
                _offsetFacetedPlane =
                    glass.Height / 2 + glass.HeightFaceted / 2;

                var angleRad = glass.AngleHeight * System.Math.PI / 180;
                var tanRad = System.Math.Tan(angleRad);

                _diameterFacetedStart = 2 * _offsetFacetedPlane * tanRad
                    + glass.DiameterBottom;

                DiameterTop = 2 * glass.Height * tanRad
                    + glass.DiameterBottom;
                _diameterSideCutting = DiameterTop *
                    (100 - glass.DepthSide) / 100;

                var diameterCutSide = _diameterFacetedStart *
                    (100 - glass.DepthSide / 1.5) / 100;
                _diameterStripsCrimp = _diameterFacetedStart
                    - diameterCutSide;

                _heightCutting = glass.Height *
                    (100 - glass.DepthBottom) / 100;

                var filletedTop = DiameterTop *
                    (1 - (100 - glass.DepthSide / 2) / 100);
                //Так как сглаживание горылшка происходит с двух сторон,
                //  то делим толщину стенки на два
                RadiusTopFilleted = filletedTop / 2;

                var delta = _offsetFacetedPlane;
                if (glass.HeightFaceted == 0)
                {
                    delta = glass.Height * _percentFilletedBottom / 100;
                }
                RadiusBottomFilleted = (glass.Height - delta);
            }

            /// <summary>
            /// Диаметр грани, 
            ///     от которой начинается отрисовка граней стакана.
            /// </summary>
            public double DiameterFacetedStart
            {
                get
                {
                    return _diameterFacetedStart;
                }
            }

            /// <summary>
            /// Диаметр вырезаемых внутренностей стакана.
            /// </summary>
            public double DiameterSideCutting
            {
                get
                {
                    return _diameterSideCutting;
                }
            }

            /// <summary>
            /// Высота вырезания внутренностей стакана.
            /// </summary>
            public double HeightCutting
            {
                get
                {
                    return _heightCutting;
                }
            }

            /// <summary>
            /// Плоскость отрисок граней стакана.
            /// </summary>
            public double OffsetFacetedPlane
            {
                get
                {
                    return _offsetFacetedPlane;
                }
            }

            /// <summary>
            /// Диаметр полосок - граней гофрированного стакана.
            /// </summary>
            public double DiameterStripsCrimp
            {
                get
                {
                    return _diameterStripsCrimp;
                }
            }

            /// <summary>
            /// Радиус сглаживания горлышка стакана.
            /// </summary>
            public double RadiusTopFilleted
            {
                get;
                private set;
            }

            /// <summary>
            /// Радиус сглаживания дна стакана.
            /// </summary>
            public double RadiusBottomFilleted
            {
                get;
                private set;
            }

            /// <summary>
            /// Диаметр горлышка стакана.
            /// </summary>
            public double DiameterTop { get; private set; }
        }
    }
}
