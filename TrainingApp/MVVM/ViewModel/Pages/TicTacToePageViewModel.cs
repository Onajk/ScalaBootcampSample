using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using TrainingApp.Core;
using TrainingApp.MVVM.Model;

namespace TrainingApp.MVVM.ViewModel
{
    // View model of Tic Tac Toe page
    class TicTacToePageViewModel : ObservableObject
    {
        enum GameModes
        {
            TwoPlayers,
            RandomGuy,
            Expert
        }
        enum Player
        {
            X,
            O
        }
        private string _ticTacToeGameComments = "Player X turn.";
        private int _startingPlayer = (int)Player.X;
        private int _gameMode = (int)GameModes.TwoPlayers;

        // list of all game fields displayed on the board
        public ObservableCollection<TicTacToeFieldViewModel> TicTacToeFieldList { get; set; } = new ObservableCollection<TicTacToeFieldViewModel>();

        public TicTacToeGameModel TicTacToeGame { get; set; }

        public RelayCommand RestartGameCommand { get; set; }

        public int GameMode
        {
            get { return _gameMode; }

            set
            {
                _gameMode = value;
                NewGame();
            }
        }

        public int StartingPlayer
        {
            get { return _startingPlayer; }
            
            set
            {
                _startingPlayer = value;
                NewGame();
            }
        }

        public string TicTacToeGameComments
        {
            get { return _ticTacToeGameComments; }
            set
            {
                _ticTacToeGameComments = value;
                OnPropertyChanged();
            }
        }

