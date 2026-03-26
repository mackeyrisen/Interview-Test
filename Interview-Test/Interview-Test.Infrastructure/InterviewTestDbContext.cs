using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Interview_Test.Infrastructure;

public class InterviewTestDbContext : DbContext
{
    public InterviewTestDbContext(DbContextOptions<InterviewTestDbContext> options) : base(options)
    {
    }
    
    public DbSet<UserModel> UserTb { get; set; }
    public DbSet<UserProfileModel> UserProfileTb { get; set; }
    public DbSet<RoleModel> RoleTb { get; set; }
    public DbSet<UserRoleMappingModel> UserRoleMappingTb { get; set; }
    public DbSet<PermissionModel> PermissionTb { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User → UserProfile (1:1)
        modelBuilder.Entity<UserProfileModel>(entity =>
        {
            entity.HasOne(up => up.User)
                  .WithOne(u => u.UserProfile)
                  .HasForeignKey<UserProfileModel>("Id")
                  .HasPrincipalKey<UserModel>(u => u.Id)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // UserRoleMapping → User (Many:1)
        modelBuilder.Entity<UserRoleMappingModel>(entity =>
        {
            entity.HasOne(urm => urm.User)
                  .WithMany(u => u.UserRoleMappings)
                  .HasForeignKey("UserId")
                  .HasPrincipalKey(u => u.Id)
                  .OnDelete(DeleteBehavior.Cascade);

            // UserRoleMapping → Role (Many:1)
            entity.HasOne(urm => urm.Role)
                  .WithMany(r => r.UserRoleMappings)
                  .HasForeignKey("RoleId")
                  .HasPrincipalKey(r => r.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Role → Permission (1:Many)
        modelBuilder.Entity<PermissionModel>(entity =>
        {
            entity.HasOne(p => p.Role)
                  .WithMany(r => r.Permissions)
                  .HasForeignKey("RoleId")
                  .HasPrincipalKey(r => r.RoleId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

public class InterviewTestDbContextDesignFactory : IDesignTimeDbContextFactory<InterviewTestDbContext>
{
    public InterviewTestDbContext CreateDbContext(string[] args)
    {
        string connectionString = "Server=interview-test-sql.database.windows.net;Database=interview-test;User Id=admintest;Password=Gtxgtx120130!;TrustServerCertificate=True;";
        var optionsBuilder = new DbContextOptionsBuilder<InterviewTestDbContext>()
            .UseSqlServer(connectionString, opts => opts.CommandTimeout(600));

        return new InterviewTestDbContext(optionsBuilder.Options);
    }
}