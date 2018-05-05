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

        private KompasWrapper _kompas;

        /// <summary>
        /// Инициализация необходимых параметров для работы с Компас 3D
        /// </summary>
        public BuilderOfBlank()
        {
            _kompas = KompasWrapper.Instance;
            _kompas.ShowCAD();
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
            _startY = _startX;

            var doc = _kompas.Document3D;
            doc.Create();

            var part = (ksPart)doc.GetPart((short)Part_Type.pTop_Part);
            if (part == null)
            {
                return;
            }

            var sketchBase = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_sketch);
            sketchBase.name = "Эскиз основания";

            var sketchDefBase =
                (ksSketchDefinition)sketchBase.GetDefinition();
            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);
            sketchDefBase.SetPlane(basePlane);
            sketchBase.Create();

            var params1 = new CalcParams(glass);

            var sketchCutSide =
                (ksEntity)part.NewEntity((short)Obj3dType.o3d_sketch);
            sketchCutSide.name = "Эскиз вырезанных внутренностей";

            var offsetCutPlane = 
                CreateOffsetPlane(part, basePlane, glass.Height);

            var sketchDefCutSide =
                (ksSketchDefinition)sketchCutSide.GetDefinition();
            sketchDefCutSide.SetPlane(offsetCutPlane);

            sketchCutSide.Create();

            GenerateBlank2d(sketchDefBase, glass);
            GenerateBlank3d(sketchBase, part, glass, params1);
            GenerateCutSide2d(sketchDefCutSide, params1, glass);
            GenerateCutSide3d(sketchCutSide, part, glass, params1);
        }

        /// <summary>
        /// Создание смещенной плоскости
        /// </summary>
        /// <param name="part">Компонент сборки</param>
        /// <param name="basePlane">Исходная плоскость</param>
        /// <param name="offset">Смещение</param>
        /// <returns>Возвращает смещенную плоскость</returns>
        private ksEntity CreateOffsetPlane(ksPart part, ksEntity basePlane,
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

        private void GenerateBlank2d(ksSketchDefinition sketchDef, IGlass glass)
        {
            var hX = glass.DiameterBottom / 2;
            var hY = glass.Height / 2;

            var draw = (ksDocument2D)sketchDef.BeginEdit();

            draw.ksCircle(hX, hY, glass.DiameterBottom / 2.0, 1);

            sketchDef.EndEdit();
        }
        
        private void GenerateBlank3d(ksEntity sketch, ksPart part,
            IGlass glass, CalcParams params1)
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
                glass.Height,
                glass.AngleHeight,
                false);
            extrDef.SetSketch(sketch);

            extr.Create();
        }

        private void GenerateCutSide2d(ksSketchDefinition sketchDef,
            CalcParams calcParams, IGlass glass)
        {
            var hX = glass.DiameterBottom / 2;
            var hY = glass.Height / 2;

            var draw =
                (ksDocument2D)sketchDef.BeginEdit();

            draw.ksCircle(hX, hY,
                calcParams.DiameterSideCutting / 2, 1);

            sketchDef.EndEdit();
        }

        private void GenerateCutSide3d(ksEntity sketch, ksPart part,
            IGlass glass, CalcParams calcParams)
        {
            var extr = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_cutExtrusion);
            extr.name = "Вырезание внутренностей";

            var extrDef =
                (ksCutExtrusionDefinition)extr.GetDefinition();
            extrDef.directionType = (short)Direction_Type.dtNormal;

            var angle = glass.AngleHeight;
            var depthCut = calcParams.HeightCutting;

            extrDef.SetSideParam(true, (short)End_Type.etBlind,
                depthCut, angle, 
                    true);//вырезание угла направлено внутрь
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

        public CalcParams(IGlass glass)
        {
            _offsetFacetedPlane = glass.Height * 0.9;

            var angleRad = glass.AngleHeight * System.Math.PI
                / 180;

            var tanRad = System.Math.Tan(angleRad);

            _diameterFacetedStart = 2 * _offsetFacetedPlane * tanRad
                + glass.DiameterBottom;

            _diameterSideCutting = glass.DiameterBottom * 
                (100 - glass.DepthSide) / 100;

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
    }


}
