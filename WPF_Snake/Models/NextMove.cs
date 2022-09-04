using WPF_Snake.DataModels;

namespace WPF_Snake.Models
{
    internal class NextMove
    {
        public int Xpos { get; }
        public int Ypos { get; }
        public Direction Direction { get; }


        public NextMove(int xpos, int ypos, Direction direction)
        {
            Xpos = xpos;
            Ypos = ypos;
            Direction = direction;
        }
    }
}
