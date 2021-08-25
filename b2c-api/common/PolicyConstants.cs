using System;
using System.Collections.Generic;
using System.Text;

namespace common
{
    public static class PolicyConstants
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string Manager = "Manager";

        internal static IReadOnlyList<string> Policies { get; } = new List<string> { Admin, Manager, User };
    }
}
