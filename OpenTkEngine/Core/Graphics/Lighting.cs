using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;

namespace OpenTkEngine.Core
{
    public class Lighting
    {
        public enum LightType
        {
            Directional, Point, Spot
        }

        public struct Light 
        {
            public bool On;
            public LightType Type;
            public Vector4 Position;
            public Vector4 Direction;
            public float CuttOff;
            public Vector3 AmbientColour;
            public Vector3 DiffuseColour;
            public Vector3 SpecularColour;
            public float ConstantAttenuation;
            public float LinearAttenuation;
            public float QuadraticAttenuation;
            public float Intensity;
        }

        private static readonly int _numLights = 5;
        private static Light[] _lights = new Light[_numLights];

        public static int NumLights()
        {
            return _numLights;
        }

        private static bool[] switches = new bool[] {
            true,
            false,
            false,
            false, 
            false
        };

        private static Vector3[] lightPositions = new Vector3[] {
            new Vector3(0, 0, 0),
            new Vector3(0, 50, 0),
            new Vector3(0, 350, 0),
            new Vector3(0, 50, 200),
            new Vector3(0, 350, 200)
        };

        private static Vector3[] lightDirections = new Vector3[] {
            new Vector3(0, -1, -1),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 0, -1)
        };

        private static Vector3[] lightColours = new Vector3[] {
            new Vector3(1, 1, 1),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 1)
        };

        private static LightType[] lightTypes = new LightType[] {
            LightType.Directional,
            LightType.Point,
            LightType.Point,
            LightType.Spot,
            LightType.Spot
        };

        private static float[] lightIntensities = new float[] {
            0.7f,
            1000.0f,
            2000.0f,
            50000.0f,
            50000.0f
        };

        public static Light GetLight(int index)
        {
            return _lights[index];
        }

        public static void EnableLighting()
        {
            int uLightingEnabledLocation = Graphics.GetShader().GetUniformLocation("uLightingEnabled");
            GL.Uniform1(uLightingEnabledLocation, 1);
        }

        public static void DisableLighting()
        {
            int uLightingEnabledLocation = Graphics.GetShader().GetUniformLocation("uLightingEnabled");
            GL.Uniform1(uLightingEnabledLocation, 0);
        }

        public static void InitLights()
        {
            for (int i = 0; i < _numLights; i++)
            {
                _lights[i].On = switches[i];
                _lights[i].Type = lightTypes[i];
                _lights[i].CuttOff = 39f;
                _lights[i].AmbientColour = new Vector3(lightColours[i]);
                _lights[i].DiffuseColour = new Vector3(lightColours[i]);
                _lights[i].SpecularColour = new Vector3(lightColours[i]);
                _lights[i].ConstantAttenuation = 0f;
                _lights[i].LinearAttenuation = 0f;
                _lights[i].QuadraticAttenuation = 1f;
                _lights[i].Intensity = lightIntensities[i];

                SetLightPosition(i, lightPositions[i]);
                SetLightDirection(i, lightDirections[i]);

                //upload data to shader
                int uLightOnLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].On");
                GL.Uniform1(uLightOnLocation, _lights[i].On ? 1 : 0);

                int uLightTypeLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].Type");
                GL.Uniform1(uLightTypeLocation, (int)_lights[i].Type);

                int uLightSpotCutOffLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].SpotCutOff");
                GL.Uniform1(uLightSpotCutOffLocation, _lights[i].CuttOff);

                int uAmbientLightLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].AmbientLight");
                GL.Uniform3(uAmbientLightLocation, _lights[i].AmbientColour);

                int uDiffuseLightLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].DiffuseLight");
                GL.Uniform3(uDiffuseLightLocation, _lights[i].DiffuseColour);

                int uSpecularLightLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].SpecularLight");
                GL.Uniform3(uSpecularLightLocation, _lights[i].SpecularColour);

                int uConstantAttenuationLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].ConstantAttenuation");
                GL.Uniform1(uConstantAttenuationLocation, _lights[i].ConstantAttenuation);

                int uLinearAttenuationLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].LinearAttenuation");
                GL.Uniform1(uLinearAttenuationLocation, _lights[i].LinearAttenuation);

                int uQuadraticAttenuationLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].QuadraticAttenuation");
                GL.Uniform1(uQuadraticAttenuationLocation, _lights[i].QuadraticAttenuation);

                int uIntensityLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].Intensity");
                GL.Uniform1(uIntensityLocation, _lights[i].Intensity);
            }
        }

        public static void SetLightPosition(int light, Vector3 position)
        {
            if (light >= _numLights)
            {
                Console.WriteLine("Invalid light ID");
                return;
            }
            _lights[light].Position = new Vector4(position, 1);
            Graphics.TranslateWorld(Vector3.Zero);
        }

        public static void SetLightDirection(int light, Vector3 direction)
        {
            if (light >= _numLights)
            {
                Console.WriteLine("Invalid light ID");
                return;
            }
            _lights[light].Direction = new Vector4(direction, 1);
            Graphics.TranslateWorld(Vector3.Zero);
        }

        public static Vector3 GetLightPosition(int light)
        {
            if (light >= _numLights)
            {
                Console.WriteLine("Invalid light ID");
                return Vector3.Zero;
            }
            return _lights[light].Position.Xyz;
        }

        public static Vector3 GetLightDirection(int light)
        {
            if (light >= _numLights)
            {
                Console.WriteLine("Invalid light ID");
                return Vector3.Zero;
            }
            return _lights[light].Direction.Xyz;
        }

        public static void SetLightSwitch(int light, bool state)
        {
            _lights[light].On = state;
            int uLightOnLocation = Graphics.GetShader().GetUniformLocation("uLight[" + light + "].On");
            GL.Uniform1(uLightOnLocation, _lights[light].On ? 1 : 0);
        }

        public static void ApplyLightMatrix(Matrix4 world)
        {
            int uEyePosition = Graphics.GetShader().GetUniformLocation("uEyePosition");
            Vector4 eyePosition = Vector4.Transform(new Vector4(world.ExtractTranslation(), 1), world);
            GL.Uniform4(uEyePosition, ref eyePosition);

            for (int i = 0; i < _numLights; i++)
            {
                Vector4 position = _lights[i].Position;
                int uLightPositionLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].Position");
                position = Vector4.Transform(position, world);
                GL.Uniform4(uLightPositionLocation, position);

                Vector4 direction = _lights[i].Direction;
                int uLightDirectionLocation = Graphics.GetShader().GetUniformLocation("uLight[" + i + "].Direction");
                direction = Vector4.Transform(direction, world.ExtractRotation());
                GL.Uniform4(uLightDirectionLocation, direction);
            }
        }
    }
}
