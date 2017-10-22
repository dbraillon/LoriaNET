using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Loria.Core.Tests
{
    [TestClass()]
    public class ActionCommandTests
    {
        [TestMethod()]
        public void ActionCommandTest()
        {
            // Arrange
            var str1 = "get";
            var str2 = "set -time 00:05 -text Lève-toi !";

            // Act
            var act1 = new ActionCommand(str1);
            var act2 = new ActionCommand(str2);

            // Assert
            Assert.AreEqual("get", act1.Intent);
            Assert.AreEqual(0, act1.Entities.Length);
            Assert.AreEqual(str1, act1.Raw);
            Assert.AreEqual("set", act2.Intent);
            Assert.AreEqual(2, act2.Entities.Length);
            Assert.AreEqual(str2, act2.Raw);
        }

        [TestMethod()]
        public void GetEntityTest()
        {
            // Arrange
            var act1 = new ActionCommand("get");
            var act2 = new ActionCommand("set -time 00:05 -text Lève-toi !");

            // Act
            var ent1 = act1.GetEntity("time");
            var ent2 = act2.GetEntity("time");
            var ent3 = act2.GetEntity("text");

            // Assert
            Assert.IsNull(ent1);
            Assert.AreEqual("time", ent2.Name);
            Assert.AreEqual("00:05", ent2.Value);
            Assert.AreEqual("text", ent3.Name);
            Assert.AreEqual("Lève-toi !", ent3.Value);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            // Arrange
            var str1 = "get";
            var str2 = "set -time 00:05 -text Lève-toi !";
            var act1 = new ActionCommand(str1);
            var act2 = new ActionCommand(str2);

            // Act
            var str3 = act1.ToString();
            var str4 = act2.ToString();

            // Assert
            Assert.AreEqual(str1, str3);
            Assert.AreEqual(str2, str4);
        }
    }
}