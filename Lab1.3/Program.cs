using System;
using System.Collections.Generic;


namespace RefactoringGuru.DesignPatterns.Builder.Conceptual
{
    // Interfața Builder declară metode de constructor pentru diferitele părți
    //a  obiectelor  Produsului.
    public interface IBuilder
    {
        void BuildPartA();

        void BuildPartB();

        void BuildPartC();
    }
    // Clasele Concrete Builder urmează interfața Builder și oferă
    // implementări concrete ale etapelor de construcție. Programul dvs. poate avea
    // mai multe variante de constructori, implementate în moduri diferite.
    public class ConcreteBuilder : IBuilder
    {
        private Product _product = new Product();

        // Noua instanță a constructorului trebuie să conțină un obiect de produs gol,
        // care este folosit la asamblarea ulterioară.
        public ConcreteBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._product = new Product();
        }

        // Toate etapele de producție funcționează cu aceeași instanță
        // produs.
        public void BuildPartA()
        {
            this._product.Add("PartA1");
        }

        public void BuildPartB()
        {
            this._product.Add("PartB1");
        }

        public void BuildPartC()
        {
            this._product.Add("PartC1");
        }

        // Constructorii Builder trebuie să furnizeze propriile metode
        // pentru obținerea rezultatelor. Acest lucru se datorează diferitelor tipuri,
        // constructorii pot crea produse complet diferite cu diferite
        // interfețe. Prin urmare, astfel de metode nu pot fi declarate în bază
        // Interfață Builder (cel puțin tastată static
        // limbaj de programare).
        //
        // De regulă, după returnarea rezultatului final clientului,
        // instanța constructor ar trebui să fie gata să înceapă producția
        // următorul produs. Prin urmare, este o practică obișnuită să se apeleze metoda
        // reset la sfârșitul corpului metodei GetProduct. Cu toate acestea, acest comportament nu este
        // este necesar, puteți face constructorii să aștepte
        // solicitați în mod explicit o resetare de la codul clientului înainte de a scăpa de
        // rezultatul anterior.
        public Product GetProduct()
        {
            Product result = this._product;

            this.Reset();

            return result;
        }
    }

    // Este logic să folosim modelul Builder numai atunci când
    // produsele sunt destul de complexe și necesită o configurație extinsă.
    //
    // Spre deosebire de alte tipare parentale, diferiți constructori 
    // poate produce produse fără legătură. Cu alte cuvinte, rezultatele
    // diferiți constructori pot să nu urmeze întotdeauna aceeasi
    // interfață.
    public class Product
    {
        private List<object> _parts = new List<object>();

        public void Add(string part)
        {
            this._parts.Add(part);
        }

        public string ListParts()
        {
            string str = string.Empty;

            for (int i = 0; i < this._parts.Count; i++)
            {
                str += this._parts[i] + ", ";
            }

            str = str.Remove(str.Length - 2); // removing last ",c"

            return "Product parts: " + str + "\n";
        }
    }

    // Directorul este responsabil doar de executarea etapelor de construire într-o anumita
    // secvența. Acest lucru este util atunci când se produc produse într-o anumita
    // comandă sau configurație specială. Strict vorbind, clasa Director
    // opțional, precum clientul poate manipula direct constructorii.
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

        // Director poate crea mai multe variante de produse folosind
        // aceiași pași de construcție.
        public void buildMinimalViableProduct()
        {
            this._builder.BuildPartA();
        }

        public void buildFullFeaturedProduct()
        {
            this._builder.BuildPartA();
            this._builder.BuildPartB();
            this._builder.BuildPartC();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Codul client creează un obiect constructor, îl transmite directorului,
            // și apoi inițiază procesul de construire. Rezultat final
            // preluat din obiectul constructor.
            var director = new Director();
            var builder = new ConcreteBuilder();
            director.Builder = builder;

            Console.WriteLine("Standard basic product:");
            director.buildMinimalViableProduct();
            Console.WriteLine(builder.GetProduct().ListParts());

            Console.WriteLine("Standard full featured product:");
            director.buildFullFeaturedProduct();
            Console.WriteLine(builder.GetProduct().ListParts());

            // Dupa cum stim că modelul Builder poate fi utilizat fără o clasă
            // Director.
            Console.WriteLine("Custom product:");
            builder.BuildPartA();
            builder.BuildPartC();
            Console.Write(builder.GetProduct().ListParts());
        }
    }
}

