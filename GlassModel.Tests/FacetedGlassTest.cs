﻿using GlassModel.Glasses;

using NUnit.Framework;

using System;

namespace GlassModel.Tests
{
    [TestFixture]
    public class FacetedGlassTest
    {
        private FacetedGlass _facetedGlass;

        private const double _angle = 0.0f;
        private const int _countFaceted = 0;

        private const double _min = 10;
        private const double _max = 200;
        private const double _minAngle = 0;
        private const double _maxAngle = 5;
        private const double _minDepth = 1;
        private const double _maxDepthSide = 5;
        private const double _maxDepthBottom = 7;
        private const int _minCountFaceted = 4;
        private const int _maxCountFaceted = 20;

        private const double _percentForDepthBottom = 7;
        private const double _percentForHeightFaceted = 90;
        private const double _percentForDepthSide = 4;

        private const bool _valid = true;
        private const bool _invalid = false;

        private const bool _filleted = true;

        [SetUp]
        public void Setup()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);
            var angleHeight = new BorderConditions<double>(_minAngle,
                _minAngle, _maxAngle);
            var depthSide = new BorderConditions<double>(_minDepth,
                _minDepth, _maxDepthSide);
            var depthBottom = new BorderConditions<double>(_minDepth,
                _minDepth, _maxDepthBottom);
            var countFaceted = new BorderConditions<int>(
                _minCountFaceted, _minCountFaceted, _maxCountFaceted);

