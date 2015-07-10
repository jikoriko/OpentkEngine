using System;
using System.Collections.Generic;
using System.IO;

namespace OpenTkEngine.Core
{
    /// <summary>
    /// Model Utility reads a very simple, inefficient file format that uses 
    /// Triangles only. This is not good practice, but it does the job :)
    /// </summary>
    public class ModelUtility
    {
        public static float[] Vertices { get; private set; }
        public static int[] Indices { get; private set; }

        private ModelUtility() { }

        private static Model LoadFromBIN(string pModelFile)
        {
            BinaryReader reader = new BinaryReader(new FileStream(pModelFile, FileMode.Open));

            int numberOfVertices = reader.ReadInt32();
            int floatsPerVertex = 6;

            Vertices = new float[numberOfVertices * floatsPerVertex];

            byte[]  byteArray = new byte[Vertices.Length * sizeof(float)];
            byteArray = reader.ReadBytes(byteArray.Length);

            Buffer.BlockCopy(byteArray, 0, Vertices, 0, byteArray.Length);

            int numberOfTriangles = reader.ReadInt32();

            Indices = new int[numberOfTriangles * 3];
            
            byteArray = new byte[Indices.Length * sizeof(int)];
            byteArray = reader.ReadBytes(Indices.Length * sizeof(int));
            Buffer.BlockCopy(byteArray, 0, Indices, 0, byteArray.Length);

            reader.Close();

            ModelMesh mesh = new ModelMesh(Vertices, Indices, false);
            List<ModelMesh> meshes = new List<ModelMesh>();
            meshes.Add(mesh);
            return new Model(meshes);
        }     
 
        private static Model LoadFromSJG(string pModelFile)
        {
            StreamReader reader;
            reader = new StreamReader(pModelFile);
            string line = reader.ReadLine(); // vertex format
            int numberOfVertices = 0;
            int floatsPerVertex = 6;
            if (!int.TryParse(reader.ReadLine(), out numberOfVertices))
            {
                throw new Exception("Error when reading number of vertices in model file " + pModelFile);
            }

            Vertices = new float[numberOfVertices * floatsPerVertex];

            string[] values;
            for (int i = 0; i < Vertices.Length; )
            {
                line = reader.ReadLine();
                values = line.Split(',');
                foreach(string s in values)
                {
                    if (!float.TryParse(s, out Vertices[i]))
                    {
                        throw new Exception("Error when reading vertices in model file " + pModelFile + " " + s + " is not a valid number");
                    }
                    ++i;
                }
            }

            reader.ReadLine();
            int numberOfTriangles = 0;
            line = reader.ReadLine();
            if (!int.TryParse(line, out numberOfTriangles))
            {
                throw new Exception("Error when reading number of triangles in model file " + pModelFile);
            }

            Indices = new int[numberOfTriangles * 3];

            for(int i = 0; i < numberOfTriangles * 3;)
            {
                line = reader.ReadLine();
                values = line.Split(',');
                foreach(string s in values)
                {
                    if (!int.TryParse(s, out Indices[i]))
                    {
                        throw new Exception("Error when reading indices in model file " + pModelFile + " " + s + " is not a valid index");
                    }
                    ++i;
                }
            }

            reader.Close();

            ModelMesh mesh = new ModelMesh(Vertices, Indices, false);
            List<ModelMesh> meshes = new List<ModelMesh>();
            meshes.Add(mesh);
            return new Model(meshes);
        }

        private static Model LoadFromWavefront(string filename)
        {
            List<ModelMesh> meshes;
            WavefrontLoader.LoadModel(filename, out meshes);
            Model model = new Model(meshes);
            return model;
        }

        public static Model LoadModel(string pModelFile)
        {
            string extension = pModelFile.Substring(pModelFile.IndexOf('.'));

            if (extension == ".sjg")
            {
                return LoadFromSJG(pModelFile);
            }
            else if (extension == ".bin")
            {
                return LoadFromBIN(pModelFile);
            }
            else if (extension == ".obj")
            {
                return LoadFromWavefront(pModelFile);
            }
            else
            {
                throw new Exception("Unknown file extension " + extension);
            }
        }

    }
}
