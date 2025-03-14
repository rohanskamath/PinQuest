using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using dotnetcorebackend.Application.Services.Pinservice.Queries;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Services.test.PinService.test
{
    public class GetPinsByIdHandlerTests
    {
        private readonly Mock<IPinsRepository> _mockPinsRepository;
        private readonly GetPinsByIdHandler _handler;
        private readonly List<GetAllPinsDTO> _pins;

        public GetPinsByIdHandlerTests()
        {
            _mockPinsRepository = new Mock<IPinsRepository>();
            _handler = new GetPinsByIdHandler(_mockPinsRepository.Object);
            _pins = new List<GetAllPinsDTO>
            {
                new GetAllPinsDTO
                {
                    PinId = Guid.NewGuid(),
                    Title = "Pin 1",
                    Category = "Category 1",
                    Description = "Description 1",
                    Rating = 4,
                    Latitude = 12.34,
                    Longitude = 56.78
                },
                new GetAllPinsDTO
                {
                    PinId = Guid.NewGuid(),
                    Title = "Pin 2",
                    Category = "Category 2",
                    Description = "Description 2",
                    Rating = 3,
                    Latitude = 23.45,
                    Longitude = 67.89
                }
            };
        }

        /// <summary>
        /// TC001:- Test case should return All pins by id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldReturnPinsById_ReturnsOk()
        {
            var userId=Guid.NewGuid();
            _mockPinsRepository.Setup(repo => repo.GetPinsByIdAsync(userId)).ReturnsAsync(_pins);

            var request=new GetPinsByIdQuery(userId);
            var result = await _handler.Handle(request, CancellationToken.None);
            Assert.Equal(_pins, result);
            Assert.NotNull(result);
            Assert.Equal(_pins.Count, result.Count());
        }

        /// <summary>
        /// TC002:- Test case should return empty list when no pins exist
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPinsExist()
        {
            var userId = Guid.NewGuid();
            _mockPinsRepository.Setup(repo => repo.GetPinsByIdAsync(userId)).ReturnsAsync(new List<GetAllPinsDTO>());

            var request = new GetPinsByIdQuery(userId);
            var result = await _handler.Handle(request, CancellationToken.None);
            Assert.Empty(result);
            Assert.NotNull(result);
        }

        /// <summary>
        /// TC003:- Test case should throw exception when repository throws exception
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
        {
            var userId = Guid.NewGuid();
            _mockPinsRepository.Setup(repo => repo.GetPinsByIdAsync(userId)).ThrowsAsync(new Exception("An error occurred while processing your request."));

            var request = new GetPinsByIdQuery(userId);
            var result = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Contains("An error occurred while processing your request", result.Message);
        }
    }
}
