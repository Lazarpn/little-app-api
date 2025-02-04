using LittleApp.Common.Enums;
using LittleApp.Entities;
using LittleApp.Entities.Identity;
using LittleApp.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LittleApp.Data;

public class LittleAppDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public virtual DbSet<Entities.Task> Tasks { get; set; }
    public virtual DbSet<Memory> Memories { get; set; }
    public virtual DbSet<Vote> Votes { get; set; }

    public LittleAppDbContext(DbContextOptions<LittleAppDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(r => r.RoleClaims)
                .WithOne(rc => rc.Role)
                .HasForeignKey(rc => rc.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(u => u.Claims)
                .WithOne(uc => uc.User)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Logins)
                .WithOne(ul => ul.User)
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.Tokens)
                .WithOne(ut => ut.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            entity.HasIndex(u => u.Email).IsUnique();
        });
    }

    public override int SaveChanges()
    {
        PopulateEntityFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        PopulateEntityFields();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PopulateEntityFields()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                var entity = (IEntity)entityEntry.Entity;
                if (entity.CreatedAt == default)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                ((IEntity)entityEntry.Entity).ModifiedAt = DateTime.UtcNow;
            }
        }
    }

    public async System.Threading.Tasks.Task SeedData(UserManager<User> userManager, RoleManager<Role> roleManager)
    {
        Role[] roles =
        {
            new() { Name = UserRoleConstants.Administrator },
        };

        bool adminRoleExists = await roleManager.RoleExistsAsync(UserRoleConstants.Administrator);

        if (!adminRoleExists)
        {
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }
    }
}
