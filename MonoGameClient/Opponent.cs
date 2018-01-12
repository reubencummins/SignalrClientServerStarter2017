using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameData;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameClient
{
    class Opponent : AnimatedSprite
    {
        public PlayerData Data { get; set; }

        public Opponent(Game game, Texture2D texture2D, PlayerData playerData, int frameCount):base(game, texture2D, new Vector2(playerData.playerPosition.X, playerData.playerPosition.Y), frameCount)
        {
            Data = playerData;
            PlayerColor = Color.Red;
            game.Components.Add(this);
        }

        public override void Update(GameTime gametime)
        {
            PreviousPosition = Position;
            Position = new Vector2(Data.playerPosition.X, Data.playerPosition.Y);
            

            base.Update(gametime);


        }
    }
}
