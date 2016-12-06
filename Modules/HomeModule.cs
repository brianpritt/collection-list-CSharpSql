using Nancy;
using System.Collections.Generic;
using Inventory.Objects;

namespace Inventory
{
	public class HomeModule : NancyModule
	{
		public HomeModule()
		{
			Get["/"] = _ =>
			{
				List<Collection> allCollections = Collection.GetAll();
				return View["index.cshtml", allCollections];
			};
			Post["/add"] = _ =>{
				Collection newCollection = new Collection (Request.Form["item"]);
				newCollection.Save();
				List<Collection> allCollections = Collection.GetAll();
				return View["index.cshtml", allCollections];
			};
		}
	}
}
