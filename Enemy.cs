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


        public enum EnemyType { enemy1};
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
            //random type 
            type = 0;

            // random pos 
            //pos = new Vector3(rand.Next(-100, 100), 0, rand.Next(-100, 100));
            pos = new Vector3(-2000, 150, -3500);

            //random rotate
            //rot = MathHelper.ToRadians(rand.Next(0, 360));
            rot = MathHelper.ToRadians(180);
            //behavior = EnemyBehavior.Random;
            radius = 1000f;
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
        public BoundingSphere boundingSphere
        {
            get { return new BoundingSphere(pos, radius); }
        }

        public bool CheckProjectile(Projectile p)
        {
            //if (Math.Abs(Vector3.Distance(Pos, p.Pos)) < 100f)
            //{
            //    behavior = EnemyBehavior.Dead;
            //    return true;
            //}
            return false;
        }
    }
}
