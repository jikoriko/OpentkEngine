using OpenTK.Graphics;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class TextField : Control, KeyListener
    {
        private String _text;
        private bool _active;
        private int _cursorPos, _lastKey, _maxChar, _offsetX;

        private long _repeatTimer = 0;
        private static readonly int _initialKeyRepeatTimer = 400;
        private static readonly int _keyRepeatTimer = 50;

        public TextField(int x, int y, int z, int width, State state)
            : base(x, y, z, width, 0, state)
        {
            int height = Graphics.GetFont().GetLineHeight();
            Resize(width, height + (_borderThickness * 2));
            _text = "";
            _active = false;
            _cursorPos = 0;
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

        public void SetText(string text)
        {
            _text = text;
            if (_cursorPos > _text.Length)
            {
                _cursorPos = _text.Length;
            }
        }

        public string GetText()
        {
            return _text;
        }

        private void ScrollCheck()
        {
            int cursorX = _body.X + 1 + Graphics.GetFont().GetWidth(_text.Substring(0, _cursorPos));
            if (cursorX + _offsetX > _body.Right - 3)
            {
                _offsetX = -(cursorX - _body.Width - (_body.X - 3));
            }
            else if (cursorX + _offsetX < _body.X + 1)
            {
                _offsetX = -(cursorX - (_body.X + 1));
            }
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

            if (this.Selectable())
            {
                if (!_active && Input.MouseLeftTriggered())
                {
                    _active = true;
                }
            }
            else
            {
                if (_active && Input.MouseLeftTriggered())
                {
                    _active = false;
                }
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
                    if (!(_text.Length == 0) && _cursorPos != 0)
                    {
                        if (_cursorPos == _text.Length)
                        {
                            _text = _text.Substring(0, _text.Length - 1);
                            _cursorPos--;
                        }
                        else
                        {
                            _text = _text.Remove(_cursorPos - 1, 1);
                            _cursorPos--;
                        }
                        ScrollCheck();
                    }
                }
                else if (key == Key.Left)
                {
                    if (_cursorPos > 0)
                    {
                        _cursorPos--;
                        ScrollCheck();
                    }
                }
                else if (key == Key.Right)
                {
                    if (_cursorPos < _text.Length)
                    {
                        _cursorPos++;
                        ScrollCheck();
                    }
                }
                else if (key == Key.Enter)
                {
                    //this.Triggered = true;
                }

            }
        }

        public void CharDown(char c)
        {
            if (_active)
            {
                if (_text.Length == _maxChar)
                    return;

                if (_cursorPos == _text.Length)
                {
                    _text += c;
                    _cursorPos++;
                }
                else
                {
                    _text = _text.Insert(_cursorPos, c.ToString());
                    _cursorPos++;
                }
                ScrollCheck();
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();

            BitmapFont font = Graphics.GetFont();
            int textHeight = font.GetHeight(_text);

            Graphics.DrawText(_text, _offsetX, _content.Height - textHeight, 0, Color4.Black);

            int cursorX = font.GetWidth(_text.Substring(0, _cursorPos));
            cursorX += 1 + _offsetX;
            if (_active)
                Graphics.FillRect(cursorX, 1, 0, 2, _content.Height - 2, Color4.Black);
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
