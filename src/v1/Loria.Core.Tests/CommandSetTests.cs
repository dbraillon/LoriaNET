using Microsoft.VisualStudio.TestTools.UnitTesting;
using Loria.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Loria.Core.Tests
{
    [TestClass()]
    public class CommandSetTests
    {
        [TestMethod()]
        public void CommandSetTest()
        {
            // Arrange
            var mock = new Mock<IHasCommand>();
            mock.Setup(m => m.Command).Returns("test");

            // Act
            var set1 = new CommandSet<IHasCommand>(mock.Object);
            var set2 = new CommandSet<IHasCommand>(null);
            var set3 = new CommandSet<IHasCommand>(new[] { mock.Object, mock.Object });

            // Assert
            Assert.IsTrue(set1.Objects.Length == 1);
            Assert.IsTrue(set2.Objects.Length == 0);
            Assert.IsTrue(set3.Objects.Length == 2);
        }

        [TestMethod()]
        public void GetByCommandTest()
        {
            // Arrange
            var mock = new Mock<IHasCommand>();
            mock.Setup(m => m.Command).Returns("test");

            // Act
            var set = new CommandSet<IHasCommand>(mock.Object);
            var item1 = set.GetByCommand("test");
            var item2 = set.GetByCommand("TEST");
            var item3 = set.GetByCommand("tesT");
            var item4 = set.GetByCommand("tests");

            // Assert
            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.IsNotNull(item3);
            Assert.IsNull(item4);
        }
    }
}