using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class DeadlyBanjo : Banjo
    {
        public int Speed { get; set; }


        int screenHeight, screenWidth;

        public DeadlyBanjo()
        {

        }

        public DeadlyBanjo(int screenHeight, int screenWidth, Player player) : base(player)
        {
            Random randomStart = new Random(Guid.NewGuid().GetHashCode());
            Speed = 100;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            Position = new Vector2(randomStart.Next(1, screenWidth - 1), 0);
        }
        

        public void Update(float dt, List<Laser> lasers)
        {
            float movement = Speed * dt;

            if (Position.X <= Player.Position.X)
            {
                Position = new Vector2(Position.X + movement, Position.Y);
            }
            else if (Position.X >= Player.Position.X)
            {
                Position = new Vector2(Position.X - movement, Position.Y);
            }

            if (Position.Y <= Player.Position.Y)
            {
                Position = new Vector2(Position.X, Position.Y + movement);
            }
            else if (Position.Y >= Player.Position.Y)
            {
                Position = new Vector2(Position.X, Position.Y - movement);
            }


            //Collisions

            if (LaserCollision(lasers))
            {
                Player.Score += 50;
            }

            if (PlayerCollsion())
            {
                Player.Health -= 50;
            }

        }

    }
}
