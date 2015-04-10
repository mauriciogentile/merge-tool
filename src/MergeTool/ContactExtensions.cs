using Newtonsoft.Json;

namespace MergeTool
{
    public static class ContactExtensions
    {
        public static string Print(this Contact contact)
        {
            return string.Format("Name: {0} {1} | E-mail: {2} | Phone: {3} | Company: {4} | Title: {5}", contact.FirstName, contact.LastName,
                contact.EmailAddress, contact.PhoneNumber,
                contact.JobCompany, contact.JobTitle);
        }

        public static string ToJson(this Contact contact)
        {
            return JsonConvert.SerializeObject(contact);
        }
    }
}
