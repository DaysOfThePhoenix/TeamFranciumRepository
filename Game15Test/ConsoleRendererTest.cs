using System;
using GameFifteen;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Game15Test
{
    [TestClass]
    public class ConsoleRendererTest
    {
        [TestMethod]
        public void TestRenderMessage()
        {
            string message = "This is some message.";
            ConsoleRenderer renderer = new ConsoleRenderer();
            renderer.RenderMessage(message);
        }
    }
}
