using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;
using System.Collections;
using System.Drawing;

namespace OpenTkEngine.Core
{
    public class Graphics
    {
        private static int _vao;
        private static int[] _vbos;

        private static int _currentVao = -1;

        private static int[] _cornerVbos;
        private static int _cornerVao;
        private static float[] _cornerVerts = new float[91 * 3];
        private static int[] _cornerIndices = new int[91];

        private static Rectangle _window;

        private static Vector2 _texturePosition = new Vector2(0.0f, 0.0f);
        private static Vector2 _textureDimension = new Vector2(1.0f, 1.0f);
        private static bool _texCoordsChanged = false;

        private static Matrix4 _worldMatrix = Matrix4.Identity;
        private static bool _worldMatrixChanged = true;
        private static Stack _worldMatrixStack = new Stack();

        private static Stack _clipStack = new Stack();
        private static bool _screenClip = false;

        private static int _stencilDepth = -1;
        private static Stack _stencilStack = new Stack();

        private static Color4 _clearColor = Color4.Black;

        public enum RenderMode
        {
            Ortho,
            Perspective
        }
        private static RenderMode _renderMode = RenderMode.Ortho;

        private static BitmapFont _defaultFont = Assets.GetFont("default.txt");
        private static BitmapFont _currentFont = _defaultFont;

        private static Shader _defaultShader = Assets.GetShader("vPassThrough.vert", "fColor.frag");
        private static Shader _currentShader = _defaultShader;

        public static void Initialize()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.CullFace(CullFaceMode.Back);

            _currentShader.Bind();
            Lighting.InitLights();
        }

        public static void SetClearColor(Color4 color)
        {
            _clearColor = color;
            GL.ClearColor(_clearColor);
        }

