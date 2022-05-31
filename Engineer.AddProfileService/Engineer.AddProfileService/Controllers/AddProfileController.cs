using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Engineer.AddProfileService.Business.Contracts;
using Engineer.AddProfileService.Kafka;
using Engineer.AddProfileService.Model;

namespace Engineer.AddProfileService.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("skill-tracker/api/v{version:apiVersion}/engineer")]
    [Produces("application/json")]
    public class AddProfileController: ControllerBase
    {
        private readonly ILogger<AddProfileController> _logger;
        private readonly ProducerConfig _config;
        private readonly IAddProfileBusiness _addProfileBC;
        private readonly IProducerWrapper _producerWrapper;

        public AddProfileController(ILogger<AddProfileController> logger, ProducerConfig config, IAddProfileBusiness addProfileBC, IProducerWrapper producerWrapper)
        {
            _addProfileBC = addProfileBC;
            _producerWrapper = producerWrapper;
            _logger = logger;
            _config = config;
        }

        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpPost("add-profile")]
        public async Task<IActionResult> AddUserProfile(UserProfile userProfile)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                bool insertOperationFlag = false;
                if (ModelState.IsValid)
                {
                    userProfile.CreatedDate = DateTime.Now;
                    userProfile.UpdatedDate = DateTime.Now;
                    response = await _addProfileBC.AddUserProfileBusiness(userProfile);

                    long result = 0;
                    long.TryParse(Convert.ToString(response.Result ?? 0), out result);
                    if (result > 0)
                    {
                        userProfile.UserId = result;
                        PublishEvent(userProfile);
                        insertOperationFlag = true;
                    }
                }
                else
                {
                    response.Status.Message = "Invalid Input";
                    response.Status.Status = "FAIL";
                    response.Status.IsValid = false;
                }

                if (insertOperationFlag)
                {
                    _logger.LogInformation("{date} : AddUserProfile of the AddProfileController executed.", DateTime.UtcNow);
                    return StatusCode(200, response);
                }
                else
                {
                    _logger.LogInformation("{date} : AddUserProfile of the AddProfileController Failed : Message {message} ", DateTime.UtcNow, response.Status.Message);
                    return StatusCode(405, response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unknown error occurred on the AddUserProfile of the AddProfileController.");
                throw ex;
            }
        }

        /// <summary>
        /// Publish Add UserProfile event to profileforadminaddtopic (KAFKA Code)
        /// </summary>
        /// <param name="userProfileForAdmin"></param>
        private async void PublishEvent(UserProfile userProfileForAdmin)
        {
            string serializedUserProfileForAdmin = JsonConvert.SerializeObject(userProfileForAdmin);
            await _producerWrapper.WriteMessage(serializedUserProfileForAdmin, "profileforadminaddtopic");

            _logger.LogInformation("{date} : PublishEvent of the AddProfileController executed.", DateTime.UtcNow);
        }
    }
}