using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Helper.HashGenerator
{
    public interface IHashGenerator
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string correctHash);
    }
}
