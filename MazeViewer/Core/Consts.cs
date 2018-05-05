using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{
    public static class Consts
    {
        public static double ActualMazeCellWidth  = 180.0;

        public static double ActualMazeWallWidth  =  12.0;
        public static double ActualMazeWallHeight =  50.0;
        public static double ActualMazeWallLength = 168.0;

        public static double ActualMazePoleWidth  = ActualMazeWallWidth;
        public static double ActualMazePoleHeight = ActualMazeWallHeight;

        public static class Half
        {
            public static double ActualMazeCellWidth = Consts.ActualMazeCellWidth / 2;

            public static double ActualMazeWallWidth = Consts.ActualMazeWallWidth / 2;
            public static double ActualMazeWallHeight = Consts.ActualMazeWallHeight / 2;
            public static double ActualMazeWallLength = Consts.ActualMazeWallLength / 2;

            public static double ActualMazePoleWidth = ActualMazeWallWidth;
            public static double ActualMazePoleHeight = ActualMazeWallHeight;
        }
    }
}
