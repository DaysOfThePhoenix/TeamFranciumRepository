using System;
using GameFifteen;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Game15Test
{
    [TestClass]
    public class TestConsoleRenderer
    {
        [TestMethod]
        public void TestRenderMessage()
        {
            ConsoleRenderer renderer = new ConsoleRenderer();
            StringWriter writer = new StringWriter();
            Console.SetOut(writer);
            using (writer)
            {
                renderer.RenderMessage(Messages.GetGoodbye());
            }

            StringBuilder expected = new StringBuilder();
            expected.AppendLine("Good bye!");

            Assert.AreEqual(expected.ToString(), writer.ToString());
        }
    }
}
