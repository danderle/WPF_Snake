using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace WPF_Snake.ViewModels;

public partial class HighScoreViewModel : ObservableObject
{
    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private string name = string.Empty;

    [JsonIgnore]
    [ObservableProperty]
    private bool isOldScore = true;

    [JsonIgnore]
    [ObservableProperty]
    private bool focus;
}
