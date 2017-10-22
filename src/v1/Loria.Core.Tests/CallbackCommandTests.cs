using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Loria.Core.Tests
{
    [TestClass()]
    public class CallbackCommandTests
    {
        [TestMethod()]
        public void CallbackCommandTest()
        {
            // Arrange
            var str1 = "Bonjour";
            var str2 = "Lève-toi !";

            // Act
            var clb1 = new CallbackCommand(str1);
            var clb2 = new CallbackCommand(str2);

            // Assert
            Assert.AreEqual(str1, clb1.Message);
            Assert.AreEqual(str2, clb2.Message);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            var str1 = "Bonjour";
            var str2 = "Lève-toi !";
            var clb1 = new CallbackCommand(str1);
            var clb2 = new CallbackCommand(str2);

            // Act
            var str3 = clb1.ToString();
            var str4 = clb2.ToString();

            // Assert
            Assert.AreEqual(str1, str3);
            Assert.AreEqual(str2, str4);
        }
    }
}