using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Models
{
    public class UserService
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "usuario1",
                Email = "usuario1@example.com",
                PasswordHash = HashPassword("password123"),
                FirstName = "Usuario",
                LastName = "Demo"
            }
        };

        public List<User> GetAllUsers()
        {
            return _users;
        }

        public User GetUserById(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            
            if (user == null)
                return false;
                
            return VerifyPassword(password, user.PasswordHash);
        }

        public User AddUser(User user)
        {
            // Check if username or email already exists
            if (_users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("El nombre de usuario ya está en uso");
                
            if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("El email ya está registrado");
            
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            
            return user;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("Usuario no encontrado");
            
            // Check if new username conflicts with another user
            if (_users.Any(u => u.Id != user.Id && u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("El nombre de usuario ya está en uso");
                
            // Check if new email conflicts with another user
            if (_users.Any(u => u.Id != user.Id && u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("El email ya está registrado");
            
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new InvalidOperationException("Usuario no encontrado");
                
            user.PasswordHash = HashPassword(newPassword);
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}