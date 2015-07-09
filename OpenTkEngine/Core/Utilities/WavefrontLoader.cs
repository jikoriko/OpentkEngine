using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class WavefrontLoader
    {

        private class Face
        {
            public List<Vector3> Positions = new List<Vector3>();
            public List<Vector3> Normals = new List<Vector3>();
            public List<Vector2> TextureUVs = new List<Vector2>();
        }

        private static List<Vector3> _positions = new List<Vector3>();
        private static List<Vector3> _normals = new List<Vector3>();
        private static List<Vector2> _textureUVs = new List<Vector2>();
        private static List<Face> _faces = new List<Face>();

        public static void LoadModel(string filename, out float[] outVerts, out int[] outIndices)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                if (line.StartsWith("v "))
                {
                    _positions.Add(ParseVector3(line));
                }
                else if (line.StartsWith("vn"))
                {
                    _normals.Add(ParseVector3(line));
                }
                else if (line.StartsWith("vt"))
                {
                    _textureUVs.Add(ParseVector2(line));
                }
                else if (line.StartsWith("f"))
                {
                    ParseFace(line.Substring(2));
                }
            }

            List<float> verts = new List<float>();
            List<int> indices = new List<int>();
            for (int i = 0; i < _faces.Count; i++)
            {
                Face face = _faces[i];
                for (int j = 0; j < 3; j++)
                {
                    verts.Add(face.Positions[j].X);
                    verts.Add(face.Positions[j].Y);
                    verts.Add(face.Positions[j].Z);

                    verts.Add(face.Normals[j].X);
                    verts.Add(face.Normals[j].Y);
                    verts.Add(face.Normals[j].Z);
                }
            }

            for (int i = 0; i < verts.Count / 6; i++)
            {
                indices.Add(i);
            }

            outVerts = verts.ToArray();
            outIndices = indices.ToArray();

            _positions.Clear();
            _normals.Clear();
            _textureUVs.Clear();
            _faces.Clear();
        }

        private static Vector2 ParseVector2(string line)
        {
            string[] parts = line.Split(new char[] { ' ' });
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            return new Vector2(x, y);
        }

        private static Vector3 ParseVector3(string line)
        {
            string[] parts = line.Split(new char[] { ' ' });
            float x = float.Parse(parts[1]);
            float y = float.Parse(parts[2]);
            float z = float.Parse(parts[3]);
            return new Vector3(x, y, z);
        }

        private static void ParseFace(string line)
        {
            string[] parts = line.Split(new char[] { ' ' });
            Face face = new Face();
            for (int i = 1; i < parts.Length; i++)
            {
                string[] subParts = parts[i].Split(new char[] { '/' });
                for (int j = 0; j < subParts.Length; j++)
                {
                    int index = int.Parse(subParts[j]) - 1;
                    switch (j)
                    {
                        case 0:
                            face.Positions.Add(_positions[index]);
                            break;
                        case 1:
                            face.TextureUVs.Add(_textureUVs[index]);
                            break;
                        case 2:
                            face.Normals.Add(_normals[index]);
                            break;
                    }
                }
            }
            _faces.Add(face);

        }
    }
}