            _facetedGlass = new FacetedGlass(height, diameterBottom,
                angleHeight, depthSide, depthBottom, countFaceted);
        }

        [Test(Description = "Check dependencies params of faceted glass" +
            " - height faceted")]
        [TestCase(_min, TestName = "Depend params - Height glass = min")]
        [TestCase(_max, TestName = "Depend params - Height glass = max")]
        [TestCase((_min + _max) / 2,
            TestName = "Depend params - Height glass = average")]
        public void CheckDependenciesParams(double height)
        {
            _facetedGlass.Height = height;

            var expHeightFaceted = height * _percentForHeightFaceted / 100;

            Assert.AreEqual(expHeightFaceted,
                _facetedGlass.HeightFaceted);
        }

        [Test(Description = "Check setted params of faceted glass -" +
            "height, diameter bottom, angle height," +
            "depth side and bottom, also count faceted - correct data")]
        [TestCase(_min, _min / 2, _minAngle, _minDepth,
            _minDepth, _minCountFaceted, _valid,
                TestName = "Setted - All params = min")]
        [TestCase(_max, _max / 2, _maxAngle, _maxDepthSide,
            _maxDepthBottom, _maxCountFaceted, _valid,
                TestName = "Setted - All params = max")]
        [TestCase((_min + _max) / 2, (_min / 2 + _max / 2) / 2, _minAngle,
            _maxDepthSide, _maxDepthBottom, _maxCountFaceted, _valid,
                TestName = "Setted - Height glass and diameter bottom" +
                    "in the allowable range")]
        [TestCase(_max, _min / 2, _minAngle, _maxDepthSide,
            _maxDepthBottom, _maxCountFaceted, _valid,
                TestName = "Setted - Height glass >= diameter bottom,")]
        public void CheckSettedParamsPositive(double height,
            double diameterBottom, double angleHeight, double depthSide,
                double depthBottom, int countFaceted, bool expIsValid)
        {
            _facetedGlass.Height = height;
            _facetedGlass.DiameterBottom = diameterBottom;
            //height, diameter bottom
            Assert.That(height, Is.EqualTo(_facetedGlass.Height));
            Assert.That(diameterBottom,
                Is.EqualTo(_facetedGlass.DiameterBottom));
            Assert.That(
                _facetedGlass.Height >= _facetedGlass.DiameterBottom);

            _facetedGlass.AngleHeight = angleHeight;
            _facetedGlass.DepthSide = depthSide;
            _facetedGlass.DepthBottom = depthBottom;
            _facetedGlass.CountFaceted = countFaceted;

            var expPercentDepthSide = depthSide;
            var expPercentForDepthBottom = depthBottom;

            //angleHeight, depthSide, depthBottom, countFaceted
            Assert.That(angleHeight,
                Is.EqualTo(_facetedGlass.AngleHeight));
            Assert.That(expPercentDepthSide,
                Is.EqualTo(_facetedGlass.DepthSide));
            Assert.That(expPercentForDepthBottom,
                Is.EqualTo(_facetedGlass.DepthBottom));
            Assert.That(countFaceted,
                Is.EqualTo(_facetedGlass.CountFaceted));

            Assert.That(expIsValid,
                Is.EqualTo(_facetedGlass.IsValid));
        }

        [Test(Description = "Check all setted params of faceted glass - " +
            "incorrect data")]
        [TestCase(_min / 10, _min / 2, _minAngle, _minDepth,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - height < min")]
        [TestCase(_max * 2, _max / 2, _maxAngle, _maxDepthSide,
            _maxDepthBottom, _maxCountFaceted, _invalid,
                TestName = "Setted neg - height > max")]
        [TestCase(_min, _min / 10, _minAngle, _minDepth,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - diameter bottom < min")]
        [TestCase(_max, _max, _minAngle, _minDepth,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - diameter bottom > max")]
        [TestCase(_max, _max / 2, _minAngle - _max, _minDepth,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - angle height < min")]
        [TestCase(_max, _min / 2, _maxAngle * 2, _minDepth,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - angle height > max")]
        [TestCase(_max, _min / 2, _maxAngle, _minDepth / 10,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - depth side < min")]
        [TestCase(_max, _min / 2, _maxAngle, _maxDepthSide * 2,
            _minDepth, _minCountFaceted, _invalid,
                TestName = "Setted neg - depth side > max")]
        [TestCase(_max, _min / 2, _maxAngle, _minDepth,
            _minDepth / 10, _minCountFaceted, _invalid,
                TestName = "Setted neg - depth bottom < min")]
        [TestCase(_max, _min / 2, _maxAngle, _maxDepthSide,
            _maxDepthBottom * 2, _minCountFaceted, _invalid,
                TestName = "Setted neg - depth bottom > max")]
        [TestCase(_max, _min / 2, _maxAngle, _minDepth,
            _minDepth, _minCountFaceted - 100, _invalid,
                TestName = "Setted neg - count faceted < min")]
        [TestCase(_max, _min / 2, _maxAngle, _maxDepthSide,
            _maxDepthBottom, _maxCountFaceted + 100, _invalid,
                TestName = "Setted neg - count faceted > max")]
        [TestCase(_min, _max / 2, _minAngle, _maxDepthSide,
            _maxDepthBottom, _maxCountFaceted, _invalid,
                TestName = "Setted neg - Height glass < diameter bottom,")]
        public void CheckSettedParamsNegative(double height,
            double diameterBottom, double angleHeight, double depthSide,
                double depthBottom, int countFaceted, bool expIsValid)
        {
            var expDepthSide = CleanGlassTest.CalcDepthOfGlass(
                diameterBottom, depthSide);
            var expDepthBottom = CleanGlassTest.CalcDepthOfGlass(
                height, depthBottom);

            Assert.Throws<ArgumentException>(() =>
            {
                _facetedGlass.Height = height;
                _facetedGlass.DiameterBottom = diameterBottom;
                _facetedGlass.AngleHeight = angleHeight;
                _facetedGlass.DepthSide = depthSide;
                _facetedGlass.DepthBottom = depthBottom;
                _facetedGlass.CountFaceted = countFaceted;
            });

            Assert.That(expIsValid,
                Is.EqualTo(_facetedGlass.IsValid));
        }

        [Test(Description = "Check auto calc params of faceted glass - " +
            "height, diameter bottom, angleHeight, depth side, " +
                "depth bottom, height faceted, count faceted")]
        [TestCase(new[] { false, false, false, false, false, true, false },
            TestName = "Check auto calc params")]
        public void CheckAutoCalcParams(bool[] exp)
        {
            CleanGlassTest.CheckAutoCalcParamsOfGlass(exp,
                _facetedGlass);
        }

        [Test(Description = "Check a filleted of faceted glass")]
        [TestCase(_filleted, TestName = "Filleted faceted glass")]
        [TestCase(!_filleted, TestName = "Not Filleted faceted glass")]
        public void CheckFilletedFacetedGlass(bool filleted)
        {
            _facetedGlass.Filleted = _filleted;
            Assert.AreEqual(_filleted, _facetedGlass.Filleted);
        }
    }
}
