using System.Text.Json.Serialization;
using WPF_Snake.ViewModels.Base;

namespace WPF_Snake.ViewModels
{
    public class HighScore : BaseViewModel
    {
        public int Score { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public bool IsOldScore { get; set; }
    }
}
