using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SaloonShootout
{
    class Projectile
    {
        private Vector3 pos;
        Vector3 dir;
        private Vector3 vel; // velocity
        private float radius;


        public Vector3 Pos
        {
            get { return pos; }
        }

        public Vector3 Dir
        {
            get { return dir; }
        }

        public Projectile(Vector3 p, Vector3 d, float r)
        {
            pos = p;
            dir = d;
            vel = Vector3.Zero;
            radius = r;
        }
        public BoundingSphere boundingSphere
        {
            get { return new BoundingSphere(pos, radius); }
        }

        public Vector3 Vel
        {
            get { return vel; }
            set { vel = value; }
        }

        public void Update(GameTime gameTime)
        {
            pos += -dir;
        }

        
        public bool checkCollision(Enemy other)
        {
            if (new BoundingSphere(this.pos, this.radius).Intersects(new BoundingSphere(other.Pos, other.Radius)))
            {
                //Vector3 axis = other.Pos - this.pos;
                //float dist = other.Radius + this.radius;
                //float move = (dist - axis.Length()) / 2f;
                //axis.Normalize();

                //Vector3 U1x = axis * Vector3.Dot(axis, this.vel);
                //Vector3 U1y = this.vel - U1x;

                //Vector3 U2x = -axis * Vector3.Dot(-axis, other.vel);
                //Vector3 U2y = other.vel - U2x;

                //// V1x = U2x
                //Vector3 V1x = U2x;
                //// V2x = ((2*mass[i]) / (mass[i] + mass[j])) * U1x
                ////      +((mass[j] - mass[i]) / (mass[i] + mass[j])) *
                //Vector3 V2x = U1x;

                //this.vel = V1x + U1y;
                //other.vel = V2x + U2y;

                //this.vel *= 0.99f;
                //other.vel *= 0.99f;

                //other.pos += axis * move;
                //this.pos -= axis * move;
                return true;
            }
            return false;
        }
    }
}


