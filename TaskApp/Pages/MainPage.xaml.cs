using TaskApp.ViewModel;

namespace TaskApp
{
	public partial class MainPage : ContentPage
	{
		private readonly TaskViewModel _viewmodel;

		public MainPage(TaskViewModel viewmodel)
		{
			InitializeComponent();
			BindingContext = viewmodel;
			_viewmodel = viewmodel;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();
			await _viewmodel.LoadTaskAsync();
		}
	}
}