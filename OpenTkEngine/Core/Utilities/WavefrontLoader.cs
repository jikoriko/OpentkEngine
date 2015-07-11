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
            public string MaterialName;
        }

        private static List<Vector3> _positions = new List<Vector3>();
        private static List<Vector3> _normals = new List<Vector3>();
        private static List<Vector2> _textureUVs = new List<Vector2>();

        private static List<List<Face>> _meshes = new List<List<Face>>();

        private static Dictionary<string, Material> _materials = new Dictionary<string, Material>();
        private static string _currentMaterial;

        public static void LoadModel(string filename, out List<ModelMesh> outMeshes)
        {
            ParseModel(filename);
            ParseMaterials(filename.Split(new char[] { '.' })[0] + ".mtl");

            List<float> verts = new List<float>();
            List<int> indices = new List<int>();

            List<ModelMesh> meshes = new List<ModelMesh>();

            for (int i = 0; i < _meshes.Count; i++)
            {
                verts.Clear();
                indices.Clear();
                for (int j = 0; j < _meshes[i].Count; j++)
                {
                    Face face = _meshes[i][j];
                    for (int k = 0; k < 3; k++)
                    {
                        verts.Add(face.Positions[k].X);
                        verts.Add(face.Positions[k].Y);
                        verts.Add(face.Positions[k].Z);

                        verts.Add(face.Normals[k].X);
                        verts.Add(face.Normals[k].Y);
                        verts.Add(face.Normals[k].Z);

                        verts.Add(face.TextureUVs[k].X);
                        verts.Add(face.TextureUVs[k].Y);
                    }
                }

                for (int j = 0; j < verts.Count / 6; j++)
                {
                    indices.Add(j);
                }

                ModelMesh mesh = new ModelMesh(verts.ToArray(), indices.ToArray(), true);
                mesh.SetMaterial(_materials[_meshes[i][0].MaterialName]);
                meshes.Add(mesh);
            }

            outMeshes = meshes;

            _positions.Clear();
            _normals.Clear();
            _textureUVs.Clear();
            _meshes.Clear();
            _materials.Clear();
        }

        private static void ParseModel(string filename)
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
                else if (line.StartsWith("usemtl"))
                {
                    _currentMaterial = line.Split(new char[] { ' ' })[1];
                    _meshes.Add(new List<Face>());
                }
                else if (line.StartsWith("f"))
                {
                    ParseFace(line.Substring(2));
                }
            }
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
            face.MaterialName = _currentMaterial;
            _meshes[_meshes.Count - 1].Add(face);

        }

        private static void ParseMaterials(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            Material current = null;
            string materialName;
            foreach (string line in lines)
            {
                if (line.StartsWith("newmtl"))
                {
                    current = new Material();
                    materialName = line.Split(new char[] { ' ' })[1];
                    _materials.Add(materialName, current);
                }
                else if (line.StartsWith("Ka"))
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    current.Ambient.X = float.Parse(parts[1]);
                    current.Ambient.Y = float.Parse(parts[2]);
                    current.Ambient.Z = float.Parse(parts[3]);
                }
                else if (line.StartsWith("Kd"))
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    current.Diffuse.X = float.Parse(parts[1]);
                    current.Diffuse.Y = float.Parse(parts[2]);
                    current.Diffuse.Z = float.Parse(parts[3]);
                }
                else if (line.StartsWith("Ks"))
                {
                    string[] parts = line.Split(new char[] { ' ' });
                    current.Specular.X = float.Parse(parts[1]);
                    current.Specular.Y = float.Parse(parts[2]);
                    current.Specular.Z = float.Parse(parts[3]);
                }
                else if (line.StartsWith("Ns"))
                {
                    current.Shininess = (float.Parse(line.Split(new char[] { ' ' })[1]) / 1000f) * 128.0f;
                }
                else if (line.StartsWith("map_Kd"))
                {
                    current.DiffuseTexture = Assets.GetTexture(line.Split(new char[] { ' ' })[1]);
                }
            }
        }
    }
}
