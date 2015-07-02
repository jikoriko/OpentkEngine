using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class TextBox : ScrollPanel, KeyListener
    {
        private List<string> _lines;
        private bool _active;
        private int _cursorRow, _cursorColumn, _lastKey, _maxChar;

        private long _repeatTimer = 0;
        private static readonly int _initialKeyRepeatTimer = 400;
        private static readonly int _keyRepeatTimer = 50;

        public TextBox(int x, int y, int z, int width, int height, State state) 
            : base(x, y, z, width, height, state)
        {
            this.DisableHorizontalScroll();
            _lines = new List<string>();
            _lines.Add("");
            _cursorRow = _cursorColumn = 0;
            _lastKey = -1;
            _maxChar = 10000;
            state.AddKeyListener(this);
            _backgroundColor = Color4.White;
        }

        public bool IsActive()
        {
            return _active;
        }

        public override void Disable()
        {
            base.Disable();
            _active = false;
        }

        public void SetMaxChar(int max)
        {
            max = max <= 0 ? 1 : max;
            _maxChar = max;
        }

        public override void Update()
        {
            base.Update();
            if (_lastKey != -1)
            {
                if (Input.KeyDown((Key)_lastKey))
                {
                    if (_repeatTimer < Environment.TickCount)
                    {
                        _repeatTimer = Environment.TickCount + _keyRepeatTimer;
                        this.KeyDown((Key)_lastKey);
                    }
                }
                else
                {
                    _lastKey = -1;
                }
            }
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!_active)
            {
                _active = true;
            }
            BitmapFont font = Graphics.GetFont();
            int relativeY = Input.GetRelativeMouseY() - GetRelativeY() - _verticalScroll.GetRelativeY();
            relativeY /= font.GetLineHeight();
            _cursorRow = Math.Min(relativeY, _lines.Count - 1);

            int relativeX = Input.GetRelativeMouseX() - GetRelativeX();

            string line = _lines[_cursorRow];
            _cursorColumn = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (relativeX > font.GetWidth(line.Substring(0, i + 1)))
                {
                    _cursorColumn++;
                }
                else
                {
                    break;
                }
            }
            _cursorColumn = Math.Min(line.Length, _cursorColumn);
        }

        public override void OnRelease()
        {
            base.OnRelease();
            if (_active)
            {
                _active = false;
            }
        }

        public void KeyDown(Key key)
        {
            if (_active)
            {
                if (_lastKey != (int)key)
                {
                    _lastKey = (int)key;
                    _repeatTimer = Environment.TickCount + _initialKeyRepeatTimer;
                }
                else
                {
                    _repeatTimer = Environment.TickCount + _keyRepeatTimer;
                }

                if (key == Key.BackSpace)
                {
                    BitmapFont font = Graphics.GetFont();
                    string text = _lines[_cursorRow];
                    if (_cursorColumn == 0)
                    {
                        if (_cursorRow > 0)
                        {
                            _lines.RemoveAt(_cursorRow);
                            _cursorRow--;
                            _cursorColumn = _lines[_cursorRow].Length;
                            string line = _lines[_cursorRow] + text;
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (font.GetWidth(line.Substring(0, i + 1)) > _content.Width)
                                {
                                    _lines.Insert(_cursorRow + 1, line.Substring(i));
                                    line = line.Substring(0, i);
                                    break;
                                }
                            }
                            _lines[_cursorRow] = line;
                            _verticalScroll.SetScrollAmount(_lines.Count * font.GetLineHeight());
                        }

                    }
                    else if (text.Length != 0)
                    {
                        if (_cursorColumn == text.Length)
                        {
                            text = text.Substring(0, text.Length - 1);
                            _lines[_cursorRow] = text;
                            _cursorColumn--;
                        }
                        else
                        {
                            text = text.Remove(_cursorColumn - 1, 1);
                            _lines[_cursorRow] = text;
                            _cursorColumn--;
                        }
                        _verticalScroll.SetScrollAmount(_lines.Count * font.GetLineHeight());
                    }
                }
                else if (key == Key.Left)
                {
                    if (_cursorColumn > 0)
                    {
                        _cursorColumn--;
                    }
                    else
                    {
                        if (_cursorRow > 0)
                        {
                            _cursorRow--;
                            _cursorColumn = _lines[_cursorRow].Length;
                        }
                    }
                }
                else if (key == Key.Right)
                {
                    string text = _lines[_cursorRow];
                    if (_cursorColumn < text.Length)
                    {
                        _cursorColumn++;
                    }
                    else
                    {
                        if (_cursorRow < _lines.Count - 1)
                        {
                            _cursorRow++;
                            _cursorColumn = 0;
                        }
                    }
                }
                else if (key == Key.Up)
                {
                    if (_cursorRow > 0)
                    {
                        _cursorRow--;
                        if (_cursorColumn > _lines[_cursorRow].Length)
                            _cursorColumn = _lines[_cursorRow].Length;
                    }
                }
                else if (key == Key.Down)
                {
                    if (_cursorRow < _lines.Count - 1)
                    {
                        _cursorRow++;
                        if (_cursorColumn > _lines[_cursorRow].Length)
                            _cursorColumn = _lines[_cursorRow].Length;
                    }
                }
                else if (key == Key.Enter)
                {
                    BitmapFont font = Graphics.GetFont();
                    if (_cursorColumn != _lines[_cursorRow].Length)
                    {
                        string line = _lines[_cursorRow].Substring(0, _cursorColumn);
                        string line2 = _lines[_cursorRow].Substring(_cursorColumn);
                        _lines[_cursorRow] = line;
                        _lines.Insert(_cursorRow + 1, line2);

                    }
                    else
                    {
                        _lines.Insert(_cursorRow + 1, "");
                    }
                    _cursorRow++;
                    _cursorColumn = 0;
                    _verticalScroll.SetScrollAmount(_lines.Count * font.GetLineHeight());
                }

            }
        }

        public void CharDown(char c)
        {
            if (_active)
            {
                string text = _lines[_cursorRow];
                BitmapFont font = Graphics.GetFont();
                //if (_text.Length == _maxChar)
                //    return;

                if (_cursorColumn == text.Length)
                {
                    text += c;
                    if (font.GetWidth(text) > _content.Width)
                    {
                        _lines.Insert(_cursorRow + 1, c.ToString());
                        _cursorColumn = 1;
                        _cursorRow++;
                        _verticalScroll.SetScrollAmount(_lines.Count * font.GetLineHeight());
                    }
                    else
                    {
                        _lines[_cursorRow] = text;
                        _cursorColumn++;
                    }
                }
                else
                {
                    text = text.Insert(_cursorColumn, c.ToString());
                    if (font.GetWidth(text) > _content.Width)
                    {
                        char lastChar = text[text.Length - 1];
                        text = text.Remove(text.Length - 1);
                        _lines[_cursorRow] = text;
                        _cursorColumn++;
                        _lines.Insert(_cursorRow + 1, lastChar.ToString());
                        _verticalScroll.SetScrollAmount(_lines.Count * font.GetLineHeight());
                    }
                    else
                    {
                        _lines[_cursorRow] = text;
                        _cursorColumn++;
                    }
                }
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();

            BitmapFont font = Graphics.GetFont();
            int lineHeight = font.GetLineHeight();

            for (int i = 0; i < _lines.Count; i++)
            {
                string text = _lines[i];
                Graphics.DrawText(text, 0, lineHeight - font.GetHeight(text) + (i * lineHeight), 0, Color4.Black);
            }
            if (_active)
            {
                int cursorX = font.GetWidth(_lines[_cursorRow].Substring(0, _cursorColumn)) + 1;
                Graphics.FillRect(cursorX, (_cursorRow * lineHeight) + 1, 0, 2, lineHeight, Color4.Black);
            }
        }

        public void OnKeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs args)
        {
            this.KeyDown(args.Key);
        }

        public void OnKeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs args)
        {
        }

        public void OnKeyPress(object sender, OpenTK.KeyPressEventArgs args)
        {
            this.CharDown(args.KeyChar);
        }
    }
}
