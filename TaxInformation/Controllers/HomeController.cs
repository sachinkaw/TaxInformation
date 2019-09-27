using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using TaxInformation.Models;

namespace TaxInformation.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            OrgCustomize orgList = new OrgCustomize();
            orgList.OrgIDDropDownProp = GetOrgsForDropDown();
            return View(orgList);
        }

        public List<OrgIDDropDown> GetOrgsForDropDown()
        {
            DBEntities context = new DBEntities();
            List<OrgIDDropDown> result = new List<OrgIDDropDown>();

            //var obj = context.OrgCustomizes.Select(u => u).ToList();
            /*
            var obj = context.Orgs
                .Join(context.OrgCustomizes,
                org => org.Id,
                orgCu => orgCu.OrgId,
                (org, orgCu) => new {Post = org, Meta = orgCu });
                */

            var obj = context.Orgs
                .Join(context.OrgCustomizes, r => r.Id, p => p.OrgId, (r, p) => new { Orgs = r, OrgCustomize = p });

            if (obj != null)
            {
                foreach (var data in obj)
                {
                    OrgIDDropDown orgList = new OrgIDDropDown();
                    orgList.OrgName = data.Orgs.Name;
                    result.Add(orgList);
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult UpdateDB(string OrganisationName, string tname, string trate)
        {
            DBEntities context = new DBEntities();

            var query = context.OrgCustomizes
                .Join(context.Orgs, r => r.OrgId, p => p.Id, (r, p) => new { Orgs = p, OrgCustomize = r });

            if (query != null)
            {
                foreach (var data in query)
                {
                    if (string.Compare(data.Orgs.Name, OrganisationName) == 0)
                    {
                        data.OrgCustomize.TaxName = tname;
                        data.OrgCustomize.TaxRate = decimal.Parse(trate);
                    }
                }
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}