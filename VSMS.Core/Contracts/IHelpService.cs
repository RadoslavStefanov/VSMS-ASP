namespace VSMS.Core.Contracts
{
    public interface IHelpService
    {
        public Task<bool> CreateResetRequest(string userName);
    }
}
