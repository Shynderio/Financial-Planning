using FinancialPlanning.Data.Data;
using FinancialPlanning.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanning.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IEmailRepository _emailRepository;

        public UserRepository(DataContext context, IEmailRepository emailRepository)
        {
            _context = context;
            _emailRepository = emailRepository;
        }



        //Get list user
        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users!
                .Include(u => u.Department)
                .Include(u => u.Position)
                .Include(u => u.Role).ToListAsync();
        }

        // Get list departments
        public async Task<List<Department>> GetListDepartments()
        {
            return await _context.Departments!.ToListAsync();
        }

        // Update user
        public async Task UpdateUser(Guid id, User user)
        {
            var updateUser = await _context.Users!.FindAsync(user.Id) ?? throw new Exception("User not found");

            updateUser.FullName = user.FullName;
            updateUser.Email = user.Email;
            updateUser.PhoneNumber = user.PhoneNumber;
            // Chuyển đổi định dạng ngày sinh từ "dd-MM-yyyy" sang "yyyy-MM-dd"
            DateTime dob;
            if (DateTime.TryParseExact(user.DOB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
            {
                updateUser.DOB = dob.ToString("dd-MM-yyyy");
            }
            else
            {
                // Xử lý trường hợp không thể chuyển đổi định dạng ngày sinh
                throw new Exception("Invalid date of birth format.");
            }
            updateUser.Address = user.Address;
            updateUser.DepartmentId = user.DepartmentId;
            updateUser.PositionId = user.PositionId;
            updateUser.RoleId = user.RoleId;
            updateUser.Notes = user.Notes;
            await _context.SaveChangesAsync();

        }

        //Add new User
        public async Task AddNewUser(User user)
        {
            var existingUser = _context.Users!.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {

                throw new Exception("Email already exists.");
            }
            string plainPassword = GeneratePassword();
            string createdUser = GenerateUserName(user.FullName);


            var newUser = new User
            {
                Username = GenerateUserName(user.FullName),
                Password = EncryptPassword(),
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB,
                Address = user.Address,
                DepartmentId = user.DepartmentId,
                PositionId = user.PositionId,
                RoleId = user.RoleId,
                Status = user.Status,
                Notes = user.Notes,
            };


            await _context.Users!.AddAsync(newUser);
            await _context.SaveChangesAsync();

            _emailRepository.SendWelcomeEmail(newUser.Username, plainPassword, newUser.Email, createdUser);

        }

        //Auto GenerateUserName
        private string GenerateUserName(string fullName)
        {
            string[] nameParts = fullName.Split(' ');

            // Extract last name
            string lastName = nameParts[nameParts.Length - 1];

            // Extract initials
            string initials = "";
            for (int i = 0; i < nameParts.Length - 1; i++)
            {
                initials += nameParts[i][0].ToString().ToUpper();
            }

            // Combine initials and last name
            string userName = $"{lastName}{initials}";

            // Check username existence
            if (_context != null && _context.Users != null)
            {
                var existingUserNames = _context.Users.Where(u => u.Username.StartsWith(userName)).Select(u => u.Username).ToList();
                int count = 1;
                string finalUserName = userName;
                while (existingUserNames.Contains(finalUserName))
                {
                    finalUserName = $"{userName}{count}";
                    count++;
                }

                return finalUserName;
            }
            else
            {
                throw new Exception("");
            }
        }

        // Auto generate password
        public static string GeneratePassword(int lenght = 8)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+=-";
            var random = new Random();
            var password = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        //EncryptPassword
        public static string EncryptPassword()
        {
            string plainPassword = GeneratePassword();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
            return hashedPassword;
        }

        //Get user by Id
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users!
                .Include(u => u.Department)
                .Include(u => u.Position)
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            // Thực hiện kiểm tra null ở đây nếu cần thiết

            if (user == null)
            {
                // Xử lý trường hợp không tìm thấy người dùng
                throw new Exception("User not found.");
            }

            return user;
        }

        //Update user status
        public async Task UpdateUserStatus(Guid id, int status)
        {
            var user = await _context.Users!.FindAsync(id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }
            user.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _context.Users!.FindAsync (id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            _context.Users!.Remove(user);
            await _context.SaveChangesAsync();
        }


    }
}

