// --------------------------------------------------------------------
// TestData.cs Copyright 2019 Craig Gjeltema
// --------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    internal class TestData
    {
        public const string TestConfig =
@"V 1.0

ACCESSMODIFIER_SORT_ORDER
public
internal
protected
protectedinternal
private
privateprotected
ACCESSMODIFIER_SORT_ORDER_END

KIND_SORT_ORDER
enum
delegate
eventField
field
event
constructor
destructor
indexer
property
operator
method
struct
interface
class
KIND_SORT_ORDER_END

HEADER
// -------------------------------------------
// Test Header for {filename} Copyright {year}
// -------------------------------------------
HEADER_END

MODIFIER_SORT_ORDER
kind
extern
const
readonly
static
accessibility
name
numberofmethodargs
MODIFIER_SORT_ORDER_END

LINE_BREAK_LENGTH
100
LINE_BREAK_LENGTH_END
";
        public const string ClassWithAttributes = @"namespace TestDummy
{

    using System;



    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }


    }
}";
        public const string ClassWithAttributesAfterNewline = @"

namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }
}";
        public const string InterfaceBeforeSorting = @"namespace TestDummy
{

    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program : IProgram
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }

    /// <summary>
    /// This is the interface that declares some useful functionality.
    /// </summary>
    internal interface IProgram
    {
        string ZZShouldBeAtTheEnd(string moreInput);
        int GetSomeData(int input);
        double AABeginning(int data, string moarData);

        void LaterFunctionButNotLast();
    }
}";
        public const string InterfaceAfterSorting = @"namespace TestDummy
{

    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program : IProgram
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }

    /// <summary>
    /// This is the interface that declares some useful functionality.
    /// </summary>
    internal interface IProgram
    {
        double AABeginning(int data, string moarData);
        int GetSomeData(int input);

        void LaterFunctionButNotLast();
        string ZZShouldBeAtTheEnd(string moreInput);
    }
}";
        public const string InterfaceAfterSortingAndFormattingNewlines = @"

namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program : IProgram
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }

    /// <summary>
    /// This is the interface that declares some useful functionality.
    /// </summary>
    internal interface IProgram
    {
        double AABeginning(int data, string moarData);

        int GetSomeData(int input);

        void LaterFunctionButNotLast();

        string ZZShouldBeAtTheEnd(string moreInput);
    }
}";
        public const string ClassWithLongLines = @"namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    public class Program<T> : IComparable<string>, IEnumerable<string>, IList<string>, IEquatable<string>, ICollection<string> where T : string
    {
        private const string LongString = ""abcdefghad;jfkaoieanvi;aifidkfajdi;avidfj;ifjdinvaivdjaidfjadfdjfadksfajkldsfjafd;ijaidfjads;ifadi;ndijvadivjdijfd;iafjdiidvadi"";
        private int numberOfSomething; // End of line comment

        public Program(string one, string two, string three, string four, string five, string six)
        {
            var testList = new List<string>();
            var result = testList.Where(x => x.Length > 5).Select(x => Enumerable.Range(0, x.Length).Sum()).ToList().Where(x => x < 250).Select(x => new string('A', x)).
Count();
        }

        public void SomeWork(int someArg)
        {
            var anotherList = new List<string>
            {  // Doesnt count as block
                ""Test1"",
                ""Test2""
            };

            var newProgram = new Program(""This is a new string for initialization."", ""This is another string for initialization."", ""This is a third string, how nice!"", ""Fourth string for init."", ""Fifth string, because it's cool"", ""Sixth string to round things out."");

            int temp = 0;

            int result = anotherList.Where(x => x.Length > 10).Select(x => Enumerable.Range(0, x.Length).Sum()).ToList().Where(x => x < 150).Select(x => new string('B', x)).Count();
            if(anotherList.Count > 0) 
                return; // Doesnt count as block
            else
            {
                Console.WriteLine(""Successful"");
            }

            temp = anotherList.Where(x => x.Length > 3).Select(x => Enumerable.Range(0, x.Length).Sum()).ToList().Where(x => x < 100).Select(x => new string('C', x)).Count();
        }

        public double MoreWork() => 3.1415926535897932384626433232389498328439480134730137876571804783014738753798367768147384138473775667174834018375763784380714387563718371834347;
    }
}";
        public const string ClassWithLongLinesAfterFormat = @"namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    public class Program<T> : IComparable<string>, IEnumerable<string>, IList<string>, IEquatable<string>, ICollection<string> where T : string
    {
        private const string LongString =
            ""abcdefghad;jfkaoieanvi;aifidkfajdi;avidfj;ifjdinvaivdjaidfjadfdjfadksfajkldsfjafd;ijaidfjads;ifadi;ndijvadivjdijfd;iafjdiidvadi"";
        private int numberOfSomething; // End of line comment

        public Program(string one, string two, string three, string four, string five, string six)
        {
            var testList = new List<string>();
            var result = testList
                .Where(x => x.Length > 5)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 250)
                .Select(x => new string('A', x))
                .Count();
        }

        public void SomeWork(int someArg)
        {
            var anotherList = new List<string>
            {  // Doesnt count as block
                ""Test1"",
                ""Test2""
            };

            var newProgram = new Program(
                ""This is a new string for initialization."", 
                ""This is another string for initialization."", 
                ""This is a third string, how nice!"", 
                ""Fourth string for init."", 
                ""Fifth string, because it's cool"", 
                ""Sixth string to round things out."");

            int temp = 0;

            int result = anotherList
                .Where(x => x.Length > 10)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 150)
                .Select(x => new string('B', x))
                .Count();
            if(anotherList.Count > 0) 
                return; // Doesnt count as block
            else
            {
                Console.WriteLine(""Successful"");
            }

            temp = anotherList
                .Where(x => x.Length > 3)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 100)
                .Select(x => new string('C', x))
                .Count();
        }

        public double MoreWork() => 3.1415926535897932384626433232389498328439480134730137876571804783014738753798367768147384138473775667174834018375763784380714387563718371834347;
    }
}";

        public const string MethodArgumentsBeforeSorting = @"namespace TestDummy
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    public class Program<T> : IComparable<string>, IEnumerable<string>, IList<string>, IEquatable<string>, ICollection<string> where T : string
    {
        private const string LongString = ""abcdefghad;jfkaoieanvi;aifidkfajdi;avidfj;if"";
        private int numberOfSomething; // End of line comment

        public void TestMethod(int first, IEnumerable<BitConverter> second) {}

        public void TestMethod(int first, int[] second) {}

        public void TestMethod(int first, Exception second) {}

        public void TestMethod(int first, IEnumerable<int> second) {}

        public void TestMethod(int first, out Exception second) {}

        public void TestMethod(int first, ref int second) {}

        public void TestMethod(int first, bool second) {}

        public void TestMethod(int first, [CallerMemberName] string second = """") {}

        public void TestMethod(int first, double second) {}

        public void TestMethod(
            int first, 
            Exception firstAndAHalf, 
            List< int > second, 
            List<string> third, 
            List<Exception> fourth, 
            int[] fifth, 
            Exception[] sixth, 
            bool seventh,
            bool? eighth,
            double ninth,
            Tuple<int, string> tenth,
            string eleventh,
            [CallerMemberName] string twelfth = """",
            out string thirteenth,
            ref float fourteenth,
            Int32 fifteenth,
            String sixteenth,
            BitConverter seventeenth,
            Func<int, bool> predicate,
            params int[] last)
        {
            var testList = new List<string>();
            var result = testList
                .Where(x => x.Length > 5)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 250)
                .Select(x => new string('A', x))
                .Count();
        }

        public void TestMethod(
            int first, 
            Exception firstAndAHalf, 
            List< int > second, 
            List<string> third, 
            List<Exception> fourth, 
            int[] fifth, 
            Exception[] sixth, 
            bool seventh,
            bool? eighth,
            double ninth,
            Tuple<int, string> tenth,
            string eleventh,
            [CallerMemberName] string twelfth = """",
            out string thirteenth,
            ref float fourteenth,
            Int32 fifteenth,
            String sixteenth,
            BitConverter seventeenth,
            Func<int, bool> predicate,
            List<string> different)
        {
            var testList = new List<string>();
            var result = testList
                .Where(x => x.Length > 5)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 250)
                .Select(x => new string('A', x))
                .Count();
        }";


        public const string MethodArgumentsAfterSorting = @"

namespace TestDummy
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    public class Program<T> : IComparable<string>, IEnumerable<string>, IList<string>, IEquatable<string>, ICollection<string> where T : string
    {
        private const string LongString = ""abcdefghad;jfkaoieanvi;aifidkfajdi;avidfj;if"";
        private int numberOfSomething; // End of line comment

        public void TestMethod(int first, [CallerMemberName] string second = """") {}

        public void TestMethod(int first, bool second) {}

        public void TestMethod(int first, double second) {}

        public void TestMethod(int first, ref int second) {}

        public void TestMethod(int first, Exception second) {}

        public void TestMethod(int first, IEnumerable<int> second) {}

        public void TestMethod(int first, IEnumerable<BitConverter> second) {}

        public void TestMethod(int first, int[] second) {}

        public void TestMethod(int first, out Exception second) {}

        public void TestMethod(
            int first, 
            Exception firstAndAHalf, 
            List< int > second, 
            List<string> third, 
            List<Exception> fourth, 
            int[] fifth, 
            Exception[] sixth, 
            bool seventh,
            bool? eighth,
            double ninth,
            Tuple<int, string> tenth,
            string eleventh,
            [CallerMemberName] string twelfth = """",
            out string thirteenth,
            ref float fourteenth,
            Int32 fifteenth,
            String sixteenth,
            BitConverter seventeenth,
            Func<int, bool> predicate,
            List<string> different)
        {
            var testList = new List<string>();
            var result = testList
                .Where(x => x.Length > 5)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 250)
                .Select(x => new string('A', x))
                .Count();
        }

        public void TestMethod(
            int first, 
            Exception firstAndAHalf, 
            List< int > second, 
            List<string> third, 
            List<Exception> fourth, 
            int[] fifth, 
            Exception[] sixth, 
            bool seventh,
            bool? eighth,
            double ninth,
            Tuple<int, string> tenth,
            string eleventh,
            [CallerMemberName] string twelfth = """",
            out string thirteenth,
            ref float fourteenth,
            Int32 fifteenth,
            String sixteenth,
            BitConverter seventeenth,
            Func<int, bool> predicate,
            params int[] last)
        {
            var testList = new List<string>();
            var result = testList
                .Where(x => x.Length > 5)
                .Select(x => Enumerable.Range(0, x.Length).Sum())
                .ToList()
                .Where(x => x < 250)
                .Select(x => new string('A', x))
                .Count();
        }
";

        public const string WhitespaceLeadingNamespaceWithOuterUsings = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2020
// -------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp;





namespace TestDummy
{



    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    class Program
    {




        private int numberOfSomething; // End of line comment


        public Program()
        {
        }



    }


}";

        public const string FormattedWhitespaceLeadingNamespaceWithOuterUsings = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2020
// -------------------------------------------

using System;
using Microsoft.CodeAnalysis.CSharp;

namespace TestDummy
{
    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    class Program
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }
}";

        public const string WhitespaceLeadingNamespace = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2020
// -------------------------------------------







namespace TestDummy
{



    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    class Program
    {




        private int numberOfSomething; // End of line comment


        public Program()
        {
        }



    }


}";

        public const string FormattedWhitespaceLeadingNamespace = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2020
// -------------------------------------------

namespace TestDummy
{
    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    class Program
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }
}";
    }
}