        private static void BindSquare()
        {
            if (_vao == 0)
            {
                _vao = GL.GenVertexArray();
                _vbos = new int[3];
                GL.GenBuffers(3, _vbos);

                float[] verts = new float[] {
                    0, 0, 0, 
                    1, 0, 0,
                    1, 1, 0,
                    0, 1, 0
                };
                
                int[] indices = new int[] {
                    0, 1, 2, 3
                };

                GL.BindVertexArray(_vao);
                _currentVao = _vao;

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(verts.Length * sizeof(float)), verts, BufferUsageHint.StaticDraw);

                int size;
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
                if (verts.Length * sizeof(float) != size)
                {
                    throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
                }

                int vPositionLocation = _currentShader.GetAttribLocation("vPosition");
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

                BindTextureCoords();

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _vbos[2]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(int)), indices, BufferUsageHint.StaticDraw);

                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                if (indices.Length * sizeof(int) != size)
                {
                    throw new ApplicationException("Index data not loaded onto graphics card correctly");
                }

                int vTexCoordsLocation = _currentShader.GetAttribLocation("vTexCoords");
                GL.EnableVertexAttribArray(vTexCoordsLocation);
                GL.VertexAttribPointer(vTexCoordsLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            }
            else
            {
                GL.BindVertexArray(_vao);
                _currentVao = _vao;
            }
        }

        private static void BindCorner()
        {
            if (_cornerVao == 0)
            {
                _cornerVao = GL.GenVertexArray();
                _cornerVbos = new int[2];
                GL.GenBuffers(2, _cornerVbos);

                _cornerVerts[0] = 1.0f;
                _cornerVerts[1] = 1.0f;
                float segments = _cornerIndices.Length - 1;
                for (int i = 0; i < segments; i++)
                {
                    float angle = MathHelper.DegreesToRadians(90) * ((float)i / (segments - 1)) - (float)Math.PI;
                    _cornerVerts[(i + 1) * 3] = (float)Math.Sin(angle) + 1.0f;
                    _cornerVerts[((i + 1) * 3) + 1] = (float)Math.Cos(angle) + 1.0f;
                    _cornerIndices[i + 1] = i + 1;
                }

                GL.BindVertexArray(_cornerVao);
                _currentVao = _cornerVao;

                GL.BindBuffer(BufferTarget.ArrayBuffer, _cornerVbos[0]);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(_cornerVerts.Length * sizeof(float)), _cornerVerts, BufferUsageHint.StaticDraw);

                int size;
                GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
                if (_cornerVerts.Length * sizeof(float) != size)
                {
                    throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
                }

                int vPositionLocation = _currentShader.GetAttribLocation("vPosition");
                GL.EnableVertexAttribArray(vPositionLocation);
                GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, _cornerVbos[1]);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(_cornerIndices.Length * sizeof(int)), _cornerIndices, BufferUsageHint.StaticDraw);

                GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
                if (_cornerIndices.Length * sizeof(int) != size)
                {
                    throw new ApplicationException("Index data not loaded onto graphics card correctly");
                }
            }
            else
            {
                GL.BindVertexArray(_cornerVao);
                _currentVao = _cornerVao;
            }
        }

        private static void BindTextureCoords()
        {
            float[] texCoords = new float[] {
                _texturePosition.X, _texturePosition.Y,
                _texturePosition.X + _textureDimension.X, _texturePosition.Y,
                _texturePosition.X + _textureDimension.X, _texturePosition.Y + _textureDimension.Y,
                _texturePosition.X, _texturePosition.Y + _textureDimension.Y,
            };

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbos[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(texCoords.Length * sizeof(float)), texCoords, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (texCoords.Length * sizeof(float) != size)
            {
                throw new ApplicationException("texture coordinate data not loaded onto graphics card correctly");
            }

        }

        public static void SetColor(Color4 color)
        {
            int uColorLocation = _currentShader.GetUniformLocation("uColour");
            GL.Uniform4(uColorLocation, color);
        }

        public static Shader GetShader()
        {
            return _currentShader;
        }

        public static void Clear()
        {
            _worldMatrixStack.Clear();
            _worldMatrix = Matrix4.Identity;
            _worldMatrixChanged = true;
            _clipStack.Clear();
            ClearScreenClip();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        }

        public static void SetRenderMode(RenderMode mode)
        {
            _renderMode = mode;
            SetProjection(_window);
        }

        public static void SetProjection(Rectangle client)
        {
            GL.Viewport(client);
            _window = client;

            Matrix4 projection = Matrix4.Identity;

            if (_renderMode == RenderMode.Ortho)
            {
                projection = Matrix4.CreateOrthographicOffCenter(0, client.Width, client.Height, 0, 100, -100);
            }
            else
            {
                projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, client.Width / (float)client.Height, 1.0f, 1000f); 
            }
            int uProjectionLocation = _currentShader.GetUniformLocation("uProjection");
            GL.UniformMatrix4(uProjectionLocation, true, ref projection);
        }

        public static void PushWorldMatrix()
        {
            _worldMatrixChanged = true;
            if (_worldMatrixStack.Count != 0)
                _worldMatrixStack.Push(_worldMatrixStack.Peek());
            else
                _worldMatrixStack.Push(_worldMatrix);
        }

        public static void PopWorldMatrix()
        {
            if (_worldMatrixStack.Count != 0)
            {
                _worldMatrixChanged = true;
                _worldMatrixStack.Pop();
            }
        }

        public static void SetWorldMatrix(Matrix4 matrix)
        {
            if (_worldMatrixStack.Count != 0)
            {
                _worldMatrixStack.Pop();
                _worldMatrixStack.Push(matrix);
            }
            else
            {
                _worldMatrix = matrix;
            }
            _worldMatrixChanged = true;
        }

        public static void SetWorldTranslation(Vector3 translation)
        {
            if (_worldMatrixStack.Count != 0)
            {
                _worldMatrixStack.Pop();
                _worldMatrixStack.Push(Matrix4.CreateTranslation(translation));
                _worldMatrixChanged = true;
            }
            else
            {
                _worldMatrix = Matrix4.Identity;
                TranslateWorld(translation);
            }
        }

        public static void TranslateWorld(Vector3 translation)
        {
            if (_worldMatrixStack.Count != 0)
            {
                Matrix4 currentMatrix = (Matrix4)_worldMatrixStack.Peek();
                _worldMatrixStack.Pop();
                currentMatrix *= Matrix4.CreateTranslation(translation);
                _worldMatrixStack.Push(currentMatrix);
            }
            else
            {
                _worldMatrix *= Matrix4.CreateTranslation(translation);
            }
            _worldMatrixChanged = true;
        }

        public static Vector3 GetTranslation()
        {
            return _worldMatrix.ExtractTranslation();
        }

        private static void SetScreenClip(Rectangle clip)
        {
            if (!_screenClip)
            {
                GL.Enable(EnableCap.ScissorTest);
                _screenClip = true;
            }
            GL.Scissor(clip.X, _window.Height - clip.Y - clip.Height, clip.Width, clip.Height);
        }

        public static void PushScreenClip(Rectangle clip)
        {
            if (_clipStack.Count > 0)
            {
                Rectangle currentClip = (Rectangle)_clipStack.Peek();
                if (currentClip.Contains(clip))
                {
                    SetScreenClip(clip);
                    _clipStack.Push(clip);
                }
                else if (currentClip.IntersectsWith(clip))
                {
                    currentClip.Intersect(clip);
                    SetScreenClip(currentClip);
                    _clipStack.Push(currentClip);
                }
                else
                {
                    SetScreenClip(Rectangle.Empty);
                    _clipStack.Push(Rectangle.Empty);
                }
            }
            else
            {
                SetScreenClip(clip);
                _clipStack.Push(clip);
            }
        }

        public static void PopScreenClip()
        {
            if (_clipStack.Count == 0)
                return;
            else
            {

                _clipStack.Pop();
                if (_clipStack.Count > 0)
                {
                    SetScreenClip((Rectangle)_clipStack.Peek());
                }
                else
                {
                    ClearScreenClip();
                }
            }
        }

        private static void ClearScreenClip()
        {
            GL.Disable(EnableCap.ScissorTest);
            _screenClip = false;
        }

        public static void PushStencilDepth(StencilOp stencilOp)
        {
            if (_stencilDepth == -1)
            {
                GL.Enable(EnableCap.StencilTest);
                GL.StencilMask(0xFF);
                GL.Clear(ClearBufferMask.StencilBufferBit);
            }
            _stencilDepth++;

            SetStencilOp(stencilOp);
            _stencilStack.Push(stencilOp);
        }

        public static void SetStencilOp(StencilOp stencilOp)
        {
            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, stencilOp);
            StencilFunction function = stencilOp == StencilOp.Keep ? StencilFunction.Less : StencilFunction.Lequal;
            GL.StencilFunc(function, _stencilDepth, 0xFF);
        }

        public static void PopStencilDepth()
        {
            if (_stencilDepth > -1)
            {
                _stencilDepth--;
                _stencilStack.Pop();
                if (_stencilDepth == -1)
                {
                    GL.Disable(EnableCap.StencilTest);
                }
                else
                {
                    StencilOp stencilOp = (StencilOp)_stencilStack.Peek();
                    SetStencilOp(stencilOp);
                }
            }
        }

        public static BitmapFont GetFont()
        {
            return _currentFont;
        }

        public static void DrawText(string text, float x, float y, float z, Color4 color)
        {
            _currentFont.RenderText(text, x, y, z, color);
        }

        public static void DrawRect(float x, float y, float z, float width, float height, int lineWidth, Color4 color)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(lineWidth);
            FillRect(x, y, z, width, height, color);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
        }

        public static void FillRect(float x, float y, float z, float width, float height, Color4 color)
        {
            Texture.BindNone();
            SetColor(color);

            RenderQuad(x, y, z, width, height);
        }

        public static void DrawTexture(Texture texture, float x, float y, float z, Color4 color)
        {
            DrawTexture(texture, x, y, z, texture.GetWidth(), texture.GetHeight(), color);
        }

        public static void DrawTexture(Texture texture, float x, float y, float z, float width, float height, Color4 color)
        {
            DrawTexture(texture, x, y, z, width, height, 0, 0, texture.GetWidth(), texture.GetHeight(), color);
        }

        public static void DrawTexture(Texture texture, Vector3 position, Vector2 size, Vector2 sourcePosition, Vector2 sourceDimension, Color4 color)
        {
            DrawTexture(texture, position.X, position.Y, position.Z, size.X, size.Y, 
                (int)sourcePosition.X, (int)sourcePosition.Y, (int)sourceDimension.X, (int)sourceDimension.Y, color);
        }

        public static void DrawTexture(Texture texture, Rectangle dest, float z, Rectangle source, Color4 color)
        {
            DrawTexture(texture, dest.X, dest.Y, z, dest.Width, dest.Height, source.X, source.Y, source.Width, source.Height, color);
        }

        public static void DrawTexture(Texture texture, float x, float y, float z, float width, float height, int sx, int sy, int sw, int sh, Color4 color)
        {
            float textX = (float)sx / texture.GetWidth();
            float textY = (float)sy / texture.GetHeight();
            float textW = (float)sw / texture.GetWidth();
            float textH = (float)sh / texture.GetHeight();

            if (textX != _texturePosition.X || textY != _texturePosition.Y || textW != _textureDimension.X || textH != _textureDimension.Y)
            {
                _texturePosition.X = textX;
                _texturePosition.Y = textY;
                _textureDimension.X = textW;
                _textureDimension.Y = textH;
                _texCoordsChanged = true;
            }

            texture.Bind();
            SetColor(color);

            RenderQuad(x, y, z, width, height);

            
        }

        public static void DrawRoundedRect(float x, float y, float z, float width, float height, float radius, int lineWidth, Color4 color)
        {
            if (radius <= 0)
            {
                DrawRect(x, y, z, width, height, lineWidth, color);
                return;
            }
            Texture.BindNone();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(lineWidth);
            SetColor(color);
            if (radius > width / 2)
            {
                radius = width / 2;
            }
            if (radius > height / 2)
            {
                radius = height / 2;
            }

            DrawCorner(x, y, z, radius, 0);
            DrawCorner(x + width - radius, y, z, radius, MathHelper.DegreesToRadians(90));

            DrawCorner(x, y + height - radius, z, radius, MathHelper.DegreesToRadians(-90));
            DrawCorner(x + width - radius, y + height - radius, z, radius, (float)Math.PI);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            bool xRect = radius < width / 2;
            bool yRect = radius < height / 2;
            if (xRect)
            {
                FillRect(x + radius, y - (lineWidth * 0.5f), z, width - (radius * 2), lineWidth, color);
                FillRect(x + radius, y + height - (lineWidth * 0.5f), z, width - (radius * 2), lineWidth, color);
            }
            if (yRect)
            {
                FillRect(x - (lineWidth * 0.5f), y + radius, z, lineWidth, height - (radius * 2), color);
                FillRect(x + width - (lineWidth * 0.5f), y + radius, z, lineWidth, height - (radius * 2), color);
            }
        }

        public static void FillRoundedRect(float x, float y, float z, float width, float height, float radius, Color4 color)
        {
            if (radius <= 0)
            {
                FillRect(x, y, z, width, height, color);
                return;
            }
            Texture.BindNone();
            SetColor(color);
            if (radius > width / 2)
            {
                radius = width / 2;
            }
            if (radius > height / 2)
            {
                radius = height / 2;
            }
            
            FillCorner(x, y, z, radius, 0);
            FillCorner(x + width - radius, y, z, radius, MathHelper.DegreesToRadians(90));

            FillCorner(x, y + height - radius, z, radius, MathHelper.DegreesToRadians(-90));
            FillCorner(x + width - radius, y + height - radius, z, radius, (float)Math.PI);

            bool xRect = radius < width / 2;
            bool yRect = radius < height / 2;
            if (xRect)
            {
                FillRect(x + radius, y, z, width - (radius * 2), radius, color);
                FillRect(x + radius, y + height - radius, z, width - (radius * 2), radius, color);
            }
            if (yRect)
            {
                FillRect(x, y + radius, z, radius, height - (radius * 2), color);
                FillRect(x + width - radius, y + radius, z, radius, height - (radius * 2), color);
            }
            if (xRect && yRect)
                FillRect(x + radius, y + radius, z, width - (radius * 2), height - (radius * 2), color);
        }

        private static void DrawCorner(float x, float y, float z, float radius, float rotation)
        {
            if (_currentVao != _cornerVao)
                BindCorner();

            int uModelLocation = _currentShader.GetUniformLocation("uModel");
            Matrix4 matrix = Matrix4.CreateTranslation(-0.5f, -0.5f, 0) * Matrix4.CreateRotationZ(rotation) * Matrix4.CreateTranslation(0.5f, 0.5f, 0.5f);
            matrix *= Matrix4.CreateScale(radius, radius, 0) * Matrix4.CreateTranslation(x, y, z);
            GL.UniformMatrix4(uModelLocation, true, ref matrix);

            if (_worldMatrixChanged)
            {
                int uWorldLocation = _currentShader.GetUniformLocation("uWorld");
                Matrix4 worldMatrix = _worldMatrixStack.Count > 0 ? (Matrix4)_worldMatrixStack.Peek() : _worldMatrix;
                GL.UniformMatrix4(uWorldLocation, true, ref worldMatrix);
            }

            GL.DrawElements(PrimitiveType.LineStrip, _cornerIndices.Length - 1, DrawElementsType.UnsignedInt, 1 * sizeof(uint));
        }

        private static void FillCorner(float x, float y, float z, float radius, float rotation)
        {
            if (_currentVao != _cornerVao)
                BindCorner();

            int uModelLocation = _currentShader.GetUniformLocation("uModel");
            Matrix4 matrix = Matrix4.CreateTranslation(-0.5f, -0.5f, 0) * Matrix4.CreateRotationZ(rotation) * Matrix4.CreateTranslation(0.5f, 0.5f, 0.5f);
            matrix *= Matrix4.CreateScale(radius, radius, 0) * Matrix4.CreateTranslation(x, y, z);
            GL.UniformMatrix4(uModelLocation, true, ref matrix);

            if (_worldMatrixChanged)
            {
                int uWorldLocation = _currentShader.GetUniformLocation("uWorld");
                Matrix4 worldMatrix = _worldMatrixStack.Count > 0 ? (Matrix4)_worldMatrixStack.Peek() : _worldMatrix;
                GL.UniformMatrix4(uWorldLocation, true, ref worldMatrix);
            }

            GL.DrawElements(PrimitiveType.TriangleFan, _cornerIndices.Length, DrawElementsType.UnsignedInt, 0);
        }

        private static void RenderQuad(float x, float y, float z, float width, float height)
        {
            RenderQuad(x, y, z, width, height, 0, 4);
        }

        private static void RenderQuad(float x, float y, float z, float width, float height, int offset, int count)
        {
            if (_currentVao != _vao)
                BindSquare();
            if (_texCoordsChanged)
                BindTextureCoords();

            int uModelLocation = _currentShader.GetUniformLocation("uModel");
            Matrix4 matrix = Matrix4.CreateScale(width, height, 0) * Matrix4.CreateTranslation(x, y, z);
            GL.UniformMatrix4(uModelLocation, true, ref matrix);

            if (_worldMatrixChanged)
            {
                int uWorldLocation = _currentShader.GetUniformLocation("uWorld");
                Matrix4 worldMatrix = _worldMatrixStack.Count > 0 ? (Matrix4)_worldMatrixStack.Peek() : _worldMatrix;
                GL.UniformMatrix4(uWorldLocation, true, ref worldMatrix);
            }

            GL.DrawElements(PrimitiveType.Polygon, count, DrawElementsType.UnsignedInt, offset * sizeof(uint));
        }

        public static void SetModelMaterial(Materials.Material material)
        {
            int uAmbientReflectivityLocation = _currentShader.GetUniformLocation("uMaterial.AmbientReflectivity");
            GL.Uniform3(uAmbientReflectivityLocation, material.ambient);
            int uDiffuseReflectivityLocation = _currentShader.GetUniformLocation("uMaterial.DiffuseReflectivity");
            GL.Uniform3(uDiffuseReflectivityLocation, material.diffuse);
            int uSpecularReflectivityLocation = _currentShader.GetUniformLocation("uMaterial.SpecularReflectivity");
            GL.Uniform3(uSpecularReflectivityLocation, material.specular);
            int uShininessLocation = _currentShader.GetUniformLocation("uMaterial.Shininess");
            GL.Uniform1(uShininessLocation, material.shininess);
        }

        public static void RenderModel(Model model, Matrix4 matrix)
        {
            RenderModel(model, matrix, 0);
        }

        public static void RenderModel(Model model, Matrix4 matrix, int offset)
        {
            RenderModel(model, matrix, offset, model.GetIndicesLength() - offset);
        }

        public static void RenderModel(Model model, Matrix4 matrix, int offset, int count)
        {
            Texture.BindNone();
            Lighting.EnableLighting();
            if (_currentVao != model.GetVaoID())
            {
                model.BindVAO();
                _currentVao = model.GetVaoID();
            }
            int uModelLocation = _currentShader.GetUniformLocation("uModel");
            GL.UniformMatrix4(uModelLocation, true, ref matrix);

            if (_worldMatrixChanged)
            {
                int uWorldLocation = _currentShader.GetUniformLocation("uWorld");
                Matrix4 worldMatrix = _worldMatrixStack.Count > 0 ? (Matrix4)_worldMatrixStack.Peek() : _worldMatrix;
                GL.UniformMatrix4(uWorldLocation, true, ref worldMatrix);
                Lighting.ApplyLightMatrix(worldMatrix);
            }

            GL.DrawElements(PrimitiveType.Triangles, count, DrawElementsType.UnsignedInt, offset * sizeof(uint));
            Lighting.DisableLighting();

        }

        public static void Destroy()
        {
            if (_vbos != null)
                GL.DeleteBuffers(3, _vbos);
            if (_vao != 0)
                GL.DeleteVertexArray(_vao);
        }
    }
}
