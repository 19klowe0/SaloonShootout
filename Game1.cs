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

        //screens
        bool mainmenu = true;
        bool info = false;
        bool endscreen = false;

        Texture2D MainmenuButton;
        Texture2D InfoButton;
        Texture2D EasyButton;
        Texture2D MediumButton;
        Texture2D HardButton;

        //1 = easy, 2 = medium, 3 = hard
        int gamemode = 1;

        //health bar
        int health = 5;
        Texture2D healthfull;
        Texture2D health4_5;
        Texture2D health3_5;
        Texture2D health2_5;
        Texture2D health1_5;
        Texture2D healthempty;

        //icons and timer
        Texture2D bulletIcon;
        SpriteFont gameFont;
        float timer;
        int score;
        int bulletCount = 6;
        TimeSpan enemySpawnTime;
        TimeSpan previousSpawnTime;


        //enemy temp variable
        bool enemyaddtemp = true;

        //needed player information
        Vector3 playerPos;
        Vector3 saloonPos;
        Model player;
        Model saloon;
        Vector3 playerDir;
        float enemyRot;
        float playerRot;


        //enemy information
        Model enemy1;
        Model enemy2;
        Model enemy3;
        Model enemy4;
        Model enemy5;
        Model enemy6;

        //bullet information 
        //bulletPos will probably need to be array since multiple can be on screen
        Model bullet;

        List<Projectile> bullets;
        List<Enemy> enemies;



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


            ////test for bullet
            //bulletPos = new Vector3(0,30,100);

            bullets = new List<Projectile>();



            enemies = new List<Enemy>();


            //setting up the spawn times
            previousSpawnTime = TimeSpan.Zero;

            enemySpawnTime = TimeSpan.FromSeconds(5.0f);


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

            healthfull = Content.Load<Texture2D>("Health_Full");
            health4_5 = Content.Load<Texture2D>("Health4_5");
            health3_5 = Content.Load<Texture2D>("Health3_5");
            health2_5 = Content.Load<Texture2D>("Health2_5");
            health1_5 = Content.Load<Texture2D>("Health1_5");
            healthempty = Content.Load<Texture2D>("Health_Empty");

            MainmenuButton = Content.Load<Texture2D>("MainMenuButton");
            EasyButton = Content.Load<Texture2D>("EasyButton");
            MediumButton = Content.Load<Texture2D>("MediumButton");
            HardButton = Content.Load<Texture2D>("HardButton");
            InfoButton = Content.Load<Texture2D>("InfoButton");

            bulletIcon = Content.Load<Texture2D>("BulletIcon");
            gameFont = Content.Load<SpriteFont>("galleryFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mstate = Mouse.GetState();

            if (mainmenu == false && info == false && endscreen == false)
            {

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

                //check for collision using vector distance
                for (int p = 0; p < bullets.Count; p++)
                {
                    foreach (Enemy e in enemies)
                    {
                        if (e.CheckProjectile(bullets[p]))
                        {
                            bullets[p].Update(gameTime);
                            bullets.RemoveAt(p);
                            score++;
                            --p;
                            break;
                        }
                    }
                }

                //boundingsphere collision 
                #region 
                //this is code for the bounding sphere 
                //for (int p = 0; p < bullets.Count; p++)
                //{
                //    Projectile b = bullets[p];
                //    b.Update(gameTime);

                //    foreach(Enemy e in enemies)
                //    {
                //        b.Update(gameTime);
                //        if (b.checkCollision(e))
                //        {
                //            e.Update(gameTime);
                //            for (int i = 0; i < bullets.Count; i++)
                //            {
                //                e.Update(gameTime);

                //                bullets[i].Update(gameTime);
                //                bullets.RemoveAt(i);
                //                score++;
                //                //--p;
                //            }
                //        }
                //    }
                //}

                #endregion




                //call for enemy movement
                foreach (Enemy e in enemies)
                {
                    e.respondToPlayer(playerPos);
                    e.Update(gameTime);
                    if (e.checkWithPlayer(playerPos))
                    {
                        health--;
                    }
                }

                //remove enemies when dead 
                for (int e1 = 0; e1 < enemies.Count; e1++)
                {
                    foreach (Enemy e in enemies)
                    {
                        if (e.Pos.Y > 100f)
                        {
                            enemies.RemoveAt(e1);
                            --e1;
                            break;
                        }
                    }
                }






                //update timer
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;


                //update method for spawning
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
                {

                    if (gameTime.TotalGameTime > TimeSpan.FromSeconds(20f))
                    {
                        enemySpawnTime = TimeSpan.FromSeconds(3f);
                    }

                    // Add an Enemy
                    enemies.Add(new Enemy());
                    foreach (Enemy e in enemies)
                    {
                        e.changeVelocity(gameTime);
                    }
                    previousSpawnTime = gameTime.TotalGameTime;

                }

                if (health == 0)
                {
                    endscreen = true;
                }

            }
            else if (mainmenu == true && info == false && endscreen == false)
            {
                Rectangle mouseRectangle = new Rectangle(mstate.X, mstate.Y, 1, 1);
                Rectangle easyRectangle = new Rectangle(200, 225, 100, 46);
                Rectangle mediumRectangle = new Rectangle(350, 225, 100, 46);
                Rectangle hardRectangle = new Rectangle(500, 225, 100, 46);
                Rectangle infoRectangle = new Rectangle(350, 325, 100, 46);

                if (mouseRectangle.Intersects(easyRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    gamemode = 1;
                    mainmenu = false;
                    mRelease = false;
                }
                if (mouseRectangle.Intersects(mediumRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    gamemode = 2;
                    mainmenu = false;
                    mRelease = false;
                }
                if (mouseRectangle.Intersects(hardRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    gamemode = 3;
                    mainmenu = false;
                    mRelease = false;
                }
                if (mouseRectangle.Intersects(infoRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    info = true;
                    mainmenu = false;
                    mRelease = false;
                }
                if (mstate.LeftButton == ButtonState.Released)
                {
                    mRelease = true;
                }
            }
            else if (mainmenu == false && info == true && endscreen == false)
            {
                Rectangle mouseRectangle = new Rectangle(mstate.X, mstate.Y, 1, 1);
                Rectangle mainmenuRectangle = new Rectangle(350, 325, 100, 46);
                if (mouseRectangle.Intersects(mainmenuRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    info = false;
                    mainmenu = true;
                    mRelease = false;
                }
                if (mstate.LeftButton == ButtonState.Released)
                {
                    mRelease = true;
                }
            }
            else if (mainmenu == false && info == false && endscreen == true)
            {
                Rectangle mouseRectangle = new Rectangle(mstate.X, mstate.Y, 1, 1);
                Rectangle mainmenuRectangle = new Rectangle(350, 325, 100, 46);
                if (mouseRectangle.Intersects(mainmenuRectangle) && mstate.LeftButton == ButtonState.Pressed && mRelease == true)
                {
                    mainmenu = true;
                    endscreen = false;
                    mRelease = false;
                }
                if (mstate.LeftButton == ButtonState.Released)
                {
                    mRelease = true;
                }
            }

            base.Update(gameTime);
        }

        void CreateEnvir()
        {
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
            Matrix world = Matrix.CreateRotationY(playerRot)
                                              * Matrix.CreateTranslation(playerPos)
                                              * Matrix.CreateScale(0.045f) *
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
            saloon.Draw(environment, view, proj);

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
                    //was out of order!
                    world =
                           Matrix.CreateScale(0.045f)
                           * Matrix.CreateRotationY(e.Rot)
                           * Matrix.CreateTranslation(e.Pos);



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
                            if (e.Behavior == Enemy.EnemyBehavior.Freeze)
                            {
                                effect.DiffuseColor = Color.DarkCyan.ToVector3();
                            }
                            else if (e.Behavior == Enemy.EnemyBehavior.Hurt)
                            {
                                effect.DiffuseColor = Color.Crimson.ToVector3();

                            }
                            else if (e.Behavior == Enemy.EnemyBehavior.Dead)
                            {
                                effect.World = Matrix.CreateScale(0.045f)
                                               * Matrix.CreateRotationY(90)
                                               * Matrix.CreateTranslation(e.Pos);
                                effect.DiffuseColor = Color.Red.ToVector3();
                            }
                            
                            else
                                effect.DiffuseColor = Color.White.ToVector3();

                        }
                        enemy1.Draw(world, view, proj);
                    }
                }
            }

            //Draw Bullets
            foreach (Projectile b in bullets)
            {
                world = Matrix.CreateRotationY(playerRot)
                    * Matrix.CreateRotationY(MathHelper.ToRadians(270))
                    * Matrix.CreateScale(0.05f)
                    * Matrix.CreateTranslation(b.Pos);


                bullet.Draw(world, view, proj);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //keep here, is needed to render models correctly
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

            if (mainmenu == true && info == false && endscreen == false)
            {

                _spriteBatch.DrawString(gameFont, "Saloon Shootout!", new Vector2(275, 150), Color.Black);
                _spriteBatch.Draw(EasyButton, new Vector2(200, 225), Color.White);
                _spriteBatch.Draw(MediumButton, new Vector2(350, 225), Color.White);
                _spriteBatch.Draw(HardButton, new Vector2(500, 225), Color.White);
                _spriteBatch.Draw(InfoButton, new Vector2(350, 325), Color.White);
            }
            else if (mainmenu == false && info == true && endscreen == false)
            {
                String info1 = "GAMEPLAY";
                String info2 = "A: Rotate Left";
                String info3 = "W: Rotate Right";
                String info4 = "Left-Click: Fire Bullet";
                String info5 = "Right-Click: Reload";
                String info6 = "DIFFICULTIES";
                String info7 = "Easy: 5 Lives";
                String info8 = "Medium: 3 Lives";
                String info9 = "Hard: 1 Life";
                _spriteBatch.DrawString(gameFont, info1, new Vector2(100, 50), Color.Black);
                _spriteBatch.DrawString(gameFont, info2, new Vector2(100, 100), Color.Black);
                _spriteBatch.DrawString(gameFont, info3, new Vector2(100, 150), Color.Black);
                _spriteBatch.DrawString(gameFont, info4, new Vector2(100, 200), Color.Black);
                _spriteBatch.DrawString(gameFont, info5, new Vector2(100, 250), Color.Black);
                _spriteBatch.DrawString(gameFont, info6, new Vector2(450, 50), Color.Black);
                _spriteBatch.DrawString(gameFont, info7, new Vector2(450, 100), Color.Black);
                _spriteBatch.DrawString(gameFont, info8, new Vector2(450, 150), Color.Black);
                _spriteBatch.DrawString(gameFont, info9, new Vector2(450, 200), Color.Black);

                _spriteBatch.Draw(MainmenuButton, new Vector2(350, 325), Color.White);
            }
            else if (mainmenu == false && info == false && endscreen == true)
            {
                _spriteBatch.DrawString(gameFont, "Game Over!", new Vector2(300, 150), Color.Black);
                _spriteBatch.DrawString(gameFont, "Total Score: " + score.ToString(), new Vector2(300, 200), Color.Black);
                _spriteBatch.DrawString(gameFont, "Total Time: " + Math.Ceiling(timer).ToString(), new Vector2(300, 250), Color.Black);
                _spriteBatch.Draw(MainmenuButton, new Vector2(350, 325), Color.White);
            }
            else
            {
                //timer and score keeper
                _spriteBatch.DrawString(gameFont, "Time: " + Math.Ceiling(timer).ToString(), new Vector2(10, 10), Color.White);
                _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(10, 40), Color.White);

                //sprite controller for health
                if (gamemode == 1 || gamemode == 2 || gamemode == 3)
                {
                    if (health == 5)
                    {
                        _spriteBatch.Draw(healthfull, new Vector2(300, 10), Color.White);
                    }
                    if (health == 4 && (gamemode != 2 || gamemode != 3))
                    {
                        _spriteBatch.Draw(health4_5, new Vector2(300, 10), Color.White);
                    }
                    if (health == 3 && (gamemode != 3))
                    {
                        _spriteBatch.Draw(health3_5, new Vector2(300, 10), Color.White);
                    }
                    if (health == 2 && (gamemode != 2 || gamemode != 3))
                    {
                        _spriteBatch.Draw(health2_5, new Vector2(300, 10), Color.White);
                    }
                    if (health == 1 && (gamemode != 3))
                    {
                        _spriteBatch.Draw(health1_5, new Vector2(300, 10), Color.White);
                    }
                    if (health == 0)
                    {
                        _spriteBatch.Draw(healthempty, new Vector2(300, 10), Color.White);
                    }
                }

                //sprite controller for buller icons
                if (bulletCount > 0)
                {
                    int right = 700;
                    for (int i = 1; i <= bulletCount; i++)
                    {
                        _spriteBatch.Draw(bulletIcon, new Vector2(right, 36), Color.White);
                        right -= 50;
                    }
                }
                else
                {
                    _spriteBatch.DrawString(gameFont, "RELOAD!", new Vector2(600, 50), Color.Black);
                }

                CreateEnvir();

                #region
                //Placing all enemies manually
                //world = Matrix.CreateTranslation(enemy1Pos)
                //               * Matrix.CreateScale(0.045f) *
                //               Matrix.CreateRotationY(MathHelper.ToRadians(180)) *
                //               Matrix.CreateTranslation(Vector3.Zero);


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

            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

