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

        
        //can use this if we want to use the boudning sphere
        public bool checkCollision(Enemy other)
        {
            if (new BoundingSphere(this.pos, this.radius).Intersects(new BoundingSphere(other.Pos, other.Radius)))
            {
                return true;
            }
            return false;
        }
    }
}


