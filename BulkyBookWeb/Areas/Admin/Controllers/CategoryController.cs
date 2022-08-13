using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        //para tener acceso a la base de datos ( esta usando dependency injection )
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // agarra todas las categories en la db y lo pasa a una List
            //var objCategoryList = _db.Categories.ToList();
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost] // debe ir cuando el action method es post
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            // validacion custom
            if (obj.Name == obj.DisplayOrder.ToString())
            {// * * *
                ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name.");
            }

            // * , * *
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                //return View(); con este al salvar los cambios nos deja en la misma parte ( el form para agregar categorias )
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                // si no es valido el obj retorna a la vista actual ( de rellenar )
                return View(obj);
            }
        }

        //////////////////////////////

        // EDIT GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.GetFirstOrDefault(c => c.Name == "id");
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully!";

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        //////////////////////////////

        // DELETE GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        // DELETE POST
        [HttpPost] /*  *-*  */
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}

// para prevenir el cross side forgery attach

// *
// el Category obj q se pasa va a tener la info de los input dentro d un
// Category obj para crear el record dentro de la DB, simplemente la sig
// linea jajajajaj el push del obj a la db se hace hasta q se ejecuta .SaveChanges();

// * *
// el ModelState.IsValid
// va a checar que el obj que se pasa ( q es lo q se mete en el input ), sea
// valido de acuerdo a lo que se especifico en el modelo ( los required y eso )
// en resumen, valida la data metida con el modelo q cree

// * * *
// si pongo com primera key "name" en lugar de "CustomError"
// ModelState.AddModelError("CustomError", "The ...
// el texto de error q pongo se pondria en el input de Name, ya q estamos trabajando
// con el Category obj ( si pasa eso, pero la razon no estoy seguro )
// estaria insertando un error en la prop Name

// *-*
// podria agregar  [HttpPost, ActionName("Delete")]
// para q en la forma hace el submit al delete poder poner en el asp-action
// <form method="post" asp-action="Delete">
// y que se venga a esta