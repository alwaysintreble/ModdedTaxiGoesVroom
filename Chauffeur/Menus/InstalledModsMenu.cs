using System.Linq;
using BepInEx.Bootstrap;
using Chauffeur.Utils.MenuUtils;

namespace Chauffeur.Menus;

public class InstalledModsMenu : CustomMenu
{
    public InstalledModsMenu() : base("Installed Mods")
    {
        var pluginNames = Chainloader.PluginInfos.Values.Select(x => x.Metadata.Name)
            .Where(plugin => plugin is not null);
        foreach (var pluginName in pluginNames)
        {
            AddButton(new MenuButton(pluginName, () => { }));
        }
    }
}