namespace MineSweeper{
    public class Turn{

        public Guid gameId { get; }

        public int col { get; }

        public int row { get; }

        public Turn(Guid gameId, int col, int row){
            this.gameId = gameId;
            this.col = col;
            this.row = row;
        }
    }
}