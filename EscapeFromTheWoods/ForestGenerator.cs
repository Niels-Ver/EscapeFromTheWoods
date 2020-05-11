using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromTheWoods
{
    public class ForestGenerator
    {
        ForestDrawer forestDrawer = new ForestDrawer();
        Dictionary<Forest, Dictionary<Monkey, Tree>> forestMonkeysStartTree = new Dictionary<Forest, Dictionary<Monkey, Tree>>();
        DataManagement dm = new DataManagement(@"Data Source=DESKTOP-FUU1BI0\SQLEXPRESS;Initial Catalog=Forest;Integrated Security=True");

        public async Task process(Forest forest)
        {
            generateForest(forest);
            await dm.AddForest(forest.forestId, forest.trees);
            Dictionary<Monkey, List<Tree>> monkeysTreeRoute = new Dictionary<Monkey, List<Tree>>();
            foreach (Monkey monkey in forest.monkeys)
            {
                List<Tree> treeRoute = calculateRoute(forest, monkey);
                monkeysTreeRoute.Add(monkey, treeRoute);
                await dm.AddMonkeyRecord(forest.forestId, monkey, treeRoute); 
            }


            forestDrawer.drawForest(forest, monkeysTreeRoute);
            await dm.AddLog(forest.forestId, monkeysTreeRoute);

            await generateForestLog(forest.forestId, monkeysTreeRoute);
        }

        public void generateForest(Forest forest)
        {
            Random r = new Random();
            
            int borderFromEdge = 1;
            int numberOfTrees = 500;

            do
            {

                Tree tree = new Tree(IDgenerator.GetTreeID(), r.Next(forest.xmin + borderFromEdge, forest.xmax - borderFromEdge), r.Next(forest.ymin + borderFromEdge, forest.ymax - borderFromEdge));

                if (!(forest.trees.Contains(tree)))
                {
                    forest.trees.Add(tree);
                }

            } while (forest.trees.Count < numberOfTrees);

            List<Tree> selectableTrees = forest.trees;

            foreach (Monkey monkey in forest.monkeys)
            {
                //Select a random tree from the treeList
                Tree selectedTree = selectableTrees[r.Next(0, selectableTrees.Count - 1)];
                //Remove tree from selectable trees so another monkey cant use that tree
                selectableTrees.Remove(selectedTree);

                

                if (forestMonkeysStartTree.ContainsKey(forest))
                    forestMonkeysStartTree[forest].Add(monkey, selectedTree);
                else
                {
                    Dictionary<Monkey, Tree> monkeyTreeDictionary = new Dictionary<Monkey, Tree>();
                    monkeyTreeDictionary.Add(monkey, selectedTree);
                    forestMonkeysStartTree.Add(forest, monkeyTreeDictionary);
                }               

                List<Tree> visitedTreeList = new List<Tree>();
                visitedTreeList.Add(selectedTree);
            }
        }

        public List<Tree> calculateRoute(Forest forest, Monkey monkey)
        {
            
            string startlog = $"start calculating escape route for wood: {forest.forestId}, monkey: {monkey.name}";
            Console.WriteLine(startlog);

            
            
            Tree closestTree = forest.trees[1];
            
            
            List<Tree> treeRoute = new List<Tree>();
            treeRoute.Add(forestMonkeysStartTree[forest][monkey]);
            
            bool hasLeftForest = false;
            do
            {
                Tree startTree = forestMonkeysStartTree[forest][monkey];
                double distanceBetweenClosestTrees = 10000;

                //De boom het dichtst bij de huidige boom ophalen
                foreach (Tree tree in forest.trees)
                {
                    if (!(treeRoute.Contains(tree)))
                    {
                        double distanceTrees = Math.Sqrt(Math.Pow(startTree.x - tree.x, 2) + Math.Pow(startTree.y - tree.y, 2));

                        if (distanceTrees < distanceBetweenClosestTrees)
                        {
                            closestTree = tree;
                            distanceBetweenClosestTrees = distanceTrees;
                        }
                    }
                }

                double distanceToBorder = (new List<double>() { forest.ymax - startTree.y, forest.xmax - startTree.x, startTree.y - forest.ymin, startTree.x - forest.xmin }).Min();

                if (distanceBetweenClosestTrees < distanceToBorder)
                {
                    
                    forestMonkeysStartTree[forest][monkey] = closestTree;
                    treeRoute.Add(closestTree);
                }
                else
                {
                    hasLeftForest = true;
                }

            } while (hasLeftForest==false);
            
            string endLog = $"end calculating escape route for wood: {forest.forestId}, monkey: {monkey.name}";
            Console.WriteLine(endLog);
            return treeRoute;
        }

        public async Task generateForestLog(int forestId ,Dictionary<Monkey, List<Tree>> monkeysTreeRoute)
        {
            Console.WriteLine($"wood : {forestId} writes log - start");
            SortedDictionary<Monkey, List<Tree>> sortedDictionary = new SortedDictionary<Monkey, List<Tree>>(monkeysTreeRoute);

            int max_length = 0;
            foreach (Monkey monkey in sortedDictionary.Keys)
            {
                if (sortedDictionary[monkey].Count() > max_length)
                    max_length = sortedDictionary[monkey].Count();
            }

            List<string> logList = new List<string>();
            for (int i = 0; i < max_length; i++)
            {
                foreach (Monkey monkey in sortedDictionary.Keys)
                {
                    if(sortedDictionary[monkey].ElementAtOrDefault(i) != null)
                    {
                        string message = $"{monkey.name} is in tree {sortedDictionary[monkey][i].treeID} at ({sortedDictionary[monkey][i].x}, {sortedDictionary[monkey][i].y})";
                        logList.Add(message);
                    }                    
                }                
            }

            using (StreamWriter sw = new StreamWriter(@$"D:\Niels\School\Prog4\EscapeFromTheWoods\Jpegs\{forestId.ToString()} + _log.txt"))
            {
                foreach (string message in logList)
                {
                    sw.WriteLine(message);
                }
            }
            Console.WriteLine($"wood : {forestId} writes log - end");
        }
    }
}
