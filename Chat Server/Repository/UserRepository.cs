﻿using Chat_Server.Repository.Interface;

using ChatRepository;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Chat_Server.Repository
{
    public class UserRepository : IUserRepository
    {
        ChatContext chatContext = new();
        public UserRepository()
        {
            chatContext.Database.EnsureCreated();
            
        }

        public async Task AddToken(User user, string token)
        {
            await chatContext.Tokens.AddAsync(new Tokens() { Token = token, UserId = user.Id });
            await chatContext.SaveChangesAsync();
        }

        public async Task AddToken(int userid, string token)
        {
            var user = await GetUser(userid);
            await chatContext.Tokens.AddAsync(new Tokens() { Token = token, UserId = user.Id });
            await chatContext.SaveChangesAsync();
        }

        public async Task<User> FindUser(string nickname)
        {
            var user = await chatContext.Users.FirstOrDefaultAsync(u => u.NickName.Contains(nickname));
            return user ?? new User() { Id = 0 };
        }

        public async Task<List<User>> FindUsers(string nickname)
        {
            var users = await chatContext.Users.Where(u => u.NickName.Contains(nickname)).ToListAsync();
            return users;
        }

        public async Task<List<User>> GetCompanyUsers(long id)
        {
            if (id < 0)
            {
                var users = await chatContext.Users.Where(c => c.CompanyId == id).ToListAsync();
                return users;
            }
            return new();
        }

        public async Task<Company> GetFullCompany(long id)
        {
            if (id > 0) throw new Exception("403");
            
            var company = await chatContext.Companys.Include("Boards").Include("Boards.Cards").FirstOrDefaultAsync(c => c.Id == id);
            return company;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await chatContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user ?? new User() { Id = 0 };
        }
        public async Task<User> GetUser(string token)
        {
            var user = await chatContext.Users.FirstOrDefaultAsync(x => x.Tokens.Any(y => y.Token == token));
            return user ?? new User() { Id = 0 };
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await chatContext.Users.FirstOrDefaultAsync(u => u.Login == username && u.CompanyId < 0 && u.Password == password);
            return user ?? new User() { Id = 0 };
        }
    }
}
