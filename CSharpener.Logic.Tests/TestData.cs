﻿// -----------------------------------------------------------------------
// TestData.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    internal class TestData
    {
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

        public Program()
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

            int someUselessValue = GetValue(1234567890, 1234567890123456789L, 123456789.1234567, 9876543210);

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

        public int GetValue(int initial, long final, double interim, int test)
        {
             return 3;
        }
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

        public Program()
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

            int someUselessValue = GetValue(
                1234567890, 
                1234567890123456789L, 
                123456789.1234567, 
                9876543210);

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

        public int GetValue(int initial, long final, double interim, int test)
        {
             return 3;
        }
    }
}";
        public const string ClassWithRegions = @"

namespace TestDummy
{#region
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
#endregion
    class Program : IProgram
    {
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
    }
             #region interface
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
#endregion}
";
        public const string ClassWithRegionsRemoved = @"

namespace TestDummy
{    using System;

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
";
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
implicitexplicit
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
        public const string UsingInAndOutAfterUsingsPlacer = @"

namespace TestDummy
{
    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;
using Microsoft.CodeAnalysis.CSharp;
// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Collections.Immutable;
using System.Collections.ObjectModel;


    class Program : SecondClass
    {

    }
}
";
        public const string UsingsInAndOutsideNamespace = @"

using Microsoft.CodeAnalysis.CSharp;
// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Collections.Immutable;
using System.Collections.ObjectModel;
namespace TestDummy
{
    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;

    class Program : SecondClass
    {

    }
}
";
        public const string UsingsInAndOutsideNamespaceAfterFileHeader = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2021
// -------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Collections.Immutable;
using System.Collections.ObjectModel;
namespace TestDummy
{
    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;

    class Program : SecondClass
    {

    }
}
";
        public const string UsingsInAndOutsideNamespaceAfterNewline = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2021
// -------------------------------------------

namespace TestDummy
{
    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;
using Microsoft.CodeAnalysis.CSharp;
// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Collections.Immutable;
using System.Collections.ObjectModel;

    class Program : SecondClass
    {

    }
}
";
        public const string UsingsInAndOutsideNamespaceWithCorrectFileHeader = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2021
// -------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Collections.Immutable;
using System.Collections.ObjectModel;
namespace TestDummy
{
    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;

    class Program : SecondClass
    {

    }
}
";
        public const string UsingsOnlyOutAfterUsingsPlacer = @"// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------

// Documentation for TestDummy namespace

namespace TestDummy
{
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using TestDummy.ExtraNS;
using System.Linq;
using static System.Math;
using BNS;
// Third comment
using System.Text;
using another = TestDummy.ExtraNS;
using System.Threading.Tasks;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;



    class Program : SecondClass
    {
    }
}";
        public const string UsingsOnlyOutsideNamespace = @"// -------------------------------------------
// Test Copyright by Craig Gjeltema - Wrong
// -------------------------------------------
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;

    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
	
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;
	
using System.Collections.Immutable;
using System.Collections.ObjectModel;

// Documentation for TestDummy namespace

namespace TestDummy
{


    class Program : SecondClass
    {
    }
}";
        public const string UsingsOnlyOutsideNamespaceAfterFileHeader = @"// -------------------------------------------
// Test Header for AnotherTestFile.cs Copyright 2021
// -------------------------------------------

using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;

    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
	
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;
	
using System.Collections.Immutable;
using System.Collections.ObjectModel;

// Documentation for TestDummy namespace

namespace TestDummy
{


    class Program : SecondClass
    {
    }
}";
        public const string UsingsOnlyOutsideNamespaceAfterNewline = @"// -------------------------------------------
// Test Header for AnotherTestFile.cs Copyright 2021
// -------------------------------------------

namespace TestDummy
{
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using TestDummy.ExtraNS;
using System.Linq;
using static System.Math;
using BNS;
// Third comment
using System.Text;
using another = TestDummy.ExtraNS;
using System.Threading.Tasks;
using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

    class Program : SecondClass
    {
    }
}";
        public const string UsingsOnlyOutsideNamespaceWithCorrectFileHeader = @"// -------------------------------------------
// Test Header for AnotherTestFile.cs Copyright 2021
// -------------------------------------------

using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis.CSharp;

    using System.Collections.Generic;
    using TestDummy.ExtraNS;
    using System.Linq;
	
    using static System.Math;
    using BNS;
    // Third comment
    using System.Text;
    using another = TestDummy.ExtraNS;
    using System.Threading.Tasks;
    using System;
	
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace TestDummy
{


    class Program : SecondClass
    {
    }
}";
    }
}
