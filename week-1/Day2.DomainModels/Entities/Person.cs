using System;

namespace Day2.DomainModels.Entities
{
    public abstract class Person
    {
        public string Name { get; protected set; }
        public string Email { get; protected set; }

        protected Person(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty", nameof(email));

            Name = name;
            Email = email;
        }

        public abstract string GetDisplayInfo();

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email cannot be empty", nameof(newEmail));

            Email = newEmail;
        }
    }
}
