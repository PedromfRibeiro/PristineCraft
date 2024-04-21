using System.Resources;
using System.Reflection;
using System.Globalization;

namespace PristineCraft.Application.Common.Resources;

public class ResourceManager
{
    private readonly System.Resources.ResourceManager _resourcemanager;

    public ResourceManager()
    {
        _resourcemanager = new System.Resources.ResourceManager("PristineCraft.Application.Common.Resources", Assembly.GetExecutingAssembly());
    }

    public string Get(string id)
    {
        return _resourcemanager.GetString(id) ?? id;
    }

    public void SetIdiom(string idiom)
    {
        CultureInfo info = new(idiom);

        CultureInfo.CurrentCulture = info;
        CultureInfo.CurrentUICulture = info;
    }
}