using System.Linq;

namespace ASD
{
    using ASD.Graphs;
    using System;
    using System.Collections.Generic;

    public class Lab06 : System.MarshalByRefObject
    {
        public List<int> WidePath(DiGraph<int> G, int start, int end)
        {
            int n = G.VertexCount;
            // Tablica przechowująca maksymalną możliwą "wąskość" do danego wierzchołka
            int[] maxBottleneck = new int[n];
            // Tablica do odtwarzania ścieżki
            int[] parent = new int[n];
            for (int i = 0; i < n; i++)
            {
                maxBottleneck[i] = -1; // -1 oznacza brak połączenia
                parent[i] = -1;
            }

            // Kolejka priorytetowa przechowująca (szerokość, wierzchołek)
            // Używamy ujemnych wartości jako priorytetów, aby PriorityQueue działała jak Max-Priority Queue
            var pq = new PriorityQueue<int, int>();

            maxBottleneck[start] = int.MaxValue;
            pq.Insert(start, 0); // W bibliotece ASD niższy priorytet wychodzi pierwszy (min-heap)

            while (pq.Count > 0)
            {
                int u = pq.Extract();

                if (u == end) break;

                foreach (var edge in G.OutEdges(u))
                {
                    int v = edge.To;
                    int weight = edge.Weight;

                    // Potencjalna nowa szerokość to minimum z szerokości do u i wagi krawędzi (u,v)
                    int potentialWidth = Math.Min(maxBottleneck[u], weight);

                    if (potentialWidth > maxBottleneck[v])
                    {
                        maxBottleneck[v] = potentialWidth;
                        parent[v] = u;
                        // Wstawiamy z priorytetem -potentialWidth, aby największa szerokość była na początku
                        pq.Insert(v, -potentialWidth);
                    }
                }
            }

            // Odtwarzanie ścieżki
            if (maxBottleneck[end] == -1)
                return new List<int>();

            List<int> path = new List<int>();
            for (int curr = end; curr != -1; curr = parent[curr])
            {
                path.Add(curr);
            }
            path.Reverse();

            return path;
        }
/// <summary>
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// </summary>
/// <param name="G"></param>
/// <param name="start"></param>
/// <param name="end"></param>
/// <param name="weights"></param>
/// <param name="maxWeight"></param>
/// <returns></returns>
        public List<int> WeightedWidePath(DiGraph<int> G, int start, int end, int[] weights, int maxWeight)
        {
            int n = G.VertexCount;
            // Pobieramy unikalne szerokości krawędzi i sortujemy malejąco
            var edgeWeights = new SortedSet<int>();
            for (int i = 0; i < n; i++)
                foreach (var e in G.OutEdges(i))
                    edgeWeights.Add(e.Weight);

            double bestScore = double.NegativeInfinity;
            List<int> bestPath = new List<int>();

            foreach (int minW in edgeWeights)
            {
                // Dijkstra na podgrafie krawędzi o wadze >= minW
                var result = DijkstraOnNodes(G, start, end, weights, minW);
                if (result.dist == int.MaxValue) continue;

                int currentScore = minW - result.dist;
                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestPath = result.path;
                }
            }

            return bestPath;
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="G"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="nodeWeights"></param>
        /// <param name="minEdgeWeight"></param>
        /// <returns></returns>
        private (int dist, List<int> path) DijkstraOnNodes(DiGraph<int> G, int start, int end, int[] nodeWeights, int minEdgeWeight)
        {
            int n = G.VertexCount;
            int[] dist = new int[n];
            int[] parent = new int[n];
            for (int i = 0; i < n; i++) { dist[i] = int.MaxValue; parent[i] = -1; }

            var pq = new PriorityQueue<int, int>();
            dist[start] = nodeWeights[start];
            pq.Insert(start, dist[start]);

            while (pq.Count > 0)
            {
                int u = pq.Extract();
                if (u == end) break;

                foreach (var e in G.OutEdges(u))
                {
                    if (e.Weight < minEdgeWeight) continue;
                    int v = e.To;
                    int newDist = dist[u] + nodeWeights[v];
                    if (newDist < dist[v])
                    {
                        dist[v] = newDist;
                        parent[v] = u;
                        pq.Insert(v, dist[v]);
                    }
                }
            }

            return (dist[end], GetPath(parent, end));
        }

        private List<int> GetPath(int[] parent, int end)
        {
            if (end == -1 || (parent[end] == -1 && !IsStart(parent, end))) return new List<int>();
            var path = new List<int>();
            for (int curr = end; curr != -1; curr = parent[curr]) path.Add(curr);
            path.Reverse();
            return path;
        }

        private bool IsStart(int[] parent, int end)
        {
            // Specyficzna obsługa przypadku, gdy start == end (jeśli graf na to pozwala)
            return false; // W tej implementacji parent[start] zawsze jest -1
        }
    }
}