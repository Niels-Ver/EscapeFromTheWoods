using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeFromTheWoods
{
    public static class IDgenerator
    {
        private static int nextTreeId = 0;
        public static int GetTreeID()
        {
            nextTreeId++;
            return nextTreeId;
        }
    }
}
