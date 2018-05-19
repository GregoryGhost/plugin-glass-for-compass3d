using Kompas6LTAPI5;
using Kompas6Constants3D;
using Kompas6Constants;
using System;

namespace GlassModel
{
    /// <summary>
    /// Построитель гофрированных стаканов.
    /// </summary>
    public class BuilderCrimpGlass : IBuilder
    {
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
        /// Построитель болванок стакана.
        /// </summary>
        private IBuilder _builderBlank;

        /// <summary>
        /// Установление связи с Компас 3D.
        /// </summary>
        public BuilderCrimpGlass()
        {
            _kompas = KompasWrapper.Instance;

            _builderBlank = new BuilderOfBlank();
        }

        /// <summary>
        /// Построить гофрированный стакан.
        /// </summary>
        /// <param name="glass">Гофрированный стакан.</param>
        /// <param name="checker">Проверяющий параметры стакана
        ///     в соответствие с требованиям предметной области.</param>
        public void Build(IGlass glass, IChecker checker)
        {
            _builderBlank.Build(glass, checker);

            _glass = glass;
            _calcParams = new CalcParams(glass);

            var doc = _kompas.ActiveDocument3D;

            var part = (ksPart)doc.GetPart(
                (short)Part_Type.pTop_Part);

            var basePlane = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_planeXOY);

            var offsetCrimpPlane = _kompas.CreateOffsetPlane(
                part, basePlane, _calcParams.OffsetFacetedPlane);

            var sketchCrimp = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_sketch);
            sketchCrimp.name = "Эскиз рефлёностей";

            var sketchDefCrimp =
                (ksSketchDefinition)sketchCrimp.GetDefinition();
            sketchDefCrimp.SetPlane(offsetCrimpPlane);

            sketchCrimp.Create();

            GenerateExtrusionCrimp2d(sketchDefCrimp);
            GenerateExtrusionCrimp3d(sketchCrimp, part);
        }

        /// <summary>
        /// Сгенерировать рефренности(полоски)
        ///     гофрированного стакана.
        /// </summary>
        /// <param name="sketch">Эскиз рефленностей стакана.</param>
        /// <param name="part">Сборка детали.</param>
        private void GenerateExtrusionCrimp3d(ksEntity sketch,
            ksPart part)
        {
            var extrConicSpiral = CreateConicSpiral(
                sketch, part);

            var extrKin = CreateKinematicOperation(
                sketch, part, extrConicSpiral);

            CopyByCircularGrid(part, extrKin);
        }

        /// <summary>
        /// Копировать полоски(узор) по концетрической сетке.
        /// </summary>
        /// <param name="part">Сборка детали.</param>
        /// <param name="extrKin">Эскиз полосок
        ///     гофрированного стакана.</param>
        private void CopyByCircularGrid(ksPart part, ksEntity extrKin)
        {
            var extrCirc = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_circularCopy);
            extrCirc.name = "Копирование элементов" +
                "по концентрической сетке";

            var axisOZ = (ksEntity)part.GetDefaultEntity(
                (short)Obj3dType.o3d_axisOZ);

            var extrDefCirc =
                (ksCircularCopyDefinition)extrCirc.GetDefinition();
            extrDefCirc.SetAxis(axisOZ);
            //количество полосок
            extrDefCirc.count2 = _glass.CountFaceted;
            extrDefCirc.inverce = false;
            extrDefCirc.geomArray = false;
            //полностью вокруг стенки стакана
            extrDefCirc.step2 = 360;

            var copyStrips =
                (ksEntityCollection)extrDefCirc.GetOperationArray();
            copyStrips.Clear();
            copyStrips.Add(extrKin);

            extrCirc.Create();
        }

        /// <summary>
        /// Создать с помощью кинематической операции
        ///     полоску гофрированного стакана.
        /// </summary>
        /// <param name="sketch">Эскиз рефленности стакана.</param>
        /// <param name="part">Сборка детали.</param>
        /// <param name="extrConicSpiral">Коническая спираль
        ///     для создания полоски стакана.</param>
        /// <returns>Модель полоски(узора)
        ///     для гофрированного стакана.</returns>
        private ksEntity CreateKinematicOperation(
            ksEntity sketch, ksPart part, ksEntity extrConicSpiral)
        {
            var extrKin = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_baseEvolution);
            extrKin.name = "Кинематическая операция для полоски";

            var extrDefKin =
                (ksBaseEvolutionDefinition)extrKin.GetDefinition();
            //образующая при переносе сохраняет 
            //  исходный угол с направляющей
            extrDefKin.sketchShiftType = 1;
            //установка эскиза сечения
            extrDefKin.SetSketch(sketch);

            var strips =
                (ksEntityCollection)extrDefKin.PathPartArray();
            strips.Clear();
            //добавить в массив эскиз
            //  с траекторией(конической спиралью)
            strips.Add(extrConicSpiral);

            extrKin.Create();
            return extrKin;
        }

        /// <summary>
        /// Создать коническую спираль.
        /// </summary>
        /// <param name="sketch">Эскиз рефленности стакана.</param>
        /// <param name="part">Сборка детали.</param>
        /// <returns>Коническая спираль для создания полоски стакана.</returns>
        private ksEntity CreateConicSpiral(ksEntity sketch,
            ksPart part)
        {
            var extrConicSpiral = (ksEntity)part.NewEntity(
                (short)Obj3dType.o3d_conicSpiral);
            extrConicSpiral.name = "Коническая спираль для полоски";

            var extrDef =
                (ksConicSpiralDefinition)extrConicSpiral.GetDefinition();
            //построение по шагу витков и высоте
            extrDef.buildMode = 1;
            extrDef.buildDir = false;
            //способ задания конечного диаметра: 
            //  по углу наклона образующей
            extrDef.terminalDiamType = 2;
            extrDef.step = 459.509;
            extrDef.heightType = 0;
            extrDef.height = _glass.HeightFaceted;
            extrDef.initialDiam = _calcParams.DiameterFacetedStart;
            //наклон образующей внутрь
            extrDef.tiltAngleHow = true;
            extrDef.tiltAngle = 5;
            extrDef.firstAngle = 25.0;
            extrDef.SetPlane(sketch);

            extrConicSpiral.Create();
            return extrConicSpiral;
        }

        /// <summary>
        /// Создать полоску на эскизе стакана.
        /// </summary>
        /// <param name="sketchDef">Описание эскиза стакана.</param>
        private void GenerateExtrusionCrimp2d(
            ksSketchDefinition sketchDef)
        {
            var draw = (ksDocument2D)sketchDef.BeginEdit();

            var radiusLineCrim = _calcParams.DiameterStripsCrimp / 2;
            var xc = _calcParams.DiameterFacetedStart * 0.7 / 2;

            draw.ksCircle(xc, xc, radiusLineCrim, 1);

            sketchDef.EndEdit();
        }
    }
}
