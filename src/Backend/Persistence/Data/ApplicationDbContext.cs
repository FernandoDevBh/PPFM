using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Persistence.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
                                                              ApplicationRole,
                                                              Guid,
                                                              ApplicationUserClaim,
                                                              ApplicationUserRole,
                                                              ApplicationUserLogin,
                                                              ApplicationRoleClaim,
                                                              ApplicationUserToken
                                                              >
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>(u => 
            u.Property(p => p.DisplayName)
             .HasMaxLength(256)
             .IsRequired(false)
             .HasColumnName("display_name"));

        builder.Entity<ApplicationUser>(u => u.ToTable("fm_users"));
        builder.Entity<ApplicationRole>(u => u.ToTable("fm_roles"));
        builder.Entity<ApplicationUserClaim>(u => u.ToTable("fm_users_claims"));
        builder.Entity<ApplicationUserRole>(u => u.ToTable("fm_users_roles"));
        builder.Entity<ApplicationUserLogin>(u => u.ToTable("fm_users_logins"));
        builder.Entity<ApplicationRoleClaim>(u => u.ToTable("fm_roles_claims"));
        builder.Entity<ApplicationUserToken>(u => u.ToTable("fm_users_tokens"));
    }
}
