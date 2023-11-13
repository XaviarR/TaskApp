using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Data;
using TaskApp.Models;

namespace TaskApp.ViewModel
{
	public partial class TaskViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public TaskViewModel(DatabaseContext context)
		{
			_context = context;
		}

		[ObservableProperty]
		private ObservableCollection<TaskModel> _tasks;

		[ObservableProperty]
		private TaskModel _operatingTask = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadTaskAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<TaskModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Tasks == null)
					{
						Tasks = new ObservableCollection<TaskModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Tasks.Any(t => t.Id == task.Id))
						{
							Tasks.Add(task);
						}
					}
				}
			}, "Fetching tasks...");
		}


		// Update Logic
		[RelayCommand]
		public void SetOperatingTask(TaskModel? task)
		{
			OperatingTask = task ?? new();
		}

		// Save Logic
		[RelayCommand]
		private async Task SaveTaskAsync()
		{
			if (OperatingTask == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Task, else Updating Task
			var busyText = OperatingTask.Id == 0 ? "Creating Task..." : "Updating Task...";

			await ExecuteAsync(async () =>
			{
				if (OperatingTask.Id == 0)
				{
					// Create Task
					await _context.AddItemAync<TaskModel>(OperatingTask);
					// Add the new task to the collection only if it doesn't already exist
					if (!Tasks.Any(t => t.Id == OperatingTask.Id))
					{
						Tasks.Add(OperatingTask);
					}
				}
				else
				{
					// Updating Task
					await _context.UpdateItemAync<TaskModel>(OperatingTask);
					// create clone to keep data
					var taskCopy = OperatingTask.Clone();
					// Get the index of the item to remove then add back
					var index = Tasks.IndexOf(OperatingTask);
					Tasks.RemoveAt(index);
					Tasks.Insert(index, taskCopy);
				}

				// Reset data of OperatingTask
				SetOperatingTaskCommand.Execute(new());
			}, busyText);
		}


		// Delete Logic
		[RelayCommand]
		private async Task DeleteTaskAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<TaskModel>(id))
				{
					var task = Tasks.FirstOrDefault(p => p.Id == id);
					Tasks.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "Task was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "Task was unsuccessfully deleted", "OK");
				}
			}, "Deleting Task...");
		}

		// Function to display text based on what CRUD function, Set Manually in code of each Function
		private async Task ExecuteAsync(Func<Task>operation,string? busyText = null)
		{
			IsBusy = true;
			BusyText = busyText ?? "Processing...";
			try
			{
				await operation?.Invoke();
			}
			finally
			{
				IsBusy = false;
				BusyText = "Processing...";
			}
		}
		
		[RelayCommand]
		async Task Tap(TaskModel task)
		{
			await Shell.Current.GoToAsync($"{nameof(DetailPage)}?Text={task.TaskText}");
		}

	}
}
