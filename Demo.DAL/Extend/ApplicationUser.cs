using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Extend
{
    public class ApplicationUser : IdentityUser //de feha kol byanat el user tlka2y ana bs hzwd el IsAgree w kman azwd ay 7aga ana 3ayzha
    { 
        public bool IsAgree { get; set; }
    }
}
