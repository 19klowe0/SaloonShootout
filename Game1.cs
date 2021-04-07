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

        //enemy information
        Model enemy1;
        Model enemy2;
        Model enemy3;
        Model enemy4;
        Model enemy5;
        Model enemy6;
        Vector3 enemy1Pos;
        Vector3 enemy2Pos;
        Vector3 enemy3Pos;
        Vector3 enemy4Pos;
        Vector3 enemy5Pos;
        Vector3 enemy6Pos;

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

            //Enemy Positions
            enemy1Pos = new Vector3(-2000, 150, -3500);
            enemy2Pos = new Vector3(2000, 150, -3500);
            enemy3Pos = new Vector3(0, 150, -4500);
            enemy4Pos = new Vector3(0, 150, -4500);
            enemy5Pos = new Vector3(250, 150, -3500);
            enemy6Pos = new Vector3(-250, 150, -3500);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = Content.Load<Model>("RevolverBeforeBake");
            saloon = Content.Load<Model>("Saloon_Environment");
            enemy1 = Content.Load<Model>("Cowboy1Tex");
            enemy2 = Content.Load<Model>("Cowboy2");
            enemy3 = Content.Load<Model>("Cowboy3");
            enemy4 = Content.Load<Model>("Cowboy4");
            enemy5 = enemy1;
            enemy6 = enemy2;

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
                view = Matrix.CreateLookAt(new Vector3(0 + playerPos.X, 10, 30),
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
            Matrix environment = Matrix.CreateRotationY(0)
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
            saloon.Draw(environment,view,proj);

            //Placing all enemies manually
            world = Matrix.CreateTranslation(enemy1Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy1.Meshes)
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

            enemy1.Draw(world, view, proj);

            world = Matrix.CreateTranslation(enemy2Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy2.Meshes)
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

            enemy2.Draw(world, view, proj);

            world = Matrix.CreateTranslation(enemy3Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy3.Meshes)
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

            enemy3.Draw(world, view, proj);

            world = Matrix.CreateTranslation(enemy4Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(-90)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy4.Meshes)
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

            enemy4.Draw(world, view, proj);

            world = Matrix.CreateTranslation(enemy5Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(-25)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy5.Meshes)
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

            enemy5.Draw(world, view, proj);

            world = Matrix.CreateTranslation(enemy6Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(25)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            //enable the lighting for the enemy meshes
            foreach (ModelMesh mesh in enemy6.Meshes)
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

            enemy6.Draw(world, view, proj);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

