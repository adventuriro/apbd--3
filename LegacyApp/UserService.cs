using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            var user = new UserFactoryService().CreateNewIfValid(firstName, lastName, email, dateOfBirth, clientId);
            if (user == null)
            {
                return false;
            }
            
            (user.HasCreditLimit, user.CreditLimit) = UserCreditService.DetermineCreditLimit(user.Client.Type,
                user.LastName, user.DateOfBirth);

            if (_CheckUserBeforeAdding(user))
            {
                UserDataAccess.AddUser(user);
                return true;
            }

            return false;
        }

        private bool _CheckUserBeforeAdding(User user)
        {
            return UserCreditService.CheckUserLimitRequirements(user.HasCreditLimit, user.CreditLimit);
        }
    }
}
