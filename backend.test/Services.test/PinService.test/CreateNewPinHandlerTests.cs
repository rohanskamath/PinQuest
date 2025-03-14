using AutoMapper;
using dotnetcorebackend.Application.DTOs.PinDTOs;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using dotnetcorebackend.Application.Services.Pinservice.Commands;
using dotnetcorebackend.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace backend.test.Services.test.PinService.test
{
    public class CreateNewPinHandlerTests
    {
        private readonly Mock<IPinsRepository> _mockPinsRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CreateNewPinHandler _createNewPinHandler;
        private readonly CreateNewPinCommand _createNewPinCommand;

        public CreateNewPinHandlerTests()
        {
            _mockPinsRepository = new Mock<IPinsRepository>();
            _mockMapper = new Mock<IMapper>();
            _createNewPinHandler = new CreateNewPinHandler(_mockPinsRepository.Object, _mockMapper.Object);
            _createNewPinCommand = new CreateNewPinCommand
            {
                UserId = Guid.NewGuid(),
                Title = "Sample Pin",
                Category = "Category",
                Description = "Description",
                Rating = 4,
                Latitude = 12.34,
                Longitude = 56.78,
            };
        }

        /// <summary>
        /// TC001:- Test case should return success when new pin is created
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Handle_ShouldCreateNewPin_RetutnOk()
        {
            var pin = new Pin
            {
                PinId = Guid.NewGuid(),
                Title = _createNewPinCommand.Title,
                Category = _createNewPinCommand.Category,
                Description = _createNewPinCommand.Description,
                Rating = _createNewPinCommand.Rating,
                Latitude = _createNewPinCommand.Latitude,
                Longitude = _createNewPinCommand.Longitude
            };

            _mockPinsRepository.Setup(repo => repo.CreateNewPinAsync(It.IsAny<Pin>())).ReturnsAsync(pin);
            _mockPinsRepository.Setup(repo => repo.AddUserPinAsync(It.IsAny<UserPin>())).ReturnsAsync(new UserPin());

            var pinDto = new PinDTO
            {
                PinId = pin.PinId,
                Title = pin.Title,
                Category = pin.Category,
                Description = pin.Description,
                Rating = pin.Rating,
                Latitude = pin.Latitude,
                Longitude = pin.Longitude
            };
            _mockMapper.Setup(mapper => mapper.Map<PinDTO>(It.IsAny<Pin>())).Returns(pinDto);

            var result=await _createNewPinHandler.Handle(_createNewPinCommand, CancellationToken.None);
            Assert.NotNull(result);
            Assert.True(result.GetType().GetProperty("success")?.GetValue(result) as bool?);
            Assert.Equal("Pins fetched successfully!", result.GetType().GetProperty("message")?.GetValue(result) as string);
            Assert.Equal(pinDto, result.GetType().GetProperty("data")?.GetValue(result) as PinDTO);
        }
    }
}
