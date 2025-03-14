using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using dotnetcorebackend.Application.Services.Pinservice.Queries;
using Moq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.test.Services.test.PinService.test
{
    public class GetAllPinsHandlerTests
    {
        private readonly Mock<IPinsRepository> _mockPinsRepository;
        private readonly GetAllPinsHandler _handler;
        private readonly List<GetAllPinsDTO> _pins;
        public GetAllPinsHandlerTests()
        {
            _mockPinsRepository = new Mock<IPinsRepository>();
            _handler = new GetAllPinsHandler(_mockPinsRepository.Object);
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
        /// TC001:- Test case should return All pins
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldReturnPins_ReturnsOk()
        {
            _mockPinsRepository.Setup(repo=>repo.GetAllPinsAsync()).ReturnsAsync(_pins);

            var result = await _handler.Handle(new GetAllPinsQuery(), CancellationToken.None);
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
            _mockPinsRepository.Setup(repo => repo.GetAllPinsAsync()).ReturnsAsync(new List<GetAllPinsDTO>());

            var result=await _handler.Handle(new GetAllPinsQuery(), CancellationToken.None);
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
            _mockPinsRepository.Setup(repo => repo.GetAllPinsAsync()).ThrowsAsync(new Exception("An error occurred while processing your request."));

            var result = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetAllPinsQuery(), CancellationToken.None));
            Assert.Contains("An error occurred while processing your request", result.Message);
        }
    }
}
