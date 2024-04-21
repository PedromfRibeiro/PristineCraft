using System.ComponentModel;

namespace PristineCraft.Application.Common.Resources;

[AttributeUsage(AttributeTargets.Property)]
public class ResourceAttribute(string name) : DisplayNameAttribute(new ResourceManager().Get(name));