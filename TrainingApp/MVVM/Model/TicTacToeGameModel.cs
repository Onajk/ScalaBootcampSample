namespace TrainingApp.MVVM.Model
{
    // Model of Tic Tac Toe game
    class TicTacToeGameModel
    {
        public bool IsPlayerXTurn { get; set; } = true;

        public int FillCounter { get; set; } = 0;
        // Every field has uniq value of binary system. WinValues holds all the possible sums for victory.
        public uint[] WinValues { get; } = new uint[8] { 7, 73, 292, 448, 146, 56, 273, 84 };

        public uint[] PlayersPoints { get; set; } = new uint[2] { 0, 0 };
    }
}
