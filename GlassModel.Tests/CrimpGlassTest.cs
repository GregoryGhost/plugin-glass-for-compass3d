using System;
using NUnit.Framework;
using GlassModel;

namespace GlassModel.Tests
{
    public class CrimpGlassTest
    {
        private CrimpGlass _crimpGlass;

        private const double _angle = 5.0f;

        private const double _min = 10;
        private const double _max = 200;
        private const int _minCountFaceted = 20;
        private const int _maxCountFaceted = 60;

        private const double _percentForDepthBottom = 4;
        private const double _heightFaceted = 9;
        private const double _percentForDepthSide = 3;

        private const bool _valid = true;
        private const bool _invalid = false;

        private const bool _filleted = true;

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
            "of crimp glass - height, diameter bottom, angleHeight, " +
                "depth side, depth bottom, height faceted, count faceted")]
        [TestCase(new[] { false, false, true, true, true, true, false },
            TestName = "Check auto calc params")]
        public void CheckAutoCalcParams(bool[] exp)
        {
            CleanGlassTest.CheckAutoCalcParamsOfGlass(exp,
                _crimpGlass);
        }

        [Test(Description = "Check fixed params of crimp glass -" +
            "angle height, height faceted, depth side, depth bottom")]
        [TestCase(_angle, _heightFaceted, _percentForDepthBottom,
            _percentForDepthSide,
                TestName = "Fixed params of crimp glass")]
        public void CheckFixedParams(double angle, double hFaceted,
            double depthBottom, double depthSide)
        {
            _crimpGlass.AngleHeight = angle + 10;
            _crimpGlass.DepthBottom = depthBottom + 10;
            _crimpGlass.DepthSide = depthSide + 10;
            _crimpGlass.HeightFaceted = hFaceted + 10;

            var expDepthSide = _percentForDepthSide;
            var expDepthBottom = _percentForDepthBottom;
            var expHeightFaceted = _heightFaceted;

            Assert.That(angle, Is.EqualTo(_crimpGlass.AngleHeight));
            Assert.That(expHeightFaceted,
                Is.EqualTo(_crimpGlass.HeightFaceted));
            Assert.That(expDepthSide, Is.EqualTo(_crimpGlass.DepthSide));
            Assert.That(expDepthBottom,
                Is.EqualTo(_crimpGlass.DepthBottom));
        }


        [Test(Description = "Check setted params of crimp glass -" +
            "height, diameter bottom - correct data")]
        [TestCase(_min, _min / 2, _minCountFaceted, _valid,
            TestName = "Setted - Height glass = min, diameter bottom = min")]
        [TestCase(_max, _max / 2, _maxCountFaceted, _valid,
            TestName = "Setted - Height glass = max, diameter bottom = max")]
        [TestCase((_min + _max) / 2, (_min / 2 + _max / 2) / 2,
            (_minCountFaceted + _maxCountFaceted) / 2, _valid,
                TestName = "Setted - Height glass and diameter bottom" +
                    "in the allowable range")]
        [TestCase(_max, _min / 2, _minCountFaceted, _valid,
            TestName = "Setted - Height glass >= diameter bottom,")]
        public void CheckSettedParamsPositive(double height,
            double diameterBottom, int countFaceted, bool expIsValid)
        {
            _crimpGlass.Height = height;
            _crimpGlass.DiameterBottom = diameterBottom;
            _crimpGlass.CountFaceted = countFaceted;

            Assert.That(height, Is.EqualTo(_crimpGlass.Height));
            Assert.That(diameterBottom,
                Is.EqualTo(_crimpGlass.DiameterBottom));
            Assert.That(_crimpGlass.Height >= _crimpGlass.DiameterBottom);
            Assert.That(countFaceted, Is.EqualTo(_crimpGlass.CountFaceted));

            Assert.That(expIsValid, Is.EqualTo(_crimpGlass.IsValid));
        }

        [Test(Description = "Check setted params of crimp glass -" +
            "height, diameter bottom - incorrect data")]
        [TestCase(_min / 10, _min / 2, _invalid,
            TestName = "Setted neg - Height glass < min, diameter bottom = min")]
        [TestCase(_min, _min / 10, _invalid,
            TestName = "Setted neg - Height glass = min, diameter bottom < min")]
        [TestCase(_max * 2, _max / 2, _invalid,
            TestName = "Setted neg - Height glass > max, diameter bottom = max")]
        [TestCase(_max, _max, _invalid,
            TestName = "Setted neg - Height glass = max, diameter bottom > max")]
        [TestCase(_min, _max / 2, _invalid,
            TestName = "Setted neg - Height glass < diameter bottom,")]
        public void CheckSettedParamsNegative(double height,
            double diameterBottom, bool expIsValid)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _crimpGlass.Height = height;
                _crimpGlass.DiameterBottom = diameterBottom;
            });
            Assert.Throws<ArgumentException>(() =>
            {
                _crimpGlass.DiameterBottom = diameterBottom;
                _crimpGlass.Height = height;
            });

            Assert.That(expIsValid, Is.EqualTo(_crimpGlass.IsValid));
        }

        [Test(Description = "Check setted params of crimp glass -" +
            "count faceted - incorrect data")]
        [TestCase(_minCountFaceted / 2, _invalid,
            TestName = "Setted neg - count faceted < min")]
        [TestCase(_maxCountFaceted * 2, _invalid,
            TestName = "Setted neg - count faceted > max")]
        public void CheckSettedParamsNegative2(int countFaceted,
            bool expIsValid)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                _crimpGlass.CountFaceted = countFaceted;
            });

            Assert.That(expIsValid, Is.EqualTo(_crimpGlass.IsValid));
        }

        [Test(Description = "Check a filleted of faceted glass")]
        [TestCase(_filleted, TestName = "Filleted faceted glass")]
        [TestCase(!_filleted, TestName = "Not Filleted faceted glass")]
        public void CheckFilletedCrimpGlass(bool filleted)
        {
            _crimpGlass.Filleted = _filleted;
            Assert.AreEqual(_filleted, _crimpGlass.Filleted);
        }
    }
}
