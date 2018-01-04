using System;
using System.Collections.Generic;
using System.Text;

namespace MXP.Util
{

    /// <summary>
    /// 3D Interval KD Tree implementation.
    /// Author: Tommi S. E. Laukkanen
    /// Project: http://www.bubblecloud.org
    /// License: Apache 2.0 License
    /// </summary>
    public class IntervalKDTree<T>
    {
        private Node rootNode;
        private int divisionThreshold;
        private double range;

        private IDictionary<Box,Node> boxNodeDictionary=new Dictionary<Box, Node>();
        private IDictionary<T,Box> valueBoxDictionary=new Dictionary<T, Box>();

        public IntervalKDTree(double range,int divisionThreshold)
        {
            this.divisionThreshold = divisionThreshold;
            this.range = range;
            rootNode = new Node(this,null, 0, -range, -range, -range, range, range, range);
        }

        #region Public Interface Methods

        public void Clear()
        {
            boxNodeDictionary.Clear();
            valueBoxDictionary.Clear();
            rootNode = new Node(this, null, 0, -range, -range, -range, range, range, range);
        }

        public void Put(double minX, double minY, double minZ, double maxX, double maxY, double maxZ, T value)
        {
            if(!valueBoxDictionary.ContainsKey(value))
            {
                AddBox(new Box(value, minX, minY, minZ, maxX, maxY, maxZ));                
            }
            else
            {
                Box box = valueBoxDictionary[value];
                box.minX = minX;
                box.minY = minY;
                box.minZ = minZ;
                box.maxX = maxX;
                box.maxY = maxY;
                box.maxZ = maxZ;
                UpdateBox(box);
            }
        }

        public HashSet<T> GetValues(double minX, double minY, double minZ, double maxX, double maxY, double maxZ, HashSet<T> values)
        {
            rootNode.GetValues(new Cube(minX,minY,minZ,maxX,maxY,maxZ),values);
            return values;
        }

        public IList<T> GetValues(double minX, double minY, double minZ, double maxX, double maxY, double maxZ, IList<T> values)
        {
            rootNode.GetValues(new Cube(minX, minY, minZ, maxX, maxY, maxZ), values);
            return values;
        }

        public void Remove(T value)
        {
            if (valueBoxDictionary.ContainsKey(value))
            {
                Box box = valueBoxDictionary[value];
                RemoveBox(box);
                boxNodeDictionary.Remove(box);
                valueBoxDictionary.Remove(value);
            }
        }

        #endregion

        #region Box Management Methods

        private void AddBox(Box box)
        {
            rootNode.AddBox(box);
            valueBoxDictionary.Add(box.value, box);
        }

        private void UpdateBox(Box box)
        {
            Node currentNode = boxNodeDictionary[box];
            
            if(
                (currentNode.HasChildren&&
                (box.IsBelow(currentNode.Depth,currentNode.DivisionBoundary)
                 ||box.IsAbove(currentNode.Depth,currentNode.DivisionBoundary))
                )
                ||!currentNode.Contains(box)
              )
            {
                // If box location has changed so that it needs to placed to different node then remove / add.
                currentNode.RemoveBox(box);
                rootNode.AddBox(box);
            }
        }

        private void RemoveBox(Box box)
        {
            boxNodeDictionary[box].RemoveBox(box);
            valueBoxDictionary.Remove(box.value);
        }

        #endregion

        /// <summary>
        /// Node is extend axis aligned 3d cube which is the node implementation of IntervalKDTree.
        /// </summary>
        private class Node : Cube
        {
            public int Depth;
            public double DivisionBoundary;

            public bool HasChildren = false;

            public Node Parent;
            public Node LowChild;
            public Node HighChild;

            private IntervalKDTree<T> tree;
            private List<Box> boxes;

