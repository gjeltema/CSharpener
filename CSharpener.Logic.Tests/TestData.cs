// -----------------------------------------------------------------------
// TestData.cs Copyright 2021 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    internal static class TestData
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
        public const string ClassWithMultipleConstructors = @"namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    public class Program
    {

        private int numberOfSomething; // End of line comment
        public Program()
        {
            var testList = new List<string>();
        }
        public Program(int number)
        {
            var testList = new List<string>();
            numberOfSomething = number;
        }
        public Program(bool flag, int number)
        {
            var testList = flag ? new List<string>() : null;
            numberOfSomething = number;
        }
        public Program(IList<string> collection, int number)
        {
            var testList = collection;
            numberOfSomething = number;
        }


        public int GetValue(int initial, long final, double interim, int test)
        {
             return 3;
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

            if(anotherList.Count > 0) 
                return; // Doesnt count as block
            else
            {
                Console.WriteLine(""Successful"");
            }
        }
    }
}";
        public const string ClassWithMultipleConstructorsAfterSort = @"namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    public class Program
    {

        private int numberOfSomething; // End of line comment
        public Program()
        {
            var testList = new List<string>();
        }
        public Program(int number)
        {
            var testList = new List<string>();
            numberOfSomething = number;
        }
        public Program(bool flag, int number)
        {
            var testList = flag ? new List<string>() : null;
            numberOfSomething = number;
        }
        public Program(IList<string> collection, int number)
        {
            var testList = collection;
            numberOfSomething = number;
        }


        public int GetValue(int initial, long final, double interim, int test)
        {
             return 3;
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

            if(anotherList.Count > 0) 
                return; // Doesnt count as block
            else
            {
                Console.WriteLine(""Successful"");
            }
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
        public const string ExpressionBodiedMethod = @"
namespace TestDummy
{
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        private IList<int> ints = new List<int>();

        public int MyProperty1 => 3;

        public int MyProperty2 => 
            3;

        public int MyProperty3 => // Some comment

            3;

        public int MyProperty4 => 
            // Another comment
            3;

        // Should not be formatted, so white space after property name should remain
        public int MyProperty5 
            => 3;

        public Func<bool> MyProperty6 => x => x == 3;

        // This property to ensure that a regular (non expression) property is not messed with
        public int MyProperty7 { get; private set; }

        // Test inline method that should be formatted
        public int MyMethod1() => ints.Single(x => x == 3);

        // Test method that has a comment in between and should not be formatted
        public int MyMethod2()  // Some method

            => ints.Single(x => x == 3);

        // Test method that has multiple newlines but no comment in between and should be formatted
        public int MyMethod3()


=> ints.Single(x => x == 3);

        // Test standard block method with a lambda token that should not be formatted
        public int Method4()
        {
            return ints.Single(x => x == 3);
        }

        // Test method that has a lambda token within the 'expression' implementation of the method.
        public Func<bool> Method5() => x => x == 3;

        // Test method that is improperly formatted.
        public Func<bool> Method6() => 
            x => x == 3;

        // Test method with generic identifier and constraint that needs to be formatted.
        public string Method7<T>(T input) where T : class => input.ToString();

        // Test method with generic identifier and constraint that needs to be formatted, with a comment in the expression.
        public string Method8<T>(T input) where T : class => 
            // This is the input as a string
            input.ToString();
    }
}";
        public const string ExpressionBodiedMethodAfterFormatting = @"
namespace TestDummy
{
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        private IList<int> ints = new List<int>();

        public int MyProperty1
            => 3;

        public int MyProperty2
            => 3;

        public int MyProperty3
            => // Some comment
3;

        public int MyProperty4
            => // Another comment
            3;

        // Should not be formatted, so white space after property name should remain
        public int MyProperty5 
            => 3;

        public Func<bool> MyProperty6
            => x => x == 3;

        // This property to ensure that a regular (non expression) property is not messed with
        public int MyProperty7 { get; private set; }

        // Test inline method that should be formatted
        public int MyMethod1()
            => ints.Single(x => x == 3);

        // Test method that has a comment in between and should not be formatted
        public int MyMethod2()  // Some method

            => ints.Single(x => x == 3);

        // Test method that has multiple newlines but no comment in between and should be formatted
        public int MyMethod3()
            => ints.Single(x => x == 3);

        // Test standard block method with a lambda token that should not be formatted
        public int Method4()
        {
            return ints.Single(x => x == 3);
        }

        // Test method that has a lambda token within the 'expression' implementation of the method.
        public Func<bool> Method5()
            => x => x == 3;

        // Test method that is improperly formatted.
        public Func<bool> Method6()
            => x => x == 3;

        // Test method with generic identifier and constraint that needs to be formatted.
        public string Method7<T>(T input) where T : class
            => input.ToString();

        // Test method with generic identifier and constraint that needs to be formatted, with a comment in the expression.
        public string Method8<T>(T input) where T : class
            => // This is the input as a string
            input.ToString();
    }
}";
        public const string InterfaceAfterSorting = @"namespace InterfaceBeforeSorting
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

        public double AABeginning(int data, string moarData)
        {
            return 0.0;
        }

        public string ZZShouldBeAtTheEnd(string moreInput)
        {
            return moreInput;
        }

        int IProgram.GetSomeData(int input)
        {
            return input;
        }
        void IProgram.LaterFunctionButNotLast()
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

