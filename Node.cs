using System;

namespace GraphsTheoryLib
{
    /// <summary>
    /// Represent the Nodes include in the graph.
    /// </summary>
    public class Node
    {
        private int nodeNumber;
        private bool visited;
        private int weight;
        private Node a;
      
        /// <summary>
        /// Get and set the number of the node in the graph.
        /// </summary>
        public int NodeNumber
        {
            get
            {
                return nodeNumber;
            }
            set
            {
                nodeNumber = value;
            }
        }
        /// <summary>
        /// Get and set the Visited Property of a node.
        /// </summary>
        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }
        /// <summary>
        /// Get and set the Weight Property of a node
        /// </summary>
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        /// <summary>
        /// Get and set the A (Previous Node) Property
        /// </summary>
        public Node A
        {
            get { return a; }
            set { a = value; }
        }
        /// <summary>
        /// Initialize a new instance of the Node class.
        /// </summary>
        private Node()
        { }

        /// <summary>
        /// Initialize a new instance of the Node class.
        /// </summary>
        /// <param name="numberOfNode">Represent the Number of the node in the graph.</param>
        public Node(int numberOfNode)
        {
            NodeNumber = numberOfNode;
        }

        /// <summary>
        /// Get string representation of the Node.
        /// </summary>
        /// <returns>Return the number of the node as string.</returns>
        public override string ToString()
        {
            return NodeNumber.ToString();
        }
    }
}