using System;
using System.Collections.Generic;
using UserGuiLib.Common.Algebra;
using UserGuiLib.Common.Drawing;
using UserGuiLib.Common.Impl;
using UserGuiLib.Common.Services.Layout;
using UserGuiLib.Common.Services.MouseEvents;
using UserGuiLib.Common.Services.Render;

namespace UserGuiLib.ExampleCommon
{
    public class BinaryTreeNode<T> : Component, IRenderer, ILayout, IMouseEvents where T : IComparable
    {
        public ITreeNode<T> Node { get; }
        
        private static readonly float NODE_SIZE = 40;

        private bool mouseOver;

        public event Action<ITreeNode<T>> OnNodeClick = delegate { };

        public BinaryTreeNode(ITreeNode<T> node)
        {
            RegisterService<ILayout>(this);
            RegisterService<IRenderer>(this);
            RegisterService<IMouseEvents>(this);
            Node = node;
        }
        
        public void Render(IGraphics graphics)
        {
            var pen = mouseOver ? new AnyPen(255, 235, 242, 252, 1) : new AnyPen(255, 255, 255, 255, 1);
            graphics.FillCircle(pen, Vector2.Zero, Vector2.Zero, NODE_SIZE - 0.5f);
            graphics.DrawCircle(new AnyPen(255, 0, 0, 0, 1), Vector2.Zero, Vector2.Zero, NODE_SIZE);

            graphics.DrawString(Node.Value.ToString(), new AnyPen(255, 0, 0, 0, 1), new AnyFont("MS Sans Serif", 10), Transform.Size/2, Vector2.Half);
        }

        public void Relayout(IGraphics g)
        {
            if (Transform.Size.x != NODE_SIZE || Transform.Size.y != NODE_SIZE)
                Transform.Size = new Vector2(NODE_SIZE, NODE_SIZE);
        }

        public void MouseDown(MouseButtons buttons, Vector2 cursor)
        {
            OnNodeClick(Node);
        }

        public void MouseUp(MouseButtons buttons, Vector2 cursor)
        {
        }

        public void MouseMove(MouseButtons buttons, Vector2 cursor)
        {
        }

        public void MouseEnter()
        {
            mouseOver = true;
        }

        public void MouseExit()
        {
            mouseOver = false;
        }
    }

    public class BinaryTreeView<T> : Component, ILayout, IRenderer where T : IComparable
    {
        private Dictionary<ITreeNode<T>, BinaryTreeNode<T>> nodeToComponentDict = new Dictionary<ITreeNode<T>, BinaryTreeNode<T>>();

        private BinaryTreeNode<T> root = null;
        
        public event Action<ITreeNode<T>> OnNodeClick = delegate { };

        public BinaryTreeView(ITree<T> tree)
        {
            RegisterService<IMouseEvents>(new MouseHandler());
            RegisterService<ILayout>(this);
            RegisterService<IRenderer>(this);
            tree.Root.OnChange += Root_OnChange;
            Root_OnChange(null, tree.Root.get());

            Transform.Size = new Vector2(300, 300);
        }

        private void Root_OnChange(ITreeNode<T> old, ITreeNode<T> nnew)
        {
            var newNode = NodeAdded(old, nnew);
            if (nnew != null)
            {
                root = newNode;
            }
        }

        private void NodeAddedHelper(ITreeNode<T> old, ITreeNode<T> nnew)
        {
            NodeAdded(old, nnew);
        }

        private BinaryTreeNode<T> NodeAdded(ITreeNode<T> old, ITreeNode<T> nnew)
        {
            if (old != null)
            {
                nodeToComponentDict[old].OnNodeClick -= NewNode_OnNodeClick;
                Transform.RemoveChild(nodeToComponentDict[old]);
                nodeToComponentDict.Remove(old);
            }
            if (nnew != null)
            {
                var newNode = new BinaryTreeNode<T>(nnew);
                nodeToComponentDict[nnew] = newNode;

                nnew.Left.OnChange += NodeAddedHelper;
                nnew.Right.OnChange += NodeAddedHelper;

                newNode.OnNodeClick += NewNode_OnNodeClick;

                Transform.AddChild(newNode);
                return newNode;
            }
            return null;
        }

        private void NewNode_OnNodeClick(ITreeNode<T> node)
        {
            OnNodeClick(node);
        }

        public void Relayout(IGraphics g)
        {
            if (root == null)
                return;

            x = 0;
            Reposition(root, 0);
        }
        
        int x = 0;
        private void Reposition(BinaryTreeNode<T> node, int depth)
        {
            if (node.Node.Left.get() != null && nodeToComponentDict.ContainsKey(node.Node.Left.get()))
                Reposition(nodeToComponentDict[node.Node.Left.get()], depth + 1);
            
            node.Transform.Location = new Vector2(x * 80, depth * 100);

            x++;

            node.Transform.Size = new Vector2(80, 80);

            if (node.Node.Right.get() != null && nodeToComponentDict.ContainsKey(node.Node.Right.get()))
                Reposition(nodeToComponentDict[node.Node.Right.get()], depth + 1);
        }

        public void Render(IGraphics graphics)
        {
            if (root != null)
                DrawArrows(graphics, root);
        }

        private void DrawArrows(IGraphics graphics, BinaryTreeNode<T> node)
        {
            if (node.Node.Left.get() != null && nodeToComponentDict.ContainsKey(node.Node.Left.get()))
            {
                var leftNode = nodeToComponentDict[node.Node.Left.get()];
                graphics.DrawLine(new AnyPen(255, 0, 0, 0, 1), node.Transform.Location + new Vector2(node.Transform.Size.x / 2, node.Transform.Size.y), leftNode.Transform.Location + new Vector2(node.Transform.Size.x / 2, 0));
                DrawArrows(graphics, leftNode);
            }
            if (node.Node.Right.get() != null && nodeToComponentDict.ContainsKey(node.Node.Right.get()))
            {
                var rightNode = nodeToComponentDict[node.Node.Right.get()];
                graphics.DrawLine(new AnyPen(255, 0, 0, 0, 1), node.Transform.Location + new Vector2(node.Transform.Size.x / 2, node.Transform.Size.y), rightNode.Transform.Location + new Vector2(node.Transform.Size.x / 2, 0));
                DrawArrows(graphics, rightNode);
            }
        }
    }
}
