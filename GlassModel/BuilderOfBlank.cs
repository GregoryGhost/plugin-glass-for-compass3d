using Kompas6LTAPI5;
using Kompas6Constants3D;
using System;

namespace GlassModel
{
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
        }

        private void GenerateBlank2d(ksSketchDefinition sketchDef)
        {
            var draw = (ksDocument2D)sketchDef.BeginEdit();

            draw.ksCircle(_startX, _startY,
                _glass.DiameterBottom / 2.0, 1);

            sketchDef.EndEdit();
        }

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

        private void GenerateCutSide2d(ksSketchDefinition sketchDef)
        {
            var draw =
                (ksDocument2D)sketchDef.BeginEdit();

            draw.ksCircle(_startX, _startY,
                _calcParams.DiameterSideCutting / 2, 1);

            sketchDef.EndEdit();
        }

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


    public class CalcParams
    {
        private double _diameterFacetedStart;
        private double _diameterSideCutting;
        private double _heightCutting;
        private double _offsetFacetedPlane;
        private double _diameterStripsCrimp;

        public CalcParams(IGlass glass)
        {
            _offsetFacetedPlane = 
                glass.Height / 2 + glass.HeightFaceted / 2;

            var angleRad = glass.AngleHeight * System.Math.PI
                / 180;

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
        }

        public double DiameterFacetedStart
        {
            get
            {
                return _diameterFacetedStart;
            }
        }

        public double DiameterSideCutting
        {
            get
            {
                return _diameterSideCutting;
            }
        }

        public double HeightCutting
        {
            get
            {
                return _heightCutting;
            }
        }

        public double OffsetFacetedPlane
        {
            get
            {
                return _offsetFacetedPlane;
            }
        }

        public double DiameterStripsCrimp
        {
            get
            {
                return _diameterStripsCrimp;
            }
        }
    }


}
