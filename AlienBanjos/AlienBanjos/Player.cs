using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class Player
    {
        public int Health { get; set; }
        public int Score { get; set; }

        public Vector2 Position { get; set; }
        public int Speed { get; set; }
        public Direction? Direction { get; set; }

        public int screenHeight, screenWidth;
        public Rectangle rectangle;

        public Player()
        {

        }

        public int Width { get; set; }

        public int Height { get; set; }


        public Player(int screenHeight, int screenWidth)
        {
            Speed = 250;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            Position = new Vector2(screenWidth / 2, screenHeight - 100);
            Health = 100;
        }


        public void Update(float dt)
        {
            float distance = Speed * dt;
            Vector2 direction;

            // Movement
            if (!Direction.HasValue)
            {
                return;
            }

            if (Direction.Value == AlienBanjos.Direction.Left)
            {
                direction = new Vector2(-1, 0);
            }
            else if (Direction.Value == AlienBanjos.Direction.Right)
            {
                direction = new Vector2(1, 0);
            }
            else
            {
                return;
            }

            if (Position.X <= 0)
            {
                Position = new Vector2(Position.X + 5, Position.Y);
            }
            if (Position.X >= 700)
            {
                Position = new Vector2(Position.X - 5, Position.Y);
            }
            
            Position += direction * distance;
        }

    }
}
