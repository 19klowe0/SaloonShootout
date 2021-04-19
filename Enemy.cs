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


        public enum EnemyType { enemy1};//the diffrent enemy types (declare more here)

        public enum EnemyBehavior { Random, Freeze, Attack, Dead };

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
            //hall = rand.Next(0, 5);
            hall = 2;

            // random pos 
            //pos = new Vector3(rand.Next(-100, 100), 0, rand.Next(-100, 100));

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
                pos = new Vector3(100, 5f, -200);
            }
            else if (hall == 4)
            {
                pos = new Vector3(100, 5f, -200);
            }
            else 
            {
                pos = new Vector3(-100, 5f, -200);
            }

           
            //rot = MathHelper.ToRadians(rand.Next(0, 360));
            
            //behavior = EnemyBehavior.Random;
            radius = 5f;
        }

        public void Update(GameTime gameTime)
        {
            switch (behavior)
            {
                case EnemyBehavior.Random:
                    rot += MathHelper.ToRadians(rand.Next(-3, 3));
                    pos += Vector3.Transform(Vector3.Backward,
                                            Matrix.CreateRotationY(rot)) * .2f;
                    break;
                case EnemyBehavior.Attack:
                    pos += Vector3.Transform(
                    Vector3.Backward, Matrix.CreateRotationY(rot)) * 0.2f;
                    break;
                case EnemyBehavior.Freeze:
                    break;
                default:
                    break;

            }
        }
        public void respondToPlayer(Vector3 playerPos)
        {
            if (Vector3.Distance(pos, playerPos) < 3)
            {
                if (behavior != EnemyBehavior.Dead)
                {
                    behavior = EnemyBehavior.Freeze;
                }
            }
            else if (Vector3.Distance(pos, playerPos) < 5)
            {
                rot = (float)Math.Atan2(playerPos.Z - pos.Z, playerPos.Z - pos.X);
                if (behavior != EnemyBehavior.Dead)
                {
                    behavior = EnemyBehavior.Attack;
                }
            }
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
