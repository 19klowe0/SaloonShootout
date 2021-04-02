using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SaloonShootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //needed player information
        Vector3 playerPos;
        Vector3 saloonPos;
        Model player;
        Model saloon;
        Vector3 playerDir;
        float playerRot;

        //for different camera view for testing
        Vector3 camOffset;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            playerPos = new Vector3(0, 0, 0); //(0,0,0)
            saloonPos = new Vector3(0, -200, 0);
            playerRot = 0.0f;
            camOffset = new Vector3(0, 6, 20);
            playerDir = Vector3.Zero;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = Content.Load<Model>("RevolverBeforeBake");
            saloon = Content.Load<Model>("Saloon_Environment");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Felt like it was going too fast, so decreased the speed
            //rotate to the Right
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                playerRot -= .10f;
            }
            //rotate to the left
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerRot += .10f;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //increased the far cipping plane
            Matrix proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60),
                                                        (float)_graphics.PreferredBackBufferWidth / _graphics.PreferredBackBufferHeight,
                                                        .001f,
                                                        1000f);

           //code for rotation
            Matrix view = Matrix.CreateLookAt(
                Vector3.Transform(camOffset, Matrix.CreateRotationY(playerRot)),
                new Vector3(0, 0, 0),
                Vector3.Up);

            //in the distance camera 
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                view = Matrix.CreateLookAt(new Vector3(0 + playerPos.X, 0, 30),
                                         new Vector3(playerPos.X, 0, 0),
                                         Vector3.Up); //(0,1,0)
            }

            //matrix world for gun
            Matrix world =   Matrix.CreateRotationY(playerRot)
                                              * Matrix.CreateTranslation(playerPos)
                                              *Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //matrix world for saloon
            Matrix enviorment = Matrix.CreateRotationY(0)
                                              * Matrix.CreateTranslation(saloonPos)
                                              * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
                           Matrix.CreateTranslation(Vector3.Zero);



            //enable the lighting for the gun meshes
            foreach (ModelMesh mesh in player.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = proj;

                    effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                }
            }

            //enable the lighting for the saloon meshes
            foreach (ModelMesh mesh in saloon.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = proj;

                    effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                }
            }


            player.Draw(world, view, proj);
            saloon.Draw(enviorment,view,proj);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

