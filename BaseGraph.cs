using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphsTheoryLib
{
    public abstract class BaseGraph : IEnumerable<Line>, IEnumerable<Node>//brilliant idea :)
    {
        private int[,] start;
        private int[,] connection;
        private SortedList<int, Node> nodesList;
        private SortedList<string, Line> linesList;

        public SortedList<int, Node> NodesList
        {
            get
            {
                return nodesList;
            }
            set
            {
                if (value != null)
                {
                    nodesList = value;
                }
            }
        }

        public SortedList<string, Line> LinesList
        {
            get
            {
                return linesList;
            }
            set
            {
                if (value != null)
                {
                    linesList = value;
                }
            }
        }

        protected abstract void SetTheConnectionAndStartArray();

        public int[,] Start
        {
            get
            {
                return start;
            }
            set
            {
                if (value != null)
                {
                    start = value;
                }
            }
        }

        public int[,] Connection
        {
            get
            {
                return connection;
            }
            protected internal set
            {
                if (value != null)
                {
                    connection = value;
                }
            }
        }

        public BaseGraph()
            : this(new SortedList<int, Node>(), new SortedList<string, Line>())
        {
        }

        public BaseGraph(SortedList<int, Node> listOfNodes, SortedList<string, Line> listOfLines)
        {
            if (listOfNodes != null)
            {
                NodesList = listOfNodes;
            }
            else
            {
                NodesList = new SortedList<int, Node>();
            }

            if (listOfLines != null)
            {
                LinesList = listOfLines;
            }
            else
            {
                LinesList = new SortedList<string, Line>();
            }


            Connection = new int[NodesCount, NodesCount];
            Start = new int[NodesCount, LinesCount];
            // SetTheConnectionAndStartArray();  dont ever make this statment to execute

        }

        /// <summary>
        /// Get the Count of Nodes in the graph.
        /// </summary>
        public int NodesCount
        {
            get
            {
                return nodesList.Count;
            }
        }

        /// <summary>
        /// Get the Count of Lines in the graph.
        /// </summary>
        public int LinesCount
        {
            get
            {
                return linesList.Count;
            }
        }
            

        /// <summary>
        /// Gets or set the Line associated with the specified lineName.
        /// </summary>
        /// <param name="lineName">The lineName whose value to get or set.</param>
        /// <returns>The line associated with the specified lineName. If the specified lineName is not found, a get and set operations throws a System.Collections.Generic.KeyNotFoundException.</returns>
        /// <exception cref=" ArgumentNullException">lineName or value is null</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and lineName does not exist in the collection.</exception>
        public Line this[string lineName]
        {
            get
            {
                ValidateArgumentIsNotNull(lineName);

                return LinesList[lineName];
            }
            set
            {
                ValidateArgumentIsNotNull(value);
                ValidateArgumentIsNotNull(lineName);

                LinesList[lineName] = value;
            }
        }

        /// <summary>
        /// Gets or set the Node associated with the specified nodeNumber.
        /// </summary>
        /// <param name="nodeNumber">the nodeNumber whose value to get or set.</param>
        /// <returns>The node associated with the specified nodeNumber. If the specified nodeNumber is not found, a get and set operations throws aSystem.Collections.Generic.KeyNotFoundException.</returns>
        /// <exception cref=" ArgumentNullException">value is null</exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and nodeNumber does not exist in the collection.</exception>
        public Node this[int nodeNumber]
        {
            get
            {
                return NodesList[nodeNumber];
            }
            set
            {
                ValidateArgumentIsNotNull(value);

                NodesList[nodeNumber] = value;
            }
        }

        public int DegreeOfNode(Node node)
        {
            int deg = 0;

            // call GetEnumerator Method in the same class.
            foreach (Line line in this)
            {
                if (line.FirstNode.NodeNumber == node.NodeNumber)
                {
                    deg++;
                }
                if (line.SecondNode.NodeNumber == node.NodeNumber)
                {
                    deg++;
                }
            }

            return deg;
        }

        public int MaxDegree()
        {
            int max = 0;
            int sum = 0;
            for (int i = 0; i < start.GetLength(0); i++)
            {
                for (int j = 0; j < start.GetLength(0); j++)
                {
                    sum += start[i, j];
                }
                if (sum > max)
                    max = sum;
                sum = 0;
            }
            return max;
        }
        public int MinDegree()
        {
            int Min = 0;
            int sum = 0;
            for (int i = 0; i < start.GetLength(0); i++)
            {
                for (int j = 0; j < start.GetLength(0); j++)
                {
                    sum += start[i, j];
                }
                if (sum < Min)
                    Min = sum;
                sum = 0;
            }
            return Min;
        }



        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsSimple
        {
            get
            {
                for (int lineCounter = 0; lineCounter < LinesCount; lineCounter++)
                {
                    if (LinesList.Values[lineCounter].FirstNode.NodeNumber == LinesList.Values[lineCounter].SecondNode.NodeNumber)
                    {
                        return false;
                    }
                }

                return true;
            }
        }
        public bool isPerfect()
        {
            bool is_simple = IsSimple;
            if (!is_simple)
                return false;
            int max = MaxDegree();
            int min = MinDegree();
            if ((MaxDegree() != MinDegree()) || (MinDegree() != NodesList.Count - 1))
                return false;
            return true;
        }

        /// <summary>
        /// Returns ture if the graph is Regular (all the Degree of the nodes are equale to each other) , otherwise , false.
        /// </summary>
        public bool IsRegularGraph
        {
            get
            {
                #region Algorithim using the nodesDegrees array

                //for (int nodeCounter = 0; nodeCounter < nodesDegrees.Length - 1; nodeCounter++)
                //{
                //    if (nodesDegrees[nodeCounter] != nodesDegrees[nodeCounter + 1])
                //    {
                //        return false;
                //    }
                //}

                //return true;

                #endregion

                // using the DegreeOfNode Method
                for (int nodeCounter = 0; nodeCounter < NodesCount - 1; nodeCounter++)
                {
                    if (DegreeOfNode(NodesList.Values[nodeCounter]) != DegreeOfNode(NodesList.Values[nodeCounter + 1]))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Determine the Near Nodes of the specified node.
        /// </summary>
        /// <param name="node">Node that we want find the Near nodes for it.</param>
        /// <returns>List of nodes contains the near Nodes of the node that we passed as a parameter.</returns>
        public SortedList<int, Node> NearOfNode(Node node)
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
                    if (line.SecondNode.NodeNumber == node.NodeNumber)
                    {
                        if (node.NodeNumber != line.FirstNode.NodeNumber)
                        {
                            NearNodeList.Add(line.FirstNode.NodeNumber, new Node(line.FirstNode.NodeNumber));
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
        /// Determine if the graph specified with list of nodes and list of nodes is partial
        /// Graph of the Graph that called the method on.
        /// </summary>
        /// <param name="nodes">List of nodes that specify the nodes of the graph that we want to exam.</param>
        /// <param name="lines">List of lines that specify the lines of the graph that we want to exam.</param>
        /// <returns>Return true if the graph is Partial , Otherwise , false.</returns>
        public bool IsPartialGraph(SortedList<int, Node> nodes, SortedList<string, Line> lines)
        {
            // Check if all nodes in parameter in the Primary graph.
            foreach (Node node in nodes.Values)
            {
                if (NodesList.ContainsValue(node) == false) // node not found in NodesList
                {
                    return false;
                }
            }

            // Check if all lines in paramter in the Primary graph.
            foreach (Line line in lines.Values)
            {
                if (LinesList.ContainsValue(line) == false) // line not found in lineslist
                {
                    return false;
                }
            }
            foreach (Line line in LinesList.Values)
            {
                if ((nodes.ContainsValue(line.FirstNode) && nodes.ContainsValue(line.SecondNode)) && (lines.ContainsValue(line) == false))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determine if the graph specified with list of nodes and list of lines is partial Graph
        /// of the Graph that called the method on.
        /// </summary>
        /// <param name="tempGraph">Specify the graph that is Partial of the Primary Graph (which we call the method on) or not.</param>
        /// <returns>Return true if the graph is partial graph of the primary graph , otherwise ,false.</returns>
        //public bool IsPartialGraph(NonDirectedGraph tempGraph)
        //{
        //    return IsPartialGraph(tempGraph.NodesList, tempGraph.LinesList);
        //}



        private void ValidateArgumentIsNotNull(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Determines whether the Graph contains a specific Line.
        /// </summary>
        /// <param name="line">The line to locate in the Graph. The value can not be null.</param>
        /// <returns>true if the Graph contains an line with the specified value; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">line parameter is Null.</exception>
        public bool ContainsLine(Line line)
        {
            ValidateArgumentIsNotNull(line);
            return ContainsLine(line.LineName);
        }

        /// <summary>
        /// Determines whether the Graph contains a specific line.
        /// </summary>
        /// <param name="lineName">The name of line to locate in the Graph. The value can not be null.</param>
        /// <returns>true if the Graph contains line with the specified name; otherwise, false.</returns>
        /// <exception cref=" ArgumentNullException">name of line is null.</exception>
        public bool ContainsLine(string lineName)
        {
            ValidateArgumentIsNotNull(lineName);
            return LinesList.ContainsKey(lineName);
        }

        /// <summary>
        /// Determines whether the Graph contains a specific Node.
        /// </summary>
        /// <param name="node">The node to locate in the Graph. The value can not be null.</param>
        /// <returns>true if the Graph contains node with the specified value; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">node parameter is Null.</exception>
        public bool ContainsNode(Node node)
        {
            ValidateArgumentIsNotNull(node);
            return ContainsNode(node.NodeNumber);
        }

        /// <summary>
        /// Determines whether the Graph contains a specific node.
        /// </summary>
        /// <param name="nodeNumber">The number of node to locate in the graph . The value can not be null.</param>
        /// <returns>true if the Graph contains line with specified number; otherwise, false.</returns>
        public bool ContainsNode(int nodeNumber)
        {
            return NodesList.ContainsKey(nodeNumber);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Graph.
        /// </summary>
        /// <returns>An System.Collections.Generic.IEnumerator<T> of type System.Collections.Generic.KeyValuePair<TKey,TValue> for the System.Collections.Generic.SortedList<TKey,TValue>.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Line of the Graph.
        /// </summary>
        /// <returns>An System.Collections.Generic.IEnumerator<T> of type System.Collections.Generic.KeyValuePair<TKey,TValue> for the System.Collections.Generic.SortedList<TKey,TValue>.</returns>
        public IEnumerator<Line> GetEnumerator()
        {
            return LinesList.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the Node of the Graph.
        /// </summary>
        /// <returns>An System.Collections.Generic.IEnumerator<T> of type System.Collections.Generic.KeyValuePair<TKey,TValue> for the System.Collections.Generic.SortedList<TKey,TValue>.</returns>
        IEnumerator<Node> IEnumerable<Node>.GetEnumerator()
        {
            return NodesList.Values.GetEnumerator();
        }


        /// <summary>
        /// Removes the line with the specified reference from the Graph.
        /// </summary>
        /// <param name="line"> The reference of the line to remove.</param>
        /// <exception cref="ArgumentNullException">line is Null.</exception>
        /// <exception cref="KeyNotFoundException">line to remove is not in the Graph.</exception>
        public void RemoveLine(Line line)
        {
            ValidateArgumentIsNotNull(line);
            RemoveLine(line.LineName);
        }

        /// <summary>
        /// Removes the line with the specified name from the Graph.
        /// </summary>
        /// <param name="lineName">The name of the line to remove.</param>
        /// <exception cref="ArgumentNullException">lineName is Null.</exception>
        /// <exception cref="KeyNotFoundException">line to remove is not in the Graph.</exception>
        public void RemoveLine(string lineName)
        {
            ValidateArgumentIsNotNull(lineName);
            LinesList.Remove(lineName);
            SetTheConnectionAndStartArray();
        }
        // <summary>
        /// Removes the node with the specified reference from the Graph.
        /// </summary>
        /// <param name="node">The number of node to remove</param>
        /// <exception cref="ArgumentNullException">node is Null.</exception>
        /// <exception cref="KeyNotFoundException">node to remove is not in the Graph.</exception>
        public void RemoveNode(Node node)
        {
            ValidateArgumentIsNotNull(node);
            RemoveNode(node.NodeNumber);
        }

        /// <summary>
        /// Removes the node with the specified number from the Graph.
        /// </summary>
        /// <param name="nodeNumber">The number of node to remove.</param>
        /// <exception cref="KeyNotFoundException">node to remove is not in the Graph.</exception>
        public void RemoveNode(int nodeNumber)  // Validated
        {
            

            List<Line> Lines = new List<Line>();
            foreach (Line line in LinesList.Values)
            {
                if ((line.FirstNode.NodeNumber == nodeNumber) || (line.SecondNode.NodeNumber == nodeNumber))
                {
                    //RemoveLine(line.LineName);
                    //line.Deleted = true;
                    Lines.Add(line);
                }
            }
            for (int i = 0; i < Lines.Count; i++)
            {
                LinesList.Remove(Lines[i].LineName);
            }


            NodesList.Remove(nodeNumber);
            //SetTheConnectionAndStartArray();
        }

        public void AddNode(int nodenumber)
        {
            bool found = false;

            foreach (Node node in NodesList.Values)
            {
                if (node.NodeNumber == nodenumber)
                {
                    found = true;
                    break;
                }   
            }
            if (!found)
            {
                Node node = new Node(nodenumber);
                NodesList.Add(nodenumber,node);
            }
            //SetTheConnectionAndStartArray();
        }

        public void AddNode(Node node)
        {
            ValidateArgumentIsNotNull(node);
            NodesList.Add(node.NodeNumber,node);

            //SetTheConnectionAndStartArray();
        }

        public void AddLine(Node startNode, Node endNode)
        {            
            int linenumber = LinesList.Count + 1;
            string linename = ("e" + linenumber.ToString());
            Line newLine = new Line(linename,startNode,endNode);
            LinesList.Add(linename,newLine);

            //SetTheConnectionAndStartArray();
        }

        public Node CallNode(int nodenumber)
        {
            foreach (Node node in NodesList.Values)
            {
                if (node.NodeNumber == nodenumber)
                {
                    return node;
                }
            }
            return null;
        }

        public Line CallLine(int linenumber)
        {
            int i = 0;
            foreach (Line line in LinesList.Values)
            {
                if (i == linenumber)
                {
                    return line;
                }
                i++;                
            }
            return null;
        }

    }
}
