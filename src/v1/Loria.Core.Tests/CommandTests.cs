using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Loria.Core.Tests
{
    [TestClass()]
    public class CommandTests
    {
        [TestMethod()]
        public void CommandTest()
        {
            // Arrange
            var str1 = "perform date get";
            var str2 = "callback voice hello, how are you?";
            var str3 = "date get tomorrow";

            // Act
            var cmd1 = new Command(str1);
            var cmd2 = new Command(str2);
            var cmd3 = new Command(str3);

            // Assert
            Assert.AreEqual("perform", cmd1.Header);
            Assert.AreEqual("date", cmd1.Module);
            Assert.AreEqual("get", cmd1.Body);
            Assert.AreEqual(str1, cmd1.Raw);
            Assert.AreEqual("callback", cmd2.Header);
            Assert.AreEqual("voice", cmd2.Module);
            Assert.AreEqual("hello, how are you?", cmd2.Body);
            Assert.AreEqual(str2, cmd2.Raw);
            Assert.AreEqual("date", cmd3.Header);
            Assert.AreEqual("get", cmd3.Module);                 
            Assert.AreEqual("tomorrow", cmd3.Body);
            Assert.AreEqual(str3, cmd3.Raw);
        }

        [TestMethod()]
        public void AsActionCommandTest()
        {
            // Arrange
            var cmd1 = new Command("perform date get");
            var cmd2 = new Command("callback voice hello, how are you?");
            var cmd3 = new Command("date get tomorrow");

            // Act
            var act1 = cmd1.AsActionCommand();
            var act2 = cmd2.AsActionCommand();
            var act3 = cmd3.AsActionCommand();

            // Assert
            Assert.IsInstanceOfType(act1, typeof(ActionCommand));
            Assert.IsInstanceOfType(act2, typeof(ActionCommand));
            Assert.IsInstanceOfType(act3, typeof(ActionCommand));
        }

        [TestMethod()]
        public void AsCallbackCommandTest()
        {
            // Arrange
            var cmd1 = new Command("perform date get");
            var cmd2 = new Command("callback voice hello, how are you?");
            var cmd3 = new Command("date get tomorrow");

            // Act
            var clb1 = cmd1.AsCallbackCommand();
            var clb2 = cmd2.AsCallbackCommand();
            var clb3 = cmd3.AsCallbackCommand();

            // Assert
            Assert.IsInstanceOfType(clb1, typeof(CallbackCommand));
            Assert.IsInstanceOfType(clb2, typeof(CallbackCommand));
            Assert.IsInstanceOfType(clb3, typeof(CallbackCommand));
        }

        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            var str1 = "perform date get";
            var str2 = "callback voice hello, how are you?";
            var str3 = "date get tomorrow";
            var cmd1 = new Command(str1);
            var cmd2 = new Command(str2);
            var cmd3 = new Command(str3);

            // Act
            var str4 = cmd1.ToString();
            var str5 = cmd2.ToString();
            var str6 = cmd3.ToString();

            // Assert
            Assert.AreEqual(str1, str4);
            Assert.AreEqual(str2, str5);
            Assert.AreEqual(str3, str6);
        }
    }
}