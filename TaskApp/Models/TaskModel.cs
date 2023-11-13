using SQLite;

namespace TaskApp.Models
{
	public class TaskModel
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string TaskText { get; set; }

		public TaskModel Clone() => MemberwiseClone() as TaskModel;
	}
}
