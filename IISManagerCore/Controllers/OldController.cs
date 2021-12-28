using IISManagerCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;

namespace IISManagerCore.Controllers
{
    public class OldController : Controller
    {
        //
        // GET: /Old/

        public ActionResult Index()
        {

            List<IIS> list_iis = new List<IIS>();
            var websites = new List<WebSite>();

            IISExpress _iisExpress = new IISExpress();
            try
            {
                string path = "IIsWebService://" + System.Environment.MachineName + "/W3SVC";
                System.Collections.ArrayList webSite = new System.Collections.ArrayList();

                ServerManager iisManager = new ServerManager();

                foreach (var site in iisManager.Sites)
                {
                    websites.Add(new WebSite()
                    {
                        Name = site.Name,
                        Identity = (int)site.Id,
                        PhysicalPath = site.Applications["/"].VirtualDirectories[0].PhysicalPath
                        //,Status = (ServerState)site.State
                    });
                }


                //DirectoryEntry iis = new DirectoryEntry("IIS://localhost/W3SVC", "acer","wu******");
                //if (iis != null)
                //{
                //    foreach (DirectoryEntry entry in iis.Children)
                //    {
                //        if (string.Compare(entry.SchemaClassName, "IIsWebServer") == 0)
                //        {
                //            IIS i = new IIS();
                //            i.Name = entry.Name;
                //            i.ServerComment = entry.Properties["ServerComment"].Value.ToString();
                //            i.Path = entry.Path;
                //            list_iis.Add(i);
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }



            return View(websites);
        }

    }

}