        // Game field adding method. Used to create game board on page load.
        private void AddNewTicTacToeField(int gridRow, int gridColumn)
        {
            var newField = new TicTacToeFieldViewModel
            {
                Content = string.Empty,
                Background = new SolidColorBrush(Colors.White),
                GridRow = gridRow,
                GridColumn = gridColumn,
                // setting value in binary system to every field for easier win check calculations
                Value = (uint)Math.Pow(2, (gridRow - 1) * 3 + (gridColumn - 1)),
                // field on the board clicked to put 'X' or 'O'
                FieldCommand = new RelayCommand(o =>
                {
                    ChangeTicTacToeField((gridRow - 1) * 3 + (gridColumn - 1));
                })
            };
            TicTacToeFieldList.Add(newField);
        }
        // Board field changing method. Change field content on passed field Id.
        private void ChangeTicTacToeField(int fieldNumber)
        {
            if (TicTacToeGame.FillCounter == 9)
            {
                // all fields filled
                NewGame();
                return;
            }
            else if (TicTacToeFieldList[fieldNumber].Content == string.Empty)
            {
                // field is empty so input player sign
                TicTacToeFieldList[fieldNumber].Content = TicTacToeGame.IsPlayerXTurn ? "X" : "O";
                TicTacToeGame.PlayersPoints[TicTacToeGame.IsPlayerXTurn ? (int)Player.X : (int)Player.O] += TicTacToeFieldList[fieldNumber].Value;
                TicTacToeGame.FillCounter++;
            }
            else return;
            
            if (TicTacToeGame.FillCounter > 4 && CheckIfPlayerWon())
            {
                // check win conditions after minimal winning moves
                if (TicTacToeGame.IsPlayerXTurn) TicTacToeGameComments = "Player X have won! Click enywhere on the board to start a new game.";
                else TicTacToeGameComments = GameMode == (int)GameModes.TwoPlayers ? "Player O have won! Click enywhere on the board to start a new game." : "Player O (AI) have won! Click enywhere on the board to start a new game.";

                TicTacToeGame.FillCounter = 9;
                return;
            }
            else if (TicTacToeGame.FillCounter == 9)
            {
                // last move without win == tie
                TicTacToeGameComments = "It is a tie. Click enywhere on the board to start a new game.";
                return;
            }

            TicTacToeGame.IsPlayerXTurn ^= true; // next player turn
            TicTacToeGameComments = TicTacToeGame.IsPlayerXTurn ? "Player X turn." : "Player O turn.";

            if (GameMode == (int)GameModes.RandomGuy && !TicTacToeGame.IsPlayerXTurn) RandomGuyAlgorithm();
            else if (GameMode == (int)GameModes.Expert && !TicTacToeGame.IsPlayerXTurn) ExpertAlgorithm();
        }
        // AI (Random moves) method
        private void RandomGuyAlgorithm()
        {
            if(TicTacToeGame.FillCounter != 8)
            {
                // if not the last move then input player sign in random field
                Random rnd = new Random();
                int randomFieldNumber;
                do
                {
                    randomFieldNumber = rnd.Next(0, 8);
                } while (TicTacToeFieldList[randomFieldNumber].Content != string.Empty);
                ChangeTicTacToeField(randomFieldNumber);
            }
            else
            {
                // if last move then input player sign in last empty field
                for (int fieldNumber = 0; fieldNumber < 9; fieldNumber++)
                {
                    if(TicTacToeFieldList[fieldNumber].Content == string.Empty)
                    {
                        ChangeTicTacToeField(fieldNumber);
                        break;
                    }
                }
            }
        }
        // AI (min-max algorithm) method
        private void ExpertAlgorithm()
        {
            // if AI starts the game then make random move for better playing experience
            if (TicTacToeGame.FillCounter == 0 && StartingPlayer == (int)Player.O) RandomGuyAlgorithm();
            else
            {
                int bestField = FindBestField(TicTacToeFieldList, TicTacToeGame.PlayersPoints, TicTacToeGame.FillCounter);
                ChangeTicTacToeField(bestField);
            }
        }
        // min-max algorithm with recursive function to find the best move
        private int FindBestField(ObservableCollection<TicTacToeFieldViewModel> fieldsCopyList, uint[] playersPointsCopy, int fillCounterCopy)
        {
            int bestField = -1;
            int bestScore = -100;

            for (int fieldNumber = 0; fieldNumber < 9; fieldNumber++)
            {
                if (fieldsCopyList[fieldNumber].Content == string.Empty)
                {
                    // simulate move for every field
                    fieldsCopyList[fieldNumber].Content = "O";
                    playersPointsCopy[(int)Player.O] += fieldsCopyList[fieldNumber].Value;
                    fillCounterCopy++;

                    // call min-max function
                    int fieldScore = MinMaxRecursiveFunction(fieldsCopyList, playersPointsCopy, fillCounterCopy, false);

                    // undo changes
                    fieldsCopyList[fieldNumber].Content = string.Empty;
                    playersPointsCopy[(int)Player.O] -= fieldsCopyList[fieldNumber].Value;
                    fillCounterCopy--;

                    if (fieldScore > bestScore)
                    {
                        bestField = fieldNumber;
                        bestScore = fieldScore;
                    }
                }
            }
            return bestField;
        }
        // recursive function for min-max algorithm
        private int MinMaxRecursiveFunction(ObservableCollection<TicTacToeFieldViewModel> fieldsCopyList, uint[] playersPointsCopy, int fillCounterCopy, bool isAITurn)
        {
            int score = WinningSimulationCheck(playersPointsCopy);

            // AI (Maximizer) - detect win - return
            if (score == 10) return score - fillCounterCopy;

            // Player (Minimizer) - detect win - return
            if (score == -10) return score + fillCounterCopy;

            // Tie detected - return
            if (fillCounterCopy == 9) return 0;

            if (isAITurn)
            {
                int bestScore = -100; // start with very bad score for Maximizer

                for (int fieldNumber = 0; fieldNumber < 9; fieldNumber++)
                {
                    if (fieldsCopyList[fieldNumber].Content == string.Empty)
                    {
                        // simulate move for every field
                        fieldsCopyList[fieldNumber].Content = "O";
                        playersPointsCopy[(int)Player.O] += fieldsCopyList[fieldNumber].Value;
                        fillCounterCopy++;

                        // recursive call and compare scores to find higher
                        bestScore = Math.Max(bestScore, MinMaxRecursiveFunction(fieldsCopyList, playersPointsCopy, fillCounterCopy, !isAITurn));

                        // undo changes
                        fieldsCopyList[fieldNumber].Content = string.Empty;
                        playersPointsCopy[(int)Player.O] -= fieldsCopyList[fieldNumber].Value;
                        fillCounterCopy--;
                    }
                }
                return bestScore;
            }
            else // if (!isIATurn)
            {
                int bestScore = 100; // start with very bad score for Minimizer

                for (int fieldNumber = 0; fieldNumber < 9; fieldNumber++)
                {
                    if (fieldsCopyList[fieldNumber].Content == string.Empty)
                    {
                        // simulate move for every field
                        fieldsCopyList[fieldNumber].Content = "X";
                        playersPointsCopy[(int)Player.X] += fieldsCopyList[fieldNumber].Value;
                        fillCounterCopy++;

                        // recursive call and compare scores to find lower
                        bestScore = Math.Min(bestScore, MinMaxRecursiveFunction(fieldsCopyList, playersPointsCopy, fillCounterCopy, !isAITurn));

                        // undo changes
                        fieldsCopyList[fieldNumber].Content = string.Empty;
                        playersPointsCopy[(int)Player.X] -= fieldsCopyList[fieldNumber].Value;
                        fillCounterCopy--;
                    }
                }
                return bestScore;
            }
        }
        // check if someone won in simulation and return scores depend on the outcome
        private int WinningSimulationCheck(uint[] playersPoints)
        {
            for (int player = (int)Player.X; player <= (int)Player.O; player++)
            {
                for (int valueId = 0; valueId < TicTacToeGame.WinValues.Length; valueId++)
                {
                    // if player points contain WinValues in binary system then it is a vicotry
                    if ((playersPoints[player] & TicTacToeGame.WinValues[valueId]) == TicTacToeGame.WinValues[valueId]) return (-10 + player * 20);
                }
            }
            return 0; // tie
        }
        // check if any player won the game
        private bool CheckIfPlayerWon()
        {
            int playerNumber = TicTacToeGame.IsPlayerXTurn ? (int)Player.X : (int)Player.O;
            for (int valueId = 0; valueId < TicTacToeGame.WinValues.Length; valueId++)
            {
                if ((TicTacToeGame.PlayersPoints[playerNumber] & TicTacToeGame.WinValues[valueId]) == TicTacToeGame.WinValues[valueId])
                {
                    // if player points contain WinValues in binary system then it is a vicotry
                    uint temp = TicTacToeGame.WinValues[valueId];
                    for (int fieldNumber = 8; fieldNumber >= 0; fieldNumber--)
                    {
                        // highlight victory fields
                        if (temp >= (uint)Math.Pow(2, fieldNumber))
                        {
                            temp -= (uint)Math.Pow(2, fieldNumber);
                            TicTacToeFieldList[fieldNumber].Background = new SolidColorBrush(Colors.Green);
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        // start new game method (restart board)
        private void NewGame()
        {
            TicTacToeGame.FillCounter = 0;
            TicTacToeGame.PlayersPoints[(int)Player.X] = 0;
            TicTacToeGame.PlayersPoints[(int)Player.O] = 0;

            foreach (var field in TicTacToeFieldList)
            {
                field.Content = string.Empty;
                field.Background = new SolidColorBrush(Colors.White);
            }

            if (StartingPlayer == (int)Player.X)
            {
                TicTacToeGameComments = "Player X turn.";
                TicTacToeGame.IsPlayerXTurn = true;
            }
            else if(GameMode == (int)GameModes.TwoPlayers)
            {
                TicTacToeGameComments = "Player O turn.";
                TicTacToeGame.IsPlayerXTurn = false;
            }
            else
            {
                TicTacToeGameComments = "Player O turn (AI).";
                TicTacToeGame.IsPlayerXTurn = false;
                if (GameMode == (int)GameModes.RandomGuy) RandomGuyAlgorithm();
                else ExpertAlgorithm();
            }
        }

        public TicTacToePageViewModel()
        {
            TicTacToeGame = new TicTacToeGameModel();

            // fill board with Tic Tac Toe fields at page load
            for (int rowId = 1; rowId <= 3; rowId++)
            {
                for (int columnId = 1; columnId <= 3; columnId++)
                {
                    AddNewTicTacToeField(rowId, columnId);
                }
            }

            // if playing with AI then make AI move
            if (GameMode == (int)GameModes.RandomGuy && StartingPlayer == (int)Player.O) RandomGuyAlgorithm();
            else if (GameMode == (int)GameModes.Expert && StartingPlayer == (int)Player.O) ExpertAlgorithm();

            // restart button clicked
            RestartGameCommand = new RelayCommand(o =>
            {
                NewGame();
            });
        }
    }
}
