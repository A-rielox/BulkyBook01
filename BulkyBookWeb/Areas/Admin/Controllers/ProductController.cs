using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //para tener acceso a la base de datos ( esta usando dependency injection )
        private readonly IUnitOfWork _unitOfWork;
        // para acceder a la carpeta wwwroot se necesita webhost enviroment, se hace mediante dependency injection
        private readonly IWebHostEnvironment _hostEnviroment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviroment)
        {
            _unitOfWork = unitOfWork;
            _hostEnviroment = hostEnviroment;
        }

        public IActionResult Index()
        {
            return View();
        }

        // CREATE GET
        // va a implementar upsert

        //////////////////////////////

        // EDIT-CREATE GET
        public IActionResult Upsert(int? id)
        {
            // voy a poner un dropdown para covertype y category, xlo q voy a 
            // necesitar los objects de estos, a traves de unitOfWork puedo 
            // acceder a cualquier repositorio
            // SelectListItem es para dropdown, va a ser una collection de SelectListItems
            // .Select --> Projects each element of a sequence into a new form.
            // Parameters
            //      source IEnumerable<TSource>
            //      A sequence of values to invoke a transform function on.
            //      Returns IEnumerable<TResult>
            //      An IEnumerable<T> whose elements are the result of invoking the
            //      transform function on each element of source.

            // VIEWBAG --> transfiere data desde controller hacia view, la data dura solo
            // una http request
            // con TempData dura 2 reqs, por eso se usa para mensajes

            // con estos Views... la view no esta binded a un solo modelo, asi que 
            // el prefiere q una view este binded a un solo modelo, asi que prefiere
            // crear un view model que sea responsable product upsert view.
            // de esta forma si hay q agregar mas categorias se agregan en 
            // ProductVM en lugar de aca en el controller, de esta manera el controller
            // esta tightly binded to ProductVM y no loosley binded con los IEnumerable
            //IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString(),
            //    });
            //IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });

            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(
                    i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
                    i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString(),
                    }),
            };

            if (id == null || id == 0)
            {
                // con id = null o 0 --> crear producto, sino (else) editar
                // le creo la prop CategoryList y le paso el valor de CategoryList
                // ViewBag.CategoryList = CategoryList;
                // con ViewData hay q hacer casting antes de usarla
                // ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                // editar producto
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault( u => u.Id == id );

                return View(productVM);
            }            
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                if(file != null)
                {
                    // para generar un nuevo filename
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    // si se esta editando => ya existe la imagen y la borra ( si es q NO es null )
                    if(obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // crea o lo vuelve a crear si se esta editando xq arriba se borro
                    using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                } else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }

                _unitOfWork.Save();
                TempData["success"] = "Product created successfully!";

                return RedirectToAction("Index");
            }
            
            return View(obj);
            
        }

    //////////////////////////////

    #region API CALLS
    // se llama en product.js y carga la info en la tabla
    [HttpGet]
    public IActionResult GetAll()
        {
            //cuidado xq ponia con espacio despues de la coma y no funcionaba
            // ... .GetAll(includeProperties:"Category, CoverType");
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new {data = productList});
        }

    // DELETE POST
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        var oldImagePath = Path.Combine(_hostEnviroment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
    #endregion
    }
}
