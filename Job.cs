using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_hr
{
    class Job
    {
        private int id;
        private string denumire;
        private bool tip;

        public Job() { }

        public Job(int id, string denumire, bool tip)
        {
            this.id = id;
            this.denumire = denumire;
            this.tip = tip;
        }

        public int Id { get => id; set => id = value; }
        public string Denumire { get => denumire; set => denumire = value; }
        public bool Tip { get => tip; set => tip = value; }
    }
}
