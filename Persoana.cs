using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app_hr
{
    class Persoana
    {
        private int id;
        private string nume;
        private double salariu;
        private string departament;
        private Job job;

        public Persoana() { }

        public Persoana(int id, string nume, double salariu, string departament, Job job)
        {
            this.Id = id;
            this.nume = nume;
            this.salariu = salariu;
            this.departament = departament;
            this.Job = job;
        }

        public string Nume { get => nume; set => nume = value; }
        public double Salariu { get => salariu; set => salariu = value; }
        public string Departament { get => departament; set => departament = value; }
        public int Id { get => id; set => id = value; }
        internal Job Job { get => job; set => job = value; }
    }
}
