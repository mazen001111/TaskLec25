using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskLec25.Data;
using TaskLec25.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TaskLec25.Controllers
{
    public class ToDoController : Controller
    {
        ApplicationDbContext _db = new ApplicationDbContext();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Index(string Name)
        {
            if (Name is null) return View(nameof(Index));

            var Users = _db.Users.FirstOrDefault(e => e.Name == Name);

            if (Users is null)
            {
                _db.Users.Add(new()
                {
                    Name = Name
                });
                _db.SaveChanges();
            }

            var user = _db.Users.FirstOrDefault(e => e.Name == Name);
            return RedirectToAction(nameof(Items), new { id = user.Id });
        }
        public IActionResult DeleteItem(int id, int userid)
        {
            var item = _db.Tasks.FirstOrDefault(e => e.MyTaskId == id);

            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", item.FileName);
            if (System.IO.File.Exists(oldPath))
            {
                System.IO.File.Delete(oldPath);
            }

            _db.Tasks.Remove(item);
            _db.SaveChanges();
            return RedirectToAction(nameof(Items),new { id = userid } );
        }
    

        public IActionResult Items(int id)
        {

            var Tasks = _db.Tasks.AsQueryable();
            Tasks = Tasks.Where(e =>e.UserId == id);

            var user = _db.Users.FirstOrDefault(e => e.Id == id);
            return View(new ItemsReturnVM
            {
                Tasks=Tasks,
                name=user.Name,
                Id=id
            });
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            
            return View(id);
        }

        [HttpPost]
        public IActionResult Create(MyTask Task , IFormFile File , int UserId)
        {
            if (Task.Deadline < DateTime.Now.AddMinutes(30))
            {
                ModelState.AddModelError("Deadline", "The deadline must be at least 1 hour from now.");

                return View(UserId);
            }

            if (File is not null && File.Length > 0 )
            {
                var  fileName = $"{Guid.NewGuid().ToString()}_{DateTime.Now.ToString("dd-mm-yy")}{Path.GetExtension(File.FileName)}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    File.CopyTo(stream);
                }

                Task.FileName = fileName;
            }
            Task.UserId = UserId;
            _db.Tasks.Add(Task);
            _db.SaveChanges();
            
            return RedirectToAction(nameof(Items) , new { id = UserId }) ;
        }

        [HttpGet]
        public IActionResult Edit(int TaskId , int userid)
        {
            var task = _db.Tasks.FirstOrDefault(e => e.MyTaskId == TaskId);

            return View(task);
        }


        [HttpPost]
        public IActionResult Edit(MyTask Task, IFormFile File, int UserId)
        {
            var TaskFromDb = _db.Tasks.AsNoTracking().FirstOrDefault(e => e.MyTaskId == Task.MyTaskId);
            if (TaskFromDb is null)  return NotFound();

            if (Task.Deadline < DateTime.Now.AddMinutes(30))
            {
                ModelState.AddModelError("Deadline", "The deadline must be at least 30 min from now.");

                return View(Task);
            }

            if (File is not null && File.Length > 0)
            {
                var fileName = $"{Guid.NewGuid().ToString()}_{DateTime.Now.ToString("dd-mm-yy")}{Path.GetExtension(File.FileName)}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    File.CopyTo(stream);
                }

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", TaskFromDb.FileName);

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                Task.FileName = fileName;
            }
            else
            {

                Task.FileName = TaskFromDb.FileName;
            }

            Task.UserId = UserId;
            _db.Tasks.Update(Task);
            _db.SaveChanges();

            return RedirectToAction(nameof(Items), new { id = UserId });
        }


    }
}
