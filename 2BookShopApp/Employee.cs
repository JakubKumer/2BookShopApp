using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _2BookShopApp
{
    public enum Type
    {
        Pracownik,
        Kierownik
    }
    internal class Employee : User
    {
        public Type type;
    }
}
