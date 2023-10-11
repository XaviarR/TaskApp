using TaskApp.ViewModel;

namespace TaskApp
{
	public partial class MainPage : ContentPage
	{

		public MainPage(MainViewModel vm)
		{
			InitializeComponent();
			BindingContext = vm;
		}
	}
}