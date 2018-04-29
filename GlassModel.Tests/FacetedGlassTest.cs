﻿using System;
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
            var diameterBottom = new BorderConditions<double>(_min / 2,
                _min / 2, _max / 2);
            _facetedGlass = new FacetedGlass(height, diameterBottom);
        }

        [Test(Description = "Check dependencies params of faceted glass" + 
            " - height faceted")]
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

        [Test(Description = "Check setted params of faceted glass -" +
            "height, diameter bottom - correct data")]
        [TestCase(_min, _min / 2, _valid,
            TestName = "Setted - Height glass = min, diameter bottom = min")]
        [TestCase(_max, _max / 2, _valid,
            TestName = "Setted - Height glass = max, diameter bottom = max")]
        [TestCase((_min + _max) / 2, (_min / 2 + _max / 2) / 2, _valid,
            TestName = "Setted - Height glass and diameter bottom" +
                "in the allowable range")]
        [TestCase(_max, _min / 2, _valid,
            TestName = "Setted - Height glass >= diameter bottom,")]
        public void CheckSettedParamsPositive(double height,
            double diameterBottom, bool expIsValid)
        {
            _facetedGlass.Height = height;
            _facetedGlass.DiameterBottom = diameterBottom;

            Assert.That(height, Is.EqualTo(_facetedGlass.Height));
            Assert.That(diameterBottom, 
                Is.EqualTo(_facetedGlass.DiameterBottom));
            Assert.That(
                _facetedGlass.Height >= _facetedGlass.DiameterBottom);

            Assert.That(expIsValid,
                Is.EqualTo(_facetedGlass.IsValid));
        }
    }
}
