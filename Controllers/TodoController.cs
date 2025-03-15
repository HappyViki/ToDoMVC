using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class TodoController : Controller
{
    private readonly AppDbContext _context;

    public TodoController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var todos = _context.Todos.ToList();
        return View(todos);
    }

    [HttpPost]
    public IActionResult AddTask(string task)
    {
        if (!string.IsNullOrEmpty(task))
        {
            _context.Todos.Add(new TodoItem { Task = task, IsCompleted = false });
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ToggleComplete(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            todo.IsCompleted = !todo.IsCompleted;
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteTask(int id)
    {
        var todo = _context.Todos.Find(id);
        if (todo != null)
        {
            _context.Todos.Remove(todo);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}