            public Node(IntervalKDTree<T> tree, Node parent, int depth, double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
                : base(minX, minY, minZ, maxX, maxY, maxZ)
            {
                this.tree = tree;
                this.Parent = parent;
                this.Depth = depth;
                boxes = new List<Box>();
            }

            public void AddBox(Box box)
            {
                if (HasChildren)
                {
                    if (box.IsBelow(Depth,DivisionBoundary))
                    {
                        LowChild.AddBox(box);
                        return;
                    }
                    if(box.IsAbove(Depth,DivisionBoundary))
                    {
                        HighChild.AddBox(box);
                        return;
                    }
                }

                boxes.Add(box);

                tree.boxNodeDictionary[box]=this;

                // Divide to children if threshold has been exceeded
                if (!HasChildren&&boxes.Count > tree.divisionThreshold)
                {
                    Divide();
                }

                return;
            }

            public void RemoveBox(Box box)
            {
                boxes.Remove(box);

                // Collapse parent node if total amount of values in parent and childen is less than maximum values per node.
                if(Parent!=null&&!Parent.LowChild.HasChildren&&!Parent.HighChild.HasChildren&&
                    Parent.boxes.Count+Parent.LowChild.boxes.Count+Parent.HighChild.boxes.Count<tree.divisionThreshold)
                {
                    Parent.Collapse();
                }
            }

            public void GetValues(Cube cube, HashSet<T> values)
            {
                foreach(Box box in boxes)
                {
                    if(cube.Intersects(box))
                    {
                        values.Add((box.value));
                    }
                }

                if(HasChildren)
                {
                    if(cube.IsBelow(Depth,DivisionBoundary))
                    {
                        LowChild.GetValues(cube,values);
                    } 
                    else if (cube.IsAbove(Depth, DivisionBoundary))
                    {
                        HighChild.GetValues(cube, values);
                    }
                    else
                    {
                        LowChild.GetValues(cube, values);
                        HighChild.GetValues(cube, values);
                    }
                }
            }

            public void GetValues(Cube cube, IList<T> values)
            {
                foreach (Box box in boxes)
                {
                    if (cube.Intersects(box))
                    {
                        values.Add((box.value));
                    }
                }

                if (HasChildren)
                {
                    if (cube.IsBelow(Depth, DivisionBoundary))
                    {
                        LowChild.GetValues(cube, values);
                    }
                    else if (cube.IsAbove(Depth, DivisionBoundary))
                    {
                        HighChild.GetValues(cube, values);
                    }
                    else
                    {
                        LowChild.GetValues(cube, values);
                        HighChild.GetValues(cube, values);
                    }
                }
            }

            private void Divide()
            {
                if(HasChildren==true)
                {
                    throw new Exception("Already has children.");
                }

                HasChildren = true;

                int dimension = Depth % 3;
                if (dimension == 0)
                {
                    DivisionBoundary = (maxX + minX) / 2;
                    LowChild = new Node(tree, this, Depth+1, minX, minY, minZ, DivisionBoundary, maxY, maxZ);
                    HighChild = new Node(tree, this, Depth + 1, DivisionBoundary, minY, minZ, maxX, maxY, maxZ);
                }
                else if (dimension == 1)
                {
                    DivisionBoundary = (maxY + minY) / 2;
                    LowChild = new Node(tree, this, Depth + 1, minX, minY, minZ, maxX, DivisionBoundary, maxZ);
                    HighChild = new Node(tree, this, Depth + 1, minX, DivisionBoundary, minZ, maxX, maxY, maxZ);
                }
                else
                {
                    DivisionBoundary = (maxZ + minZ) / 2;
                    LowChild = new Node(tree, this, Depth + 1, minX, minY, minZ, maxX, maxY, DivisionBoundary);
                    HighChild = new Node(tree, this, Depth + 1, minX, minY, DivisionBoundary, maxX, maxY, maxZ);
                }

                IList<Box> oldBoxList = boxes;
                boxes= new List<Box>();    
                foreach(Box cube in oldBoxList)
                {
                    AddBox(cube);
                }

            }

            private void Collapse()
            {
                foreach (Box box in LowChild.boxes)
                {
                    boxes.Add(box);
                    tree.boxNodeDictionary[box] = this;
                }
                foreach (Box box in HighChild.boxes)
                {
                    boxes.Add(box);
                    tree.boxNodeDictionary[box] = this;
                }
                LowChild = null;
                HighChild = null;
                HasChildren = false;

                if (Parent != null && !Parent.LowChild.HasChildren && !Parent.HighChild.HasChildren &&
                    Parent.boxes.Count + Parent.LowChild.boxes.Count + Parent.HighChild.boxes.Count < tree.divisionThreshold)
                {
                    Parent.Collapse();
                }
            }

        }

        /// <summary>
        /// Box is axis aligned 3d cube which can hold value. Acts as capsule in IntervalKDTree data structure.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class Box : Cube
        {
            public T value;

            public Box(T value, double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
                : base(minX, minY, minZ, maxX, maxY, maxZ)
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Axis aligned 3d cube implementation with math functions.
        /// </summary>
        private class Cube
        {
            
            public double minX;
            public double minY;
            public double minZ;
            public double maxX;
            public double maxY;
            public double maxZ;

            public Cube(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
            {
                this.minX = minX;
                this.minY = minY;
                this.minZ = minZ;
                this.maxX = maxX;
                this.maxY = maxY;
                this.maxZ = maxZ;
            }

            public bool IsBelow(int depth,double boundary)
            {
                int dimension = depth%3;
                if(dimension==0)
                {
                    return maxX < boundary;
                }
                else if (dimension == 1)
                {
                    return maxY < boundary;
                }
                else
                {
                    return maxZ < boundary;
                }
            }

            public bool IsAbove(int depth, double boundary)
            {
                int dimension = depth % 3;
                if (dimension == 0)
                {
                    return boundary <= minX;
                }
                else if (dimension == 1)
                {
                    return boundary <= minY;
                }
                else
                {
                    return boundary <= minZ;
                }                
            }

            public bool Contains(Cube cube)
            {
                return minX <= cube.minX && cube.maxX < maxX &&
                    minY <= cube.minY && cube.maxY < maxY &&
                    minZ <= cube.minZ && cube.maxZ < maxZ;
            }

            public bool Intersects(Cube cube)
            {
                return Contains(cube.minX, cube.minY, cube.minZ) 
                    || Contains(cube.maxX, cube.minY, cube.minZ) 
                    || Contains(cube.minX, cube.maxY, cube.minZ) 
                    || Contains(cube.minX, cube.minY, cube.maxZ) 
                    || Contains(cube.minX, cube.maxY, cube.maxZ) 
                    || Contains(cube.maxX, cube.minY, cube.maxZ) 
                    || Contains(cube.maxX, cube.maxY, cube.minZ) 
                    || Contains(cube.maxX, cube.maxY, cube.maxZ)                     
                    || cube.Contains(minX, minY, minZ) 
                    || cube.Contains(maxX, minY, minZ) 
                    || cube.Contains(minX, maxY, minZ) 
                    || cube.Contains(minX, minY, maxZ) 
                    || cube.Contains(minX, maxY, maxZ)
                    || cube.Contains(maxX, minY, maxZ)
                    || cube.Contains(maxX, maxY, minZ)
                    || cube.Contains(maxX, maxY, maxZ);
            }

            private bool Contains(double cx, double cy, double cz)
            {
                return minX <= cx && cx < maxX &&
                    minY <= cy && cy < maxY &&
                    minZ <= cz && cz < maxZ;
            }

        }
    }

}
