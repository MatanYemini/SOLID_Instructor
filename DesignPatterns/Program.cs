using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DesignPatterns
{
    /// <summary>
    /// Checking the first principle of SOLID 
    /// </summary>
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private int count = 0;

        public int addEntry(string text)
        {
            entries.Add($"{++count}: {text}");
            return count; // Memento
        }

        public void removeEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, entries);
        }

        // * Exp #1
        // * This code section breaks the single responsibility principle  
        // * We are adding another responsibility - keeping the entry and also manage persistence
        // * What we should do is to create another class that will take care of persistence

        //public void save(string fileName)
        //{
        //    File.WriteAllText(fileName, ToString());
        //}

        //public static Journal Load(string fileName)
        //{

        //}
        //************
    }

    public class Persistence
    {
        public void SaveToFile (Journal j, string filename, bool overwrite = false)
        {
            if(overwrite || !File.Exists(filename))
                File.WriteAllText(filename, j.ToString());
        }
    }
    /// <summary>
    /// This Section refers to Open-Closed principle 
    /// </summary>

    public enum Color
    {
        Red, Green, Blue
    }

    public enum Size
    {
        Small, Medium, Big
    }

    public class Product
    {
        public string Name;
        public Color Color;
        public Size Size;

        public Product(string name, Color color, Size size)
        { 
            Name = name;
            Color = color;
            Size = size;
        }
    }

    public class ProductFilter
    {
        public static IEnumerable<Product> FilterBySize(IEnumerable<Product> products, Size size)
        {
            foreach (var p in products)
            {
                if (p.Size == size)
                    yield return p; // used to return Enumerators (a value set in loop body) stateful iteration
            }
        }
        public IEnumerable<Product> FilterByColor(IEnumerable<Product> products, Color color)
        {
            foreach (var p in products)
            {
                if (p.Color == color)
                    yield return p; // used to return Enumerators (a value set in loop body) stateful iteration
            }
        }
    }

    // Interfaces for Open-Closed
    public interface ISpecification<T>
    {
        bool IsSatisfied(T t);
    }
    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // Now use them:
    public class ColorSpecification : ISpecification<Product>
    {
        private Color color;

        public ColorSpecification(Color color)
        {
            this.color = color;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Color == color;
        }
    }

    public class SizeSpecification : ISpecification<Product>
    {
        private Size size;

        public SizeSpecification(Size size)
        {
            this.size = size;
        }
        public bool IsSatisfied(Product t)
        {
            return t.Size == size;
        }
    }

    public class AndSpecification<T> : ISpecification<T>
    {
        private ISpecification<T> first, second;

        public AndSpecification(ISpecification<T> first, ISpecification<T> second)
        {
            this.first = first;
            this.second = second;
        }

        public bool IsSatisfied(T t)
        {
            return first.IsSatisfied(t) && second.IsSatisfied(t);

        }



    }

    public class BetterFilter : IFilter<Product>
    {
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var i in items)
            {
                if (spec.IsSatisfied(i))
                    yield return i;
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Refers to first section
            var j = new Journal();
            j.addEntry("I Cried");
            j.addEntry("I know");
            Console.WriteLine(j);

            var p = new Persistence();
            var filename = @"d:\temp\journals\j1.txt";
            p.SaveToFile(j, filename, true);
            //Process.Start(filename);

            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Big);
            var house = new Product("House", Color.Blue, Size.Big);

            Product[] products = {apple, tree, house};

            var pf = new ProductFilter(); 
            Console.WriteLine("Green products (old):"); // Every time we are adding filter we will need to add another method in  the product filter class
            foreach (var prod in pf.FilterByColor(products,Color.Green))
            {
                Console.WriteLine($" - {prod.Name} is Green");
            }
            // Now we to filter by both. So this is a problem - we will need another function
            // Adding a function with filtering by both color and size.
            // The main concept is that you can always expand the class but without changing the things before
            // We can implement a pattern - specification pattern - we will add interfaces
            // Now using the better filter
            // The second iteration is for both size and color - adding another class AndSpecification for combining any both filters
            var bf = new BetterFilter();
            Console.WriteLine("Green Products (new)");
            foreach (var prod in bf.Filter(products, new ColorSpecification(Color.Green)))
            {
                Console.WriteLine($" - {prod.Name} is Green");
            }

            Console.WriteLine("Big Blue Houses");
            foreach (var prod in bf.Filter
            (products,
                new AndSpecification<Product>(new ColorSpecification(Color.Blue),
                    new SizeSpecification(Size.Big))))
            {
                Console.WriteLine($" - {prod.Name} is big and blue");
            }

        }
    }
}
