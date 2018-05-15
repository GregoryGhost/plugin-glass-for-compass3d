using Kompas6LTAPI5;
using Kompas6Constants3D;
using System;
using System.Collections.Generic;

namespace GlassModel
{
    /// <summary>
    /// Ребра болванки стакана.
    /// </summary>
    public enum EdgesBlankGlass
    {
        /// <summary>
        /// Внешняя ребро дна стакана.
        /// </summary>
        Bottom = 0,
        /// <summary>
        /// Внешнее ребро горлышка стакана.
        /// </summary>
        TopOut,
        /// <summary>
        /// Внутреннее ребро горлышка стакана.
        /// </summary>
        TopIn
    }


    /// <summary>
    /// Мастер по созданию болванок стакана в САПР Компас 3D
    /// </summary>
    public class BuilderOfBlank : IBuilder
    {
        /// <summary>
        /// Начальная координата по OX отрисовки для эскиза
        /// </summary>
        private double _startX;

        /// <summary>
        /// Начальная координата по OY отрисовки для эскиза
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
        /// Инициализация необходимых параметров для работы с Компас 3D
        /// </summary>
        public BuilderOfBlank()
        {
            _kompas = KompasWrapper.Instance;
            _kompas.ShowCAD();

            _startX = 0;
            _startY = _startX;
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
                var start = (int)EdgesBlankGlass.Bottom;
                var end = (int)EdgesBlankGlass.TopIn;

                var rTopOut = _calcParams.RadiusTopFilleted;
                var rTopIn = rTopOut;
                var rBottom = _calcParams.RadiusBottomFilleted;

                var radiuses = new List<double> 
                { 
                    rBottom,
                    rTopOut,
                    rTopIn
                };
                var j = 0;
                for (var i = start; i <= end; i++)
                {
                    FilletedBottomAndTopOnEdges(
                        part, i, radiuses[j]);
                    j++;
                }
            }
        }

        /// <summary>
        /// Скругление дна и горлышка стакана по граням.
        /// </summary>
        /// <param name="part">Сборка детали.</param>
        /// <param name="numberFace">Номер грани в детали.</param>
        /// <param name="radius">Радиус сглаживания.</param>
        private void FilletedBottomAndTopOnFaces(ksPart part,
            int numberFace, double radius)
        {
            var extrFillet = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_fillet);
            extrFillet.name = String.Format("Скругление №{0}", numberFace);

            var filletDef = (ksFilletDefinition)extrFillet.GetDefinition();
            filletDef.radius = radius;
            //Не продолжать по касательным ребрам
            filletDef.tangent = false;

            var facesGlass = (ksEntityCollection)part.EntityCollection(
                (short)Obj3dType.o3d_face);

            var filletFaces = (ksEntityCollection)(filletDef.array());
            filletFaces.Clear();
            filletFaces.Add(facesGlass.GetByIndex(numberFace));

            extrFillet.Create();
        }

        /// <summary>
        /// Скругление дна и горлышка стакана по ребрам.
        /// </summary>
        /// <param name="part">Сборка детали.</param>
        /// <param name="numberEdge">Номер ребра в детали.</param>
        /// <param name="radius">Радиус сглаживания.</param>
        private void FilletedBottomAndTopOnEdges(ksPart part, int numberEdge,
            double radius)
        {
            var extrFillet = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_fillet);
            extrFillet.name = String.Format("Скругление №{0}", numberEdge);

            var filletDef = (ksFilletDefinition)extrFillet.GetDefinition();
            filletDef.radius = radius;
            //Не продолжать по касательным ребрам
            filletDef.tangent = false;

            var edgesGlass = (ksEntityCollection)part.EntityCollection(
                (short)Obj3dType.o3d_edge);

            var filletEdges = (ksEntityCollection)(filletDef.array());
            filletEdges.Clear();
            filletEdges.Add(edgesGlass.GetByIndex(numberEdge));

            extrFillet.Create();
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

    /// <summary>
    /// Вычисляемые параметры стакана, необходимые для
    ///     работы алгоритмов построения стакана.
    /// </summary>
    public class CalcParams
    {
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
        private double _radiusTopFilleted;
        private double _radiusBottomFilleted;

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

            var diameterTop = 2 * glass.Height * tanRad
                + glass.DiameterBottom;
            _diameterSideCutting = diameterTop *
                (100 - glass.DepthSide) / 100;

            var diameterCutSide = _diameterFacetedStart *
                (100 - glass.DepthSide / 1.5) / 100;
            _diameterStripsCrimp = _diameterFacetedStart
                - diameterCutSide;

            _heightCutting = glass.Height *
                (100 - glass.DepthBottom) / 100;
            
            _radiusTopFilleted = diameterTop *
                (1 - (100 - glass.DepthSide / 2) / 100);

            _radiusBottomFilleted = glass.DiameterBottom / 50;
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

        public double RadiusTopFilleted
        {
            get
            {
                return _radiusTopFilleted;
            }
        }

        public double RadiusBottomFilleted
        {
            get
            {
                return _radiusBottomFilleted;
            }
        }
    }


}
