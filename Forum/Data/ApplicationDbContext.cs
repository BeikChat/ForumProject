using System;
using System.Collections.Generic;
using System.Text;
using Forum.Data.Db;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Forum.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
		
		public DbSet<DbMessage> Messages { get; set; }
		public DbSet<DbForumSection> ForumSections { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<DbMessage>().ToTable("Message");
			modelBuilder.Entity<DbForumSection>().ToTable("ForumSection");
		}
	}
}