using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DatabaseFirstCar.Models.ViewModel;

namespace DatabaseFirstCar.Controllers
{
    public class CustomersController : Controller
    {
       
        protected readonly CustomerCarDBEntities2 _context = new CustomerCarDBEntities2();
        public ActionResult Index()
        {
            var customerList = _context.Customers.Include(q => q.Links.Select(s => s.Model))
                .OrderByDescending(e => e.CustomerId).ToList();
            return View(customerList);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var customer = await _context.Customers.Where(x => x.CustomerId == id).FirstOrDefaultAsync();
            if (customer == null)
            {
                return HttpNotFound("No employee found");
            }
            if (!string.IsNullOrEmpty(customer.Picture))
            {
                string imgPath = Path.Combine("Image", customer.Picture);
                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
            }
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult AddNewModel(int? id)
        {
            ViewBag.models = new SelectList(_context.Models.ToList(),
                "ModelId", "Model1", (id != null) ? id.ToString() : "");
            return PartialView("_AddNewModel");
        }
        [HttpPost]
        public ActionResult Create(CustomerViewModel vobj, int[] modelId)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    FirstName = vobj.FirstName,
                    LastName = vobj.LastName,
                    DateOfBirth = vobj.DateOfBirth,
                    Price = vobj.Price,
                    IsRegular = vobj.IsRegular,
                    MobileNo = vobj.MobileNo
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                   
                    string imagePath = Path.Combine("/images/", Guid.NewGuid().ToString())
                       + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(imagePath));
                    customer.Picture = imagePath;
                }
                foreach (int item in modelId)
                {
                    Link q = new Link
                    {
                        Customer = customer,
                        CustomerId = customer.CustomerId,
                        ModelId = item
                    };
                    _context.Links.Add(q);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            else
            { return View(); }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
           Customer customer = _context.Customers.First(x => x.CustomerId == id);
            var model = _context.Links.Where(x => x.CustomerId == id).ToList();
            CustomerViewModel vobj = new CustomerViewModel
            {
                CustomerId = customer.CustomerId,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DateOfBirth = customer.DateOfBirth,
                Price = customer.Price,
                IsRegular = customer.IsRegular,
                MobileNo = customer.MobileNo,
                Picture = customer.Picture,
            };
            if (model.Count > 0)
            {
                foreach (var item in model)
                {
                    vobj.ModelList.Add(item.ModelId);
                }
            }
            return View(vobj);

        }

        [HttpPost]
        public ActionResult Edit(CustomerViewModel vobj, int[] modelId)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer
                {
                    CustomerId = vobj.CustomerId,
                    FirstName = vobj.FirstName,
                    LastName = vobj.LastName,
                    Price = vobj.Price,
                    DateOfBirth = vobj.DateOfBirth,
                    IsRegular = vobj.IsRegular,
                    MobileNo = vobj.MobileNo,
                };
                HttpPostedFileBase file = vobj.PicturePath;
                if (file != null)
                {
                    string imagePath = Path.Combine("/images/", Guid.NewGuid().ToString())
                        + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath(imagePath));
                    customer.Picture = imagePath;
                }
                else
                    customer.Picture = vobj.Picture;
                var model = _context.Links
                    .Where(x => x.CustomerId == customer.CustomerId).ToList();
                foreach (var item in model)
                {
                    _context.Links.Remove(item);
                }
                foreach (var item in modelId)
                {
                    Link q = new Link
                    {
                        CustomerId = customer.CustomerId,
                        ModelId = item
                    };
                    _context.Links.Add(q);
                }
                _context.Entry(customer).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            { return View(); }
        }


    }
}
