namespace Paramulate.Test
{
    internal static class TestUtils
    {
        public static T Build<T>() where T : class
        {
            var builder = ParamsBuilder<T>.New(nameof(T));
            var result = builder.Build();
            return result;
        }
    }
}