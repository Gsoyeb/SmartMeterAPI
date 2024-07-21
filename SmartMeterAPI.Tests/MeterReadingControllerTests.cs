using SmartMeterAPI.Controllers;
using SmartMeterAPI.Infrastracture.Repositories.IRepositories;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartMeterAPI.Domain;
using System.Text;


namespace SmartMeterAPI.Tests
{
    public class MeterReadingControllerTests
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly Mock<IMeterReaderRepository> _mockMeterReaderRepo;
        private readonly MeterReadingController _controller;
        public MeterReadingControllerTests()
        {
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockMeterReaderRepo = new Mock<IMeterReaderRepository>();
            _controller = new MeterReadingController(_mockCustomerRepo.Object, _mockMeterReaderRepo.Object);
            
            //// Set up necessary method returns to avoid null references
            //_mockCustomerRepo.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(true);
            //_mockMeterReaderRepo.Setup(repo => repo.GetLatestReading(It.IsAny<int>())).ReturnsAsync((MeterReading)null);
        }


        [Fact]
        public async Task UploadMeterReadings_FileIsNull_ReturnsBadRequest()
        {
            // Arrange
            IFormFile file = null;

            // Act
            var result = await _controller.UploadMeterReadings(file);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UploadMeterReadings_ValidReadings_AddsSuccessfully()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var content = "101,01/01/2020 00:00,9999\n102,01/01/2020 00:00,8888";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            file.Setup(f => f.OpenReadStream()).Returns(stream);
            file.Setup(f => f.Length).Returns(stream.Length);

            _mockCustomerRepo.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(true);
            _mockMeterReaderRepo.Setup(repo => repo.GetLatestReading(It.IsAny<int>())).ReturnsAsync((MeterReading)null);

            // Act
            var result = await _controller.UploadMeterReadings(file.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<UploadResult>(okResult.Value);
            Assert.Equal(2, data.SuccessfulReads);
            Assert.Equal(0, data.FailedReads);
            _mockMeterReaderRepo.Verify(repo => repo.AddReading(It.IsAny<MeterReading>()), Times.Exactly(2));
        }




    }
}