using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace JocDameMultyplayer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Client client1;
        private Texture2D _texture;
        private Color _color;

        public Game1()
        {
            //Initializarea tuturor variabilelor noastre 
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            client1 = new Client(); // Se declara un nou client participant la joc
            //_texture = Content.Load<Texture2D>();
            _color = Color.CornflowerBlue;

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            if (client1.Start())
            {
                _color = Color.Green;
            }
            else
            {
                _color = Color.Red;
            }
            base.Initialize();
            _texture = Content.Load<Texture2D>("checkerBlack");
            ///Se fac setarile pentru autosize
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += OnWindowClientSizeChanged;

        }

        private void OnWindowClientSizeChanged(object sender, EventArgs e)
        {
            // Other logic to handle the window size change
            GraphicsDevice.Viewport = new Viewport(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_color);

            _spriteBatch.Begin();
            if (client1.Active)
            {
                _spriteBatch.Draw(_texture, new Rectangle(client1.PlayerDetails.XPosiion, client1.PlayerDetails.YPosiion, 50, 50), Color.Black);
            }
            
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
