using HarmoniX_Repository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarmoniX_Repository.Repositories
{
    public class AccountRepository
    {
        private HarmonixDbContext _context;

        public async Task<Account> GetAccountAsync(Account account)
        {
            _context = new HarmonixDbContext();

            var result = await _context.Accounts
                .Where(x => x.Username == account.Username)
                .FirstOrDefaultAsync();

            if (result != null && BCrypt.Net.BCrypt.Verify(account.Password, result.Password))
            {
                return result;
            }

            return null;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            _context = new HarmonixDbContext();
            var result = await _context.Accounts
                .ToListAsync();
            return result;
        }

        public async Task CreateAccountAsync(Account account)
        {
            _context = new HarmonixDbContext();
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account> FindAccountAsync(string username)
        {
            _context = new HarmonixDbContext();
            var result = await _context.Accounts
                .Where(x => x.Username == username)
                .FirstOrDefaultAsync();
            return result;
        }
    }
}
