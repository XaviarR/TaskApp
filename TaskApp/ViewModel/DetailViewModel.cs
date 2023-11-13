using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaskApp.ViewModel;

[QueryProperty("Text", "Text")]
public partial class DetailViewModel : ObservableObject
{
    private string text;


    public string Text
    {
        get => text;
        set => SetProperty(ref text, value);
    }

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("../");
    }
}
