using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModCore.Models.Objects
{
    public enum PropertyType
    {
        String = 1,
        ReferenceList = 2, //Links to a list of ObjectDefinitions
        Boolean = 3,
        Reference = 4, //Links to another ObjectDefinition
        Double = 5,
        Int = 6,
        Date = 7,
        Contact = 8,
        
    }
}
