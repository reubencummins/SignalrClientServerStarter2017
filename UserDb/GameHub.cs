﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GameData;
using System.Timers;
using UserDb.Controllers;

namespace Week21112016
{
    
    public class GameHub : Hub
    {
        static List<PlayerData> Players = new List<PlayerData>();
        static Random r = new Random();
        static int gameID = 1;

        static List<CollectableData> Collectables = new List<CollectableData>()
        {
            new CollectableData(0,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,10),
            new CollectableData(1,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,20),
            new CollectableData(1,
            new Position { X= r.Next(100,1800), Y = r.Next(100,1800) }
            ,30),

        };

        public static int WorldX = 2000;
        public static int WorldY = 2000;
        static bool PLAYING = false;
        public static Timer _startTime;
        private int achievementScoreTarget = 100;

        public void Hello()
        {
            Clients.All.hello();
        }

        public void join()
        {
            Clients.Caller.joined(WorldX,WorldY);
            Clients.Others.OpponentJoined();
            Clients.All.SpawnCollectables(Collectables);
        }

        public void Moved(string PlayerID, Position p)
        {
            Clients.All.Moved(PlayerID, p);
        }

        public void Message(string messageText)
        {
            Clients.All.Message(messageText);
        }

        public void FinalScore(int score, PlayerData player)
        {
            //call AchievementController UserScore
            var ach = new AchievementController();
            ach.UserScore(player.playerID, score, gameID);

            //check for achievement unlocking
            if (score>achievementScoreTarget)
            {
                ach.UserAchieve(player.playerID, 1);
            }
        }
    }
}