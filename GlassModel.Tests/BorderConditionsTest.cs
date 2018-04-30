using NUnit.Framework;
using GlassModel;
using System;

namespace GlassModel.Tests
{
    [TestFixture]
    public class BorderConditionsTests
    {
        private BorderConditions<double> _parameter;

        private const double _min = -10.0f;
        private const double _value = 0.0f;
        private const double _max = 10.0f;

        [SetUp]
        public void Setup()
        {
            _parameter = new BorderConditions<double>(_min, _value, _max);
        }

        [Test(Description = "BorderConditions constructor test positive")]
        [TestCase(_min, _value, _max, TestName = "Min < Value < Max")]
        [TestCase(_max, _max, _max, TestName = "Min = Value = Max")]
        public void PositiveBorderConditionsConstructorTest(double min,
            double value, double max)
        {
            var parameter = new BorderConditions<double>(min, value, max);

            Assert.That(min, Is.EqualTo(parameter.Min));
            Assert.That(value, Is.EqualTo(parameter.Value));
            Assert.That(max, Is.EqualTo(parameter.Max));
        }

        [Test(Description = "BorderConditions constructor test negative")]
        [TestCase(_max, _value, _min, TestName = " Max < Min")]
        [TestCase(_min, _min * 2, _max, TestName = "Value < Min")]
        [TestCase(_min, _max * 2, _max, TestName = "Value > Max")]
        public void NegativeBorderConditionsConstructorTest(double min,
            double value, double max)
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var parameter = new BorderConditions<double>(min, value, max);
            });
        }


        [Test(Description = "Set value great than max value")]
        [TestCase(_max * 2, TestName = "Max < Value")]
        public void SetGreatValueThanMax(double value)
        {
            Assert.Throws<ArgumentException>(() =>
                _parameter.Value = value);
        }

        [Test(Description = "Set value less than min value")]
        [TestCase(_min * 2, TestName = "Min > Value")]
        public void SetLessValueThanMin(double value)
        {
            Assert.Throws<ArgumentException>(() =>
                _parameter.Value = value);
        }

        [Test(Description = "Set a value within the range")]
        [TestCase((_min + _max) / 2, TestName = "Min < Avg value < Max")]
        [TestCase(_min, TestName = "Value = Min")]
        [TestCase(_max, TestName = "Value = Max")]
        public void SetValueWithinRange(double value)
        {
            _parameter.Value = value;
            Assert.AreEqual(value, _parameter.Value);
        }
    }
}