using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DLL.Premetives
{
    public abstract class Entity
    {
        protected Entity(string id) => Id = id;

        public string Id { get; set;}
      
    }
}
