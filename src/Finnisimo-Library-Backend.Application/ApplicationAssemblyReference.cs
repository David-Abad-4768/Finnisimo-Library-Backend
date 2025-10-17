using System.Reflection;

namespace Finnisimo_Library_Backend.Application;

public static class ApplicationAssemblyReference
{
  public static readonly Assembly Assembly =
      typeof(ApplicationAssemblyReference).Assembly;
}
