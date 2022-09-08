using System.Text.Json.Serialization;
using WPF_Snake.ViewModels.Base;

namespace WPF_Snake.ViewModels
{
    public class HighScoreViewModel : BaseViewModel
    {
        public int Score { get; set; }

        public string Name { get; set; } = string.Empty;

        [JsonIgnore]
        public bool IsOldScore { get; set; } = true;

        [JsonIgnore]
        public bool Focus { get; set; }
    }
}
