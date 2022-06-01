using System;
namespace Engineer.AddProfileService.CustomException
{
    [Serializable]
    public class InvalidExpertiseLevelException : Exception
    {
        //public InvalidExpertiseLevelException() { }

        public InvalidExpertiseLevelException()
            : base(string.Format("Expertise level can't be null."))
        {

        }

        public InvalidExpertiseLevelException(int expertiseLevel)
            : base(string.Format("Invalid Expertise Level {0}. Expertise level for each skill must range between 0-20.", expertiseLevel))
        {

        }
        public InvalidExpertiseLevelException(string expertiseLevel)
            : base(string.Format("Invalid Expertise Level {0}. Expertise level must not be non empty or a non-numeric value.", string.IsNullOrEmpty(expertiseLevel)? "<NULL/EMPTY>": expertiseLevel))
        {

        }
    }
}
