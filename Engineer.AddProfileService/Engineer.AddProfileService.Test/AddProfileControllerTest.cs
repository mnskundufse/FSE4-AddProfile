using System.Threading.Tasks;
using Confluent.Kafka;
using Engineer.AddProfileService.Business.Contracts;
using Engineer.AddProfileService.Controllers;
using Engineer.AddProfileService.Kafka;
using Engineer.AddProfileService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Seller.AddProfileService.Test
{
    public class AddProfileControllerTest
    {
        readonly Mock<IAddProfileBusiness> _mockProfileBusiness = new Mock<IAddProfileBusiness>();
        readonly Mock<IProducerWrapper> _mockProducerWrapper = new Mock<IProducerWrapper>();
        readonly Mock<ILogger<AddProfileController>> _mockLogger = new Mock<ILogger<AddProfileController>>();
        readonly Mock<ProducerConfig> _mockProducerConfig = new Mock<ProducerConfig>();

        [Fact]
        public async Task AddUserProfile_ValidRequest()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "1",
                Name = "manas",
                Mobile = "32314532132"
            };

            ApiResponse response = new ApiResponse()
            {
                Result = 2,
                Status = new StatusResponse
                {
                    IsValid = true,
                    Status = "SUCCESS",
                    Message = string.Empty
                }
            };

            AddProfileController _testObject = new AddProfileController(_mockLogger.Object, _mockProducerConfig.Object, _mockProfileBusiness.Object, _mockProducerWrapper.Object);
            ProducerWrapper _producerTestObject = new ProducerWrapper(_mockProducerConfig.Object);
            Mock<IProducer<string, string>> _mockProducerBuilder = new Mock<IProducer<string, string>>();
            _mockProducerWrapper.Setup(x => x.WriteMessage(It.IsAny<string>(), It.IsAny<string>()));
            _mockProfileBusiness.Setup(x => x.AddUserProfileBusiness(It.IsAny<UserProfile>())).Returns(Task.FromResult(response));
            var result = (ObjectResult)await _testObject.AddUserProfile(request);

            ApiResponse apiResponse = (ApiResponse)result.Value;

            Assert.NotNull(apiResponse.Result);
            Assert.Equal(2, (int)apiResponse.Result);
            Assert.NotNull(apiResponse.Status);
            Assert.True(apiResponse.Status.IsValid);
            Assert.Equal("SUCCESS", apiResponse.Status.Status);
            Assert.Empty(apiResponse.Status.Message);
        }

        [Fact]
        public async Task AddUserProfile_invalidRequest()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "1",
                Name = "manas",
                Mobile = "32314532132"
            };

            ApiResponse response = new ApiResponse()
            {
                Result = 0,
                Status = new StatusResponse
                {
                    IsValid = false,
                    Status = "FAIL",
                    Message = string.Empty
                }
            };

            AddProfileController _testObject = new AddProfileController(_mockLogger.Object, _mockProducerConfig.Object, _mockProfileBusiness.Object, _mockProducerWrapper.Object);
            ProducerWrapper _producerTestObject = new ProducerWrapper(_mockProducerConfig.Object);
            Mock<IProducer<string, string>> _mockProducerBuilder = new Mock<IProducer<string, string>>();
            _mockProducerWrapper.Setup(x => x.WriteMessage(It.IsAny<string>(), It.IsAny<string>()));
            _mockProfileBusiness.Setup(x => x.AddUserProfileBusiness(It.IsAny<UserProfile>())).Returns(Task.FromResult(response));
            var result = (ObjectResult)await _testObject.AddUserProfile(request);

            ApiResponse apiResponse = (ApiResponse)result.Value;

            Assert.NotNull(apiResponse.Result);
            Assert.Equal(0, (int)apiResponse.Result);
            Assert.NotNull(apiResponse.Status);
            Assert.False(apiResponse.Status.IsValid);
            Assert.Equal("FAIL", apiResponse.Status.Status);
        }
    }
}
