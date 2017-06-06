using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class Laser
    {
        
        public Vector2 Position { get; set; }     
        public int Speed { get; set; }
        public bool IsDead { get; set; }


        public static Texture2D texture;

        int screenHeight, screenWidth;

        public Laser()
        {

        }

        public Laser(int screenHeight, int screenWidth, Vector2 playerPosition)
        {
            Speed = 500;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            IsDead = false;
            Position = playerPosition;
        }

        public void Draw(SpriteBatch sb)
        {
            if (!IsDead)
            {
                sb.Draw(texture, Position, Color.White);
            }
        }

        public void Update(float dt)
        {

            if (!IsDead)
            {

                float distance = Speed * dt;
                Vector2 direction = new Vector2(0, -1);
                Position += direction * distance;

                // If shot is off top of screen
                if (Position.Y < 0)
                {
                    IsDead = false;
                }
            }
        }
    }
}
