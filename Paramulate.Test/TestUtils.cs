namespace Paramulate.Test
{
    internal static class TestUtils
    {
        public static T Build<T>() where T : class
        {
            var builder = ParamsBuilder<T>.New();
            var result = builder.Build(nameof(T));
            return result;
        }
    }
}