using System;

namespace Interface_Segregation
{
    // Interfaces should be simple so that when a class uses an interface it will need all the functions that are
    // declared in the interface
    public class Document
    {

    }

    // This is an example of not a good interface - pay attention to the old printer class (will not need all the functions)
    public interface IMachine
    {
        void Print(Document d);
        void Scan(Document d);
        void Fax(Document d);
    }

    public class MultiPrinter : IMachine
    {
        public void Print(Document d)
        {
            //
        }

        public void Scan(Document d)
        {
            //
        }

        public void Fax(Document d)
        {
            //
        }
    }

    // See the problem in the IMachine
    public class OldFashionPrinter : IMachine
    {
        public void Print(Document d)
        {
            // the only thing it can do
        }

        public void Scan(Document d)
        {
            // what we can do?
        }

        public void Fax(Document d)
        {
            // what we can do?
        }
    }

    // What we will do:
    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }

    public interface IFax
    {
        void Fax(Document d);
    }

    // Now let the class take the interfaces they can work with and if many classes need them so:
    public interface  IMultiDevice : IScanner, IPrinter {} //...
    // And then create a class that takes this mega interface
    public class MultiMachine : IMultiDevice
    {
        private IScanner scanner;
        private IPrinter printer;

        public MultiMachine(IScanner scanner, IPrinter printer)
        {
            this.scanner = scanner;
            this.printer = printer;
        }

        // now we can delegate the calls to each of the interfaces methods
        // its a decorator 
        public void Print(Document d)
        {
            printer.Print(d);
        }

        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
    }

    class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
