using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        #endregion

        #region Properties

        /// <summary>
        /// The size of the game grid
        /// </summary>
        public int GameGridSize => MAX_GAME_GRID_SIZE;

        /// <summary>
        /// Flag to let us know if the game is over
        /// </summary>
        public bool GameOver { get; set; } = false;

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

        public ICommand ClickCommand { get; set; }

        #endregion

        #region Constructor

        public MainWindowViewModel(Window window)
        {
            _window = window;
            _window.KeyUp += _window_KeyUp;
            var snakeHead = new CellViewModel(200, 200);
            snakeHead.Rgb = CellViewModel.SNAKE_HEAD_RGB;
            Snake.Add(snakeHead);
            SpawnFruit();
        }

        #endregion

        #region Command Methods

        #endregion

        #region Methods

        /// <summary>
        /// The window key up event notifies us that a key was let go
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _window_KeyUp(object sender, KeyEventArgs e)
        {
            int xPos = 0;
            int yPos = 0;
            bool growSnake = false;
            switch (e.Key)
            {
                case Key.Space:
                    growSnake = true;
                    break;
                case Key.Left:
                    xPos -= 10;
                    break;
                case Key.Up:
                    yPos -= 10;
                    break;
                case Key.Right:
                    xPos += 10;
                    break;
                case Key.Down:
                    yPos += 10;
                    break;
            }

            if (growSnake)
            {
                GrowSnake();
                growSnake = false;
            }

            CheckIfSnakeEatSelf(xPos, yPos);
            if (!GameOver)
            {
                MoveSnake(xPos, yPos);
                CheckIfFruitEaten();
            }
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
