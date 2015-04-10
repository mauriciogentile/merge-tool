using System;

namespace MergeTool
{
    public class Contact : ICloneable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string JobTitle { get; set; }
        public string JobCompany { get; set; }

        #region ICloneable Members

        public object Clone()
        {
            return new Contact
            {
                EmailAddress = EmailAddress,
                FirstName = FirstName,
                JobCompany = JobCompany,
                JobTitle = JobTitle,
                LastName = LastName,
                PhoneNumber = PhoneNumber
            };
        }

        #endregion
    }
}