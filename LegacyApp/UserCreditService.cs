using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyApp
{
    public class UserCreditService : IDisposable
    {
        /// <summary>
        /// Simulating database
        /// </summary>
        private readonly Dictionary<string, int> _database =
            new Dictionary<string, int>()
            {
                {"Kowalski", 200},
                {"Malewski", 20000},
                {"Smith", 10000},
                {"Doe", 3000},
                {"Kwiatkowski", 1000}
            };
        
        public void Dispose()
        {
            //Simulating disposing of resources
        }

        /// <summary>
        /// This method is simulating contact with remote service which is used to get info about someone's credit limit
        /// </summary>
        /// <returns>Client's credit limit</returns>
        internal int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            int randomWaitingTime = new Random().Next(3000);
            Thread.Sleep(randomWaitingTime);

            if (_database.ContainsKey(lastName))
                return _database[lastName];

            throw new ArgumentException($"Client {lastName} does not exist");
        }

        internal static (bool HasLimit, int LimitValue) DetermineCreditLimit(
            string clientType,
            string userLastName,
            DateTime userDateOfBirth
        )
        {
            if (clientType == "VeryImportantClient")
            {
                return (false, 0);
            }

            using var userCreditService = new UserCreditService();

            if (clientType == "ImportantClient")
            {
                int creditLimit = userCreditService.GetCreditLimit(userLastName, userDateOfBirth);
                return (false, creditLimit * 2);
            }

            return (true, userCreditService.GetCreditLimit(userLastName, userDateOfBirth));
        }

        internal static bool CheckUserLimitRequirements(bool hasLimit, int limitValue)
        {
            return !hasLimit || limitValue >= 500;
        }
    }
}