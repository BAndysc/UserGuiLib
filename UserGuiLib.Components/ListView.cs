using ObservableObjects;
using System.Collections.Generic;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.Components
{
    public class ListView<T> : Component, IRenderer
    {
        public ObservableList<T> Items { get; private set; } = new ObservableList<T>();

        private Dictionary<T, ListViewItem<T>> keyToView = new Dictionary<T, ListViewItem<T>>();

        public ListView()
        {
            var layout = new VerticalGroupLayout();
            layout.HorizontalStrech = true;
            RegisterService<ILayout>(layout);
            RegisterService<IRenderer>(this);

            RegisterService<IMouseEvents>(new MouseHandler());

            Items.OnInsertItem += Items_OnInsertItem;
            Items.OnRemoveItem += Items_OnRemoveItem;
        }

        private void Items_OnRemoveItem(T obj)
        {
            Transform.RemoveChild(keyToView[obj]);
            keyToView.Remove(obj);
        }

        private void Items_OnInsertItem(T obj, int index)
        {
            var view = new ListViewItem<T>(obj);
            keyToView[obj] = view;
            Transform.AddChild(view);
        }

        public void Render(IGraphics g)
        {
            g.FillRectangle(new AnyPen(255, 255, 255, 255, 1), Vector2.Zero, Transform.Size);
            g.DrawRectangle(new AnyPen(255, 0, 0, 0, 1), Vector2.Zero, Transform.Size);
        }
    }
}
