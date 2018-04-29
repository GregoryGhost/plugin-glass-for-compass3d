﻿using NUnit.Framework;
using GlassModel;
using System;


namespace GlassModel.Tests
{
    [TestFixture]
    public class CleanGlassTest
    {
        private CleanGlass _cleanGlass;
        private const double _angle = 0.0f;
        private const double _heightFaceted = 0.0f;
        private const int _countFaceted = 0;
        private const double _min = 10;
        private const double _max = 200;
        private const double _percentForDepthBottom = 7;
        private const double _percentForDepthSide = 2;

        [SetUp]
        public void Setup()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);

            _cleanGlass = new CleanGlass(diameterBottom, height);
        }

        [Test(Description = "Check fixed params of clean glass -" +
            "angle height, count faceted, height faceted")]
        [TestCase(_angle, _heightFaceted, _countFaceted,
            TestName = "Fixed params of clean glass")]
        public void CheckFixedParams(double angle, double hFaceted,
            int kFaceted)
        {
            _cleanGlass.AngleHeight = angle + 10;
            _cleanGlass.HeightFaceted = hFaceted + 10;
            _cleanGlass.CountFaceted = kFaceted + 10;

            Assert.That(angle, Is.EqualTo(_cleanGlass.AngleHeight));
            Assert.That(hFaceted, Is.EqualTo(_cleanGlass.HeightFaceted));
            Assert.That(kFaceted, Is.EqualTo(_cleanGlass.CountFaceted));
        }


        [Test(Description = "Check setted params of clean glass -" +
            "height, diameter bottom - correct data")]
        [TestCase(_min, _min / 2,
            TestName = "Setted - Height glass = min, diameter bottom = min")]
        [TestCase(_max, _max / 2,
            TestName = "Setted - Height glass = max, diameter bottom = max")]
        [TestCase((_min + _max) / 2, (_min / 2 + _max / 2) / 2,
            TestName = "Setted - Height glass and diameter bottom" +
                "in the allowable range")]
        [TestCase(_max, _min / 2,
            TestName = "Setted - Height glass >= diameter bottom,")]
        public void CheckSettedParamsPositive(double height,
            double diameterBottom)
        {
            _cleanGlass.Height = height;
            _cleanGlass.DiameterBottom = diameterBottom;

            Assert.That(height, Is.EqualTo(_cleanGlass.Height));
            Assert.That(diameterBottom, Is.EqualTo(_cleanGlass.DiameterBottom));
            Assert.That(_cleanGlass.Height >= _cleanGlass.DiameterBottom);
        }

        [Test(Description = "Check setted params of clean glass -" +
            "height, diameter bottom - incorrect data")]
        [TestCase(_min / 10, _min / 10,
            TestName = "Setted neg - Height glass < min, diameter bottom < min")]
        [TestCase(_max * 2, _max,
            TestName = "Setted neg - Height glass > max, diameter bottom > max")]
        [TestCase(_min, _max / 2,
            TestName = "Setted neg - Height glass < diameter bottom,")]
        public void CheckSettedParamsNegative(double height,
            double diameterBottom)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _cleanGlass.Height = height;
                _cleanGlass.DiameterBottom = diameterBottom;
            });
            Assert.Throws<ArgumentException>(() =>
            {
                _cleanGlass.DiameterBottom = diameterBottom;
                _cleanGlass.Height = height;
            });
        }

        [Test(Description = "Check dependencies params of clean glass -" +
            "depth side, depth bottom - correct data")]
        [TestCase(_min , _min / 2,
            TestName = "Depend - Height glass = min, diameter bottom = min")]
        [TestCase(_max, _max / 2,
            TestName = "Depend - Height glass = max, diameter bottom = max")]
        [TestCase((_min + _max) / 2, (_min / 2 + _max / 2) / 2,
            TestName = "Depend - Height glass and diameter bottom" +
                "in the allowable range")]
        public void CheckDependenciesParamsPositive(double height,
            double diameterBottom)
        {
            _cleanGlass.Height = height;
            _cleanGlass.DiameterBottom = diameterBottom;

            _cleanGlass.DepthSide = height;
            _cleanGlass.DepthBottom = height;

            var expDepthBottom = calcDepthOfGlass(height, _percentForDepthBottom);
            var expDepthSide = calcDepthOfGlass(diameterBottom,
                _percentForDepthSide);

            Assert.That(expDepthBottom, Is.EqualTo(_cleanGlass.DepthBottom));
            Assert.That(expDepthSide, Is.EqualTo(_cleanGlass.DepthSide));
        }

        private double calcDepthOfGlass(double height, double percent)
        {
            return height * percent / 100;
        }
    }
}
