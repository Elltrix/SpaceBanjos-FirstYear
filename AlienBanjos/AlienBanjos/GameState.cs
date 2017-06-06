using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace AlienBanjos
{
    public class GameState
    {
        public Player Player { get; set; }
        public List<PlainBanjo> PlainBanjos { get; set; }
        public List<HunterBanjo> HunterBanjos { get; set; }
        public List<DeadlyBanjo> DeadlyBanjos { get; set; }
        public List<Laser> Lasers { get; set; }

        public float TotalGameTime { get; set; }
        public float MakeHarderAt { get; set; }
        public float LastFiredLaser { get; set; }


        public int MaxPlainBanjos { get; set; }
        public int MaxHunerBanjos { get; set; }
        public int MaxDeadlyBanjos { get; set; }



        [XmlIgnore]
        public Textures Textures { get; set; }



        private float laserFireRate = 0.2f;


        public GameState()
        {
        }

        public void CreateBanjos(int screenWidth, int screenHeight)
        {

            if (TotalGameTime >= MakeHarderAt)
            {
                MakeHarderAt += 3;

                MaxPlainBanjos += 3;
                MaxHunerBanjos += 2;
                MaxDeadlyBanjos += 1;
            }


            int currentPlainBanjos = 0;
            foreach (var plainBanjo in PlainBanjos)
            {
                if (!plainBanjo.IsDead)
                {
                    currentPlainBanjos++;
                }
            }
            int requiredNewPlainBanjos = MaxPlainBanjos - currentPlainBanjos;

            for (int i = 0; i < requiredNewPlainBanjos; i++)
            {
                var banjo = new PlainBanjo(screenHeight, screenWidth, Player);

                banjo.Width = Textures.PlainBanjo.Width;
                banjo.Height = Textures.PlainBanjo.Height;

                PlainBanjos.Add(banjo);
            }


            int currentHunterBanjos = 0;
            foreach (var hunterBanjos in HunterBanjos)
            {
                if (!hunterBanjos.IsDead)
                {
                    currentHunterBanjos++;
                }
            }
            int requiredNewHunterBanjos = MaxHunerBanjos - currentHunterBanjos;

            for (int i = 0; i < requiredNewHunterBanjos; i++)
            {
                var banjo = new HunterBanjo(screenHeight, screenWidth, Player);

                banjo.Width = Textures.HunterBanjo.Width;
                banjo.Height = Textures.HunterBanjo.Height;

                HunterBanjos.Add(banjo);
            }


            int currentDeadlyBanjos = 0;
            foreach (var deadlyBanjo in DeadlyBanjos)
            {
                if (!deadlyBanjo.IsDead)
                {
                    currentDeadlyBanjos++;
                }
            }
            int requiredNewDeadlyBanjos = MaxDeadlyBanjos - currentDeadlyBanjos;

            for (int i = 0; i < requiredNewDeadlyBanjos; i++)
            {
                var banjo = new DeadlyBanjo(screenHeight, screenWidth, Player);

                banjo.Width = Textures.DeadlyBanjo.Width;
                banjo.Height = Textures.DeadlyBanjo.Height;

                DeadlyBanjos.Add(banjo);
            }
        }

        public bool FireLaser(int screenWidth, int screenHeight)
        {
            if (TotalGameTime >= LastFiredLaser + laserFireRate)
            {
                LastFiredLaser = TotalGameTime;
                Lasers.Add(new Laser(screenWidth, screenHeight, Player.Position));

                return true;
            }

            return false;
        }

        public void NewGame(int screenWidth, int screenHeight, Textures textures)
        {
            Player = new Player(screenWidth, screenHeight);

            Player.Width = textures.Player.Width;
            Player.Height = textures.Player.Height;

            PlainBanjos = new List<PlainBanjo>();
            HunterBanjos = new List<HunterBanjo>();
            DeadlyBanjos = new List<DeadlyBanjo>();

            Lasers = new List<Laser>();

            Textures = textures;
        }

        static string savePath = @"C:\temp\save.xml";

        public void SaveGame()
        {
            var serializer = new XmlSerializer(this.GetType());

            if (HasExistingGame())
            {
                File.Delete(savePath);
            }

            using (var file = new StreamWriter(savePath))
            {
                serializer.Serialize(file, this);
            }
        }

        public static GameState LoadGame(int screenWidth, int screenHeight, Textures textures)
        {
            GameState state;

            var serializer = new XmlSerializer(typeof(GameState));

            using (var file = new StreamReader(savePath))
            {
                state = (GameState)serializer.Deserialize(file);
            }

            // fix some state

            state.Player.Width = textures.Player.Width;
            state.Player.Height = textures.Player.Height;

            state.Textures = textures;

            foreach (var plainBanjo in state.PlainBanjos)
            {
                plainBanjo.Player = state.Player;
                plainBanjo.ScreenWidth = screenWidth;
                plainBanjo.ScreenHeight = screenHeight;
            }

            foreach (var hunterBanjo in state.HunterBanjos)
            {
                hunterBanjo.Player = state.Player;
                hunterBanjo.ScreenWidth = screenWidth;
                hunterBanjo.ScreenHeight = screenHeight;
            }

            foreach (var deadlyBanjo in state.DeadlyBanjos)
            {
                deadlyBanjo.Player = state.Player;
            }

            return state;
        }

        public static bool HasExistingGame()
        {
            return File.Exists(savePath);
        }
    }
}
