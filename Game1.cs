using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SaloonShootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model gun;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gun = Content.Load<Model>("RevolverBeforeBake");

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60),
                                                                                  1,
                                                                                  0.001f,
                                                                                  100f);

            Matrix view = Matrix.CreateLookAt(new Vector3(0, 6, 20),
                                              new Vector3(0, 0, 0),
                                              new Vector3(0, 1, 0));

            Matrix world = Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            gun.Draw(world, view, proj);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
