﻿using TaskApp.Data;
using TaskApp.ViewModel;

namespace TaskApp
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				});

			//#if DEBUG
			//		builder.Logging.AddDebug();
			//#endif

			builder.Services.AddSingleton<DatabaseContext>();
			builder.Services.AddSingleton<TaskViewModel>();

			builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

			builder.Services.AddSingleton<MainPage>();
			builder.Services.AddSingleton<MainViewModel>();

			builder.Services.AddTransient<DetailPage>();
			builder.Services.AddTransient<DetailViewModel>();

			return builder.Build();
		}
	}
}