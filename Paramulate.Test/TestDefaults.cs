using System;
using NUnit.Framework;
using Paramulate.Attributes;
using Paramulate.Exceptions;

namespace Paramulate.Test
{
    [Paramulate]
    public interface ITestParameterObject
    {
        [Default(TestData.IntStr)]
        int Int { get; }

        [Default(TestData.UnquotedString)]
        string UnquotedString { get; }

        [Default(TestData.QuotedString)]
        string QuotedString { get; }

        [Default(TestData.UnquotedStringWithQuote)]
        string UnquotedStringWithQuote { get; }

        [Default(TestData.QuotedStringWithEscapedQuote)]
        string QuotedStringWithEscapedQuote { get; }

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

        [Default(TestData.TimeSpanStr)]
        TimeSpan TimeSpan { get; }

        string PropertyWithoutDefault { get; }
    }

    [Paramulate]
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

        [Default(TestData.NullStr)]
        TimeSpan? TimeSpanNull { get; set; }
    }

    [Paramulate]
    public interface ITestInvalidParameterObject
    {
        [Default(TestData.InvalidIntStr)]
        int InvalidInt { get; }
    }

    [Paramulate]
    public interface ITestInvalidParameterObject2
    {
        [Default(TestData.InvalidTimeSpanStr)]
        TimeSpan InvalidTimeSpan { get; }
    }

    [TestFixture]
    public class TestDefaults
    {
        [Test]
        public void TestInt()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().Int,
                Is.EqualTo(TestData.Int));
        }
        
        [Test]
        public void TestUnquotedString()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().UnquotedString,
                Is.EqualTo(TestData.UnquotedString));
        }
        
        [Test]
        public void TestQuotedString()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().QuotedString,
                Is.EqualTo(TestData.UnquotedString));
        }
        
        [Test]
        public void TestUnquotedStringWithQuote()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().UnquotedStringWithQuote,
                Is.EqualTo(TestData.UnquotedStringWithQuote));
        }
        
        [Test]
        public void QuotedStringWithEscapedQuote()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().QuotedStringWithEscapedQuote,
                Is.EqualTo(TestData.UnquotedStringWithQuote));
        }
        
        [Test]
        public void TestLong()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().Long,
                Is.EqualTo(TestData.Long));
        }
        
        [Test]
        public void TestFloat()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().Float,
                Is.EqualTo(TestData.Float));
        }
        
        [Test]
        public void TestFloatNaN()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().FloatNaN,
                Is.EqualTo(TestData.FloatNaN));
        }
        
        [Test]
        public void TestFloatNegInfinity()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().FloatNegInfinity,
                Is.EqualTo(TestData.FloatNegInfinity));
        }
        
        [Test]
        public void TestDouble()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().Double,
                Is.EqualTo(TestData.Double));
        }
        
        [Test]
        public void TestDateTime()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().DateTime,
                Is.EqualTo(TestData.DateTime));
        }
        
        [Test]
        public void TestQuotedDateTime()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().QuotedDateTime,
                Is.EqualTo(TestData.DateTime));
        }
        
        [Test]
        public void TestDate()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().Date,
                Is.EqualTo(TestData.Date));
        }
        
        [Test]
        public void TestTimeSpan()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().TimeSpan,
                Is.EqualTo(TestData.TimeSpan));
        }
        
        [Test]
        public void TestPropertyWithoutDefault()
        {
            Assert.That(TestUtils.Build<ITestParameterObject>().PropertyWithoutDefault,
                Is.EqualTo(null));
        }
        
        [Test]
        public void TestNullableInt()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().Int,
                Is.EqualTo(TestData.Int));
        }
        
        [Test]
        public void TestNullableIntNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().IntNull,
                Is.EqualTo(null));
        }
        
        [Test]
        public void TestNullableStringNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().StringNull,
                Is.EqualTo(null));
        }

        [Test]
        public void TestNullableLong()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().Long,
                Is.EqualTo(TestData.Long));
        }
        
        [Test]
        public void TestNullableLongNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().LongNull,
                Is.EqualTo(null));
        }
        
        [Test]
        public void TestNullableFloat()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().Float,
                Is.EqualTo(TestData.Float));
        }
        
        [Test]
        public void TestNullableFloatNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().FloatNull,
                Is.EqualTo(null));
        }
        
        [Test]
        public void TestNullableDateTime()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().DateTime,
                Is.EqualTo(TestData.DateTime));
        }
        
        [Test]
        public void TestNullableDateTimeNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().DateTimeNull,
                Is.EqualTo(null));
        }
        
        [Test]
        public void TestNullableTimeSpanNull()
        {
            Assert.That(TestUtils.Build<ITestParameterObjectNullables>().TimeSpanNull,
                Is.EqualTo(null));
        }

        [Test]
        public void TestInvalidInt()
        {
            Assert.That(TestUtils.Build<ITestInvalidParameterObject>,
                Throws.TypeOf<InvalidProvidedValueException>()
                .With.Message.EqualTo("Failed to convert value '9223372036854775808' for property 'InvalidInt'" +
                                      " (type:System.Int32) when setting default value"));
        }

        [Test]
        public void TestInvalidTimespan()
        {
            Assert.That(TestUtils.Build<ITestInvalidParameterObject2>, Throws.TypeOf<InvalidProvidedValueException>()
                .With.Message.EqualTo("Failed to convert value '09::11' for " +
                                      "property 'InvalidTimeSpan' (type:System.TimeSpan) when setting default value"));
        }
    }
}