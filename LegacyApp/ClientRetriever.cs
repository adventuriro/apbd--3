namespace LegacyApp
{
    public class ClientRetriever
    {
        private ClientRepository clientRepository;

        public ClientRetriever(ClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public Client retrieveClient(int clientId)
        {
            return clientRepository.GetById(clientId);
        }
    }
}
