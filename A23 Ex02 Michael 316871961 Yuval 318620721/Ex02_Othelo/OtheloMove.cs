namespace Ex02_Othelo
{
    public class OtheloMove
    {
        public int row { get; set; }
        public int column { get; set; }
        public OtheloMove(int row, int col)
        {
            this.row = row;
            this.column = col;
        }
    }
}
