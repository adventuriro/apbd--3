using System;

namespace LegacyApp
{
    public class UserService
    {
        private static ClientRepository _clientRepository;
        private static UserCreditService _userCreditService;

        public UserService()
        {
            _clientRepository ??= new ClientRepository();
            _userCreditService ??= new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!ValidateInput(firstName, lastName, email, dateOfBirth))
                return false;

            var client = _clientRepository.GetById(clientId);

            var user = CreateUser(firstName, lastName, email, dateOfBirth, client);

            CalculateCreditLimit(user);

            if (!IsCreditLimitSufficient(user))
                return false;

            SaveUserToDatabase(user);

            return true;
        }

        private bool ValidateInput(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                return false;

            return email.Contains("@") && email.Contains(".") && CalculateAge(dateOfBirth) >= 21;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age;
        }

        private User CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private void CalculateCreditLimit(User user)
        {
            if (user.Client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else
            {
                int creditLimit = _userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                user.CreditLimit = user.Client.Type == "ImportantClient" ? creditLimit * 2 : creditLimit;
                user.HasCreditLimit = true;
            }
        }

        private bool IsCreditLimitSufficient(User user)
        {
            return !user.HasCreditLimit || user.CreditLimit >= 500;
        }

        private void SaveUserToDatabase(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}
