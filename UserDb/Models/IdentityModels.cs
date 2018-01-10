using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Data.Entity;

namespace UserDb.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int XP { get; set; }
        public List<PlayerAchievement> PlayerAchievements { get; set; }
        public string DisplayName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    [Table("Achievement")]
    public class Achievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        [ForeignKey("Game")]
        public int GameID { get; set; }
        public Game Game { get; set; }
    }

    [Table("PlayerAchievement")]
    public class PlayerAchievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [ForeignKey("Achievement")]
        public int AchievementID { get; set; }
        [ForeignKey("Player")]
        public string PlayerID { get; set; }
        public virtual Achievement Achievement { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }

    [Table("Game")]
    public class Game
    {
        public Game()
        {
            Scores = new HashSet<GameScore>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameID { get; set; }

        public string GameName { get; set; }

        public virtual ICollection<GameScore> Scores { get; set; }

        public virtual ICollection<Achievement> Achievements { get; set; }
    }

    public class GameScore
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScoreID { get; set; }

        [ForeignKey("Game")]
        public int GameID { get; set; }

        [ForeignKey("Player")]
        public string PlayerID { get; set; }

        public int score { get; set; }

        public virtual Game Game { get; set; }
        public virtual ApplicationUser Player { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameScore> GameScores { get; set; }
        public virtual DbSet<PlayerAchievement> PlayerAchievements { get; set; }
        public virtual DbSet<Achievement> Achievements { get; set; }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}