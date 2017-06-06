using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class Textures
    {
        public Texture2D Player { get; set; }
        public Texture2D PlainBanjo { get; set; }
        public Texture2D HunterBanjo { get; set; }
        public Texture2D DeadlyBanjo { get; set; }
        public Texture2D Background1 { get; set; }
        public Texture2D Background2 { get; set; }
        public Texture2D Laser { get; set; }

        public void Load(ContentManager Content)
        {
            Player = Content.Load<Texture2D>("Textures/accordian");
            PlainBanjo = Content.Load<Texture2D>("Textures/PlainBanjo");
            HunterBanjo = Content.Load<Texture2D>("Textures/AttackerBanjo");
            DeadlyBanjo = Content.Load<Texture2D>("Textures/DeadlyStrummer");
            Background1 = Background2 = Content.Load<Texture2D>("Background/space");
            Laser = Content.Load<Texture2D>("Textures/note");
        }
    }
}
