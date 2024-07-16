using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PasswordHashedDemo_DotNetAPI.Models;

namespace PasswordHashedDemo_DotNetAPI.Services  //comment
{
    public interface IPasswordHashService
    {
        Task<Boolean> HashPassword(UserRegisterDTO userReg);

        Task<Boolean> ValidatePassWord(UserLoginDTO userLogin);
    }

    public class PasswordHashService:IPasswordHashService
    {
        private readonly ApplicationDbContext _dbContext;

        public PasswordHashService(ApplicationDbContext DbContext)
        {
            this._dbContext = DbContext;
        }

        public async Task<Boolean> HashPassword(UserRegisterDTO userRegDTO)
        {
            User user = new User();

            if (userRegDTO != null)
            {
                using(var hmac= new System.Security.Cryptography.HMACSHA256())
                {
                   byte[] SaltHashed = hmac.Key;
                   byte[] passwordHashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userRegDTO.Password));

                    user.Name = userRegDTO.Name;
                    user.Email = userRegDTO.Email;
                    user.SaltHashed = SaltHashed;
                    user.PasswordHashed = passwordHashed;
                }
            }

            if (user != null)
            {
                await _dbContext.AddAsync<User>(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            throw new Exception(" Register data is not valid OR  password could not be hashed!");

        }

        public async Task<Boolean> ValidatePassWord(UserLoginDTO userLogin)
        {
            User userStored = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
            
            if (userStored != null)
            {

                using (var hmac = new System.Security.Cryptography.HMACSHA256(userStored.SaltHashed))
                {
                    byte[] ComputedPasswordHashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userLogin.Password));   

                    return ComputedPasswordHashed.SequenceEqual(userStored.PasswordHashed);
                }

            }

            return false;

        }
    }
}
