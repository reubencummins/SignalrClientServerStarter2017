using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserDb.Models;

namespace UserDb.Controllers
{
    public class AchievementController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Achievement
        public IEnumerable<Achievement> Get()
        {
            return db.Achievements.ToArray();
        }

        // GET: api/Achievement/5
        public dynamic Get(int id)
        {
            return db.Achievements.FirstOrDefault(a => a.ID == id);
        }

        public dynamic GetGameAchievements(int id)
        {
            return db.Achievements.Where(a => a.GameID == id);
        }

        //// POST: api/Achievement
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Achievement/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Achievement/5
        //public void Delete(int id)
        //{
        //}

        //POST
        [Route("api/PlayerAchievement")]
        public void UserAchieve([FromBody] string UserID, [FromBody] int NewAchievementID)
        {
            //IF matching PlayerAchievement does not exist, add new PlayerAchievement
            var user = db.Users.FirstOrDefault(p => p.Id == UserID);
            var ach = db.Achievements.FirstOrDefault(a => a.ID == NewAchievementID);
            if (user.PlayerAchievements.FirstOrDefault(pa => pa.Achievement == ach) == null)
            {
                user.PlayerAchievements.Add(new PlayerAchievement() { PlayerID = UserID, AchievementID = NewAchievementID });
            }
        }
    }
}
