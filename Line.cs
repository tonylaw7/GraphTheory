using System;

namespace GraphsTheoryLib
{
    /// <summary>
    /// Represent the line that connect from one node to the other in the graph.
    /// </summary>
    public class Line
    {
        private string lineName;
        private Node firstNode;
        private Node secondNode;
        private int weight;
        private bool visited;       



        /// <summary>
        /// Get or set the name of the line.
        /// </summary>
        public string LineName
        {
            get
            {
                return lineName;
            }
            set
            {
                lineName = value;
            }
        }

        /// <summary>
        /// Get or set the first node that the line connect to.
        /// </summary>
        public Node FirstNode
        {
            get
            {
                return firstNode;
            }
            set
            {
                firstNode = value;
            }
        }

        /// <summary>
        /// Get or set the second node that the line connect to.
        /// </summary>
        public Node SecondNode
        {
            get
            {
                return secondNode;
            }
            set
            {
                secondNode = value;
            }
        }

        /// <summary>
        /// Get or Set the Weight Property in a Line
        /// </summary>
        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        /// <summary>
        /// Get or set the Visited Property in a Line
        /// </summary>
        public bool Visited
        {
            get { return visited; }
            set { visited = value; }
        }

        /// <summary>
        /// Initialize a new instance of the Line class.
        /// </summary>
        public Line()
        { }


        /// <summary>
        /// Initialzie a new instance of the line class.
        /// </summary>
        /// <param name="nameOfLine">Represent the name of the line.</param>
        /// <param name="first">Represent the first node that connnect to the line.</param>
        /// <param name="second">Represent the second node that connect to the line.</param>
        public Line(string nameOfLine, Node first, Node second)
        {
            LineName = nameOfLine;
            FirstNode = first;
            SecondNode = second;
        }

        /// <summary>
        /// Get the string representation of the line Object.
        /// </summary>
        /// <returns>Return the name of the line.</returns>
        public override string ToString()
        {
            return LineName;
        }
    }
}