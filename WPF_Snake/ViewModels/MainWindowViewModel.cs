using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WPF_Snake.DataModels;
using WPF_Snake.Models;
using WPF_Snake.ViewModels.Base;

namespace WPF_Snake.ViewModels
{
    /// <summary>
    /// The main window view model for the main window
    /// </summary>
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private Window _window;

        private Random _random = new Random();

        private const int MAX_GAME_GRID_SIZE = 400;
        private const int MAX_GAME_GRID_ROWS = 40;
        private const int MAX_GAME_GRID_COLUMNS = 40;

        private Direction _currentDirection = Direction.DOWN;
        private Queue<NextMove> _nextMoves = new Queue<NextMove>();
        private object _lock = new object();
        private int _snakeSpeed = 200;

        #endregion

        #region Properties

        /// <summary>
        /// The size of the game grid
        /// </summary>
        public int GameGridSize => MAX_GAME_GRID_SIZE;

        /// <summary>
        /// The current score
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// Flag to let us know if the game is over
        /// </summary>
        public bool GameOver { get; set; } = true;

        /// <summary>
        /// Flag to let us know if the main menu is visible
        /// </summary>
        public bool MainMenuVisible { get; set; } = true;

        /// <summary>
        /// Fruit grows randomly and grows the snake
        /// </summary>
        public CellViewModel Fruit { get; set; }

        /// <summary>
        /// The snake to draw in the game grid
        /// </summary>
        public ObservableCollection<CellViewModel> Snake { get; set; } = new ObservableCollection<CellViewModel>();

        #endregion

        #region Commands

        public ICommand PlayCommand { get; set; }

        public ICommand MainMenuCommand { get; set; }


        #endregion

        #region Constructor

        public MainWindowViewModel(Window window)
        {
            _window = window;
            _window.KeyUp += _window_KeyUp;

            BindingOperations.EnableCollectionSynchronization(Snake, _lock);

            var snakeHead = new CellViewModel(200, 200);
            snakeHead.Rgb = CellViewModel.SNAKE_HEAD_RGB;
            Snake.Add(snakeHead);
            SpawnFruit();

            PlayCommand = new RelayCommand(Play);
        }
        #endregion

        #region Command Methods

        /// <summary>
        /// Hides the main menu and starts the gameloop
        /// </summary>
        private void Play()
        {
            MainMenuVisible = false;
            GameOver = false;
            Task.Run(() => GameLoop());
        }


        #endregion

        #region Methods

        /// <summary>
        /// The game loop
        /// </summary>
        private void GameLoop()
        {
            while (!GameOver)
            {
                Thread.Sleep(_snakeSpeed);

                if (_nextMoves.Any())
                {
                    var move = _nextMoves.Dequeue();
                    _currentDirection = move.Direction;
                    MoveSnake(move.Xpos, move.Ypos);
                }
                else
                {
                    MoveSnake();
                }

                CheckIfFruitEaten();
            }
        }

        /// <summary>
        /// The window key up event notifies us that a key was let go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _window_KeyUp(object sender, KeyEventArgs e)
        {
            int xPos = 0;
            int yPos = 0;
            Direction newDirecton = Direction.LEFT;

            switch (e.Key)
            {
                case Key.Left:
                    if (_currentDirection == Direction.RIGHT && Snake.Count > 2)
                        return;
                    xPos -= CellViewModel.CELL_SIZE;
                    newDirecton = Direction.LEFT;
                    break;
                case Key.Up:
                    if (_currentDirection == Direction.DOWN && Snake.Count > 2)
                        return;
                    yPos -= CellViewModel.CELL_SIZE;
                    newDirecton = Direction.UP;
                    break;
                case Key.Right:
                    if (_currentDirection == Direction.LEFT && Snake.Count > 2)
                        return;
                    xPos += CellViewModel.CELL_SIZE;
                    newDirecton = Direction.RIGHT;
                    break;
                case Key.Down:
                    if (_currentDirection == Direction.UP && Snake.Count > 2)
                        return;
                    yPos += CellViewModel.CELL_SIZE;
                    newDirecton = Direction.DOWN;
                    break;
            }

            _nextMoves.Enqueue(new NextMove(xPos, yPos, newDirecton));
        }

        /// <summary>
        /// Checks if the snake will hit a wall
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void CheckIfSnakeHitWall(int xPos, int yPos)
        {
            var nextHeadPositionX = Snake.Last().XPos + xPos;
            var nextHeadPositionY = Snake.Last().YPos + yPos;

            if(nextHeadPositionX < 0 || 
               nextHeadPositionY < 0 ||
               nextHeadPositionX > MAX_GAME_GRID_SIZE || 
               nextHeadPositionY > MAX_GAME_GRID_SIZE)
            {
                GameOver = true;
            }
        }

