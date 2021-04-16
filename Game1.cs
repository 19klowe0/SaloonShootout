using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SaloonShootout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //icons and timer
        Texture2D bulletIcon;
        SpriteFont gameFont;
        float timer;
        int score;
        int bulletCount = 6;

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

        //bullet information 
        //bulletPos will probably need to be array since multiple can be on screen
        Model bullet;

        List<Projectile> bullets;
        List<Enemy> enemies;

        MeshCollider worldMesh;


        //mousestate
        MouseState mstate;
        bool mRelease = true;

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

            ////test for bullet
            //bulletPos = new Vector3(0,30,100);

            bullets = new List<Projectile>();
            


            enemies = new List<Enemy>();
            enemies.Add(new Enemy());
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = Content.Load<Model>("RevolverBeforeBake");
            saloon = Content.Load<Model>("Saloon_Environment");
            enemy1 = Content.Load<Model>("Cowboy1");
            enemy2 = Content.Load<Model>("Cowboy2");
            enemy3 = Content.Load<Model>("Cowboy3");
            enemy4 = Content.Load<Model>("Cowboy4");
            enemy5 = enemy1;
            enemy6 = enemy2;
            bullet = Content.Load<Model>("Bullet");

            //worldMesh = new MeshCollider(enemy1, Matrix.CreateScale(0.004f));

            bulletIcon = Content.Load<Texture2D>("BulletIcon");
            gameFont = Content.Load<SpriteFont>("galleryFont");

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
                playerRot -= .05f;
            }
            //rotate to the left
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                playerRot += .05f;
            }

            //mstate controls
            mstate = Mouse.GetState();
            if (mstate.LeftButton == ButtonState.Pressed && bulletCount > 0 && mRelease == true)
            {
                playerDir = Vector3.Transform(Vector3.Backward,
                                              Matrix.CreateRotationY(playerRot));

                bullets.Add(new Projectile(new Vector3(playerPos.X, (float)3.5, playerPos.Z), playerDir, bullet.Meshes[0].BoundingSphere.Radius));

                bulletCount--;
                mRelease = false;
            }

            for (int b = 0; b < bullets.Count; b++)
            {
                bullets[b].Update(gameTime);
            }

            if (mstate.LeftButton == ButtonState.Released)
            {
                mRelease = true;
            }
            if (mstate.RightButton == ButtonState.Pressed)
            {
                bulletCount = 6;
            }

            //for (int p = 0; p < bullets.Count; p++)
            //{
            //    foreach (Enemy e in enemies)
            //    {
            //        if (e.CheckProjectile(bullets[p]))
            //        {
            //            bullets[p].Update(gameTime);
            //            //bullets.RemoveAt(p);
            //            score++;
            //            --p;
            //            break;
            //        }
            //    }
            //}

            for (int p = 0; p < bullets.Count; p++)
            {
                Projectile b = bullets[p];
                b.Update(gameTime);
              
                foreach(Enemy e in enemies)
                {
                    b.Update(gameTime);
                    if (b.checkCollision(e))
                    {
                        e.Update(gameTime);
                        for (int i = 0; i < bullets.Count; i++)
                        {
                            e.Update(gameTime);

                            bullets[i].Update(gameTime);
                            bullets.RemoveAt(i);
                            score++;
                            //--p;
                        }
                    }
                    
                }

            }


            // update timer
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //keep here, is needed to render models correctly
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            //timer and score keeper
            _spriteBatch.DrawString(gameFont, "Time: " + Math.Ceiling(timer).ToString(), new Vector2(10, 10), Color.Black);
            _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(10, 40), Color.Black);

            //sprite controller for buller icons
            if (bulletCount > 0)
            {
                int right = 700;
                for (int i = 1; i <= bulletCount; i++)
                {
                    _spriteBatch.Draw(bulletIcon, new Vector2(right, 0), Color.White);
                    right -= 50;
                }
            }
            else
            {
                _spriteBatch.DrawString(gameFont, "RELOAD!", new Vector2(600, 15), Color.Black);
            }

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
                           Matrix.CreateRotationY(MathHelper.ToRadians(90));

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

            //enable the lighting for the bullet meshes
            foreach (ModelMesh mesh in bullet.Meshes)
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

           
            //draw enemies 
            foreach (Enemy e in enemies)
            {
                if (e.Type == Enemy.EnemyType.enemy1)
                {
                    world = Matrix.CreateTranslation(e.Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(e.Rot) 
                           ;


                    foreach (ModelMesh mesh in enemy1.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.World = world;
                            effect.View = view;
                            effect.Projection = proj;

                            effect.EnableDefaultLighting();
                            effect.LightingEnabled = true;
                            effect.EmissiveColor = new Vector3(.05f, .05f, .05f);

                            //if (e.Behavior == Enemy.EnemyBehavior.Freeze)
                            //{
                            //    effect.DiffuseColor = Color.DarkCyan.ToVector3();
                            //}
                            if (e.Behavior == Enemy.EnemyBehavior.Dead)
                            {
                                effect.World = Matrix.CreateScale(0.004f)
                                                * Matrix.CreateRotationY(e.Rot)
                                                * Matrix.CreateRotationX(90)
                                                * Matrix.CreateTranslation(e.Pos);
                                effect.DiffuseColor = Color.DarkCyan.ToVector3();
                            }
                        }
                        enemy1.Draw(world, view, proj);
                        //mesh.Draw();
                    }
                }
            }

                //Draw Bullets
                foreach (Projectile b in bullets)
            {
                world = Matrix.CreateRotationY(playerRot)
                    *Matrix.CreateRotationY(MathHelper.ToRadians(270))
                    *Matrix.CreateScale(0.05f) 
                    * Matrix.CreateTranslation(b.Pos);


                bullet.Draw(world, view, proj);
            }

            #region
            //Placing all enemies manually
            world = Matrix.CreateTranslation(enemy1Pos)
                           * Matrix.CreateScale(0.045f) *
                           Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
                           Matrix.CreateTranslation(Vector3.Zero);

            
            //enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy1.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy1.Draw(world, view, proj);

            //world = Matrix.CreateTranslation(enemy2Pos)
            //               * Matrix.CreateScale(0.045f) *
            //               Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
            //               Matrix.CreateTranslation(Vector3.Zero);

            ////enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy2.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy2.Draw(world, view, proj);

            //world = Matrix.CreateTranslation(enemy3Pos)
            //               * Matrix.CreateScale(0.045f) *
            //               Matrix.CreateRotationY(MathHelper.ToRadians(90)) *
            //               Matrix.CreateTranslation(Vector3.Zero);

            ////enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy3.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy3.Draw(world, view, proj);

            //world = Matrix.CreateTranslation(enemy4Pos)
            //               * Matrix.CreateScale(0.045f) *
            //               Matrix.CreateRotationY(MathHelper.ToRadians(-90)) *
            //               Matrix.CreateTranslation(Vector3.Zero);

            ////enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy4.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy4.Draw(world, view, proj);

            //world = Matrix.CreateTranslation(enemy5Pos)
            //               * Matrix.CreateScale(0.045f) *
            //               Matrix.CreateRotationY(MathHelper.ToRadians(-25)) *
            //               Matrix.CreateTranslation(Vector3.Zero);

            ////enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy5.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy5.Draw(world, view, proj);

            //world = Matrix.CreateTranslation(enemy6Pos)
            //               * Matrix.CreateScale(0.045f) *
            //               Matrix.CreateRotationY(MathHelper.ToRadians(25)) *
            //               Matrix.CreateTranslation(Vector3.Zero);

            ////enable the lighting for the enemy meshes
            //foreach (ModelMesh mesh in enemy6.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.World = world;
            //        effect.View = view;
            //        effect.Projection = proj;

            //        effect.EnableDefaultLighting();
            //        effect.LightingEnabled = true;
            //    }
            //}

            //enemy6.Draw(world, view, proj);
            #endregion

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

