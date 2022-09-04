using WPF_Snake.ViewModels.Base;

namespace WPF_Snake.ViewModels
{
    /// <summary>
    /// The View Model for each cell in the game grid
    /// </summary>
    public class CellViewModel : BaseViewModel
    {
        #region Fields

        public static int[] SNAKE_HEAD_RGB = { 0, 0, 255 };

        public static int[] SNAKE_BODY1_RGB = { 0, 255, 0 };
        public static int[] SNAKE_BODY2_RGB = { 0, 200, 0 };
        public static int[] SNAKE_BODY3_RGB = { 0, 150, 0 };

        public static int CELL_SIZE = 10;

        #endregion

        #region Properties

        /// <summary>
        /// Width of the cell
        /// </summary>
        public int Width => CELL_SIZE;

        /// <summary>
        /// Height of the cell
        /// </summary>
        public int Height => CELL_SIZE;

        /// <summary>
        /// The x position of a cell
        /// </summary>
        public int XPos { get; set; }

        /// <summary>
        /// The y position of a cell
        /// </summary>
        public int YPos { get; set; }

        /// <summary>
        /// The cells color in rgb 
        /// </summary>
        public int[] Rgb { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Overloaded constructor
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        public CellViewModel(int xpos, int ypos)
        {
            XPos = xpos;
            YPos = ypos;
            Rgb = SNAKE_HEAD_RGB;
        } 

        #endregion
    }
}