        /// <summary>
        /// Checks if the snake will eat its self
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void CheckIfSnakeEatSelf(int xPos, int yPos)
        {
            var nextHeadPositionX = Snake.Last().XPos + xPos;
            var nextHeadPositionY = Snake.Last().YPos + yPos;

            for (int index = 1; index < Snake.Count - 1; index++)
            {
                if (Snake[index].YPos.Equals(nextHeadPositionY) && Snake[index].XPos.Equals(nextHeadPositionX))
                {
                    GameOver = true;
                }
            }
        }

        /// <summary>
        /// Checks if the snake ate a fruit. If yes than a new fruit will spawn and the snake grows
        /// </summary>
        private void CheckIfFruitEaten()
        {
            var snakeX = Snake.Last().XPos;
            var snakeY = Snake.Last().YPos;
            if (snakeX.Equals(Fruit.XPos) && snakeY.Equals(Fruit.YPos))
            {
                GrowSnake();
                SpawnFruit();
                Score++;
                _snakeSpeed -= 2;
            }
        }

        /// <summary>
        /// Moves the snake one step in current direction
        /// </summary>
        private void MoveSnake()
        {
            int xPos = 0;
            int yPos = 0;

            switch (_currentDirection)
            {
                case Direction.LEFT:
                    xPos -= CellViewModel.CELL_SIZE;
                    break;
                case Direction.UP:
                    yPos -= CellViewModel.CELL_SIZE;
                    break;
                case Direction.RIGHT:
                    xPos += CellViewModel.CELL_SIZE;
                    break;
                case Direction.DOWN:
                    yPos += CellViewModel.CELL_SIZE;
                    break;
            }

            CheckIfSnakeEatSelf(xPos, yPos);
            if (!GameOver)
            {
                MoveSnake(xPos, yPos);
            }
        }

        /// <summary>
        /// Moves the snake to the next position
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        private void MoveSnake(int xPos, int yPos)
        {
            for(int index = 0; index < Snake.Count-1; index++)
            {
                Snake[index].XPos = Snake[index + 1].XPos;
                Snake[index].YPos = Snake[index + 1].YPos;
            }

            int newX = Snake.Last().XPos + xPos;
            int newY = Snake.Last().YPos + yPos;

            if(newX < 0)
            {
                newX = MAX_GAME_GRID_SIZE-10;
            }
            else if(newX > MAX_GAME_GRID_SIZE-10)
            {
                newX = 0;
            }

            if (newY < 0)
            {
                newY = MAX_GAME_GRID_SIZE-10;
            }
            else if (newY > MAX_GAME_GRID_SIZE-10)
            {
                newY = 0;
            }

            Snake.Last().XPos = newX;
            Snake.Last().YPos = newY;
        }

        /// <summary>
        /// Grows the snake
        /// </summary>
        private void GrowSnake()
        {
            var snakeSection = new CellViewModel(Snake.First().XPos, Snake.First().YPos);
            if (Snake.First().Rgb.Equals(CellViewModel.SNAKE_HEAD_RGB))
            {
                snakeSection.Rgb = CellViewModel.SNAKE_BODY1_RGB;
            }
            else if (Snake.First().Rgb.Equals(CellViewModel.SNAKE_BODY1_RGB))
            {
                snakeSection.Rgb = CellViewModel.SNAKE_BODY2_RGB;
            }
            else if (Snake.First().Rgb.Equals(CellViewModel.SNAKE_BODY2_RGB))
            {
                snakeSection.Rgb = CellViewModel.SNAKE_BODY3_RGB;
            }
            else
            {
                snakeSection.Rgb = CellViewModel.SNAKE_BODY1_RGB;
            }

            Snake.Insert(0, snakeSection);
        }

        /// <summary>
        /// Generates a new fruit at a random location
        /// </summary>
        private void SpawnFruit()
        {
            bool foundSection = false;
            int xPos = 0;
            int yPos = 0;
            do
            {
                xPos = _random.Next(0, MAX_GAME_GRID_ROWS) * 10;
                yPos = _random.Next(0, MAX_GAME_GRID_COLUMNS) * 10;
                foundSection = Snake.FirstOrDefault(item => item.XPos.Equals(xPos) && item.YPos.Equals(yPos)) != null;
            } while (foundSection);

            Fruit = new CellViewModel(xPos, yPos);
        }

        #endregion
    }
}
