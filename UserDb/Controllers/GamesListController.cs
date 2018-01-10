using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using UserDb.Models;

namespace UserDb.Controllers
{
    public class GamesListController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public dynamic getGameList()
        {
            return db.Games.ToList();
        }
    }
}