using Kompas6LTAPI5;
using Kompas6Constants3D;
using Kompas6Constants;
using System;

namespace GlassModel
{
    public class BuilderFacetedGlass : IBuilder
    {
        private IBuilder _builderBlank;

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


        public BuilderFacetedGlass()
        {
            _kompas = KompasWrapper.Instance;

            _builderBlank = new BuilderOfBlank();
        }

        public void Build(IGlass glass, IChecker checker)
        {
            _builderBlank.Build(glass, checker);

            _startX = glass.DiameterBottom / 2;
            _startY = glass.Height / 2;

            _glass = glass;
            _calcParams = new CalcParams(glass);

            var doc = _kompas.Document3D;
            doc.Create();

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);

            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);

            var offsetCutFacetedPlane = _kompas.CreateOffsetPlane(
                part, basePlane, _calcParams.OffsetFacetedPlane);

            var sketchCutFaceted = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_sketch);
            sketchCutFaceted.name = "Эскиз граней";

            var sketchDefCutFaceted =
                (ksSketchDefinition)sketchCutFaceted.GetDefinition();
            sketchDefCutFaceted.SetPlane(offsetCutFacetedPlane);

            sketchCutFaceted.Create();

            GenerateCutFaceted2d(sketchDefCutFaceted);
            GenerateCutFaceted3d(sketchCutFaceted, part);
        }

        private void GenerateCutFaceted2d(ksSketchDefinition sketchDef)
        {
            var draw = (ksDocument2D)sketchDef.BeginEdit();

            var polygon = (ksRegularPolygonParam)_kompas.GetParamStruct(
                (short)StructType2DEnum.ko_RegularPolygonParam);
            polygon.radius = _calcParams.DiameterFacetedStart / 2;
            polygon.ang = 0;
            polygon.style = 1;
            polygon.describe = false;
            polygon.xc = _startX;
            polygon.yc = _startY;
            polygon.count = _glass.CountFaceted;

            draw.ksRegularPolygon(polygon);
            draw.ksCircle(_startX, _startX,
                _calcParams.DiameterFacetedStart, 1);

            sketchDef.EndEdit();
        }

        private void GenerateCutFaceted3d(ksEntity sketch, ksPart part)
        {
            var extr = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_cutExtrusion);
            extr.name = "Вырезание граней";

            var extrDef = (ksCutExtrusionDefinition)extr.GetDefinition();
            extrDef.directionType = (short)Direction_Type.dtNormal;

            var angle = _glass.AngleHeight;
            var depthCut = _glass.HeightFaceted;
            var draftOutward = false; //вырезание угла направлено наружу

            extrDef.SetSideParam(true, (short)End_Type.etBlind,
                depthCut, angle, draftOutward);
            extrDef.SetSketch(sketch);

            extr.Create();
        }
    }
}
