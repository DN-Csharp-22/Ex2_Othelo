namespace Ex02_Othelo
{
    public class OtheloMove
    {
        public int row { get; set; }
        public int col { get; set; }
        public OtheloMove(int row, int col)
        {
            this.row = row;
            this.col = col;
        }
    }
}
