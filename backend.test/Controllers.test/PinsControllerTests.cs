using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Services.Pinservice.Commands;
using dotnetcorebackend.Application.Services.Pinservice.Queries;
using dotnetcorebackend.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Controllers.test
{
    public class PinsControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly PinsController _pinsController;
        private readonly List<GetAllPinsDTO> _mockPins;
        private readonly CreateNewPinCommand _createNewPinCommand;

        public PinsControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _pinsController = new PinsController(_mockMediator.Object);
            _createNewPinCommand = new CreateNewPinCommand
            {
                UserId = Guid.NewGuid(),
                Title = "Sample Pin",
                Category = "Sample Category",
                Description = "Sample Description",
                Rating = 4,
                Latitude = 12.34,
                Longitude = 56.78
            };
            _mockPins = new List<GetAllPinsDTO>
            {
                new GetAllPinsDTO
                {
                    PinId = Guid.NewGuid(),
                    Title = "Sample Pin",
                    Category = "Sample Category",
                    Description = "Sample Description",
                    Rating = 4,
                    Latitude = 12.34,
                    Longitude = 56.78,
                    UserId = Guid.NewGuid(),
                    Username = "SampleUser"
                }
            };
        }

        /// <summary>
        /// TC001:- Test to check GetAllPins and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllPins_Successful_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllPinsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_mockPins);
            var result = await _pinsController.GetAllPins() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
        }

        /// <summary>
        /// TC002:- Test to check GetAllPins and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllPins_Failed_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<GetAllPinsQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _pinsController.GetAllPins() as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC003:- Test to check CreatePin and return 200-OK
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePin_successful_ReturnsOk()
        {
            _mockMediator.Setup(m => m.Send(_createNewPinCommand, It.IsAny<CancellationToken>())).ReturnsAsync(new PinDTO
            {
                PinId = Guid.NewGuid(),
                Title = "Sample Pin",
                Category = "Sample Category",
                Description = "Sample Description",
                Rating = 4,
                Latitude = 12.34,
                Longitude = 56.78
            });

            var result = await _pinsController.CreatePins(_createNewPinCommand) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        /// <summary>
        /// TC004:- Test to check CreatePin and return 400-Bad request
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePin_Failed_ReturnsBadRequest()
        {
            _mockMediator.Setup(m => m.Send(_createNewPinCommand, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _pinsController.CreatePins(_createNewPinCommand) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }

        /// <summary>
        /// TC005:- Test to check GetPinsByUserId and return 400-BadRequestResponse
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserById_Successful_ReturnsOk()
        {
            var userid = Guid.NewGuid();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetPinsByIdQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("An error occurred while processing your request"));

            var result = await _pinsController.GetPinsByUserId(userid) as BadRequestObjectResult;
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Contains("An error occurred while processing your request", result.Value.ToString());
        }
    }
}
