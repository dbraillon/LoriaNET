using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Loria.Core.Tests
{
    [TestClass()]
    public class SetTests
    {
        [TestMethod()]
        public void SetTest()
        {
            // Arrange
            var mock = new Mock<IHasName>();
            mock.Setup(m => m.Name).Returns("test");

            // Act
            var set1 = new Set<IHasName>(mock.Object);
            var set2 = new Set<IHasName>(null);
            var set3 = new Set<IHasName>(new[] { mock.Object, mock.Object });

            // Assert
            Assert.IsTrue(set1.Objects.Length == 1);
            Assert.IsTrue(set2.Objects.Length == 0);
            Assert.IsTrue(set3.Objects.Length == 2);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            var mock = new Mock<IHasName>();
            mock.Setup(m => m.Name).Returns("test");

            // Act
            var set = new Set<IHasName>(mock.Object);
            var item1 = set.Get("test");
            var item2 = set.Get("TEST");
            var item3 = set.Get("tesT");
            var item4 = set.Get("tests");

            // Assert
            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.IsNotNull(item3);
            Assert.IsNull(item4);
        }
    }
}