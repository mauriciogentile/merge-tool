namespace MergeTool
{
    public class SimpleIdentityMerge : IIdentityMerge<Contact>
    {
        public bool Merge(Contact contact1, Contact contact2)
        {
            bool modified = false;

            if (string.IsNullOrEmpty(contact1.FirstName) && !string.IsNullOrEmpty(contact2.FirstName))
            {
                contact1.FirstName = contact2.FirstName;
                modified = true;
            }

            if (string.IsNullOrEmpty(contact1.LastName) && !string.IsNullOrEmpty(contact2.LastName))
            {
                contact1.LastName = contact2.LastName;
                modified = true;
            }

            if (string.IsNullOrEmpty(contact1.JobCompany) && !string.IsNullOrEmpty(contact2.JobCompany))
            {
                contact1.JobCompany = contact2.JobCompany;
                modified = true;
            }

            if (string.IsNullOrEmpty(contact1.JobTitle) && !string.IsNullOrEmpty(contact2.JobTitle))
            {
                contact1.JobTitle = contact2.JobTitle;
                modified = true;
            }

            // email addresses should be merged
            if (string.IsNullOrEmpty(contact1.EmailAddress) && !string.IsNullOrEmpty(contact2.EmailAddress))
            {
                contact1.EmailAddress = contact2.EmailAddress;
                modified = true;
            }
            else if (!string.IsNullOrEmpty(contact2.EmailAddress) && contact1.EmailAddress != contact2.EmailAddress)
            {
                contact1.EmailAddress += ";" + contact2.EmailAddress;
                modified = true;
            }

            // phones should be merged
            if (string.IsNullOrEmpty(contact1.PhoneNumber) && !string.IsNullOrEmpty(contact2.PhoneNumber))
            {
                contact1.PhoneNumber = contact2.PhoneNumber;
                modified = true;
            }
            else if (!string.IsNullOrEmpty(contact2.PhoneNumber) && contact1.PhoneNumber != contact2.PhoneNumber)
            {
                contact1.PhoneNumber += ";" + contact2.PhoneNumber;
                modified = true;
            }

            return modified;
        }
    }
}