using System;
using System.Threading.Tasks;

namespace WebSocketEnhanced
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TestClass t = new TestClass();
            await t.testing();
        }
    }
}
