using Dynamic_Cascadind_Seletlist.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Cascadind_Seletlist.Controllers
{
    public class CostomerController : Controller
    {
        private readonly AppDbCounext _counext;
        private readonly IWebHostEnvironment _webHost;

        public CostomerController(AppDbCounext Counext,IWebHostEnvironment webHost)
        {
            _counext = Counext;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            List<Customer> Cities;
            Cities = _counext.Customers.ToList();
            return View(Cities);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Customer customer = new Customer();
            ViewBag.Countries = GetCountries();
            return View(customer);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            string uniqueFileName = GetProfilePhotoFileName(customer);
            customer.PhotoUrl = uniqueFileName;

            _counext.Add(customer);
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int Id)
        {
            Customer customer = _counext.Customers
                .Include(cty => cty.City)
                .Include(cou => cou.City.Countries)
                .Where(c => c.Id == Id).FirstOrDefault();
            return View(customer);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Customer customer = _counext.Customers
                .Include(c0=>c0.City)
                .Where(c => c.Id == Id).FirstOrDefault();

            customer.CountryId =customer.City.CountryId;

            ViewBag.Countries = GetCountries();
            ViewBag.Cities = GetCities(customer.CountryId);

            return View(customer);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (customer.ProfilePhoto != null)
            {
                string uniqueFileName = GetProfilePhotoFileName(customer);
                customer.PhotoUrl = uniqueFileName;
            }
            _counext.Attach(customer);
            _counext.Entry(customer).State = EntityState.Modified;
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Customer customer = _counext.Customers.Where(c => c.Id == Id).FirstOrDefault();
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _counext.Attach(customer);
            _counext.Entry(customer).State = EntityState.Deleted;
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private List<SelectListItem> GetCountries()
        {
            var lstCountries = new List<SelectListItem>();
            List<Country> countries = _counext.Countries.ToList();
            lstCountries = countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Country----"
            };
            lstCountries.Insert(0, defItem);
            return lstCountries;
        }
       
        [HttpGet]
        public JsonResult  GetCitiesByCountry(int countryId)
        {
            List<SelectListItem> cities = _counext.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(n => n.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            return Json(cities);
        }
        private string GetProfilePhotoFileName(Customer customer)
        {
            string uniqueFileName = null;
            if (customer.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString()+ "_" + customer.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customer.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private List<SelectListItem> GetCities(int countryId)
        {
            List<SelectListItem> cities = _counext.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(n => n.Name)
                .Select(n =>
                new SelectListItem
                {
                    Value = n.Id.ToString(),
                    Text = n.Name,
                }).ToList();
            return cities;
        }
    }
}
