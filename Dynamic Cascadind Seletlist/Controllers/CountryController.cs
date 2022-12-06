using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Cascadind_Seletlist.Controllers
{
    public class CountryController : Controller
    {
        private readonly AppDbCounext _counext;

        public CountryController(AppDbCounext Counext)
        {
            _counext = Counext;
        }
        public IActionResult Index()
        {
            List<Country> countries;
            countries = _counext.Countries.ToList();
            return View(countries);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Country country = new Country();
            return View(country);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Country country)
        {
            _counext.Add(country);
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult CreateModalForm()
        {
            Country country = new Country();
            return PartialView("_CreateModalForm", country);
        }
        [HttpPost]
        public IActionResult CreateModalForm(Country country)
        {
            _counext.Add(country);
            _counext.SaveChanges();
            return NoContent();
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            Country country = GetCountry(id);
            return View(country);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Country country = GetCountry(Id);
            return View(country);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Country country)
        {
            _counext.Attach(country);
            _counext.Entry(country).State= EntityState.Modified;
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        private Country GetCountry(int id)
        {
            Country country;
            country = _counext.Countries.Where(c=>c.Id == id).FirstOrDefault();
            return country;
        }
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Country country = GetCountry(Id);
            return View(country);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Country country)
        {
            try
            {
                _counext.Attach(country);
                _counext.Entry(country).State = EntityState.Deleted;
                _counext.SaveChanges();
            }
            catch (Exception ex)
            {
                _counext.Entry(country).Reload();
                ModelState.AddModelError("",ex.InnerException.Message);
                return View(country);
            }
           
            return RedirectToAction(nameof(Index));
        }
        public JsonResult GetCountries()
        {
            var lstCountries = new List<SelectListItem>();
            List<Country> countries = _counext.Countries.ToList();
            lstCountries = countries.Select(ct=>new SelectListItem()
            {
                Value=ct.Id.ToString(),
                Text =ct.Name
            }).ToList();
            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Select Country----"
            };
            lstCountries.Insert(0, defItem);
            return Json(lstCountries);
        }
    }
}
