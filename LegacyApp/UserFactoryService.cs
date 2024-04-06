#nullable enable
using System;

namespace LegacyApp
{
    internal class UserFactoryService
    {
        internal User? CreateNewIfValid(string firstName, string lastName, string email, DateTime dateOfBirth,
            int clientId)
        {
            var user = new User
            {
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (!_ValidateUserInfo(user))
            {
                return null;
            }
            
            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            user.Client = client;
            
            return user;
        }
        
        private int _GetUserAgeFromDoB(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }
        
        private bool _ValidateUserInfo(User user)
        {
            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
            {
                return false;
            }

            if (!user.EmailAddress.Contains("@") && !user.EmailAddress.Contains("."))
            {
                return false;
            }
            
            if (_GetUserAgeFromDoB(user.DateOfBirth) < 21)
            {
                return false;
            }

            return true;
        }
    }
}
