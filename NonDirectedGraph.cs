using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphsTheoryLib
{
    class NonDirectedGraph:BaseGraph
    {
        private int[] nodesDegrees;


        /// <summary>
        /// Initializes a new instance of NonDirectedGraph Class.
        /// </summary>
        public NonDirectedGraph()
            : this(new SortedList<int, Node>(), new SortedList<string, Line>())
        { }

        /// <summary>
        /// Initializes a new instance of NonDirectedGraph Class with Nodes and Lines.
        /// </summary>
        /// <param name="nodes">List Contains the nodes of the NonDirectedGraph.</param>
        /// <param name="lines">List Contains the lines of the NonDirectedGraph.</param>
        public NonDirectedGraph(SortedList<int, Node> nodes, SortedList<string, Line> lines)
            : base(nodes, lines)
        {
            //SetTheConnectionAndStartArray();
        }

        /// <summary>
        /// Gets the NodesDegrees of the Graph.
        /// An array that contains values represent the degree of each nodes in the graph.
        /// </summary>
        public int[] NodesDegrees
        {
            get
            {
                return nodesDegrees;
            }
        }

        /// <summary>
        /// Set the values of the start, Connection and nodesDegrees
        /// according to the lines and nodes in the graph.
        /// </summary>
        protected override void SetTheConnectionAndStartArray()
        {
            if (NodesCount != 0 && LinesCount != 0)
            {
                // Initialize a start (alen6lak) array and Connection array and nodesDegrees array.
                Start = new int[NodesCount, LinesCount];
                Connection = new int[NodesCount, NodesCount];
                nodesDegrees = new int[NodesCount];

                // fill the start array when we used the SortedList<int, Node>.
                for (int lineCounter = 0; lineCounter < LinesCount; lineCounter++)
                {
                    Start[LinesList.Values[lineCounter].FirstNode.NodeNumber - 1, lineCounter]++;
                    Start[LinesList.Values[lineCounter].SecondNode.NodeNumber - 1, lineCounter]++;
                }

                // Fill the Connection array with the Data of the Nodes and Lines List using SortedList.
                foreach (Line line in LinesList.Values)
                {
                    if (line.FirstNode.NodeNumber == line.SecondNode.NodeNumber)
                    {
                        Connection[line.FirstNode.NodeNumber - 1, line.SecondNode.NodeNumber - 1]++;
                    }
                    else
                    {
                        Connection[line.FirstNode.NodeNumber - 1, line.SecondNode.NodeNumber - 1]++;
                        Connection[line.SecondNode.NodeNumber - 1, line.FirstNode.NodeNumber - 1]++;
                    }
                }

                // fill The nodesDegrees array with the Data of the Nodes and Lines List.
                for (int nodeCounter = 0; nodeCounter < NodesList.Values.Count; nodeCounter++)
                {
                    nodesDegrees[nodeCounter] = DegreeOfNode(NodesList.Values[nodeCounter]);
                }
            }
        }

        /// <summary>
        /// Retruns true if the Graph is simple, otherwise, false;
        /// </summary>
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
                            LinesList.Values[lineCounter].SecondNode.NodeNumber == LinesList.Values[nextLineCounter].SecondNode.NodeNumber) ||
                            (LinesList.Values[lineCounter].FirstNode.NodeNumber == LinesList.Values[nextLineCounter].SecondNode.NodeNumber &&
                            LinesList.Values[lineCounter].SecondNode.NodeNumber == LinesList.Values[nextLineCounter].FirstNode.NodeNumber))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Return ture if the Graph is perfect , otherwise, false.
        /// A Graph will be perfect if it Contains all the lines that possible between nodes and the graph is simple.
        /// </summary>
        public bool IsPerfect
        {
            get
            {
                int m = LinesCount;
                int n = NodesCount;

                //check if simple and in each two nodes there is a line
                if ((m == (n * (n - 1)) / 2) && (IsSimple == true))
                {
                    return true;
                }

                return false;
            }
        }


    }
}
