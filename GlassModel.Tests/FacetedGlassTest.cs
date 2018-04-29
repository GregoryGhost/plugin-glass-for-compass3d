using System;
using NUnit.Framework;
using GlassModel;

namespace GlassModel.Tests
{
    [TestFixture]
    public class FacetedGlassTest
    {
        private FacetedGlass _facetedGlass;

        private const double _angle = 0.0f;
        private const double _heightFaceted = 0.0f;
        private const int _countFaceted = 0;

        private const double _min = 10;
        private const double _max = 200;

        private const double _percentForDepthBottom = 7;
        private const double _percentForHeightFaceted = 90;
        private const double _percentForDepthSide = 4;

        private const bool _valid = true;
        private const bool _invalid = false;

        [SetUp]
        public void Setup()
        {
            var height = new BorderConditions<double>(_min, _min, _max);
            _facetedGlass = new FacetedGlass(height);
        }

        [Test(Description = "")]
        [TestCase(_min, TestName="Depend params - Height glass = min")]
        [TestCase(_max, TestName = "Depend params - Height glass = max")]
        [TestCase((_min + _max) / 2,
            TestName = "Depend params - Height glass = average")]
        public void CheckDependenciesParams(double height)
        {
            _facetedGlass.Height = height;

            var expHeightFaceted = height * 
                _percentForHeightFaceted / 100;

            Assert.AreEqual(expHeightFaceted,
                _facetedGlass.HeightFaceted);
        }
    }
}
