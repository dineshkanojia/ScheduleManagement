using System;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
namespace DataLayer
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<MeetingEvent> MeetingEvents { get; set; }
    }
}
