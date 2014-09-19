using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphsTheoryLib
{
    class DirectedGraph:BaseGraph
    {
        // Contains the count of the followers of each node of the graph.
        // Sorted by the index of the array.
        private int[] ps;
        private int[] ls;
        private int[] lp;   // Lines Preceder
        private int[] pp;   // Points Preceder


        /// <summary>
        /// 
        /// </summary>
        public DirectedGraph()
            : this(new SortedList<int, Node>(), new SortedList<string, Line>())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="lines"></param>
        public DirectedGraph(SortedList<int, Node> nodes, SortedList<string, Line> lines)
            : base(nodes, lines)
        {
            Ps = new int[NodesCount + 1];
            Ls = new int[LinesCount + 1];
            Pp = new int[NodesCount + 1];
            Lp = new int[LinesCount + 1];

            SetTheConnectionAndStartArray();
            SetTheFollowersArray();
            //SetThePrecederArray();
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Pp
        {
            get
            {
                return pp;
            }
            private set
            {
                pp = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Lp
        {
            get
            {
                return lp;
            }
            private set
            {
                lp = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Ps
        {
            get
            {
                return ps;
            }
            private set
            {
                ps = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int[] Ls
        {
            get
            {
                return ls;
            }
            private set
            {
                ls = value;
            }
        }

        protected override void SetTheConnectionAndStartArray()
        {
            // Set the connection array
            //for (int lineCounter = 0; lineCounter < LinesList.Values.Count; lineCounter++)
            //{
            //    Connection[LinesList.Values[lineCounter].FirstNode.NodeNumber - 1, LinesList.Values[lineCounter].SecondNode.NodeNumber - 1]++;
            //}
            foreach (Line line in LinesList.Values)
            {
                Connection[line.FirstNode.NodeNumber - 1, line.SecondNode.NodeNumber - 1]++;
            }
            // Set the Start Array
            for (int lineCounter = 0; lineCounter < LinesList.Values.Count; lineCounter++)
            {
                Start[LinesList.Values[lineCounter].FirstNode.NodeNumber - 1, lineCounter]++;
                Start[LinesList.Values[lineCounter].SecondNode.NodeNumber - 1, lineCounter]--;
            }
        }

        private void SetTheFollowersArray()
        {
            int lsCounter = 1;
            for (int psCounter = 1; psCounter < Ps.Length; psCounter++)
            {
                Ps[psCounter] = lsCounter;
                for (int lineCounter = 0; lineCounter < LinesList.Values.Count; lineCounter++)
                {
                    if (LinesList.Values[lineCounter].FirstNode.NodeNumber == psCounter)
                    {
                        
                        Ls[lsCounter] = LinesList.Values[lineCounter].SecondNode.NodeNumber;
                        lsCounter++;
                    }
                }
            }
        }

        //private void SetThePrecederArray()
        //{
        //    int subscript = 1;  // index of lp array
        //    Pp[1] = 1;

        //    for (int nodeCounter = 1; nodeCounter <= NodesCount; nodeCounter++)
        //    {
        //        List<int> indexesOfFound = FindElementInLs(nodeCounter + 1);

        //        Pp[nodeCounter] = subscript;

        //        if (indexesOfFound.Count != 0)
        //        {
        //            foreach (int foundElement in indexesOfFound)
        //            {
        //                int i = 1;
        //                for (i = 1; i < Ps.Length - 1; i++)
        //                {
        //                    if (foundElement >= Ps[i] && foundElement < Ps[i + 1])
        //                    {
        //                        Lp[subscript] = i;
        //                        subscript++;
        //                    }
        //                }
        //                if (i == Ps.Length - 1)
        //                {
        //                    //Lp[subscript] = Ps.Length - 1;
        //                }
        //            }
        //        }
        //    }
        //}

        private List<int> FindElementInLs(int nodeCounter)
        {
            List<int> indexesOfFound = new List<int>();

            for (int lsCounter = 1; lsCounter < ls.Length; lsCounter++)
            {
                if (ls[lsCounter] == nodeCounter)
                {
                    indexesOfFound.Add(lsCounter);
                }
            }

            return indexesOfFound;
        }

        //brings near of node case it's the start of the line
        public SortedList<int, Node> NearOfNodeDirected(Node node)
        {
            SortedList<int, Node> NearNodeList = new SortedList<int, Node>();

            foreach (Line line in this)
            {
                try
                {
                    if (line.FirstNode.NodeNumber == node.NodeNumber)
                    {
                        if (node.NodeNumber != line.SecondNode.NodeNumber)
                        {
                            NearNodeList.Add(line.SecondNode.NodeNumber, new Node(line.SecondNode.NodeNumber));
                        }
                    }

                }
                catch (ArgumentException)
                {
                    // the best handling here is to do nothing
                    // this exception will thrown by Add Method of SortedList Class if the key value that we
                    // trying to enter is duplicated (for more information see ObjectBrowser)
                }             
            }

            return NearNodeList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startNode"></param>
        /// <returns></returns>
        public List<int> BreadthFirstSearch(Node startNode)
        {
            return SearchTheGraph(startNode, WayOfSearch.BreadthFirst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startNode"></param>
        /// <returns></returns>
        public List<int> DepthFirstSearch(Node startNode)
        {
            return SearchTheGraph(startNode, WayOfSearch.DepthFirst);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startNode"></param>
        /// <param name="way"></param>
        /// <returns></returns>
        private List<int> SearchTheGraph(Node startNode, WayOfSearch way)
        {
            if (startNode == null)
            {
                throw new ArgumentNullException();
            }
            if (ContainsNode(startNode) == false)
            {
                throw new KeyNotFoundException();
            }

            int nodeNumber = startNode.NodeNumber;
            List<int> closed = new List<int>();
            List<int> open = new List<int>();
            open.Insert(0, nodeNumber);

            while (open.Count > 0)
            {
                int temp = open[0];
                open.RemoveAt(0);
                closed.Add(temp);
                SortedList<int, Node> neighbor = this.NearOfNodeDirected(new Node(temp));//verify

                foreach (int number in neighbor.Keys)
                {
                    if ((!closed.Contains(number)) && (!open.Contains(number)))
                    {
                        switch (way)
                        {
                            case WayOfSearch.BreadthFirst:
                                open.Add(number);
                                break;

                            case WayOfSearch.DepthFirst:                                
                                open.Insert(0, number); // add at the front of the open List
                                break;
                        }
                    }
                }
            }

            return closed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int InnerDegree(Node node)
        {
            return InnerDegree(node.NodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public int InnerDegree(int nodeKey)
        {
            int degree = 0;

            foreach (Line line in LinesList.Values)
            {
                if (line.SecondNode.NodeNumber == nodeKey)
                {
                    degree++;
                }
            }

            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public int OuterDegree(Node node)
        {
            return OuterDegree(node.NodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public int OuterDegree(int nodeKey)
        {
            int degree = 0;

            foreach (Line line in LinesList.Values)
            {
                if (line.FirstNode.NodeNumber == nodeKey)
                {
                    degree++;
                }
            }

            return degree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public SortedList<int, Node> InnerNodes(Node node)
        {
            return InnerNodes(node.NodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public SortedList<int, Node> InnerNodes(int nodeKey)
        {
            SortedList<int, Node> output = new SortedList<int, Node>();

            foreach (Line line in LinesList.Values)
            {
                // here we process the Circle line
                if (line.SecondNode.NodeNumber == nodeKey)
                {
                    output.Add(line.FirstNode.NodeNumber, new Node(line.FirstNode.NodeNumber));
                }
            }

            if (output.Count == 0)
            {
                return null;
            }

            return output;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public SortedList<int, Node> OuterNodes(Node node)
        {
            return OuterNodes(node.NodeNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeKey"></param>
        /// <returns></returns>
        public SortedList<int, Node> OuterNodes(int nodeKey)
        {
            SortedList<int, Node> output = new SortedList<int, Node>();

            foreach (Line line in LinesList.Values)
            {
                if (line.FirstNode.NodeNumber == nodeKey)
                {
                    output.Add(line.SecondNode.NodeNumber, new Node(line.SecondNode.NodeNumber));
                }
            }
            if (output.Count == 0)
            {
                return null;
            }
            else
            {
                return output;
            }
        }

        public bool IsStrongConnected
        {
            get
            {
                foreach (Node node in NodesList.Values)
                {
                    if (OuterNodes(node) == null)
                    {
                        return false;
                    }

                    if (SearchTheGraph(node, WayOfSearch.BreadthFirst) == NodesList.Values)
                    {
                        return true;
                    }
                }

                

                return false;
            }
        }

        public bool isWell(Node node)
        {
            if (OuterDegree(node) == 0)
            {
                return true;
            }
            return false;
        }

        public bool isSpring(Node node)
        {
            if (InnerDegree(node) == 0)
            {
                return true;
            }
            return false;
        }

        public bool DetectCycle()
        {
            
            while (true)
            {
                foreach (Node node in NodesList.Values)
                {
                    if (isSpring(node))
                    {
                        RemoveNode(node);
                    }
                }
                foreach (Node node in NodesList.Values)
                {
                    if (isWell(node))
                    {
                        RemoveNode(node);
                    }
                }                
            }
        }

        public override bool IsSimple
        {
            get
            {
                if (base.IsSimple == false)
                {
                    return false;
                }

                for (int lineCounter = 0; lineCounter < LinesCount; lineCounter++)
                {
                    for (int nextLineCounter = lineCounter + 1; nextLineCounter < LinesCount; nextLineCounter++)
                    {
                        if ((LinesList.Values[lineCounter].FirstNode.NodeNumber == LinesList.Values[nextLineCounter].FirstNode.NodeNumber &&
                            LinesList.Values[lineCounter].SecondNode.NodeNumber == LinesList.Values[nextLineCounter].SecondNode.NodeNumber))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        //validated
        public bool ContainCircularWay(Node firstNode, ref List<int> circlenodes)
        {
            Stack<int> stack = new Stack<int>();
            SortedList<int, bool> marked = new SortedList<int, bool>();

            for (int markedCounter = 0; markedCounter < NodesCount; markedCounter++)
            {
                marked.Add(NodesList.Keys[markedCounter], false);
            }

            // this statment is add in the last time modifiy the code
            marked[firstNode.NodeNumber] = true;

            stack.Push(firstNode.NodeNumber);
            while (stack.Count != 0)
            {
                int father = stack.Peek();
                SortedList<int, Node> successor = OuterNodes(father);
                bool successorFinish = false;
                int indexOfSuccessor = 0;

                if (successor == null)
                {
                    successorFinish = true;
                }
                while (!successorFinish)
                {
                    if (marked[successor.Values[indexOfSuccessor].NodeNumber])
                    {
                        if (stack.Contains(successor.Values[indexOfSuccessor].NodeNumber))
                        {
                            //return true;
                            int tempn = successor.Values[indexOfSuccessor].NodeNumber;
                            circlenodes = new List<int>();
                            circlenodes.Add(tempn);
                            while (stack.Peek() != tempn)
                            {
                                int stacknode;
                                stacknode = stack.Pop();
                                circlenodes.Add(stacknode);
                            }
                            circlenodes.Add(tempn);

                            return true;
                        }
                        else
                        {
                            int counter = 0;
                            for (counter = 0; counter < successor.Count; counter++)
                            {
                                if (!(marked[successor.Values[counter].NodeNumber]))
                                {
                                    break;
                                }
                            }

                            if (counter == successor.Count)
                            {
                                successorFinish = true;
                            }
                            else
                            {
                                indexOfSuccessor = counter;
                            }
                        }
                    }
                    else
                    {
                        marked[successor.Values[indexOfSuccessor].NodeNumber] = true;
                        stack.Push(successor.Values[indexOfSuccessor].NodeNumber);
                        break;
                    }
                }
                if (successorFinish)
                {
                    stack.Pop();
                }
            }

            return false;
        }
    }
}
