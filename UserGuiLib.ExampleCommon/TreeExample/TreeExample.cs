using UserGuiLib.Common.Component;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Components;

namespace UserGuiLib.ExampleCommon.TreeExample
{
    public class TreeExample : Component, ILayout
    {
        private Label label;
        private Button addRandomNode;
        private BinaryTreeView<int> binaryTreeView;
        private ITree<int> tree;
        private MessagePopup popup;

        private MouseHandler mouse;

        public TreeExample()
        {
            mouse = new MouseHandler();
            RegisterService<ILayout>(this);
            RegisterService<IMouseEvents>(mouse);

            var random = new System.Random();

            addRandomNode = new Button("Add random node");
            tree = new BSTTree<int>();
            binaryTreeView = new BinaryTreeView<int>(tree);
            label = new Label
            {
                Text = "Click on nodes",
                Padding = new Common.Algebra.Vector2(10, 10)
            };

            addRandomNode.Transform.Size = new Common.Algebra.Vector2(220, 40);
            addRandomNode.OnClick += () => tree.Insert(random.Next(-20, 20));

            binaryTreeView.OnNodeClick += (node) =>
            {
                popup = new MessagePopup(node.Value.ToString(), 2);
                mouse.Block(true);
                popup.TimePassed += () =>
                {
                    Transform.RemoveChild(popup);
                    mouse.Block(false);
                    popup = null;

                };
                Transform.AddChild(popup);
            };

            Transform.AddChild(addRandomNode);
            Transform.AddChild(binaryTreeView);
            Transform.AddChild(label);
        }
        
        public void Relayout(IGraphics g)
        {
            Transform.Size = Transform.Parent.Size;
            binaryTreeView.Transform.Location = new Common.Algebra.Vector2(0, addRandomNode.Transform.Size.y);
            binaryTreeView.Transform.Size = Transform.Parent.Size - binaryTreeView.Transform.Location;

            label.GetService<ILayout>().Relayout(g);
            label.Transform.Location = new Common.Algebra.Vector2(Transform.Size.x - label.Transform.Size.x, 0);

            binaryTreeView.GetService<ILayout>().Relayout(g);

            if (popup != null)
                popup.GetService<ILayout>().Relayout(g);
        }
    }
}
