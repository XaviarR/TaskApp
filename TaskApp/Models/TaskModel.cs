using SQLite;

namespace TaskApp.Models
{
	public class TaskModel
	{
		// Create primary key that auto increments
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string TaskText { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public TaskModel Clone() => MemberwiseClone() as TaskModel;
	}
}
