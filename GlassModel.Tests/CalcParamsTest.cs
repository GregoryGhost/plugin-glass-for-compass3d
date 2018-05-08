using System;
using GlassModel;
using NUnit.Framework;
using System.Collections;

namespace GlassModel.Tests
{
    [TestFixture]
    public class CalcParamsTest
    {
        [Test(Description = "Checking calculate params of glass" +
            " used in the building glass in concrete algorithms"),
                TestCaseSource("GlassesCases")]
        public void CheckCalculateParams(IGlass glass)
        {
            var actual = new CalcParams(glass);
            var exp = new CalcParamsFake(glass);

            Assert.That(exp.DiameterFacetedStart,
                Is.EqualTo(actual.DiameterFacetedStart),
                "DiameterFacetedStart");
            Assert.That(exp.DiameterSideCutting,
                Is.EqualTo(actual.DiameterSideCutting),
                "DiameterSideCutting");
            Assert.That(exp.DiameterStripsCrimp,
                Is.EqualTo(actual.DiameterStripsCrimp),
                "DiameterStripsCrimp");
            Assert.That(exp.HeightCutting,
                Is.EqualTo(actual.HeightCutting),
                "HeightCutting");
            Assert.That(exp.OffsetFacetedPlane,
                Is.EqualTo(actual.OffsetFacetedPlane),
                "OffsetFacetedPlane");
        }

        private const double _min = 10;
        private const double _max = 200;
        private const double _minAngle = 0;
        private const double _maxAngle = 5;
        private const double _minDepthForFacetedGlass = 3;
        private const double _maxDepthSide = 5;
        private const double _maxDepthBottom = 7;
        private const int _minCountFaceted = 8;
        private const int _maxCountFaceted = 20;
        private const int _minCountStrips = 20;
        private const int _maxCountStrips = 60;

        static object[] GlassesCases()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);
            var angleHeight = new BorderConditions<double>(_minAngle,
                _minAngle, _maxAngle);
            var depthSideForFacetedGlass = new BorderConditions<double>(
                _minDepthForFacetedGlass, _minDepthForFacetedGlass,
                    _maxDepthSide);
            var depthBottom = new BorderConditions<double>(
                _minDepthForFacetedGlass, _minDepthForFacetedGlass,
                    _maxDepthBottom);
            var countFaceted = new BorderConditions<int>(
                _minCountFaceted, _minCountFaceted, _maxCountFaceted);

            var facetedGlass = new FacetedGlass(height, diameterBottom,
                angleHeight, depthSideForFacetedGlass, depthBottom,
                    countFaceted);

            var cleanGlass = new CleanGlass(diameterBottom, height);

            countFaceted = new BorderConditions<int>(_minCountStrips,
                _minCountStrips, _maxCountStrips);
            var crimpGlass = new CrimpGlass(height,
                diameterBottom, countFaceted);

            var glasses = new object[]{
                cleanGlass,
                facetedGlass,
                crimpGlass
            };
            return glasses;
        }
    }

    public class CalcParamsFake
    {
        private double _diameterFacetedStart;
        private double _diameterSideCutting;
        private double _heightCutting;
        private double _offsetFacetedPlane;
        private double _diameterStripsCrimp;

        public CalcParamsFake(IGlass glass)
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
