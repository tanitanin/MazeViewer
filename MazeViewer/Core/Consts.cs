using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{
    public static class Consts
    {
        public static double ActualMazeCellWidth  = 18.0;

        public static double ActualMazeWallWidth  =  1.2;
        public static double ActualMazeWallHeight =  5.0;
        public static double ActualMazeWallLength = 16.8;

        public static double ActualMazePoleWidth  = ActualMazeWallWidth;
        public static double ActualMazePoleHeight = ActualMazeWallHeight;

        public static TimeSpan CompetetTime = TimeSpan.FromMinutes(7);
        public static int MaxGoalCount = 5;

        public static class Half
        {
            public static double ActualMazeCellWidth = Consts.ActualMazeCellWidth / 2;

            public static double ActualMazeWallWidth = Consts.ActualMazeWallWidth / 2;
            public static double ActualMazeWallHeight = Consts.ActualMazeWallHeight / 2;
            public static double ActualMazeWallLength = Consts.ActualMazeWallLength / 2;

            public static double ActualMazePoleWidth = ActualMazeWallWidth;
            public static double ActualMazePoleHeight = ActualMazeWallHeight;

            public static TimeSpan CompetetTime = TimeSpan.FromMinutes(15);
            public static int MaxGoalCount = 5;
        }
    }
}
