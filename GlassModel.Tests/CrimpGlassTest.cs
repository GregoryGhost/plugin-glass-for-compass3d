using System;
using NUnit.Framework;
using GlassModel;

namespace GlassModel.Tests
{
    public class CrimpGlassTest
    {
        private CrimpGlass _crimpGlass;

        private const double _angle = 0.0f;
        private const double _heightFaceted = 0.0f;

        private const double _min = 10;
        private const double _max = 200;
        private const int _minCountFaceted = 20;
        private const int _maxCountFaceted = 60;

        private const double _percentForDepthBottom = 4;
        private const double _percentForHeightFaceted = 90;
        private const double _percentForDepthSide = 3;

        private const bool _valid = true;
        private const bool _invalid = false;

        [SetUp]
        public void Setup()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);
            var countFaceted = new BorderConditions<int>(_minCountFaceted,
                _minCountFaceted, _maxCountFaceted);

            _crimpGlass = new CrimpGlass(height, diameterBottom,
                countFaceted);
        }

        [Test(Description = "Check auto calc and fixed params" +
            "of crimp glass - " +
                "height, diameter bottom, angleHeight, depth side, " +
                    "depth bottom, height faceted, count faceted")]
        [TestCase(new[] { false, false, true, true, true, true, false },
            TestName = "Check auto calc params")]
        public void CheckAutoCalcParams(bool[] exp)
        {
            CleanGlassTest.CheckAutoCalcParamsOfGlass(exp,
                _crimpGlass);
        }

        [Test(Description = "Check fixed params of crimp glass -" +
            "angle height, count faceted, height faceted")]
        [TestCase(_angle, _heightFaceted, _percentForDepthBottom,
            _percentForDepthSide,
                TestName = "Fixed params of clean glass")]
        public void CheckFixedParams(double angle, double hFaceted,
            double depthBottom, double depthSide)
        {
            _crimpGlass.AngleHeight = angle + 10;
            _crimpGlass.DepthBottom = depthBottom + 10;
            _crimpGlass.DepthSide = depthSide + 10;
            _crimpGlass.HeightFaceted = hFaceted + 10;

            var expDepthSide = CleanGlassTest.CalcDepthOfGlass(
                _crimpGlass.DiameterBottom, _percentForDepthSide);
            var expDepthBottom = CleanGlassTest.CalcDepthOfGlass(
                _crimpGlass.Height, _percentForDepthBottom);
            var expHeightFaceted = CleanGlassTest.CalcDepthOfGlass(
                _crimpGlass.Height, _percentForHeightFaceted);

            Assert.That(angle, Is.EqualTo(_crimpGlass.AngleHeight));
            Assert.That(expHeightFaceted,
                Is.EqualTo(_crimpGlass.HeightFaceted));
            Assert.That(expDepthSide, Is.EqualTo(_crimpGlass.DepthSide));
            Assert.That(expDepthBottom, Is.EqualTo(_crimpGlass.DepthBottom));
        }
    }
}
