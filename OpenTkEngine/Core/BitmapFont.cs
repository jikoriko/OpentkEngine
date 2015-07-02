using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core
{
    public class BitmapFont
    {
        public struct BitmapGlyph
        {
            public int X, Y, Width, Height, OffsetX, OffsetY;
        }

        private Texture texture;
        private BitmapGlyph[] glyphs;
        private int lineHeight;

        public BitmapFont(String filename)
        {
            glyphs = new BitmapGlyph[256];
            ParseFont(filename);
            //lineHeight = 24;
        }

        private void ParseFont(String filename)
        {
            String[] lines = System.IO.File.ReadAllLines(filename);
            for (int i = 0; i < lines.Length; i++)
            {
                String[] parameters = lines[i].Split(new char[] { ' ' });
                switch (parameters[0])
                {
                    case "file:":
                        this.texture = Assets.GetTexture(parameters[1]);
                        break;
                    case "char:":
                        int id = int.Parse(parameters[1].Split(new char[] { '=' })[1]);
                        glyphs[id] = new BitmapGlyph();
                        glyphs[id].OffsetX = 0;
                        glyphs[id].OffsetY = 0;
                        for (int j = 2; j < parameters.Length; j++)
                        {
                            String[] parameter = parameters[j].Split(new char[] { '=' });
                            switch (parameter[0])
                            {
                                case "x":
                                    glyphs[id].X = int.Parse(parameter[1]);
                                    break;
                                case "y":
                                    glyphs[id].Y = int.Parse(parameter[1]);
                                    break;
                                case "width":
                                    glyphs[id].Width = int.Parse(parameter[1]);
                                    break;
                                case "height":
                                    glyphs[id].Height = int.Parse(parameter[1]);
                                    if (lineHeight < glyphs[id].Height)
                                        lineHeight = glyphs[id].Height;
                                    break;
                                case "offsetx":
                                    glyphs[id].OffsetX = int.Parse(parameter[1]);
                                    break;
                                case "offsety":
                                    glyphs[id].OffsetY = int.Parse(parameter[1]);
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        public int GetLineHeight()
        {
            return lineHeight;
        }

        public void RenderText(String text, float x, float y, float z, Color4 color)
        {
            float rX = x;
            float rY = y;
            int height = this.GetHeight(text);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                int sx = glyphs[(int)c].X;
                int sy = glyphs[(int)c].Y;
                int sw = glyphs[(int)c].Width;
                int sh = glyphs[(int)c].Height;
                int ox = glyphs[(int)c].OffsetX;
                int oy = glyphs[(int)c].OffsetY;
                Vector3 pos = new Vector3(rX + ox, rY + (height - sh) + oy, z);
                Vector2 dim = new Vector2(sw, sh);
                Vector2 src = new Vector2(sx, sy);
                Graphics.DrawTexture(this.texture, pos, dim, src, dim, color);
                rX += sw;
            }
        }

        public int GetWidth(String text)
        {
            int width = 0;
            for (int i = 0; i < text.Length; i++)
                width += glyphs[(int)text[i]].Width;
            return width;
        }

        public int GetHeight(String text)
        {
            int height = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (glyphs[(int)text[i]].Height > height)
                    height = glyphs[(int)text[i]].Height;
            }
            return height;
        }
    }
}
