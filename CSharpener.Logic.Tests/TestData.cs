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
        public const string ClassWithAttributesAfterNewline = @"namespace TestDummy
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
    }
}
