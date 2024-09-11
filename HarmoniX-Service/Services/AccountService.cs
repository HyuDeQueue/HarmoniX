using HarmoniX_Repository.Models;
using HarmoniX_Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Service.Services
{
    public class AccountService
    {
        private AccountRepository _repo = new();

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _repo.GetAllAccountsAsync();
        }

        public async Task<Account> CheckAccountExistsAsync(Account account)
        {
            return await _repo.FindAccountAsync(account.Username);
        }

        public async Task RegisterAsync(Account account)
        {
            await _repo.CreateAccountAsync(account);
        }

        public async Task<Account> LoginAsync(Account account)
        {
            Account loggedIn = await _repo.GetAccountAsync(account);
            return loggedIn;
        }
    }
}
