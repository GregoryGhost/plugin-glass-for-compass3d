using NUnit.Framework;
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
        private const double min = 10;
        private const double max = 200;

        [SetUp]
        public void Setup()
        {
            var height = new BorderConditions<double>(min, min, max);
            var diameterBottom = new BorderConditions<double>(min / 2,
                min / 2, max / 2);

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

        //[Test(Description="Setting the correct data for the parameters")]
        //[TestCase()]
        //public void SettingCorrectDataForParamsPositive()
        //{
        //    _cleanGlass.DiameterBottom = diameter;          
        //    _cleanGlass.Height = height;
        //    _cleanGlass.DepthSide = dSide;
        //    _cleanGlass.DepthBottom = dBottom;


        //}
    }
}
