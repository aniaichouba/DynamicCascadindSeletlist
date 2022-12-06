using Microsoft.AspNetCore.Mvc;

namespace Dynamic_Cascadind_Seletlist.Controllers
{
    public class CityController : Controller
    {
        private readonly AppDbCounext _counext;

        public CityController(AppDbCounext Counext)
        {
            _counext = Counext;
        }
        public IActionResult Index()
        {
            List<City> Cities;
            Cities = _counext.Cities.ToList();
            return View(Cities);
        }
        [HttpGet]
        public IActionResult Create()
        {
            City City = new City();
            ViewBag.Countries = GetCountries();
            return View(City);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(City city)
        {
            _counext.Add(city);
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int Id)
        {
            City city = _counext.Cities.Where(c => c.Id == Id).FirstOrDefault();
            return View(city);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            City city = _counext.Cities.Where(c => c.Id == Id).FirstOrDefault();
            return View(city);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(City city)
        {
            _counext.Attach(city);
            _counext.Entry(city).State = EntityState.Modified;
            _counext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public IActionResult Delete(int Id)
        {
            City city = _counext.Cities.Where(c => c.Id == Id).FirstOrDefault();
            return View(city);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(City city)
        {
            _counext.Attach(city);
            _counext.Entry(city).State = EntityState.Deleted;
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
        private Country GetCountry(int id)
        {
            Country country;
            country = _counext.Countries.Where(c => c.Id == id).FirstOrDefault();
            return country;
        }
        [HttpGet]
        public IActionResult CreateModalForm(int countyId)
        {
            City city = new City();
            city.CountryId = countyId;
            city.CountryName = GetCountryName(countyId);
            return PartialView("_CreateModalForm", city);
        }
        [HttpPost]
        public IActionResult CreateModalForm(City city)
        {
            _counext.Add(city);
            _counext.SaveChanges();
            return NoContent();
        }
        private string GetCountryName(int countyId)
        {
            if (countyId == 0)
                return "";
            string strCoutryName = _counext.Countries
                .Where(ct=> ct.Id==countyId)
                .Select(nm=>nm.Name).Single().ToLower();
            return strCoutryName;
        }
    }
}
