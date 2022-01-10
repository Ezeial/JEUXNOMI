using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace YIKES
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Class.TilesManager tileManager;
        private Class.Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            tileManager = new Class.TilesManager(Content);
            player = new Class.Player(tileManager, Content);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here
            using (Stream fileStream = TitleContainer.OpenStream("Content/Level/0.txt"))
                tileManager.LoadAllTiles(fileStream);
            player.LoadPlayer(new Vector2(0, 0));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
             if (Keyboard.GetState().IsKeyDown(Keys.D))
             {
                player.isMoving = true;
                player.Movement = new Vector2(1, 0);
             }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player.isMoving = true;
                player.Movement = new Vector2(-1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                player.isJumping = true;
            }

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            _spriteBatch.Begin();

            tileManager.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
