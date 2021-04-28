using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace SaloonShootout
{
    class Enemy
    {
        static Random rand;
        Vector3 pos;
        float rot;
        float radius;
        int hall;
        Vector3 moveDir;
        Vector3 velocity;


        public enum EnemyType { enemy1};//the diffrent enemy types (declare more here)

        public enum EnemyBehavior {Freeze, Attack, Dead, Hurt};

        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public float Rot
        {
            get { return rot; }
            set { rot = value; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        EnemyType type;
        public EnemyType Type
        {
            get { return type; }
            set { type = value; }
        }

        EnemyBehavior behavior;
        public EnemyBehavior Behavior
        {
            get { return behavior; }
            set { behavior = value; }
        }

        public Enemy()
        {
            if (rand == null)
            {
                rand = new Random();
            }

            //grayson add different types/ randomize it here!
            //random type 
            type = 0;

            //random hall 
            hall = rand.Next(0, 6);
            //hall = 4;

            // random pos 
            //different halls random positions
            if (hall == 0)
            {
                pos = new Vector3(rand.Next(95, 130), 5f, -200);
                rot = MathHelper.ToRadians(0);

            }
            else if (hall == 1)
            {
                pos = new Vector3(235 ,5f,rand.Next(-15, 15));
                rot = MathHelper.ToRadians(270);
            }
            else if (hall == 2)
            {
                //pos = new Vector3(100, 5f, 200);
                pos = new Vector3(rand.Next(90, 105), 5f, rand.Next(150, 200));
                rot = MathHelper.ToRadians(180);

            }
            else if (hall == 3)
            {
                pos = new Vector3(rand.Next(-130, -100), 5f, 200);
                rot = MathHelper.ToRadians(150);
            }
            else if (hall == 4)
            {
                pos = new Vector3(-225, 5f, rand.Next(-15, 15));
                rot = MathHelper.ToRadians(90);
            }
            else 
            {
                pos = new Vector3(rand.Next(-140,-100), 5f, -205);
            }
            
            
           
            //rot = MathHelper.ToRadians(rand.Next(0, 360));
            radius = 5f;
            behavior = EnemyBehavior.Attack;
            

        }

        public void Update(GameTime gameTime)
        {
            switch (behavior)
            {
                case EnemyBehavior.Attack:
                    pos += velocity;

                    break;
                case EnemyBehavior.Freeze:
                    velocity = moveDir * .2f;
                    pos += velocity;
                    break;
                case EnemyBehavior.Hurt:
                    pos += new Vector3(0, 1, 0);
                    break;
                default:
                    pos += new Vector3(0, 1, 0);
                    break;

            }
        }
        public void respondToPlayer(Vector3 playerPos)
        { 
            //code for movement
            moveDir = playerPos - pos;
            moveDir.Normalize();


            if (Vector3.Distance(playerPos, pos) < 5f && behavior != EnemyBehavior.Dead)
            {
                behavior = EnemyBehavior.Hurt;
            }
            else if (Vector3.Distance(playerPos, pos) < 50f && behavior != EnemyBehavior.Hurt && behavior != EnemyBehavior.Dead)
            {
                behavior = EnemyBehavior.Freeze;

            }
            else if (behavior != EnemyBehavior.Dead && behavior != EnemyBehavior.Hurt)
            {
                behavior = EnemyBehavior.Attack;
            }
            else
                behavior = EnemyBehavior.Dead;
            
        }
        public void changeVelocity(GameTime gameTime)
        {
            if (gameTime.TotalGameTime > TimeSpan.FromSeconds(50f))
            {
                velocity = moveDir * 2f;
            }
            else
                velocity = moveDir * .75f;

        }

        //for other collision process
        //public BoundingSphere boundingSphere
        //{
        //    get { return new BoundingSphere(pos, radius); }
        //}

        public bool CheckProjectile(Projectile p)
        {
            if (Math.Abs(Vector3.Distance(Pos, p.Pos)) < 5f)
            {
                behavior = EnemyBehavior.Dead;
                return true;
            }
            return false;
        }
    }
}
