using Joc.Library;
using JocDameMultyplayer.Manager;
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
        public Client client1{get; set;}
        public Client _client { get; set;}
        private ManagerInput _managerInput;
        private Texture2D _textureKingRed;
        private Texture2D _textureKingBlack;
        private Texture2D _textureBlack;
        private Texture2D _textureRed;
        private Texture2D whiteSquareTexture;
        private SpriteFont _font;
        private Constants _constants;

        private Color _color;

        public Game1(Client client)
        {
            //Initializarea tuturor variabilelor noastre 
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //client1 = new Client(); // Se declara un nou client participant la joc
            _client = client;
            //_texture = Content.Load<Texture2D>();
            _color = Color.CornflowerBlue;
            _constants = new Constants();
            //_managerInput = new ManagerInput(client1);
            _managerInput = new ManagerInput(_client);

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //client1.Start();
            base.Initialize();
            _textureBlack = Content.Load<Texture2D>("checkerBlack");
            _textureRed = Content.Load<Texture2D>("checkerRed");
            _textureKingRed = Content.Load<Texture2D>("checkerKingRed");
            _textureKingBlack = Content.Load<Texture2D>("checkerBlackKing");
            _graphics.PreferredBackBufferWidth =1000;  //SETAM 1000 pentru a adauga eventual si ceva nume 
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
            _font = Content.Load<SpriteFont>("File");
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
            Texture2D whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Color.Red });
            whiteSquareTexture = whiteTexture;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //client1.Update();
            _client.Update();
            _managerInput.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(client1.Active ? Color.Green : Color.Red);
            GraphicsDevice.Clear(_client.Active ? Color.Green : Color.Red);

            _spriteBatch.Begin();
            //if (client1.Active)
            if (_client.Active)
            {
                GraphicsDevice.Clear(Color.Blue);
                //Desenarea board-ului pe care clientul o primeste de la server updatata
                for (int row = 0; row < _constants.ROWS; row++)
                {
                    for (int col = row%2; col < _constants.ROWS; col+=2)
                    {
                        _spriteBatch.Draw(whiteSquareTexture, new Rectangle(row*_constants.SQUARE_SIZE, col * _constants.SQUARE_SIZE, _constants.SQUARE_SIZE, _constants.SQUARE_SIZE), Color.Red);
                    }

                }

                //adaugam si piesele

                for (int i = 0; i < _constants.ROWS; i++)
                {
                    for (int j = 0; j < _constants.COLS; j++)
                    {
                        //object obj = client1.Board.board[i, j];

                        object obj = _client.board.board[i, j];
                        //  object obj = _client.Players;
                        if (obj is Piece)
                        {
                            Piece piece = (Piece)obj;
                            if (piece.color == _constants.BLACK)
                            {
                                // _textureRed    _textureBlack                                
                                if (piece.king)
                                {
                                    _spriteBatch.Draw(_textureKingBlack, new Rectangle(piece.posX - _constants.SQUARE_SIZE / 2, piece.posY - _constants.SQUARE_SIZE / 2, _constants.SQUARE_SIZE - 2, _constants.SQUARE_SIZE - 2), Color.Red);
                                    // _textureKingBlack   _textureKingRed
                                }
                                else
                                {
                                    _spriteBatch.Draw(_textureBlack, new Rectangle(piece.posX - _constants.SQUARE_SIZE/2, piece.posY- _constants.SQUARE_SIZE / 2, _constants.SQUARE_SIZE - 2, _constants.SQUARE_SIZE - 2), Color.Red);
                                }
                            }
                            else if (piece.color == _constants.RED)
                            {
                                if (piece.king)
                                {
                                    _spriteBatch.Draw(_textureKingRed, new Rectangle(piece.posX - _constants.SQUARE_SIZE / 2, piece.posY - _constants.SQUARE_SIZE / 2, _constants.SQUARE_SIZE - 2, _constants.SQUARE_SIZE - 2), Color.Red);
                                    // _textureKingBlack   _textureKingRed
                                }
                                else
                                {
                                    _spriteBatch.Draw(_textureRed, new Rectangle(piece.posX - _constants.SQUARE_SIZE / 2, piece.posY - _constants.SQUARE_SIZE / 2, _constants.SQUARE_SIZE - 2, _constants.SQUARE_SIZE - 2), Color.Red);
                                }
                            }
                        }
                    }
                }

                if(_client.validMoves.Count != 0)
                {
                    foreach (var item in _client.validMoves)
                    {
                        _spriteBatch.Draw(_textureKingRed, new Rectangle(item.Item2*100 , item.Item1 * 100, 100, 100), Color.White);
                    }
                }

                //foreach (var otherplayer in client1.Players)
                foreach (var otherplayer in _client.Players)
                {
                    //if(otherplayer.Name == client1.Username)
                    if(otherplayer.Name == _client.Username)
                    {
                        _spriteBatch.DrawString(_font, otherplayer.Name, new Vector2(otherplayer.XPosition, otherplayer.YPosition - 20), Color.Red);
                        _spriteBatch.Draw(_textureKingRed, new Rectangle(otherplayer.XPosition, otherplayer.YPosition, 50, 50), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_font, otherplayer.Name, new Vector2(otherplayer.XPosition, otherplayer.YPosition - 20), Color.Black);
                        _spriteBatch.Draw(_textureBlack, new Rectangle(otherplayer.XPosition, otherplayer.YPosition, 50, 50), Color.White);
                    }
                    
                }
            }
            
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
