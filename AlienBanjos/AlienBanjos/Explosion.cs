using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienBanjos
{
    public class Explosion
    {
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float timer;
        public float interval;
        public int currentFrame, spriteWidth, spriteHeight;
        public Rectangle sourceRect;
        public bool isVisible;

        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth =  96;
            spriteHeight = 96;
            isVisible = true;
        }

        //Load Content
        public void LoadContent(ContentManager content)
        {
        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Increase the timer by the number of miliseconds since update was last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Check the timer is more than the chosen interval
            if (timer > interval)
            {
                //Show next frame
                currentFrame++;
                //Reset Timer
                timer = 0f;
            }

            // If we're on last frame make the explosion invisible and reset current frame to beginning of sprite sheet
            if (currentFrame == 17)
            {
                isVisible = false;
                currentFrame = 0;
            }

            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
        }

        // Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            // if visible then draw
            if (isVisible == true)
            {
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0);
            }
        }

    }
}
