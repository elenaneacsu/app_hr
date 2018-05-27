using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_hr
{
    class Departament
    {
        private int id;
        private string denumire;

        public Departament() { }

        public Departament(int id, string denumire)
        {
            this.id = id;
            this.denumire = denumire;
        }

        public int Id { get => id; set => id = value; }
        public string Denumire { get => denumire; set => denumire = value; }
    }
}
