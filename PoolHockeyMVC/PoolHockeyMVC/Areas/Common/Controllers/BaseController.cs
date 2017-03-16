using System.Web.Mvc;
using PoolHockeyBLL.Contracts;

namespace PoolHockeyMVC.Areas.Common.Controllers
{
    public class BaseController : Controller
    {
        private IConfigServices _configServices;

        internal BaseController()
        {
            //_configServices = new ConfigServices();
            //SetLayoutViewBags();
        }

        public BaseController(IConfigServices configServices)
        {
            _configServices = configServices;
            SetLayoutViewBags();
        }

        private void SetLayoutViewBags()
        {
            ViewBag.LastUpdate = _configServices.GetLastUpdate().ToString("F");
            //TempData.Keep("LastUpdate");
        }
    }
}