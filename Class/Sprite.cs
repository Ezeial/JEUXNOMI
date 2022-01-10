using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace YIKES.Class
{
    class Sprite
    {
        public Texture2D Texture;
        public Vector2 FrameSize;

        public Vector2 Origin
        {
            get { return new Vector2(FrameIndex * FrameSize.X, 0); }
        }

        public float AnimationTime = 1f;
        public float TimeTemp = 0f;
        public float FrameIndex = 0;

        public float FrameNumber
        {
            get { return (float)Math.Round(Texture.Width / FrameSize.X); }
        }

        public float FrameTime
        {
            get { return AnimationTime / FrameNumber; }
        }

        public Rectangle Frame
        {
            get
            {
                return new Rectangle(Origin.ToPoint(), FrameSize.ToPoint());
            }
        }

        public Sprite(Texture2D _texture, Vector2 _frame_size)
        {
            Texture = _texture;
            FrameSize = _frame_size;
        }

        public void Update(float elapsed)
        {
            TimeTemp += elapsed;

            while (TimeTemp > FrameTime)
            {
                FrameIndex = (FrameIndex + 1) % FrameNumber;
                TimeTemp -= FrameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Rectangle _frame = Frame;
            spriteBatch.Draw(Texture, position, _frame, Color.White);
        }
    }
}
