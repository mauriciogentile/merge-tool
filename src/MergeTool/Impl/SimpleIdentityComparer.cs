using System;

namespace MergeTool
{
    public class SimpleIdentityComparer : IIdentityComparer<Contact>
    {
        private readonly double _confidence;

        public SimpleIdentityComparer(double confidence = 0.7)
        {
            if (confidence < 0 || confidence > 1)
            {
                throw new ArgumentOutOfRangeException("confidence", "Should be between 0 and 1");
            }

            _confidence = confidence;
        }

        public IdentityComparison Compare(Contact contact1, Contact contact2)
        {
            double confidence = 0;

            //first & last equals
            if (CompareStrings(contact1.FirstName, contact2.FirstName) &&
                CompareStrings(contact1.LastName, contact2.LastName))
            {
                confidence += 0.8;
            }
            if (CompareStrings(contact1.FirstName, contact2.FirstName))
            {
                confidence += 0.2;
            }
            if (CompareStrings(contact1.EmailAddress, contact2.EmailAddress) ||
                CompareStrings(contact1.PhoneNumber, contact2.PhoneNumber))
            {
                //e-mails and phone are almost unique (specially if contacts)
                confidence += 0.9;
            }
            if (CompareStrings(contact1.JobCompany, contact2.JobCompany))
            {
                confidence += 0.1;
            }
            if (CompareStrings(contact1.JobTitle, contact2.JobTitle))
            {
                confidence += 0.1;
            }
            if (confidence > 1)
            {
                confidence = 1;
            }
            return new IdentityComparison
            {
                Confidence = confidence,
                Match = confidence >= _confidence
            };
        }

        static bool CompareStrings(string str1, string str2)
        {
            if (str1 == null)
            {
                return false;
            }
            if (str2 == null)
            {
                return false;
            }
            if (string.Compare(str1.Trim(), str2.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            return false;
        }
    }
}