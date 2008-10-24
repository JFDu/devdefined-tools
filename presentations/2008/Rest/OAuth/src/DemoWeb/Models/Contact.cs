using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoRailsOAuth.Web.Models
{
    public class Contact
    {
        private static List<Contact> _contacts;

        static Contact()
        {
            Reset();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Blog { get; set; }

        public static void Reset()
        {
            _contacts = new List<Contact>
            {
                new Contact {Id = 1, Name = "Ayende", Blog = "http://ayende.com/Blog/Default.aspx"},
                new Contact {Id = 2, Name = "Andre Murer", Blog = "http://blog.andremeurer.com/"},
                new Contact {Id = 3, Name = "Keith Nicholas", Blog = "http://designingcode.blogspot.com/"},
                new Contact  {Id = 4, Name = "Nigel Parker", Blog = "http://blogs.msdn.com/nigel/default.aspx"}
            };
        }

        public static Contact FindById(int id)
        {
            return _contacts.Where(c => c.Id == id).First();
        }

        public static Contact[] FindAll()
        {
            return _contacts.ToArray();
        }

        public static void AddNew(Contact c)
        {
            if (c.Id != 0)
            {
                throw new ApplicationException("Customer should not have id assigned yet");
            }

            int maxID = _contacts.Max(cust => cust.Id);
            c.Id = maxID + 1;
            _contacts.Add(c);
        }

        public static void UpdateContact(Contact c)
        {
            Contact current = _contacts.Where(t => t.Id == c.Id).First();
            if (current != null)
            {
                _contacts.Remove(current);
                _contacts.Add(c);
            }
            else
            {
                throw new ApplicationException("Customer " + c.Id + " does not exist");
            }
        }

        public static void Delete(int id)
        {
            _contacts.RemoveAll(t => t.Id == id);
        }
    }
}