using System;
using Xunit;
using Moq;
using Seller.AddProductService.Business.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Confluent.Kafka;
using Seller.AddProductService.Controllers;
using Seller.AddProductService.Model;
using Seller.AddProductService.Kafka;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Seller.AddProductService.Test
{
    public class AddProductControllerTest
    {
        readonly Mock<IAddProductBusiness> _mockProductBusiness = new Mock<IAddProductBusiness>();
        readonly Mock<IProducerWrapper> _mockProducerWrapper = new Mock<IProducerWrapper>();
        readonly Mock<ILogger<AddProductController>> _mockLogger = new Mock<ILogger<AddProductController>>();
        readonly Mock<ProducerConfig> _mockProducerConfig = new Mock<ProducerConfig>();

        [Fact]
        public async Task AddSellerProduct_ValidResponse()
        {
            Product request = new Product
            {
                ProductName = "xyzabc",
                ShortDescription = "xyzabc",
                DetailedDescription = "xyzabc",
                Category = "Painting",
                StartingPrice = 10,
                BidEndDate = DateTime.Now
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

            AddProductController _testObject = new AddProductController(_mockLogger.Object, _mockProducerConfig.Object, _mockProductBusiness.Object, _mockProducerWrapper.Object);
            ProducerWrapper _producerTestObject = new ProducerWrapper(_mockProducerConfig.Object);
            Mock<IProducer<string, string>> _mockProducerBuilder = new Mock<IProducer<string, string>>();
            _mockProducerWrapper.Setup(x => x.WriteMessage(It.IsAny<string>(), It.IsAny<string>()));
            _mockProductBusiness.Setup(x=>x.AddSellerProductBusiness(It.IsAny<Product>())).Returns(Task.FromResult(response));
            var result = (ObjectResult) await _testObject.AddSellerProduct(request);

            ApiResponse apiResponse = (ApiResponse)result.Value;

            Assert.NotNull(apiResponse.Result);
            Assert.Equal(2, (int)apiResponse.Result);
            Assert.NotNull(apiResponse.Status);
            Assert.True(apiResponse.Status.IsValid);
            Assert.Equal("SUCCESS", apiResponse.Status.Status);
            Assert.Empty(apiResponse.Status.Message);
        }
    }
}
