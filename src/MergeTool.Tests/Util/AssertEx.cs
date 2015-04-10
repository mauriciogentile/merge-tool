using Newtonsoft.Json;

namespace MergeTool.Tests.Util
{
    static class AssertEx
    {
        public static void AreEqualByJson(object expected, object actual)
        {
            var string1 = JsonConvert.SerializeObject(expected);
            var string2 = JsonConvert.SerializeObject(actual);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(string1, string2);
        }
    }
}
