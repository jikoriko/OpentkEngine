using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkEngine.Core.Gui
{
    public class ListBox : ScrollPanel
    {
        protected int _index;
        protected List<string> _items;
        protected Color4 _selectionColor = Color4.SkyBlue;
        protected bool _selectionChanged;
        protected int _maxItems;

        public ListBox(int x, int y, int z, int width, int height, List<string> items, State state)
            : base(x, y, z, width, height, state)
        {
            _backgroundColor = Color4.AliceBlue;
            this.InitializeItems(items);
            _selectionChanged = false;
            this.SetContentDimensions(_content.Width, _items.Count * 32);
            DisableHorizontalScroll();
        }

        public ListBox(int x, int y, int z, int width, int height, string[] items, State state)
            : base(x, y, z, width, height, state)
        {
            _backgroundColor = Color4.AliceBlue;
            this.InitializeItems(items);
            _selectionChanged = false;
            this.SetContentDimensions(_content.Width, _items.Count * 32);
            DisableHorizontalScroll();
        }

        public ListBox(int x, int y, int z, int width, int height, int maxItems, State state)
            : base(x, y, z, width, height, state)
        {
            _backgroundColor = Color4.AliceBlue;
            this.InitializeItems(maxItems);
            _selectionChanged = false;
            this.SetContentDimensions(_content.Width, _items.Count * 32);
            DisableHorizontalScroll();
        }

        private void InitializeItems(List<String> items)
        {
            _maxItems = items.Count;
            if (items.Count > 0)
            {
                _index = 0;
                _selectionChanged = true;
            }
            else
                _index = -1;
            _items = items;
        }

        private void InitializeItems(string[] items)
        {
            List<string> itemsList = new List<string>();
            for (int i = 0; i < items.Length; i++)
            {
                itemsList.Add(items[i]);
            }
            this.InitializeItems(itemsList);
        }

        private void InitializeItems(int maxItems)
        {
            List<string> itemsList = new List<string>();
            for (int i = 0; i < maxItems; i++)
            {
                itemsList.Add("Item " + i);
            }
            this.InitializeItems(itemsList);
        }

        public void SetSelection(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                _index = index;
                _selectionChanged = true;
            }
        }

        public int Getselection()
        {
            return _index;
        }

        public bool SelectionChanged()
        {
            return _selectionChanged;
        }

        public List<string> GetItems()
        {
            return _items;
        }

        public override void Update()
        {
            base.Update();
            _selectionChanged = false;
            if (_maxItems != _items.Count)
            {
                _maxItems = _items.Count;
                this.SetContentDimensions(_content.Width, _items.Count * 32);
            }
            if (_index >= _items.Count)
            {
                _index = _items.Count - 1;
                _selectionChanged = true;
            }
            if (this.IsTriggered())
            {
                int target = (Input.GetRelativeMouseY() - this.GetRelativeY()) / 32;
                if (target < _items.Count)
                {
                    _index = target;
                    _selectionChanged = true;
                }
            }
        }

        public override void RenderContent()
        {
            base.RenderContent();
            if (_index >= 0)
            {
                Graphics.FillRect(0, _index * 32, 0, _content.Width, 32, _selectionColor);
            }
            for (int i = 0; i < _items.Count; i++)
            {
                int x = (_content.Width / 2) - (Graphics.GetFont().GetWidth(_items[i]) / 2);
                int y = 16 - (Graphics.GetFont().GetHeight(_items[i]) / 2) + (i * 32);
                Graphics.DrawText(_items[i], x, y, 0, Color4.Black);
            }
        }
    }
}
