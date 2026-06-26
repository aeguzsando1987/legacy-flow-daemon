using System;

namespace HypertermFlow.Core.Tests
{
    /// <summary>Mini framework de aserciones sin dependencias externas.</summary>
    public static class MiniAssert
    {
        public static int Passed;
        public static int Failed;

        public static void IsTrue(bool condition, string name)
        {
            if (condition)
            {
                Passed++;
                Console.WriteLine("  [PASS] " + name);
            }
            else
            {
                Failed++;
                Console.WriteLine("  [FAIL] " + name);
            }
        }

        public static void AreEqual(object expected, object actual, string name)
        {
            bool ok = (expected == null && actual == null)
                      || (expected != null && expected.Equals(actual));
            if (ok)
            {
                Passed++;
                Console.WriteLine("  [PASS] " + name);
            }
            else
            {
                Failed++;
                Console.WriteLine("  [FAIL] " + name
                    + "  (esperado=" + expected + ", actual=" + actual + ")");
            }
        }
    }
}
