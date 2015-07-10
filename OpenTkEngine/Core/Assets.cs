using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class Assets
    {
        static readonly string ASSETS = "Assets/";
        static readonly string TEXTURES = "Textures/";
        static readonly string SHADERS = "Shaders/";
        static readonly string FONTS = "Fonts/";
        static readonly string MODELS = "Models/";
        static readonly string AUDIO = "Audio/";

        static Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        static Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
        static Dictionary<string, BitmapFont> _fonts = new Dictionary<string, BitmapFont>();
        static Dictionary<string, Model> _models = new Dictionary<string, Model>();
        static Dictionary<string, Sound> _sounds = new Dictionary<string, Sound>();

        public static bool LoadTexture(string filename)
        {
            if (!_textures.ContainsKey(filename))
            {
                try
                {
                    Texture texture = new Texture(ASSETS + TEXTURES + filename);
                    _textures.Add(filename, texture);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
                Console.WriteLine("a texture already exists with the filename: " + filename);
            return true;
        }

        public static Texture GetTexture(string name)
        {
            if (!_textures.ContainsKey(name))
            {
                if (!LoadTexture(name))
                {
                    return null;
                }
            }
            return _textures[name];
        }

        public static bool LoadShader(string vertShader, string fragShader)
        {
            string fullName = vertShader + " " + fragShader;
            if (!_shaders.ContainsKey(fullName))
            {
                try
                {
                    Shader shader = new Shader(ASSETS + SHADERS + vertShader, ASSETS + SHADERS + fragShader);
                    _shaders.Add(fullName, shader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
                Console.WriteLine("a shader already exists with the vertshader: " + vertShader + " and the fragshader: " + fragShader);
            return true;
        }

        public static Shader GetShader(string vertShader, string fragShader)
        {
            string fullName = vertShader + " " + fragShader;
            if (!_shaders.ContainsKey(fullName))
            {
                if (!LoadShader(vertShader, fragShader))
                {
                    return null;
                }
            }
            return _shaders[fullName];

        }

        public static bool LoadFont(string filename)
        {
            if (!_fonts.ContainsKey(filename))
            {
                try
                {
                    BitmapFont font = new BitmapFont(ASSETS + FONTS + filename);
                    _fonts.Add(filename, font);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
                Console.WriteLine("a font already exists with the filename: " + filename);
            return true;
        }

        public static BitmapFont GetFont(string name)
        {
            if (!_fonts.ContainsKey(name))
            {
                if (!LoadFont(name))
                {
                    return null;
                }
            }
            return _fonts[name];
        }

        public static bool LoadModel(string filename)
        {
            if (!_models.ContainsKey(filename))
            {
                try
                {
                    Model model = new Model(ASSETS + MODELS + filename);
                    _models.Add(filename, model);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
                Console.WriteLine("a model already exists with the filename: " + filename);
            return true;
        }

        public static Model GetModel(string name)
        {
            if (!_models.ContainsKey(name))
            {
                if (!LoadModel(name))
                {
                    return null;
                }
            }
            return _models[name];
        }

        public static bool LoadSound(string filename)
        {
            if (!_sounds.ContainsKey(filename))
            {
                try
                {
                    Sound sound = new Sound(ASSETS + AUDIO + filename);
                    _sounds.Add(filename, sound);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else
                Console.WriteLine("a audio file already exists with the filename: " + filename);
            return true;
        }

        public static Sound GetSound(string name)
        {
            if (!_sounds.ContainsKey(name))
            {
                if (!LoadSound(name))
                {
                    return null;
                }
            }
            return _sounds[name];
        }

        public static void Delete()
        {
            for (int i = 0; i < _textures.Count; i++)
            {
                _textures.ElementAt(i).Value.Delete();
            }
            _textures.Clear();

            for (int i = 0; i < _shaders.Count; i++)
            {
                _shaders.ElementAt(i).Value.Delete();
            }
            _shaders.Clear();

            _fonts.Clear();

            for (int i = 0; i < _models.Count; i++)
            {
                _models.ElementAt(i).Value.DeleteBuffers();
            }
            _models.Clear();

            for (int i = 0; i < _sounds.Count; i++)
            {
            }
            _sounds.Clear();
        }
    }
}
