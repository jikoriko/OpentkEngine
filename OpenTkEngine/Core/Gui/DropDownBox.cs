using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class DropDownBox : ScrollPanel
    {
        private int _maxItemsVisible = 10;
        private bool _droppedDown = false;
        int _selectedItem = 0;
        private bool _selectionChanged = false;

        string[] _items = new string[] 
        {
            "item 1", "item 2", "item 3", "item 4", "item 5", 
            "item 6", "item 7", "item 8", "item 9", "item 10"
            , "item 11"
        };


        public DropDownBox(int x, int y, int z, int width, State state)
            : base(x, y, z, width, Graphics.GetFont().GetLineHeight(), state)
        {
            Resize(GetWidth(), Graphics.GetFont().GetLineHeight() + (_borderThickness * 2));
            DisableHorizontalScroll();
            DisableVerticalScroll();
            _backgroundColor = Color4.White;
            _verticalScroll.SetScrollAmount(_items.Length * Graphics.GetFont().GetLineHeight());
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!_droppedDown)
            {
                _droppedDown = true;
                if (_items.Length > _maxItemsVisible)

                    EnableVerticalScroll();
            }
            else
            {
                int target = (Input.GetRelativeMouseY() - this.GetRelativeY()) / Graphics.GetFont().GetLineHeight();
                Console.WriteLine(target);
                if (target < _items.Length && target > 0)
                {
                    if (target <= _selectedItem)
                        target -= 1;
                    _selectedItem = target;
                    _droppedDown = false;
                    //DisableVerticalScroll();
                    _selectionChanged = true;
                }
            }
        }

        public virtual bool SelectionChanged()
        {
            return _selectionChanged;
        }

        public int GetSelection()
        {
            return _selectedItem;
        }

        public override void OnRelease()
        {
            base.OnRelease();
            if (!_verticalScroll.Selectable() && _droppedDown)
            {
                _droppedDown = false;
            }
        }

        public override void Update()
        {
            base.Update();
            if (this._droppedDown)
            {
                int maxHeight = (_maxItemsVisible * Graphics.GetFont().GetLineHeight()) + (_borderThickness * 2);
                int minHeight = (_items.Length * Graphics.GetFont().GetLineHeight()) + (_borderThickness * 2);
                int targetHeight = Math.Min(maxHeight, minHeight);
                if (_body.Height < targetHeight)
                {
                    Resize(_body.Width, _body.Height + (int)(targetHeight * 0.2f));
                    if (_body.Height > targetHeight)
                    {
                        int difference = _body.Height - targetHeight;
                        Resize(_body.Width, _body.Height - difference);
                    }
                }
                
            }
            else
            {
                if (_body.Height > Graphics.GetFont().GetLineHeight())
                {
                    int maxHeight = (_maxItemsVisible * Graphics.GetFont().GetLineHeight()) + (_borderThickness * 2);
                    int minHeight = (_items.Length * Graphics.GetFont().GetLineHeight()) + (_borderThickness * 2);
                    int targetHeight = Math.Min(maxHeight, minHeight);
                    Resize(_body.Width, _body.Height - (int)(targetHeight * 0.2f));
                    if (_body.Height <= Graphics.GetFont().GetLineHeight() + (_borderThickness * 2))
                    {
                        int difference = (Graphics.GetFont().GetLineHeight() + (_borderThickness * 2)) - _body.Height;
                        Resize(_body.Width, _body.Height + difference);
                        DisableVerticalScroll();
                    }
                }
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();
            Graphics.FillRect(0, 0, 0, _content.Width, Graphics.GetFont().GetLineHeight(), Color4.SkyBlue);

            string item1 = _items[_selectedItem];
            int cx = (_content.Width / 2) - (Graphics.GetFont().GetWidth(item1) / 2);
            Graphics.DrawText(item1, cx, 0, 0, Color4.Black);
            if (_droppedDown)
            {
                int offsetY = Graphics.GetFont().GetLineHeight();
                for (int i = 0; i < _items.Length; i++)
                {
                    if (i == _selectedItem) continue;
                    string item = _items[i];
                    cx = (_content.Width / 2) - (Graphics.GetFont().GetWidth(item) / 2);
                    Graphics.DrawText(item, cx, offsetY, 0, Color4.Black);
                    offsetY += Graphics.GetFont().GetLineHeight();
                }
            }
        }
    }
}