namespace InterfaceBeforeSorting
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

        public double AABeginning(int data, string moarData)
        {
            return 0.0;
        }

        public string ZZShouldBeAtTheEnd(string moreInput)
        {
            return moreInput;
        }

        int IProgram.GetSomeData(int input)
        {
            return input;
        }

        void IProgram.LaterFunctionButNotLast()
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
        public const string InterfaceBeforeSorting = @"namespace InterfaceBeforeSorting
{

    using System;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    class Program : IProgram
    {
        void IProgram.LaterFunctionButNotLast()
        {
            
        }
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }

        public string ZZShouldBeAtTheEnd(string moreInput)
        {
            return moreInput;
        }

        int IProgram.GetSomeData(int input)
        {
            return input;
        }

        public double AABeginning(int data, string moarData)
        {
            return 0.0;
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
        public const string RecordAfterSorting = @"namespace TestDummy
{
    using System;

    public record Program(int Id, string Name)
    {

        public string Abacus { get; init; }

        public string Description { get; init; }
        public string Elided { get; init; }

        public string Xylophone { get; init; }
    }
}";
        public const string RecordAfterSortingAndFormattingNewlines = @"

namespace TestDummy
{
    using System;

    public record Program(int Id, string Name)
    {
        public string Abacus { get; init; }

        public string Description { get; init; }

        public string Elided { get; init; }

        public string Xylophone { get; init; }
    }
}";
        public const string RecordBeforeSorting = @"namespace TestDummy
{
    using System;

    public record Program(int Id, string Name)
    {
        public string Elided { get; init; }

        public string Description { get; init; }

        public string Xylophone { get; init; }

        public string Abacus { get; init; }
    }
}";
        public const string RecordWithAttributes = @"namespace TestDummy
{

    using System;



    /// <summary>
    /// This is the record that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    public record Program(int Id, string Name)
    {
        
        public string Description { get; init; }


    }
}";
        public const string RecordWithAttributesAfterNewline = @"

namespace TestDummy
{
    using System;

    /// <summary>
    /// This is the record that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]

    public record Program(int Id, string Name)
    {
        public string Description { get; init; }
    }
}";
        public const string StructAfterSorting = @"

namespace TestSpace
{
    using System.Diagnostics;

    [DebuggerDisplay(""{IdValue}"")]
    public readonly struct VersionId : IEquatable<VersionId>, IComparable<VersionId>
    {
		private readonly int IdValue;

		public VersionId(int versionId)
        {
            if (versionId <= 0)
                throw new ArgumentException(""Invalid value was provided: "" + versionId, nameof(versionId));

            IdValue = versionId;
        }

		public static VersionId NoId { get; } = new();

		public static bool operator ==(VersionId left, VersionId right)
            => left.Equals(right);

        public static bool operator !=(VersionId left, VersionId right)
            => !left.Equals(right);

        public static bool operator >(VersionId left, VersionId right)
           => left.CompareTo(right) > 0;

        public static bool operator >=(VersionId left, VersionId right)
        {
            int comparison = left.CompareTo(right);
            return comparison > -1;
        }

        public static bool operator <(VersionId left, VersionId right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(VersionId left, VersionId right)
        {
            int comparison = left.CompareTo(right);
            return comparison < 1;
        }

        public static implicit operator int(VersionId id) => id.IdValue;

		public static explicit operator VersionId(int idValue)
        {
            if (idValue <= 0)
                throw new InvalidCastException(""Cannot cast a value less than or equal to 0."");

            return new(idValue);
        }

        public int CompareTo(VersionId other)
        {
            if (IdValue < other.IdValue)
                return -1;
            return IdValue == other.IdValue ? 0 : 1;
        }

        public override bool Equals(object obj)
        {
            if (obj is VersionId aavId)
                return Equals(aavId);
            return false;
        }

        public bool Equals(VersionId other)
            => IdValue == other.IdValue;

        public override int GetHashCode()
            => IdValue;

        /// <summary>
        /// Returns the value of this instance as a string.
        /// </summary>
        public override string ToString()
            => IdValue.ToString();
    }
}
";
        public const string StructBeforeSorting = @"namespace TestSpace
{
    using System.Diagnostics;

    [DebuggerDisplay(""{IdValue}"")]
    public readonly struct VersionId : IEquatable<VersionId>, IComparable<VersionId>
    {

        public static bool operator >=(VersionId left, VersionId right)
        {
            int comparison = left.CompareTo(right);
            return comparison > -1;
        }
        public static bool operator <(VersionId left, VersionId right)
            => left.CompareTo(right) < 0;

        /// <summary>
        /// Returns the value of this instance as a string.
        /// </summary>
        public override string ToString()
            => IdValue.ToString();



        public override int GetHashCode()
            => IdValue;

        public bool Equals(VersionId other)
            => IdValue == other.IdValue;

        

        public static bool operator !=(VersionId left, VersionId right)
            => !left.Equals(right);

        
		public static explicit operator VersionId(int idValue)
        {
            if (idValue <= 0)
                throw new InvalidCastException(""Cannot cast a value less than or equal to 0."");

            return new(idValue);
        }

        public static bool operator >(VersionId left, VersionId right)
           => left.CompareTo(right) > 0;

        public static bool operator <=(VersionId left, VersionId right)
        {
            int comparison = left.CompareTo(right);
            return comparison < 1;
        }
		private readonly int IdValue;
        
		
		public static VersionId NoId { get; } = new();

        public int CompareTo(VersionId other)
        {
            if (IdValue < other.IdValue)
                return -1;
            return IdValue == other.IdValue ? 0 : 1;
        }
		
		public static bool operator ==(VersionId left, VersionId right)
            => left.Equals(right);
        public static implicit operator int(VersionId id) => id.IdValue;
        public override bool Equals(object obj)
        {
            if (obj is VersionId aavId)
                return Equals(aavId);
            return false;
        }
		public VersionId(int versionId)
        {
            if (versionId <= 0)
                throw new ArgumentException(""Invalid value was provided: "" + versionId, nameof(versionId));

            IdValue = versionId;
        }
    }
}
";
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
        public const string ClassBeforeAccessLevelModifierFormat = @"namespace InterfaceBeforeSorting
{

    using System;
    using System.Diagnostics;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
    class Program : IProgram
    {
        static int SomeValue;
        void IProgram.LaterFunctionButNotLast()
        {
            
        }
        int numberOfSomething; // End of line comment

        public Program()
        {
        }
        [Conditional(""Debug"")]
        string ZZShouldBeAtTheEnd(string moreInput)
        {
            return moreInput;
        }

        static Program()
        {
            SomeValue = 3;
        }

        int GetSomeData(int input)
        {
            return input;
        }

        ~Program()
        {
        }

        internal double AABeginning(int data, string moarData)
        {
            return 0.0;
        }

        int Sum => 3;
        public bool IsAvailable { get; private set; }

        bool UnAvailable { get; }
    }

    enum DocStatus : byte
    {
        Good,
        Bad
    }

    public enum Uptime
    {
        Good,
        Ok,
        Terrible
    }

    /// <summary>
    /// This is the interface that declares some useful functionality.
    /// </summary>
    interface IProgram
    {
        void LaterFunctionButNotLast();

        bool IsAvailable { get; }
    }

    readonly struct IdValue
    {
        const int DefaultValue = 3;
        IdValue(int newValue) { Value = newValue; }
        internal int Value { get; }
    }

    internal class TestClass
    {
        TestClass() {}

        protected internal int State;
    }

    public record Status(int state, bool valid);

    record Doc
    {
        public int Initialized { get; init; }
    }


}";
        public const string ClassAfterAccessLevelModifierFormat = @"namespace InterfaceBeforeSorting
{

    using System;
    using System.Diagnostics;

    /// <summary>
    /// This is the class that implements some useful functionality.
    /// </summary>
    [Guid(""ec250ee0-f916-4335-9764-17b6cd3573fc"")]
internal     class Program : IProgram
    {
        private static int SomeValue;
        void IProgram.LaterFunctionButNotLast()
        {
            
        }
        private int numberOfSomething; // End of line comment

        public Program()
        {
        }
        [Conditional(""Debug"")]
private         string ZZShouldBeAtTheEnd(string moreInput)
        {
            return moreInput;
        }

        static Program()
        {
            SomeValue = 3;
        }

        private int GetSomeData(int input)
        {
            return input;
        }

        ~Program()
        {
        }

        internal double AABeginning(int data, string moarData)
        {
            return 0.0;
        }

        private int Sum => 3;
        public bool IsAvailable { get; private set; }

        private bool UnAvailable { get; }
    }

    public enum DocStatus : byte
    {
        Good,
        Bad
    }

    public enum Uptime
    {
        Good,
        Ok,
        Terrible
    }

    /// <summary>
    /// This is the interface that declares some useful functionality.
    /// </summary>
    internal interface IProgram
    {
        void LaterFunctionButNotLast();

        bool IsAvailable { get; }
    }

    internal readonly struct IdValue
    {
        private const int DefaultValue = 3;
        private IdValue(int newValue) { Value = newValue; }
        internal int Value { get; }
    }

    internal class TestClass
    {
        private TestClass() {}

        protected internal int State;
    }

    public record Status(int state, bool valid);

    internal record Doc
    {
        public int Initialized { get; init; }
    }


}";
    }
}
