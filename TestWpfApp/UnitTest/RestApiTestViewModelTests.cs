using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TestWpfApp;

namespace UnitTest
{
    [TestClass]
    public class RestApiTestViewModelTests
    {
        private Mock<IXmlReader> _xmlReader;

        private RestApiTestViewModel _restApiTestViewModel;

        private string value = "val";
        private string _exceptionMessage = "Exception message";

        [TestInitialize]
        public void Setup()
        {
            _xmlReader = new Mock<IXmlReader>();
            _xmlReader.Setup(f => f.GetValue(It.IsAny<string>(), It.IsAny<string>())).Returns(value);

            _restApiTestViewModel = new RestApiTestViewModel(_xmlReader.Object);
        }

        [TestMethod]
        public void Run_ShouldUpdateOutputValue()
        {
            _restApiTestViewModel.Run();

            Assert.AreEqual(value, _restApiTestViewModel.OutputValue);
        }

        [TestMethod]
        public void Run_ShouldUpdateSymbols()
        {
            _restApiTestViewModel.Run();

            Assert.AreEqual(3, _restApiTestViewModel.Symbols.Count);
        }

        [TestMethod]
        public void Run_OutputValue_ShouldContainError_IfException()
        {
            _xmlReader.Setup(f => f.GetValue(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception(_exceptionMessage));

            _restApiTestViewModel.Run();

            Assert.AreEqual(_exceptionMessage, _restApiTestViewModel.OutputValue);
        }
    }
}
