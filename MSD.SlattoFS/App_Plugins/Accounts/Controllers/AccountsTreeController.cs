using System.Net.Http.Formatting;
using umbraco;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using Umbraco.Core;

namespace MSD.SlattoFS.App_Plugins.Accounts.Controllers
{
    [PluginController("Accounts")]
    [Tree("accounts", "AccountsTree", "Accounts", iconClosed: "icon-folder", iconOpen: "icon-folder-open")]
    public class AccountsTreeController : TreeController
    {
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();

            if (id == Constants.System.Root.ToInvariantString())
            {
                // root actions              
                menu.Items.Add<CreateChildEntity, ActionNew>(ui.Text("actions", ActionNew.Instance.Alias));
                menu.Items.Add<RefreshNode, ActionRefresh>(ui.Text("actions", ActionRefresh.Instance.Alias), true);
                //return menu;
            }
            else
            {
                menu.Items.Add<ActionDelete>(ui.Text("actions", ActionDelete.Instance.Alias));
            }
            return menu;
        }

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var nodes = new TreeNodeCollection();

            //check if we’re rendering the root node’s children
            if (id == Constants.System.Root.ToInvariantString())
            {
                var accountApi = new AccountApiController();
                foreach (var account in accountApi.GetAll())
                {
                    var node = CreateTreeNode(
                    account.Id.ToString(),
                    "-1",
                    queryStrings,
                    account.Name,
                    "icon-user-glasses",
                    false);

                    nodes.Add(node);

                }
            }
            return nodes;
        }
    }
}