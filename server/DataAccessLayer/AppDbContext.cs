using Domain.CSVModels;
using Domain.Models;
using Domain.Models.Youtube;
using Microsoft.EntityFrameworkCore;
using Helpers.Mappers;
using Helpers.CSV;
using Services.YoutubeAPI;
using Helpers.Strings;

namespace DataAccessLayer
{
    public class AppDbContext : DbContext
    {
        private readonly IYoutubeService _ytService;       
      
        public AppDbContext(DbContextOptions options, IYoutubeService ytService) : base(options)
        {
            _ytService = ytService;
        }   
        public DbSet<YoutubeChannel> Channels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ChannelCategory> ChannelCategories { get; set; }
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();
                entity.HasIndex(x=> x.NormalizedName).IsUnique();
                entity.HasKey(x => new { x.Name, x.NormalizedName });
            });

            modelBuilder.Entity<ChannelCategory>()
                .HasKey(c => new { c.ChannelId, c.CategoryName, c.CategoryNormalizedName });

            modelBuilder.Entity<ChannelCategory>()
                .HasOne(channelCategory => channelCategory.Channel)
                .WithMany(channel => channel.Categories)
                .HasForeignKey(channelCategory => channelCategory.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChannelCategory>()
                .HasOne(channelCategory => channelCategory.Category)
                .WithMany(category => category.Channels)
                .HasForeignKey(channelCategory => new { channelCategory.CategoryName, channelCategory.CategoryNormalizedName })
                .OnDelete(DeleteBehavior.Cascade);            
        }
    }
}
