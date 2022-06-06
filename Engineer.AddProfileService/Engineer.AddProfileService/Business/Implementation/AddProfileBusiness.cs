using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Engineer.AddProfileService.Business.Contracts;
using Engineer.AddProfileService.CustomException;
using Engineer.AddProfileService.Model;
using Engineer.AddProfileService.Repository.Contracts;

namespace Engineer.AddProfileService.Business.Implementation
{
    public class AddProfileBusiness: IAddProfileBusiness
    {
        public readonly IAddProfileRepository _repo;
        public AddProfileBusiness(IAddProfileRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Insert User Profile (Business)
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public async Task<ApiResponse> AddUserProfileBusiness(UserProfile userProfile)
        {
            ApiResponse response = new ApiResponse();
            bool profileStatus = ValidateUserProfileRequest(userProfile, ref response);
            if (profileStatus)
            {
                userProfile.UserId = await _repo.GetNextUserId();

                await _repo.AddUserProfileRepository(userProfile);

                response.Result = userProfile.UserId;
            }
            if (profileStatus == false)
            {
                response.Status.Status = "FAIL";
                response.Status.IsValid = false;
            }

            return response;
        }

        #region Private Methods (Validation of the Requests)
        /// <summary>
        /// Validate User Profile Request
        /// </summary>
        /// <param name="userProfile"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private bool ValidateUserProfileRequest(UserProfile userProfile, ref ApiResponse response)
        {
            ValidateExpertiseLevel(userProfile.TechnicalSkillExpertiseLevel);
            ValidateExpertiseLevel(userProfile.NonTechnicalSkillExpertiseLevel);
            
            if (!NotNullMaxMinLength(userProfile.Name, 5, 30))
            {
                response.Status.Message = "Name can't be NULL, minimum length 5 and max 30 characters.";
            }
            else if (!NotNullMaxMinLength(userProfile.AssociateId, 5, 30) || !userProfile.AssociateId.StartsWith("CTS"))
            {
                response.Status.Message = "AssociateId can't be NULL, minimum length 5 and max 30 characters and must start with 'CTS'.";
            }
            else if (!ValidateEmail(userProfile.Email))
            {
                response.Status.Message = "Email can't be NULL, it should have valid Email Pattern, containing a single @.";
            }
            else if (!NotNullMaxMinLength(userProfile.Mobile, 10, 10) || !long.TryParse(userProfile.Mobile, out _))
            {
                response.Status.Message = "Mobile can't be NULL, minimum 10 and max 10 character and all must be numeric.";
            }
            return string.IsNullOrEmpty(response.Status.Message);
        }

        /// <summary>
        /// Validate Expertise level
        /// </summary>
        /// <param name="myObject"></param>
        private void ValidateExpertiseLevel(object myObject)
        {
            if (myObject == null)
            {
                throw new InvalidExpertiseLevelException();
            }
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value) || !(int.TryParse(value, out int numericValue)))
                    {
                        //Expertise level for each skill set must not be empty or non-numeric value, else throw a custom exception
                        throw new InvalidExpertiseLevelException(value);
                    }
                    else
                    {
                        if (numericValue < 0 || numericValue > 20)
                        {
                            //Expertise level for each skill set must range between 0-20, else throw a custom exception
                            throw new InvalidExpertiseLevelException(numericValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Validate Not Null condition and MIN-MAX Length
        /// </summary>
        /// <param name="item"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        private bool NotNullMaxMinLength(string item, int minLength, int maxLength)
        {
            return !string.IsNullOrEmpty(item) && item.Length >= minLength && item.Length <= maxLength;
        }

        /// <summary>
        /// Validate Email pattern
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                        RegexOptions.CultureInvariant | RegexOptions.Singleline);
            return regex.IsMatch(email);
        }
        #endregion
    }
}
