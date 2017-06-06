using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AlienBanjos
{
    public abstract class Banjo
    {
        [XmlIgnore]
        public Player Player { get; set; }

        public Vector2 Position { get; set; }

        public int ScoreValue { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        

        public bool IsDead { get; set; }

        public bool HasExploded { get; set; }


        public Banjo()
        {

        }

        public Banjo(Player player)
        {
            this.Player = player;
            this.IsDead = false;
            this.HasExploded = false;
        }


        public bool IsCollision(Vector2 point)
        {
            return
                point.X >= Position.X
             && point.X <= Position.X + Width
             && point.Y >= Position.Y
             && point.Y <= Position.Y + Height;
        }

        public bool LaserCollision(List<Laser> lasers)
        {
            bool isHit = false;

            foreach (Laser laser in lasers)
            {
                if (!laser.IsDead && IsCollision(laser.Position))
                {
                    IsDead = true;

                    // start explosion

                    laser.IsDead = true;

                    isHit = true;

                    System.Diagnostics.Debug.WriteLine("Enemy Hit");
                }
            }

            return isHit;
        }

        public bool PlayerCollsion()
        {
            Vector2 enemyBottomLeft = new Vector2(Position.X, Position.Y + Height);
            Vector2 enemyBottomRight = new Vector2(Position.X + Width, Position.Y + Height);

            if (IsPlayerCollsion(enemyBottomLeft) || IsPlayerCollsion(enemyBottomRight))
            {
                IsDead = true;

                System.Diagnostics.Debug.WriteLine("Player Hit");


                // lower the player health

                return true;
            }

            return false;

        }

        public bool IsPlayerCollsion(Vector2 point)
        {
            return
                point.X >= Player.Position.X
             && point.X <= Player.Position.X + Player.Width
             && point.Y >= Player.Position.Y
             && point.Y <= Player.Position.Y + Player.Height;
        }

    }
}
