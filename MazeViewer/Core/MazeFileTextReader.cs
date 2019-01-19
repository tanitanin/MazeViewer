using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeViewer.Core
{
    /// <summary>
    /// 迷路データのテキストファイルの中身を表したクラス
    /// </summary>
    /// <seealso cref="https://github.com/micromouseonline/mazefiles"/>
    class MazeFileTextReader : IDisposable
    {

        private TextReader textReader = null;

        #region Constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MazeFileTextReader(TextReader reader)
        {
            textReader = reader;
        }

        #endregion

        #region Properties

        /// <summary>
        /// スタート位置
        /// </summary>
        public Index2D Start { get; private set; }

        /// <summary>
        /// ゴール位置
        /// </summary>
        public IReadOnlyList<Index2D> Goals { get => this.goals; }
        private List<Index2D> goals = new List<Index2D>();

        /// <summary>
        /// セル
        /// </summary>
        public IReadOnlyList<Cell> Cells { get => this.cells; }
        private List<Cell> cells = new List<Cell>();

        private Dictionary<int, Cell> mapToCell = new Dictionary<int, Cell>();

        /// <summary>
        /// 迷路の横セル数
        /// </summary>
        public int NumOfHorizontalCells { get; private set; }

        /// <summary>
        /// 迷路の縦セル数
        /// </summary>
        public int NumOfVerticalCells { get; private set; }

        #endregion

        /// <summary>
        /// 迷路データを読み込む
        /// </summary>
        /// <returns></returns>
        public MazeData Read()
        {
            // 迷路の大きさを取得するためにテキストをすべて読み込む
            var text = new List<string>();
            while (this.textReader.Peek() >= 0)
            {
                text.Add(this.textReader.ReadLine());
            }

            // 迷路の大きさを計算する
            NumOfHorizontalCells = ((text.FirstOrDefault()?.Length - PoleExpressionLength) / (PoleExpressionLength + HorizontalWallExpressionLength)) ?? 0;
            NumOfVerticalCells = (text.Count - 1) / 2;
            for (var x = 0; x < NumOfHorizontalCells; ++x)
            {
                for (var y = 0; y < NumOfVerticalCells; ++y)
                {
                    var cell = new Cell() { Pos = new Index2D(x, y) };
                    this.cells.Add(cell);
                    this.mapToCell.Add(GetIndex(cell.Pos), cell);
                }
            }
            
            // データを読み込む
            this.currentIndex = new Index2D(0, NumOfVerticalCells);
            foreach (var line in text)
            {
                ReadLine(line);
            }

            // MazeDataに変換する
            var mazeData = new MazeData(NumOfHorizontalCells, NumOfVerticalCells);
            for (var x = 0; x < NumOfHorizontalCells; ++x)
            {
                for (var y = 0; y < NumOfVerticalCells; ++y)
                {
                    var cell = this.mapToCell[GetIndex(x,y)];
                    mazeData.At(x, y).East = cell.East;
                    mazeData.At(x, y).West = cell.West;
                    mazeData.At(x, y).North = cell.North;
                    mazeData.At(x, y).South = cell.South;
                    mazeData.At(x, y).IsStart = cell.IsStart;
                    mazeData.At(x, y).IsGoal = cell.IsGoal;
                }
            }
            return mazeData;
        }

        private Index2D currentIndex = new Index2D(-1, -1);

        #region Private Functions

        private int GetIndex(Index2D index)
        {
            return GetIndex(index.X, index.Y);
        }

        private int GetIndex(int x, int y)
        {
            return x * NumOfVerticalCells + y;
        }

        /// <summary>
        /// 1行分を読み込む
        /// </summary>
        /// <param name="line"></param>
        private void ReadLine(string line)
        {
            this.currentIndex.X = 0;
            if (line.StartsWith(PoleExpression))
            {
                foreach (var str in line.Split(new string[] { PoleExpression }, StringSplitOptions.RemoveEmptyEntries))
                {
                    ReadHorizontalWall(str);
                    this.currentIndex.X++;
                }
                this.currentIndex.Y--;
            }
            else
            {
                var index = 0;
                while (index < line.Length)
                {
                    ReadVerticalWall(line.Substring(index, VerticalWallExpressionLength));
                    index += VerticalWallExpressionLength;

                    if (index < line.Length)
                    {
                        ReadCell(line.Substring(index, CellExpressionLength));
                        index += CellExpressionLength;
                        this.currentIndex.X++;
                    }
                }
            }
        }

        /// <summary>
        /// セルを読み込む
        /// </summary>
        /// <param name="str"></param>
        private void ReadCell(string str)
        {
            if (str == StartCellExpression)
            {
                this.mapToCell[GetIndex(this.currentIndex)].IsStart = true;
                Start = this.currentIndex;
            }
            else if (str == GoalCellExpression)
            {
                this.mapToCell[GetIndex(this.currentIndex)].IsGoal = true;
                this.goals.Add(this.currentIndex);
            }
            else if (str == EmptyCellExpression)
            {
            }
        }

        /// <summary>
        /// 縦方向の壁を読み込む
        /// </summary>
        /// <param name="str"></param>
        private void ReadVerticalWall(string str)
        {
            if (str == VerticalWallExpression)
            {
                if (this.currentIndex.X > 0)
                {
                    this.mapToCell[GetIndex(this.currentIndex.X - 1, this.currentIndex.Y)].East = true;
                }
                if (this.currentIndex.X < NumOfHorizontalCells)
                {
                    this.mapToCell[GetIndex(this.currentIndex)].West = true;
                }
            }
            else if (str == VerticalWallEmpty)
            {
            }
        }

        /// <summary>
        /// 横方向の壁を読み込む
        /// </summary>
        /// <param name="str"></param>
        private void ReadHorizontalWall(string str)
        {
            if (str == HorizontalWallExpression)
            {
                if (this.currentIndex.Y > 0)
                {
                    this.mapToCell[GetIndex(this.currentIndex.X, this.currentIndex.Y - 1)].North = true;
                }
                if (this.currentIndex.Y < NumOfHorizontalCells)
                {
                    this.mapToCell[GetIndex(this.currentIndex)].South = true;
                }
            }
            else if (str == HorizontalWallEmpty)
            {
            }
        }

        #endregion

        #region Fields

        const int PoleExpressionLength          = 1;
        const string PoleExpression             = "o";

        const int VerticalWallExpressionLength  = 1;
        const string VerticalWallExpression     = "|";
        const string VerticalWallEmpty          = " ";

        const int HorizontalWallExpressionLength = 3;
        const string HorizontalWallExpression   = "---";
        const string HorizontalWallEmpty        = "   ";

        const int CellExpressionLength          = 3;
        const string StartCellExpression        = " S ";
        const string GoalCellExpression         = " G ";
        const string EmptyCellExpression        = "   ";

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    this.textReader.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                disposedValue = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~MazeFileTextReader() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
