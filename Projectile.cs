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
        Vector3 pos;
        Vector3 dir;

        public Vector3 Pos
        {
            get { return pos; }
        }

        public Vector3 Dir
        {
            get { return dir; }
        }

        public Projectile(Vector3 p, Vector3 d)
        {
            pos = p;
            dir = d;
        }

        public void Update(GameTime gameTime)
        {
            pos += -dir;
        }
    }
}
