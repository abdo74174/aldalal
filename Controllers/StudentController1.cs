using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp.Models;
using System.Threading.Tasks;

namespace SchoolApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Inject ApplicationDbContext into the controller
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students/Index
        // This will list all the students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // GET: Students/AddStudent
        // This action returns the AddStudent view to add a new student
        public IActionResult AddStudent()
        {
            return View();
        }

        // POST: Students/AddStudent
        // This action receives the student data, adds it to the database, and redirects to the student list
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);  // Add the student to the DbContext
                await _context.SaveChangesAsync();  // Save changes to the database
                return RedirectToAction(nameof(Index));  // Redirect to the Index action to list students
            }
            return View(student);  // If model is invalid, return to the same view with the invalid model
        }

        // GET: Students/EditStudent/5
        // This action will retrieve the student by Id and return the EditStudent view with the student data
        public async Task<IActionResult> EditStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);  // Return the Edit view with the student data
        }

        // POST: Students/EditStudent/5
        // This action updates the student data in the database and redirects to the student list
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(int id, [Bind("Id,Name,RollNumber,GradeLevel,DateOfBirth,Email,PhoneNumber")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);  // Update the student in the DbContext
                    await _context.SaveChangesAsync();  // Save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  // Redirect to the Index action to list students
            }
            return View(student);  // If model is invalid, return to the Edit view with the invalid model
        }

        // GET: Students/DeleteStudent/5
        // This action retrieves the student by Id and shows the DeleteStudent view
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Remove the student from the database
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            // Redirect back to the index (student list page)
            return RedirectToAction(nameof(Index));
        }

        // POST: Students/DeleteStudent/5
        // This action deletes the student from the database
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);  // Remove the student from the DbContext
            await _context.SaveChangesAsync();  // Save changes to the database
            return RedirectToAction(nameof(Index));  // Redirect to the Index action to list students
        }

        // Helper method to check if a student exists in the database
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
