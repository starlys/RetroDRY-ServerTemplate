using RetroDRY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyServer
{
    //TODO:Modify this class to represent users, or derive some other class from IUser so that RetroDRY can
    //detemine what tables and columns the user is allowed to view and edit, via the user's collection of roles.
    public class AppUser : IUser
    {
        //TODO:remove these static members; they grant permission to everything
        public static RetroRole GenericRole = new RetroRole
        {
            BaseLevel = PermissionLevel.All
        };
        public static AppUser GenericUser = new AppUser();
            
        public string Id => "x";

        public RetroRole[] Roles => new[] { GenericRole };

        public string LangCode => "";
    }
}
