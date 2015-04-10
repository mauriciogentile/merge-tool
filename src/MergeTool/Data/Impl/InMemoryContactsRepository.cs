using System;
using System.Collections.Generic;
using System.Linq;

namespace MergeTool.Data
{
    public sealed class InMemoryContactsRepository : IRepository<Contact>
    {
        static List<Contact> _list = new List<Contact>();

        static InMemoryContactsRepository()
        {
            _list.AddRange(new[] {
                new Contact { EmailAddress = "Tester@tester.org" },
                new Contact { FirstName = "John", LastName = "Tester", EmailAddress = "john@tester.org", PhoneNumber = "703-555-1212", JobTitle = "Dude", JobCompany = "CircleBack" },
                new Contact { FirstName = "Jane", LastName = "Tester", EmailAddress = "jane@tester.org", PhoneNumber = "703-555-9876", JobTitle = "Dudette", JobCompany = "Dub Labs" },
                new Contact { FirstName = "Betty", LastName = "Tester", EmailAddress = "betty@tester.org", PhoneNumber = "703-555-2343", JobTitle = "Dudette", JobCompany = "IBM" },
                new Contact { FirstName = "Ted", LastName = "Nolan", EmailAddress = "ted@tester.org", PhoneNumber = "301-555-2343", JobTitle = "Coach", JobCompany = "Sabres" },
                new Contact { FirstName = "Dianna", LastName = "Russini", EmailAddress = "diana@tester.org", PhoneNumber = "212-555-2343", JobTitle = "Repoter", JobCompany = "NBC" },
                new Contact { FirstName = "Jim", LastName = "Kelly", EmailAddress = "jim@tester.org", PhoneNumber = "716-555-2343", JobTitle = "QA", JobCompany = "Bills" },
                new Contact { FirstName = "Tim", LastName = "Murray", EmailAddress = "tim@tester.org", PhoneNumber = "716-545-2343", JobTitle = "GM", JobCompany = "Sabres" }
            });
        }

        public InMemoryContactsRepository(List<Contact> initValues = null)
        {
            _list = initValues ?? _list;
        }

        public Contact Save(Contact entity)
        {
            _list.Add(entity);
            return entity;
        }

        public IQueryable<Contact> Find(Func<Contact, bool> predicate)
        {
            return _list.Where(predicate).AsQueryable();
        }

        public IQueryable<Contact> FindAll()
        {
            return _list.AsQueryable();
        }
    }
}
