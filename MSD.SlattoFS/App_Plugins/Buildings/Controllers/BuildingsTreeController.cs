using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using umbraco;
using umbraco.BusinessLogic.Actions;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace MSD.SlattoFS.App_Plugins.Buildings.Controllers
{
    [Tree("buildings", "BuildingsTree", "Buildings")]
    [PluginController("Buildings")]
    public class BuildingsTreeController : TreeController
    {

        protected override Umbraco.Web.Models.Trees.TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            //check if we’re rendering the root node’s children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var ctrl = new BuildingApiController();
                var nodes = new TreeNodeCollection();

                //foreach (var building in ctrl.GetAll())
                //{
                //    var node = CreateTreeNode(
                //        building.Id.ToString(),
                //        "-1",
                //        queryStrings,
                //        building.Name,
                //        "icon-untitled",
                //        false);

                //    nodes.Add(node);

                //}
                return nodes;
            }

            //this tree doesn’t suport rendering more than 1 level
            throw new NotSupportedException();
        }

        protected override Umbraco.Web.Models.Trees.MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                // root actions              
                menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
            }
            else
            {
                menu.Items.Add<ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias));
            }
            return menu;
        }
    }


}