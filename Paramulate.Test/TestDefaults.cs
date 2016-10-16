using System;
using System.Collections;
using NUnit.Framework;
using Paramulate.Attributes;
using Paramulate.Serialisation;

namespace Paramulate.Test
{
    public interface ITestParameterObject
    {
        [Default(TestData.IntStr)]
        int Int { get; }

        [Default(TestData.UnquotedString)]
        string UnquotedString { get; }

        [Default(TestData.QuotedString)]
        string QuotedString { get; }

        [Default(TestData.LongStr)]
        long Long { get; }

        [Default(TestData.FloatStr)]
        float Float { get; }

        [Default(TestData.FloatNaNStr)]
        float FloatNaN { get; }

        [Default(TestData.FloatNegInfinityStr)]
        float FloatNegInfinity { get; }

        [Default(TestData.DoubleStr)]
        double Double { get; }

        [Default(TestData.DateTimeStr)]
        DateTime DateTime { get; }

        [Default(TestData.QuotedDateTimeStr)]
        DateTime QuotedDateTime { get; }

        [Default(TestData.DateStr)]
        DateTime Date { get; }
    }

    public interface ITestParameterObjectNullables
    {
        [Default(TestData.IntStr)]
        int? Int { get; }

        [Default(TestData.NullStr)]
        int? IntNull { get; }

        [Default(TestData.NullStr)]
        string StringNull { get; }

        [Default(TestData.LongStr)]
        long? Long { get; }

        [Default(TestData.NullStr)]
        long? LongNull { get; }

        [Default(TestData.FloatStr)]
        float? Float { get; }

        [Default(TestData.NullStr)]
        float? FloatNull { get; }

        [Default(TestData.DateTimeStr)]
        DateTime? DateTime { get; }

        [Default(TestData.NullStr)]
        DateTime? DateTimeNull { get; }
    }

    public interface ITestInvalidParameterObject
    {
        [Default(TestData.InvalidIntStr)]
        int InvalidInt { get; }
    }

    [TestFixture]
    public class TestDefaults
    {
        public delegate T Getter<out T>(ITestParameterObject fromObject);
        public delegate T NullableGetter<out T>(ITestParameterObjectNullables fromObject);

        private static IEnumerable TestCases
        {
            get
            {
                yield return new TestCaseData(new Getter<int>(r => r.Int), TestData.Int)
                    .SetName("Int");
                yield return new TestCaseData(new Getter<string>(r => r.UnquotedString), TestData.UnquotedString)
                    .SetName("UnquotedString");
                yield return new TestCaseData(new Getter<string>(r => r.QuotedString), TestData.UnquotedString)
                    .SetName("QuotedString");
                yield return new TestCaseData(new Getter<long>(r => r.Long), TestData.Long)
                    .SetName("Long");
                yield return new TestCaseData(new Getter<float>(r => r.Float), TestData.Float)
                    .SetName("Float");
                yield return new TestCaseData(new Getter<float>(r => r.FloatNaN), TestData.FloatNaN)
                    .SetName("FloatNaN");
                yield return new TestCaseData(new Getter<float>(r => r.FloatNegInfinity), TestData.FloatNegInfinity)
                    .SetName("FloatNegInfinity");
                yield return new TestCaseData(new Getter<double>(r => r.Double), TestData.Double)
                    .SetName("Double");
                yield return new TestCaseData(new Getter<DateTime>(r => r.DateTime), TestData.DateTime)
                    .SetName("DateTime");
                yield return new TestCaseData(new Getter<DateTime>(r => r.QuotedDateTime), TestData.DateTime)
                    .SetName("QuotedDateTime");
                yield return new TestCaseData(new Getter<DateTime>(r => r.Date), TestData.Date)
                    .SetName("Date");
            }
        }

        [Test]
        [TestCaseSource(nameof(TestCases))]
        public void TestPrimitives<T>(Getter<T> valueToCheck, T expected)
        {
            var result = Build<ITestParameterObject>();
            Assert.That(valueToCheck(result), Is.EqualTo(expected));
        }

        private static IEnumerable NullableTestCases
        {
            get
            {
                yield return new TestCaseData(new NullableGetter<int?>(r => r.Int), TestData.Int)
                    .SetName("Int");
                yield return new TestCaseData(new NullableGetter<int?>(r => r.IntNull), null)
                    .SetName("IntNull");
                yield return new TestCaseData(new NullableGetter<string>(r => r.StringNull), null)
                    .SetName("StringNull");
                yield return new TestCaseData(new NullableGetter<long?>(r => r.Long), TestData.Long)
                    .SetName("Long");
                yield return new TestCaseData(new NullableGetter<long?>(r => r.LongNull), null)
                    .SetName("LongNull");
                yield return new TestCaseData(new NullableGetter<float?>(r => r.Float), TestData.Float)
                    .SetName("Float");
                yield return new TestCaseData(new NullableGetter<float?>(r => r.FloatNull), null)
                    .SetName("FloatNull");
                yield return new TestCaseData(new NullableGetter<DateTime?>(r => r.DateTime), TestData.DateTime)
                    .SetName("DateTime");
                yield return new TestCaseData(new NullableGetter<DateTime?>(r => r.DateTimeNull), null)
                    .SetName("DateTimeNull");
            }
        }

        [Test]
        [TestCaseSource(nameof(NullableTestCases))]
        public void TestNullables<T>(NullableGetter<T> valueToCheck, T expected)
        {
            var result = Build<ITestParameterObjectNullables>();
            Assert.That(valueToCheck(result), Is.EqualTo(expected));
        }

        [Test]
        public void TestInvalidInt()
        {
            Assert.That(Build<ITestInvalidParameterObject>, Throws.TypeOf<InvalidProvidedValueException>());
        }

        private static T Build<T>() where T : class
        {
            var builder = ParamsBuilder<T>.New();
            var result = builder.Build();
            return result;
        }
    }
}