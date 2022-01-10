using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace YIKES.Class
{
    class Player
    {

        private Sprite Sprite;
        private TilesManager Board;
        private ContentManager Content;

        private bool isOnGround = false;
        public bool isMoving = false;
        public bool isJumping = false;

        private float MoveSpeed = 1000f;
        private float Mass = 100f;
        private float JumpAccel = 1000f;

        private Vector2 Acceleration = new Vector2();
        private Vector2 Speed = new Vector2();
        private Vector2 Gravity = new Vector2(0, 9.81f);

        private const float MaxSpeed = 1000f;

        public Vector2 Movement = Vector2.Zero;

        public Vector2 Position;
        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), (int)Sprite.FrameSize.X, (int)Sprite.FrameSize.Y);
            }
        }

        public Player(TilesManager _board, ContentManager _content)
        {
            Board = _board;
            Content = _content;
        }

        public void LoadPlayer(Vector2 initialTile)
        {
            Sprite = new Sprite(Content.Load<Texture2D>("Player/_Idle"), new Vector2(21, 38));

            Position = initialTile * Tile.Size;
        }

        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Sprite.Update(elapsed);

            applyPhysicsLaw(elapsed);

            handleCollision();
        } 

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, Position);
        }

        public void applyPhysicsLaw(float elapsed)
        {
            Acceleration = Gravity * Mass;

            if (isMoving)
                Run();

            if (isOnGround)
                Acceleration *= 10;
            else
                Acceleration *= 0.2f;

            if (isJumping)
                Jump();

            Speed += Acceleration * elapsed;
            Speed.Y = Math.Clamp(Speed.Y, -MaxSpeed, MaxSpeed);

            Position += Speed * elapsed;
            Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));
        }

        public void handleCollision()
        {
            Rectangle playerBound = BoundingRectangle;

            int leftTile = (int)Math.Floor((float)playerBound.Left / Tile.Width);
            int rightTile = (int)Math.Ceiling(((float)playerBound.Right / Tile.Width)) - 1;
            int topTile = (int)Math.Floor((float)playerBound.Top / Tile.Height);
            int bottomTile = (int)Math.Ceiling(((float)playerBound.Bottom / Tile.Height)) - 1;

            isOnGround = false;

            for (int y = topTile; y <= bottomTile; ++y)
            {
                for (int x = leftTile; x <= rightTile; ++x)
                {
                    Rectangle tileBound = Board.GetBounds(new Vector2(x, y));
                    Vector2 depth = RectangleUtils.GetIntersectionDepth(playerBound, tileBound);

                    if (x < 0 || x >= Board.TilesSize.X || y < 0 || y >= Board.TilesSize.Y)
                        return ;

                    if (Board.Tiles[x, y].Collision == TileCollision.Impassable && depth != Vector2.Zero)
                    {
                        if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                        {
                            Position = new Vector2(Position.X, Position.Y + depth.Y);
                            playerBound = BoundingRectangle;
                            isOnGround = true;
                        }
                        /* if (Math.Abs(depth.Y) > 0f)
                        {
                            Position = new Vector2(Position.X, Position.Y + depth.Y);
                            playerBound = BoundingRectangle;
                        } */

                        depth = RectangleUtils.GetIntersectionDepth(playerBound, tileBound);
                        if (depth != Vector2.Zero && Math.Abs(depth.X) > 0f)
                        {
                            Position = new Vector2(Position.X + depth.X, Position.Y);
                            playerBound = BoundingRectangle;
                        }
                        Speed = Vector2.Zero; 
                    }
                }
            }
        }
    
        public void Run()
        {
            Acceleration += Movement * MoveSpeed;
        }

        public void Jump()
        {
            Acceleration = new Vector2(0, -1) * JumpAccel;
            isJumping = false;
        }
    }
}
