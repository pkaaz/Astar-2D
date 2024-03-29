﻿using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Astar_Algorithm
{
    public class NodeInformation
    {
        public int X; //Nodes X location
        public int Y; //Nodes Y location
        public int G = 0; //Value from node to node
        public int H; //This is the value from node to X
        public int F; //Sum value of G and H
        public NodeInformation Parent = null; //This is used to later reconstruct the path easier
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //All of the code below was for testing on Console, The main methods of A* algorithm are later used in the GUI for calculation purposes

            /*
            public static string[] map = new string[]     {"+-----------------+",
                                                           "|A  X             |",
                                                           "| X X X           |",
                                                           "|XX   X     B     |",
                                                           "|   XXX           |",
                                                           "+-----------------+",
                                                          };
            */
            /*
            foreach (var a in map)
                Console.WriteLine(a);

            NodeInformation start = new NodeInformation { X = 1, Y = 1 };
            NodeInformation goal = new NodeInformation { X = 12, Y = 3 };
            SimplePriorityQueue<NodeInformation> queue = Astar(map, start, goal);
            if (queue == null)
            {
                Console.WriteLine("No path was found");
            }
            else
            {
                queue.Remove(queue.Last());
                queue.Remove(queue.First());
                //queue.Remove(queue.Last());
                while (queue.Count > 0)
                {
                    NodeInformation node = queue.Dequeue();
                    Console.WriteLine(node.X + " " + node.Y);
                    //Ugly way to insert a "."s into the map by path queue
                    char[] line = map[node.Y].ToCharArray();
                    line[node.X] = '.';
                    map[node.Y] = String.Join("", line);
                    //
                    foreach (var a in map)
                        Console.WriteLine(a);
                }
            }
            foreach (var a in map)
                Console.WriteLine(a);
            Console.ReadKey();
            */
        }
        /// <summary>
        /// The A* algorithm
        /// </summary>
        /// <param name="map">The map that needs to be analysed</param>
        /// <param name="start">The node of the starting position</param>
        /// <param name="goal">The node of the ending position</param>
        /// <param name="distanceCalculateValue">Value given by the GUI(0 or 1) for the distance method</param>
        /// <param name="neighbourValue">Value given by the GUI(0 or 1) for the neighbor count</param>
        /// <returns>Priority queue with nodes of A* result</returns>
        public static SimplePriorityQueue<NodeInformation> Astar(string[] map, NodeInformation start, NodeInformation goal, int distanceCalculateValue, int neighbourValue)
        {
            NodeInformation currentNode = null;

            SimplePriorityQueue<NodeInformation> openSet = new SimplePriorityQueue<NodeInformation>();

            openSet.Enqueue(start, 0);

            List<NodeInformation> closedList = new List<NodeInformation>();

            while (openSet.Count > 0)
            {
                currentNode = openSet.Dequeue();
                if (currentNode.X == goal.X && currentNode.Y == goal.Y)
                    return reconstructPath(currentNode);

                closedList.Add(currentNode);
                List<NodeInformation> neighbours = calculateNeighbours(currentNode, map, neighbourValue);
                foreach(var neighbour in neighbours)
                {
                    if (closedList.Exists(o => o.X == neighbour.X && o.Y == neighbour.Y))
                        continue;

                    neighbour.H = distanceCalculateValue == 0 ? calculateHManhattanValue(neighbour, goal) : calculateHEuclidianValue(neighbour, goal);
                    neighbour.G = currentNode.G + 1;
                    neighbour.F = neighbour.H + neighbour.G;
                    neighbour.Parent = currentNode;

                    //There should be an if statement to check if coming from this node is better than any other before
                    //Since it's a 2D array I don't think this really matters since you're most likely already going from the best position

                    //Writing an equals in NodeInformation class and using .Contains() might be more clear than this, but this works just fine
                    if (!openSet.Any(o => o.X == neighbour.X && o.Y == neighbour.Y))
                    {
                        openSet.Enqueue(neighbour, neighbour.F);//fScore[neighbour]);
                    }
                }
            }
            return null; //If all failed
        }
        /// <summary>
        /// Gives back all the neighbours of a given node from map
        /// </summary>
        /// <param name="current">current Node information</param>
        /// <param name="map">the map that is being analysed</param>
        /// <param name="value">value given by the GUI(0 or 1)</param>
        /// <returns>All the neighbours that can be visited from the given node</returns>
        static List<NodeInformation> calculateNeighbours(NodeInformation current, string[] map, int value)
        {
            //Value 0 for 4 neighbours and Value 1 for 8 neighbours

            List<NodeInformation> newList = new List<NodeInformation>();
            //Gather all possible neighbours(depending on selected directions)
            newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y });
            newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y });
            newList.Add(new NodeInformation { X = current.X, Y = current.Y + 1 });
            newList.Add(new NodeInformation { X = current.X, Y = current.Y - 1 });
            //Selected value is for 8 neighbours so add the 4 corner ones
            if (value == 1)
            {
                newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y + 1});
                newList.Add(new NodeInformation { X = current.X + 1, Y = current.Y - 1 });
                newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y + 1 });
                newList.Add(new NodeInformation { X = current.X - 1, Y = current.Y - 1 });
            }
            return newList.Where(o => map[o.Y][o.X] == ' ' || map[o.Y][o.X] == 'B').ToList();
        }
        /// <summary>
        /// Calculates the distance by using Manhattan method
        /// </summary>
        /// <param name="current">Current node information</param>
        /// <param name="goal">Goal node information</param>
        /// <returns>Distance between two nodes in Manhattan method</returns>
        static int calculateHManhattanValue(NodeInformation current, NodeInformation goal)
        {
            return Math.Abs(current.X - goal.X) + Math.Abs(current.Y - goal.Y);
        }
        /// <summary>
        /// Calculates the distance by using Euclidian method
        /// </summary>
        /// <param name="current">Current node information</param>
        /// <param name="goal">Goal node information</param>
        /// <returns>Distance between two nodes in Euclidian method</returns>
        static int calculateHEuclidianValue(NodeInformation current, NodeInformation goal)
        {
            return (int) Math.Sqrt(Math.Pow(current.X - goal.X, 2) + Math.Pow(current.Y - goal.Y, 2));
        }
        /// <summary>
        /// Gives a list with all the nodes that were in the shortest path according to the A* algorithm
        /// </summary>
        /// <param name="current">Current node information</param>
        /// <returns>List of NodeInformation that were used to get there</returns>
        static SimplePriorityQueue<NodeInformation> reconstructPath(NodeInformation current)
        {
            SimplePriorityQueue<NodeInformation> total_path = new SimplePriorityQueue<NodeInformation>();
            total_path.Enqueue(current, 0);
            while(current.Parent != null)
            {
                current = current.Parent;
                total_path.Enqueue(current, 0);
            }
            return total_path;
        }
    }
}
