using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedditClone.Common.Constants;
using RedditClone.Models;
using System;

namespace RedditClone.Data
{
    public class RedditCloneDbContext : IdentityDbContext<User>
    {
        public RedditCloneDbContext(DbContextOptions<RedditCloneDbContext> options)
            : base(options) { }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Subreddit> Subreddits { get; set; }

        public DbSet<UserSubreddit> UserSubreddits { get; set; }

        public DbSet<VotePost> VotesPosts { get; set; }

        public DbSet<VoteComment> VotesComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>(user =>
            {
                user.HasMany(u => u.Posts)
                    .WithOne(p => p.Author)
                    .HasForeignKey(p => p.AuthorId);

                user.HasMany(u => u.Comments)
                    .WithOne(c => c.Author)
                    .HasForeignKey(r => r.AuthorId);
            });

            builder.Entity<Post>(post =>
            {
                post.HasKey(p => p.Id);

                post.Property(p => p.Title)
                    .IsRequired()
                    .HasMaxLength(ModelsConstants.TitlePostMaxLength);

                post.Property(p => p.Description)
                    .IsRequired()
                    .HasMaxLength(ModelsConstants.DescriptionPostMaxLength);

                post.Property(p => p.PostDate)
                    .HasDefaultValue(DateTime.UtcNow)
                    .IsRequired();

                post.Property(p => p.UpVotesCount)
                    .IsRequired();

                post.Property(p => p.DownVotesCount)
                    .IsRequired();

                post.HasOne(p => p.Author)
                    .WithMany(u => u.Posts)
                    .HasForeignKey(p => p.AuthorId);

                post.HasOne(p => p.Subreddit)
                    .WithMany(s => s.Posts)
                    .HasForeignKey(p => p.SubredditId);

                post.HasMany(p => p.Comments)
                    .WithOne(c => c.Post)
                    .HasForeignKey(r => r.PostId);
            });

            builder.Entity<Comment>(comments =>
            {
                comments.HasKey(c => c.Id);

                comments.Property(c => c.Description)
                    .IsRequired()
                    .HasMaxLength(ModelsConstants.DescriptionPostMaxLength);

                comments.Property(c => c.PostDate)
                    .HasDefaultValue(DateTime.UtcNow)
                    .IsRequired();

                comments.Property(c => c.UpVotesCount)
                    .IsRequired();

                comments.Property(c => c.DownVotesCount)
                    .IsRequired();

                comments.HasOne(c => c.Author)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.AuthorId);

                comments.HasOne(c => c.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(c => c.PostId);
            });

            builder.Entity<Subreddit>(subreddit =>
            {
                subreddit.HasKey(s => s.Id);

                subreddit.Property(s => s.Name)
                    .HasMaxLength(ModelsConstants.TitleSubredditMaxLength)
                    .IsRequired();

                subreddit.HasIndex(s => s.Name)
                    .IsUnique();

                subreddit.Property(s => s.Description)
                    .IsRequired()
                    .HasMaxLength(ModelsConstants.DescriptionSubredditMaxLength);

                subreddit.Property(s => s.CreatedDate)
                    .HasDefaultValue(DateTime.UtcNow)
                    .IsRequired();

                subreddit.HasOne(s => s.Author)
                    .WithMany(u => u.CreatedSubreddits)
                    .HasForeignKey(s => s.AuthorId);

                subreddit.HasMany(s => s.Posts)
                    .WithOne(p => p.Subreddit)
                    .HasForeignKey(p => p.SubredditId);

                subreddit.HasMany(s => s.SubscribedUsers)
                    .WithOne(su => su.Subreddit)
                    .HasForeignKey(su => su.SubredditId);
            });

            builder.Entity<UserSubreddit>(userSubreddit =>
            {
                userSubreddit.HasKey(us => new { us.SubredditId, us.UserId });

                userSubreddit.HasOne(us => us.User)
                    .WithMany(u => u.SubscribedSubreddits)
                    .HasForeignKey(us => us.UserId);

                userSubreddit.HasOne(us => us.Subreddit)
                    .WithMany(s => s.SubscribedUsers)
                    .HasForeignKey(us => us.SubredditId);
            });

            builder.Entity<VotePost>(votePost =>
            {
                votePost.HasKey(v => v.Id);

                votePost.Property(v => v.Value)
                    .HasMaxLength(1)
                    .HasDefaultValue(0);

                votePost.HasOne(v => v.Post);

                votePost.HasOne(v => v.User)
                    .WithMany(u => u.VotesOnPosts)
                    .HasForeignKey(v => v.UserId);
            });

            builder.Entity<VoteComment>(voteComment =>
            {
                voteComment.HasKey(v => v.Id);

                voteComment.Property(v => v.Value)
                    .HasMaxLength(1)
                    .HasDefaultValue(0);

                voteComment.HasOne(v => v.Comment);

                voteComment.HasOne(v => v.User)
                    .WithMany(u => u.VotesOnComments)
                    .HasForeignKey(v => v.UserId);
            });

            base.OnModelCreating(builder);
        }
    }
}