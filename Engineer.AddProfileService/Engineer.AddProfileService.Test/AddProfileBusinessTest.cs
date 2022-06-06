using System;
using System.Threading.Tasks;
using Engineer.AddProfileService.Business.Implementation;
using Engineer.AddProfileService.CustomException;
using Engineer.AddProfileService.Model;
using Engineer.AddProfileService.Repository.Contracts;
using Moq;
using Xunit;

namespace Seller.AddProfileService.Test
{
    public class AddProfileBusinessTest
    {
        readonly Mock<IAddProfileRepository> _mockAddProfileRepo = new Mock<IAddProfileRepository>();

        [Fact]
        public async Task AddUserProfileBusiness_ValidRequest()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS03",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }

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

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            _mockAddProfileRepo.Setup(x => x.AddUserProfileRepository(It.IsAny<UserProfile>())).Returns(Task.FromResult(response));
            _mockAddProfileRepo.Setup(x => x.GetNextUserId()).Returns(Task.FromResult(Convert.ToInt64(2)));

            var result = await _testObject.AddUserProfileBusiness(request);

            Assert.Equal((long)2, result.Result);

        }

        [Fact]
        public async Task AddUserProfileBusiness_InValidName()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS03",
                Name = "mana",
                Mobile = "3231453213",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }

            };
            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var result = await _testObject.AddUserProfileBusiness(request);
            Assert.Equal("Name can't be NULL, minimum length 5 and max 30 characters.", result.Status.Message);

        }
        [Fact]
        public async Task AddUserProfileBusiness_InValidAssociateID()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "1",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var result = await _testObject.AddUserProfileBusiness(request);
            Assert.Equal("AssociateId can't be NULL, minimum length 5 and max 30 characters and must start with 'CTS'.", result.Status.Message);

        }
        [Fact]
        public async Task AddUserProfileBusiness_InValidmailID()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS14",
                Name = "manas",
                Mobile = "3231453213",
                Email = "manas",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var result = await _testObject.AddUserProfileBusiness(request);
            Assert.Equal("Email can't be NULL, it should have valid Email Pattern, containing a single @.", result.Status.Message);

        }
        [Fact]
        public async Task AddUserProfileBusiness_InValidMobileNumber()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS14",
                Name = "manas",
                Mobile = "323145321",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "19",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var result = await _testObject.AddUserProfileBusiness(request);
            Assert.Equal("Mobile can't be NULL, minimum 10 and max 10 character and all must be numeric.", result.Status.Message);

        }

        [Fact]
        public async Task AddUserProfileBusiness_ExpertiselevelError()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS14",
                Name = "manas",
                Mobile = "3231453212",
                Email = "manas@gmail.com"
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var ex = await Assert.ThrowsAsync<InvalidExpertiseLevelException>(async () => await _testObject.AddUserProfileBusiness(request));
            Assert.Equal("Expertise level can't be null.", ex.Message);

        }
        [Fact]
        public async Task AddUserProfileBusiness_nullExpertiselevel()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS14",
                Name = "manas",
                Mobile = "3231453212",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {

                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3"
                }
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var ex = await Assert.ThrowsAsync<InvalidExpertiseLevelException>(async () => await _testObject.AddUserProfileBusiness(request));
            Assert.Equal("Invalid Expertise Level <NULL/EMPTY>. Expertise level must not be non empty or a non-numeric value.", ex.Message);
        }
        [Fact]
        public async Task AddUserProfileBusiness_wrongExpertiselevel()
        {
            UserProfile request = new UserProfile
            {
                AssociateId = "CTS14",
                Name = "manas",
                Mobile = "3231453212",
                Email = "manas@gmail.com",
                TechnicalSkillExpertiseLevel = new TechnicalSkillExpertiseLevel
                {
                    AzureExpertiseLevel = "0",
                    DockerExpertiseLevel = "1",
                    EntityFrameworkExpertiseLevel = "25",
                    GitExpertiseLevel = "5",
                    HTMLCSSJavaScriptExpertiseLevel = "2",
                    JenkinsExpertiseLevel = "6",
                    ReactExpertiseLevel = "3",
                    RestfulExpertiseLevel = "8",
                    AngularExpertiseLevel = "3",
                    AspNetCoreExpertiseLevel = "10"
                },
                NonTechnicalSkillExpertiseLevel = new NonTechnicalSkillExpertiseLevel
                {
                    AptitudeExpertiseLevel = "10",
                    CommunicationExpertiseLevel = "3",
                    SpokenExpertiseLevel = "1"
                }
            };

            AddProfileBusiness _testObject = new AddProfileBusiness(_mockAddProfileRepo.Object);
            var ex = await Assert.ThrowsAsync<InvalidExpertiseLevelException>(async () => await _testObject.AddUserProfileBusiness(request));
            Assert.Equal("Invalid Expertise Level 25. Expertise level for each skill must range between 0-20.", ex.Message);

        }
    }
}
