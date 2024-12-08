using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Areas.Identity.Data;
using WebApplication1.Models;

namespace WebApplication1.Areas.Identity.Data;

public class AppDBcontext : IdentityDbContext<User>
{
    public AppDBcontext(DbContextOptions<AppDBcontext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);


        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        
        builder.ApplyConfiguration(new EventEntityConfiguration());

    }
}


public class EventEntityConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name)
                  .IsRequired()
                  .HasMaxLength(100); // Event name, required and max length 100

        builder.Property(e => e.Description)
               .IsRequired()
               .HasMaxLength(500); // Event description, required and max length 500

        builder.Property(e => e.Location)
               .IsRequired()
               .HasMaxLength(200); // Event location, required and max length 200

        builder.Property(e => e.EventDate)
               .IsRequired(); // Event date, required

        builder.Property(e => e.SeatsAvailable)
               .IsRequired(); // Seats available for the event, required

        // Define foreign key relationship with User
        builder.HasOne<User>()
               .WithMany() // One user can have many events
               .HasForeignKey(e => e.UserId)
               .OnDelete(DeleteBehavior.Cascade); // Ensure that if a user is deleted, their events are deleted too
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
{ 
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x=>x.FirstName).HasMaxLength(50);
        builder.Property(x=> x.LastName).HasMaxLength(50);
    }
}