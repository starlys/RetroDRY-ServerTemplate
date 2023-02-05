using RetroDRY;

//TODO: Globally rename your project from MyServer to something else; likewise globally replace all the namespaces in all cs files.
//And note that when you rename the .csproj file, you will need to open the .sln file in Notepad to make the name change there as well.
namespace MyServer
{
    public static class Globals
    {
        public static Retroverse? Retroverse { get; set; }
    }
}
