using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class HunterBanjo : Banjo
    {
        public int Speed { get; set; }
        public Direction Direction { get; set; }

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        float timeAlive = 0;

        public HunterBanjo()
        {

        }

        public HunterBanjo(int screenHeight, int screenWidth, Player player)
            : base(player)
        {
            Random randomStart = new Random(Guid.NewGuid().GetHashCode());
            Speed = 150;
            this.ScreenHeight = screenHeight;
            this.ScreenWidth = screenWidth;
            Position = new Vector2(randomStart.Next(1, screenWidth - 40), 0);
            Direction = Direction.Right;
        }

        public void Update(float dt, List<Laser> lasers)
        {
            timeAlive += dt;

            if (timeAlive < 5)
            {
                float distance = Speed * dt;
                Vector2 direction;

                if (Direction == Direction.Left)
                {
                    direction = new Vector2(-1, 0);
                }
                else if (Direction == Direction.Right)
                {
                    direction = new Vector2(1, 0);
                }
                else
                {
                    return;
                }

                Position += direction * distance;


                if (Position.X + Width >= ScreenWidth)
                {
                    Direction = Direction.Left;
                    Position = new Vector2(Position.X, Position.Y + (Height / 2));
                }
                else if (Position.X <= 0)
                {
                    Direction = Direction.Right;
                    Position = new Vector2(Position.X, Position.Y + (Height / 2));
                }

            }

            else
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
            }


            //Collisions

            if (LaserCollision(lasers))
            {
                Player.Score += 30;
            }

            if (PlayerCollsion())
            {
                Player.Health -= 30;
            }

        }
    }
}
