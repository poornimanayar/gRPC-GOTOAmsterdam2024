using System.ComponentModel.DataAnnotations.Schema;

namespace gRPC.Demo.Transcoding.Models;

public class ToDoItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DueDate { get; set; }
    public bool Complete { get; set; }
    public string Notes { get; set; }
    public int Priority { get; set; }
    public bool SendAlert { get; set; }
}
