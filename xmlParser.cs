using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace GraphsTheoryLib
{
    public static class xmlparser
    {
        #region xmlReader
        public static BaseGraph parser(string url)
        {
            BaseGraph graph = new NonDirectedGraph();

            XmlTextReader reader = new XmlTextReader(url);
            SortedList<int,Node> nodesList = new SortedList<int,Node>();
            SortedList<string,Line> linesList = new SortedList<string,Line>();
            Node graphNode;
            Line graphLine;
            int kindOfGraph =0;

            while (reader.Read())
            {
                #region Switch-Case XMLNodes
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        {
                            switch (reader.Name.ToLower())
                            {
                                #region NodeCase
                                //case "nodes":
                                case "node":
                                    {
                                        graphNode = new Node(Convert.ToInt32(reader.ReadString()));
                                        reader.ReadEndElement();
                                        nodesList.Add(graphNode.NodeNumber, graphNode);
                                    }
                                    break;
                                #endregion

                                #region LineCase
                                case "line":
                                    {

                                        graphLine = new Line();
                                        while (reader.Read())
                                        {
                                            if (reader.NodeType == XmlNodeType.Element)
                                            {
                                                if ((reader.Name.ToLower() == "name") || (reader.Name.ToLower() == "linename"))
                                                {
                                                    graphLine.LineName = reader.ReadString();
                                                    reader.ReadEndElement();
                                                }
                                                else if ((reader.Name.ToLower() == "firstnode") || (reader.Name.ToLower() == "source" || reader.Name.ToLower() == "from"))
                                                {
                                                    graphLine.FirstNode = new Node(Convert.ToInt32(reader.ReadString()));
                                                    reader.ReadEndElement();
                                                }
                                                else if (reader.Name.ToLower() == "weight")
                                                {
                                                    graphLine.Weight = Convert.ToInt32(reader.ReadString());
                                                    reader.ReadEndElement();
                                                }
                                                else if ((reader.Name.ToLower() == "secondnode") || (reader.Name.ToLower() == "destination" || reader.Name.ToLower() == "to"))
                                                {
                                                    graphLine.SecondNode = new Node(Convert.ToInt32(reader.ReadString()));
                                                    reader.ReadEndElement();
                                                }
                                            }
                                            else if (reader.Name == "line")
                                                break;
                                        }
                                        linesList.Add(graphLine.LineName, graphLine);

                                        //while (reader.Name.ToLower() != "name" && reader.Name.ToLower()!="linename" && reader.Read() == true) ;
                                        //if ((reader.Name.ToLower() == "name") || (reader.Name.ToLower() == "linename"))
                                        //{
                                        //    graphLine.LineName = reader.ReadString();
                                        //    reader.ReadEndElement();
                                        //}

                                        //while (reader.Name.ToLower() != "firstnode" && reader.Name.ToLower() != "source" && reader.Read() == true) ;
                                        //if ((reader.Name.ToLower().ToLower() == "firstnode") || (reader.Name.ToLower() =="source"))
                                        //{
                                        //    graphLine.FirstNode = new Node(Convert.ToInt32(reader.ReadString()));
                                        //    reader.ReadEndElement();
                                        //}
                                        
                                        
                                        //while (reader.Name.ToLower() != "weight" && reader.Read() == true) ;
                                        //if (reader.Name.ToLower() == "weight")
                                        //{
                                        //    graphLine.Weight = Convert.ToInt32(reader.ReadString());
                                        //    reader.ReadEndElement();
                                        //}

                                        //while (reader.Name.ToLower() != "secondnode" && reader.Name.ToLower() != "destination" && reader.Read() == true) ;
                                        //if ((reader.Name.ToLower() == "secondnode") || (reader.Name.ToLower() == "destination"))
                                        //{
                                        //    graphLine.SecondNode = new Node(Convert.ToInt32(reader.ReadString()));
                                        //    reader.ReadEndElement();
                                        //}




                                       //linesList.Add(graphLine.LineName, graphLine);
                                    }
                                    break;
                                #endregion

                                case "nondirectedgraph":
                                    kindOfGraph = 0;
                                    break;

                                case "graph":
                                case "directedgraph":
                                    kindOfGraph = 1;
                                    break;

                                default:
                                    break;
                            }// end inner switch (reader.Name.ToLower())
                        }// end inner case NodeType.Element
                        break;

                    default:
                        break;
                }//end outer switch(reader.NodeType)            
            }//end While(reader.Read()) 
                #endregion

            if (kindOfGraph == 0)
            {
                graph = new NonDirectedGraph(nodesList, linesList);
            }
            if (kindOfGraph == 1)
            {
                graph = new DirectedGraph(nodesList, linesList);
            }
            return graph;
        }//end method 
        #endregion

        public static void WriteToXMLFile(string url, BaseGraph graph)
        {
            StreamWriter filewriter = new StreamWriter(url);
            XmlTextWriter writer = new XmlTextWriter(filewriter);

            if (graph is NonDirectedGraph)
            {
                writer.WriteElementString("NonDirectedGraph", "");
            }
            else if(graph is DirectedGraph)
            {
                writer.WriteElementString("DirectedGraph", "");
            }

            writer.WriteElementString("Nodes", "");
            foreach (Node node in graph.NodesList.Values)
            {
                writer.WriteElementString("Node", node.NodeNumber.ToString());
                writer.WriteEndElement(); //</Node>
            }
            writer.WriteEndElement(); //</Nodes>

            writer.WriteName("Lines");
            foreach (Line line in graph.LinesList.Values)
            {
                writer.WriteElementString("Name", line.LineName);
                writer.WriteEndElement();

                writer.WriteElementString("FirstNode", line.FirstNode.NodeNumber.ToString());
                writer.WriteEndElement();

                writer.WriteElementString("SecondNode", line.SecondNode.NodeNumber.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();   // end </Lines>

            writer.WriteEndElement();   // end </NonDirectedGraph> or <DirectedGraph>
        }
    }
}
