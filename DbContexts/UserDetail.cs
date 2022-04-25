using System;
using System.Collections.Generic;

#nullable disable

namespace NitinChandraSecondTest.DbContexts
{
    public partial class UserDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string Password { get; set; }
    }
}
