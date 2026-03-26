using Interview_Test.Infrastructure;
using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InterviewTestDbContext _context;

    public UserRepository(InterviewTestDbContext context)
    {
        _context = context;
    }

    public dynamic GetUserById(string id)
    {
        var guid = Guid.Parse(id);
        return _context.UserTb
            .Where(u => u.Id == guid)
            .Select(u => new
            {
                id = u.Id,
                userId = u.UserId,
                username = u.Username,
                firstName = u.UserProfile.FirstName,
                lastName = u.UserProfile.LastName,
                age = u.UserProfile.Age,
                roles = u.UserRoleMappings
                    .Select(urm => new { roleId = urm.Role.RoleId, roleName = urm.Role.RoleName })
                    .ToList(),
                permissions = u.UserRoleMappings
                    .SelectMany(urm => urm.Role.Permissions.Select(p => p.Permission))
                    .Distinct()
                    .OrderBy(p => p)
                    .ToList()
            })
            .FirstOrDefault();
    }

    public int CreateUser(UserModel user)
    {
        foreach (var mapping in user.UserRoleMappings)
        {
            // ตัด circular reference ของ nested User ใน mapping
            mapping.User = null!;

            // ถ้า Role มีอยู่แล้วใน DB ให้ attach แทนการ insert ใหม่
            var existingRole = _context.RoleTb.Local.FirstOrDefault(r => r.RoleId == mapping.Role.RoleId)
                               ?? _context.RoleTb.Find(mapping.Role.RoleId);
            if (existingRole != null)
            {
                mapping.Role = existingRole;
            }
        }

        _context.UserTb.Add(user);
        return _context.SaveChanges();
    }
}
