using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace SaloonShootout
{
    class MeshCollider
    {
        List<Vector3> verts;
        List<Vector3> norms;

        public MeshCollider(Model model, Matrix world)
        {
            verts = new List<Vector3>();
            norms = new List<Vector3>();

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    VertexPositionNormalTexture[] vertexData =
                        new VertexPositionNormalTexture[part.NumVertices];
                    // The line below was missing
                    part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertexData);

                    ushort[] indices = new ushort[part.IndexBuffer.IndexCount];
                    part.IndexBuffer.GetData<ushort>(indices);

                    Vector3 v = new Vector3();
                    for (int i = 0; i < indices.Length; i++)
                    {
                        v.X = vertexData[indices[i]].Position.X;
                        v.Y = vertexData[indices[i]].Position.Y;
                        v.Z = vertexData[indices[i]].Position.Z;

                        verts.Add(Vector3.Transform(v, mesh.ParentBone.Transform * world)); ;

                        if (verts.Count % 3 == 0)
                        {
                            Vector3 Normal =
                                Vector3.Cross(
                                    verts[verts.Count - 1] - verts[verts.Count - 3],
                                    verts[verts.Count - 2] - verts[verts.Count - 3]);
                            Normal.Normalize();
                            norms.Add(Normal);
                        }
                    }
                }
            }
        }

        public static bool SameSide(Vector3 p1, Vector3 p2, Vector3 a, Vector3 b)
        {
            Vector3 cp1 = Vector3.Cross(b - a, p1 - a);
            Vector3 cp2 = Vector3.Cross(b - a, p2 - a);
            if (Vector3.Dot(cp1, cp2) >= 0)
            {
                return true;
            }
            return false;
        }

        public static bool PointInTriangle(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            if (SameSide(p, a, b, c) && SameSide(p, b, a, c) && SameSide(p, c, a, b))
            {
                return true;
            }
            return false;
        }


        public bool checkCollisionAndResponse(Projectile e)
        {
            bool collision = false;

            for (int w = 0; w < verts.Count; w += 3)
            {

                //sign of dist will determine which side of the wall we are on
                float dist = Vector3.Dot(norms[w / 3], (e.Pos - verts[w]));

                if (Math.Abs(dist) < e.boundingSphere.Radius
                      && PointInTriangle(e.Pos, verts[w], verts[w + 1], verts[w + 2]))  //collision
                {

                    //if (dist < e.boundingSphere.Radius)
                    //{
                    //    e.Pos = e.Pos + (norms[w / 3] * (e.boundingSphere.Radius - dist));
                    //}
                    //else if (dist > -e.boundingSphere.Radius)
                    //{
                    //    e.Pos = e.Pos + (-norms[w / 3] * (e.boundingSphere.Radius + dist));
                    //}

                    Vector3 V = e.Vel;
                    V.Normalize();
                    V = 2 * (Vector3.Dot(-V, norms[w / 3]))
                             * norms[w / 3] + V;
                    e.Vel = e.Vel.Length() * V;

                    collision = true;

                }
            }

            return collision;
        }

    }
}
