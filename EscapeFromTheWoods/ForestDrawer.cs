using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EscapeFromTheWoods
{
    public class ForestDrawer
    {
        string path = @"D:\Niels\School\Prog4\EscapeFromTheWoods\Jpegs";
        int drawingFactor = 12;

        public ForestDrawer()
        {
            colorMonkeyDictionary.Add(1, Color.Red);
            colorMonkeyDictionary.Add(2, Color.Blue);
            colorMonkeyDictionary.Add(3, Color.Yellow);
            colorMonkeyDictionary.Add(4, Color.Purple);
        }

        Dictionary<int, Color> colorMonkeyDictionary = new Dictionary<int, Color>();

        public void drawForest(Forest forest, Dictionary<Monkey, List<Tree>> monkeyTreeDictionary)
        {
            Console.WriteLine($"write bitmap routes wood : {forest.forestId} - start");
            Bitmap forestBitMap = new Bitmap((forest.xmax - forest.xmin) * drawingFactor, (forest.ymax - forest.ymin) * drawingFactor);

            Graphics g = Graphics.FromImage(forestBitMap);

            Pen p = new Pen(Color.Green, 1);
            foreach (Tree tree in forest.trees)
            {
                g.DrawEllipse(p, tree.x * drawingFactor - 5, tree.y * drawingFactor - 5, 10, 10);
            }

            foreach (Monkey monkey in monkeyTreeDictionary.Keys)
            {
                Brush brush = new SolidBrush(colorMonkeyDictionary[monkey.ID]);                
                g.FillEllipse(brush, monkeyTreeDictionary[monkey][0].x * drawingFactor - 5, monkeyTreeDictionary[monkey][0].y * drawingFactor - 5, 10, 10);

                Pen pen = new Pen(colorMonkeyDictionary[monkey.ID], 3);
                for (int i = 0; i < monkeyTreeDictionary[monkey].Count-1 ; i++)
                {
                    g.DrawLine(pen, monkeyTreeDictionary[monkey][i].x * drawingFactor, monkeyTreeDictionary[monkey][i].y * drawingFactor, monkeyTreeDictionary[monkey][i + 1].x * drawingFactor, monkeyTreeDictionary[monkey][i + 1].y * drawingFactor);
                }
            }

            forestBitMap.Save(Path.Combine(path, forest.forestId.ToString() + "_escapeRoutes.jpg"), ImageFormat.Jpeg);

            Console.WriteLine($"write bitmap routes wood : {forest.forestId} - end");
        }
    }
}
