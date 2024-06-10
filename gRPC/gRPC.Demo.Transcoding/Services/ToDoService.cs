using JSONOverHTTP.gRPCApi.Proto;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using gRPC.Demo.Transcoding.Repository;
using gRPC.Demo.Transcoding.Models;

namespace gRPC.Demo.Transcoding.Services;

public class ToDoService : ToDo.ToDoBase
{
    private readonly ToDoItemsRepository _toDoItemRepository;

    public ToDoService(ToDoItemsRepository toDoItemRepository)
    {
        _toDoItemRepository = toDoItemRepository;
    }
    public override Task<ListReply> ListItems(Empty request, ServerCallContext context)
    {
        var todoItems = _toDoItemRepository.GetToDoItems();

        var listReply = new ListReply();

        var todoItemsReply = new List<ToDoItemReply>();

        foreach (var item in todoItems)
        {
            todoItemsReply.Add(new ToDoItemReply { Id = item.Id, Complete = item.Complete, DueDate = Timestamp.FromDateTime(item.DueDate.ToUniversalTime()), Name = item.Name, Notes = item.Notes, Priority = item.Priority, SendAlert = item.SendAlert });
        }

        listReply.ToDoItems.AddRange(todoItemsReply);

        return Task.FromResult(listReply);
    }


    public override Task<ToDoItemReply> GetToDoItem(ToDoItemRequest request, ServerCallContext context)
    {
        var todoItem = _toDoItemRepository.GetToDoItemById(request.Id);

        if (todoItem == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "todo item not found"));
        }

        var toDoItemReply = new ToDoItemReply { Id = todoItem.Id, Complete = todoItem.Complete, DueDate = Timestamp.FromDateTime(todoItem.DueDate.ToUniversalTime()), Name = todoItem.Name, Notes = todoItem.Notes, Priority = todoItem.Priority, SendAlert = todoItem.SendAlert };

        return Task.FromResult(toDoItemReply);

    }

    public override Task<ToDoItemReply> CreateToDoItem(CreateToDoItemRequest request, ServerCallContext context)
    {

        var toDoItemToCreate = new ToDoItem
        {
            Complete = request.Complete,
            DueDate = request.DueDate.ToDateTime(),
            Name = request.Name,
            Notes = request.Notes,
            Priority = request.Priority,
            SendAlert = request.SendAlert
        };

        var todoItem = _toDoItemRepository.AddToDoItem(toDoItemToCreate);

        var toDoItemReply = new ToDoItemReply { Id = todoItem.Id, Complete = todoItem.Complete, DueDate = Timestamp.FromDateTime(todoItem.DueDate.ToUniversalTime()), Name = todoItem.Name, Notes = todoItem.Notes, Priority = todoItem.Priority, SendAlert = todoItem.SendAlert };

        return Task.FromResult(toDoItemReply);

    }

    public override Task<ToDoItemReply> UpdateToDoItem(UpdateToDoItemRequest request, ServerCallContext context)
    {

        var toDoItemToUpdate = new ToDoItem
        {
            Id = request.Id,
            Complete = request.Complete,
            DueDate = request.DueDate.ToDateTime(),
            Name = request.Name,
            Notes = request.Notes,
            Priority = request.Priority,
            SendAlert = request.SendAlert
        };

        var updated = _toDoItemRepository.UpdateToDoItem(toDoItemToUpdate);

        var toDoItemReply = new ToDoItemReply { Id = toDoItemToUpdate.Id, Complete = toDoItemToUpdate.Complete, DueDate = Timestamp.FromDateTime(toDoItemToUpdate.DueDate.ToUniversalTime()), Name = toDoItemToUpdate.Name, Notes = toDoItemToUpdate.Notes, Priority = toDoItemToUpdate.Priority, SendAlert = toDoItemToUpdate.SendAlert };

        return Task.FromResult(toDoItemReply);

    }

    public override Task<Empty> DeleteToDoItem(DeleteToDoItemRequest request, ServerCallContext context)
    {
        _toDoItemRepository.DeleteToDoItem(request.Id);

        return Task.FromResult(new Empty());
    }

    public override Task<ListReply> GetCompletedItems(Empty request, ServerCallContext context)
    {
        var todoItems = _toDoItemRepository.GetToDoItemsByStatus(true);

        var listReply = new ListReply();

        var todoItemsReply = new List<ToDoItemReply>();

        foreach (var item in todoItems)
        {
            todoItemsReply.Add(new ToDoItemReply { Id = item.Id, Complete = item.Complete, DueDate = Timestamp.FromDateTime(item.DueDate.ToUniversalTime()), Name = item.Name, Notes = item.Notes, Priority = item.Priority, SendAlert = item.SendAlert });
        }

        listReply.ToDoItems.AddRange(todoItemsReply);


        return Task.FromResult(listReply);
    }

    public override Task<ListReply> GetInCompleteItems(Empty request, ServerCallContext context)
    {
        var todoItems = _toDoItemRepository.GetToDoItemsByStatus(false);

        var listReply = new ListReply();

        var todoItemsReply = new List<ToDoItemReply>();

        foreach (var item in todoItems)
        {
            todoItemsReply.Add(new ToDoItemReply { Id = item.Id, Complete = item.Complete, DueDate = Timestamp.FromDateTime(item.DueDate.ToUniversalTime()), Name = item.Name, Notes = item.Notes, Priority = item.Priority, SendAlert = item.SendAlert });
        }

        listReply.ToDoItems.AddRange(todoItemsReply);


        return Task.FromResult(listReply);
    }

    public override Task<ListReply> GetItemsDueByDate(Timestamp request, ServerCallContext context)
    {
        var todoItems = _toDoItemRepository.GetToDoItemsByDueDate(request.ToDateTime());

        var listReply = new ListReply();

        var todoItemsReply = new List<ToDoItemReply>();

        foreach (var item in todoItems)
        {
            todoItemsReply.Add(new ToDoItemReply { Id = item.Id, Complete = item.Complete, DueDate = Timestamp.FromDateTime(item.DueDate.ToUniversalTime()), Name = item.Name, Notes = item.Notes, Priority = item.Priority, SendAlert = item.SendAlert });
        }

        listReply.ToDoItems.AddRange(todoItemsReply);

        return Task.FromResult(listReply);
    }

    public override Task<SendAlertReply> SendAlert(SendAlertRequest request, ServerCallContext context)
    {
        return Task.FromResult(new SendAlertReply { AlertMessage = "There is a task to be completed!" });
    }


    public override Task<SendAlertReply> SendArray(Arrayrequest request, ServerCallContext context)
    {
        foreach (var id in request.Id)
        {
            Console.WriteLine(id);
        }
        return Task.FromResult(new SendAlertReply { AlertMessage = "There is a task to be completed!" });
    }
}
