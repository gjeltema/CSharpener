// -----------------------------------------------------------------------
// ComplexFileTestData.cs Copyright 2022 Craig Gjeltema
// -----------------------------------------------------------------------

namespace CSharpener.Logic.Tests
{
    internal class ComplexFileTestData
    {
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
        private bool Predicate()
        {
            return false;
        }

        protected internal int StrangeFunction()
        {
            return 1 + 2;
        }

		#region StartRegion
        static void Main(string[] args)
        {
            var coll = new List<int> { 1, 2, 10 };
            var newColl = coll.Select(x => x > 1);
            Console.WriteLine(""Test"" + string.Join("","", coll));
        }

internal enum Translate
        {
            RedToBlue,
            BlueToGreen,
            GreenToRed
        }
        private protected bool Blah(bool returnValue)
        {
            return returnValue && true;
        }public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

        public const double PI = 3.1415;

        protected int SomeProp
        {
            get; private set;
        }

        public static bool operator !=(Program left, Program right)
        {
            return !left.Equals(right);
        }

        public string DoWork(string input, string moreData)
        {
            return ""valid work complete"";
        }

        public string DoWork(bool output)
        {
            return ""valid work complete"";
        }
		
		
        public static readonly int VersionNumber = 10;
        public string Name
        {
            get => ""MyName"";
            set
            {
                if (value != _name)
                    _name = value;
            }
        }

		public event EventHandler<EventArgs> SomeEvent
        {
            add {
			}
            remove
			
            {
            }
        }
		
		protected override string ToBeOverridden { get; set; } = ""TestString"";
		
        internal class NestedClass
        {
            internal void RevertWork()
            {
            }

            public bool isTrue = false;
        }
		public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        [Obsolete]
        public string DoWork(string input)
        {
            return ""valid work complete"";
        }

        protected delegate bool Adder(int a, int b);

        public enum Color
        {
            Red, Green,
                Blue
        }
		
		#endregion

        public event EventHandler<EventArgs> PropertiesChanged;

private string _name;
        internal static readonly int NumDocs = 10;
        public Program()
        {

        }

        internal static bool SomeFlag = false;

        [DllImport(@""C:\Native.dll"", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Foo(IntPtr lplf);

        public static bool operator ==(Program left, Program right)
        {
            return left.Equals(right);
        }

        internal event EventHandler<EventArgs> DontChange
        {
		#region StartRegion2
            add {}
            remove
            {
            }
        }
#endregion
        private const int DummyValue = 100;

        protected virtual bool worthless { get; } = false;

        internal static void DoWork()
        {
        }


        public int Count = 0;
        private string _value = ""test"";
        public bool IsAvailable => true;
		
		public override void OverriddenFunction(bool isTrueOrFalse)
		{
			if(isTrueOrFalse)
				DoWork(""TestInput"");
		}
		
		
    }
	public interface SomeInterface
	{
		string DoWork(string input);
	}
	
	internal abstract class SecondClass
	{
		protected SecondClass()
		{}
		
		protected virtual string ToBeOverridden { get; set; }
		
		public abstract void OverriddenFunction(bool isTrueOrFalse);
	}
	#region AnotherRegion
	namespace MoreBusyWork
	{
	using System;
		internal class SomeGeneric<T, U> where T : SecondClass
			where U : Program
		{
			public static bool operator ==(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
                {
                    return left.Equals(right);
                }
			private protected bool Blah(bool returnValue)
			{
				return returnValue && true;
			}
	
			public const double PI = 3.1415;
	
			protected int SomeProp
			{
				get; private set;
			}
	
			public static bool operator !=(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
			{
				return !left.Equals(right);
			}
	
			public string DoWork(string input, string moreData)
			{
				return ""valid work complete"";
			}
	
			public string DoWork(bool output)
			{
				return ""valid work complete"";
			}

		}
		
		internal class DerivedGeneric<SecondClass, Program>
		{
			protected delegate bool Adder(int a, int b);

			public enum Color
			{
				Red, Green,
					Blue
			}
			
			#endregion
	
			public event EventHandler<EventArgs> PropertiesChanged;
	
				private string _name;
			internal static readonly int NumDocs = 10;
			public DerivedGeneric()
			{
	
			}
			
			
			public X GenericFunction<X>(X argument) where X : SecondClass, new()
			{
				return new X();
			}
		}
	}
}
";
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
        private bool Predicate()
        {
            return false;
        }

        protected internal int StrangeFunction()
        {
            return 1 + 2;
        }

		#region StartRegion
        static void Main(string[] args)
        {
            var coll = new List<int> { 1, 2, 10 };
            var newColl = coll.Select(x => x > 1);
            Console.WriteLine(""Test"" + string.Join("","", coll));
        }

internal enum Translate
        {
            RedToBlue,
            BlueToGreen,
            GreenToRed
        }
        private protected bool Blah(bool returnValue)
        {
            return returnValue && true;
        }public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

        public const double PI = 3.1415;

        protected int SomeProp
        {
            get; private set;
        }

        public static bool operator !=(Program left, Program right)
        {
            return !left.Equals(right);
        }

        public string DoWork(string input, string moreData)
        {
            return ""valid work complete"";
        }

        public string DoWork(bool output)
        {
            return ""valid work complete"";
        }
		
		
        public static readonly int VersionNumber = 10;
        public string Name
        {
            get => ""MyName"";
            set
            {
                if (value != _name)
                    _name = value;
            }
        }

		public event EventHandler<EventArgs> SomeEvent
        {
            add {
			}
            remove
			
            {
            }
        }
		
		protected override string ToBeOverridden { get; set; } = ""TestString"";
		
        internal class NestedClass
        {
            internal void RevertWork()
            {
            }

            public bool isTrue = false;
        }
		public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        [Obsolete]
        public string DoWork(string input)
        {
            return ""valid work complete"";
        }

        protected delegate bool Adder(int a, int b);

        public enum Color
        {
            Red, Green,
                Blue
        }
		
		#endregion

        public event EventHandler<EventArgs> PropertiesChanged;

private string _name;
        internal static readonly int NumDocs = 10;
        public Program()
        {

        }

        internal static bool SomeFlag = false;

        [DllImport(@""C:\Native.dll"", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Foo(IntPtr lplf);

        public static bool operator ==(Program left, Program right)
        {
            return left.Equals(right);
        }

        internal event EventHandler<EventArgs> DontChange
        {
		#region StartRegion2
            add {}
            remove
            {
            }
        }
#endregion
        private const int DummyValue = 100;

        protected virtual bool worthless { get; } = false;

        internal static void DoWork()
        {
        }


        public int Count = 0;
        private string _value = ""test"";
        public bool IsAvailable => true;
		
		public override void OverriddenFunction(bool isTrueOrFalse)
		{
			if(isTrueOrFalse)
				DoWork(""TestInput"");
		}
		
		
    }
	public interface SomeInterface
	{
		string DoWork(string input);
	}
	
	internal abstract class SecondClass
	{
		protected SecondClass()
		{}
		
		protected virtual string ToBeOverridden { get; set; }
		
		public abstract void OverriddenFunction(bool isTrueOrFalse);
	}
	#region AnotherRegion
	namespace MoreBusyWork
	{
	using System;
		internal class SomeGeneric<T, U> where T : SecondClass
			where U : Program
		{
			public static bool operator ==(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
                {
                    return left.Equals(right);
                }
			private protected bool Blah(bool returnValue)
			{
				return returnValue && true;
			}
	
			public const double PI = 3.1415;
	
			protected int SomeProp
			{
				get; private set;
			}
	
			public static bool operator !=(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
			{
				return !left.Equals(right);
			}
	
			public string DoWork(string input, string moreData)
			{
				return ""valid work complete"";
			}
	
			public string DoWork(bool output)
			{
				return ""valid work complete"";
			}

		}
		
		internal class DerivedGeneric<SecondClass, Program>
		{
			protected delegate bool Adder(int a, int b);

			public enum Color
			{
				Red, Green,
					Blue
			}
			
			#endregion
	
			public event EventHandler<EventArgs> PropertiesChanged;
	
				private string _name;
			internal static readonly int NumDocs = 10;
			public DerivedGeneric()
			{
	
			}
			
			
			public X GenericFunction<X>(X argument) where X : SecondClass, new()
			{
				return new X();
			}
		}
	}
}";
        //====================================================================

        public const string UsingsInAndOutsideNamespaceAfterCompositeRun = @"// -------------------------------------------
// Test Header for TestFile.cs Copyright 2022
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
        public enum Color
        {
            Red, Green,
                Blue
        }

internal enum Translate
        {
            RedToBlue,
            BlueToGreen,
            GreenToRed
        }

        protected delegate bool Adder(int a, int b);

        public event EventHandler<EventArgs> PropertiesChanged;

        public const double PI = 3.1415;
        private const int DummyValue = 100;
        public static readonly int VersionNumber = 10;
        internal static readonly int NumDocs = 10;
        internal static bool SomeFlag = false;
        public int Count = 0;
private string _name;
        private string _value = ""test"";

		public event EventHandler<EventArgs> SomeEvent
        {
            add {
			}
            remove
			
            {
            }
        }

        internal event EventHandler<EventArgs> DontChange
        {
		            add {}
            remove
            {
            }
        }

        public Program()
        {

        }

        public bool IsAvailable
            => true;

        public string Name
        {
            get => ""MyName"";
            set
            {
                if (value != _name)
                    _name = value;
            }
        }

        protected int SomeProp
        {
            get; private set;
        }

		protected override string ToBeOverridden { get; set; } = ""TestString"";

        protected virtual bool worthless { get; } = false;

        public static bool operator ==(Program left, Program right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Program left, Program right)
        {
            return !left.Equals(right);
        }

        [DllImport(@""C:\Native.dll"", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Foo(IntPtr lplf);

        internal static void DoWork()
        {
        }

		        static void Main(string[] args)
        {
            var coll = new List<int> { 1, 2, 10 };
            var newColl = coll.Select(x => x > 1);
            Console.WriteLine(""Test"" + string.Join("","", coll));
        }

        public string DoWork(bool output)
        {
            return ""valid work complete"";
        }

        [Obsolete]
        public string DoWork(string input)
        {
            return ""valid work complete"";
        }

        public string DoWork(string input, string moreData)
        {
            return ""valid work complete"";
        }

public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

		public override int GetHashCode()
            {
                return base.GetHashCode();
            }

		public override void OverriddenFunction(bool isTrueOrFalse)
		{
			if(isTrueOrFalse)
				DoWork(""TestInput"");
		}

        protected internal int StrangeFunction()
        {
            return 1 + 2;
        }

        private bool Predicate()
        {
            return false;
        }

        private protected bool Blah(bool returnValue)
        {
            return returnValue && true;
        }

        internal class NestedClass
        {
            public bool isTrue = false;

            internal void RevertWork()
            {
            }
        }
    }

	public interface SomeInterface
	{
		string DoWork(string input);
	}

	internal abstract class SecondClass
	{
		protected SecondClass()
		{}

		protected virtual string ToBeOverridden { get; set; }

		public abstract void OverriddenFunction(bool isTrueOrFalse);
	}

namespace MoreBusyWork
	{
	using System;

		internal class SomeGeneric<T, U> where T : SecondClass
			where U : Program
		{
			public const double PI = 3.1415;

			protected int SomeProp
			{
				get; private set;
			}

			public static bool operator ==(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
                {
                    return left.Equals(right);
                }

			public static bool operator !=(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
			{
				return !left.Equals(right);
			}

			public string DoWork(bool output)
			{
				return ""valid work complete"";
			}

			public string DoWork(string input, string moreData)
			{
				return ""valid work complete"";
			}

			private protected bool Blah(bool returnValue)
			{
				return returnValue && true;
			}
		}

		internal class DerivedGeneric<SecondClass, Program>
		{
			public enum Color
			{
				Red, Green,
					Blue
			}

			protected delegate bool Adder(int a, int b);

			public event EventHandler<EventArgs> PropertiesChanged;

			internal static readonly int NumDocs = 10;
				private string _name;

			public DerivedGeneric()
			{
	
			}

			public X GenericFunction<X>(X argument) where X : SecondClass, new()
			{
				return new X();
			}
		}
	}
}
";
        public const string UsingsOnlyOutsideNamespaceAfterCompositeRun = @"// -------------------------------------------
// Test Header for AnotherTestFile.cs Copyright 2022
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
        public enum Color
        {
            Red, Green,
                Blue
        }

internal enum Translate
        {
            RedToBlue,
            BlueToGreen,
            GreenToRed
        }

        protected delegate bool Adder(int a, int b);

        public event EventHandler<EventArgs> PropertiesChanged;

        public const double PI = 3.1415;
        private const int DummyValue = 100;
        public static readonly int VersionNumber = 10;
        internal static readonly int NumDocs = 10;
        internal static bool SomeFlag = false;
        public int Count = 0;
private string _name;
        private string _value = ""test"";

		public event EventHandler<EventArgs> SomeEvent
        {
            add {
			}
            remove
			
            {
            }
        }

        internal event EventHandler<EventArgs> DontChange
        {
		            add {}
            remove
            {
            }
        }

        public Program()
        {

        }

        public bool IsAvailable
            => true;

        public string Name
        {
            get => ""MyName"";
            set
            {
                if (value != _name)
                    _name = value;
            }
        }

        protected int SomeProp
        {
            get; private set;
        }

		protected override string ToBeOverridden { get; set; } = ""TestString"";

        protected virtual bool worthless { get; } = false;

        public static bool operator ==(Program left, Program right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Program left, Program right)
        {
            return !left.Equals(right);
        }

        [DllImport(@""C:\Native.dll"", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Foo(IntPtr lplf);

        internal static void DoWork()
        {
        }

		        static void Main(string[] args)
        {
            var coll = new List<int> { 1, 2, 10 };
            var newColl = coll.Select(x => x > 1);
            Console.WriteLine(""Test"" + string.Join("","", coll));
        }

        public string DoWork(bool output)
        {
            return ""valid work complete"";
        }

        [Obsolete]
        public string DoWork(string input)
        {
            return ""valid work complete"";
        }

        public string DoWork(string input, string moreData)
        {
            return ""valid work complete"";
        }

public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

		public override int GetHashCode()
            {
                return base.GetHashCode();
            }

		public override void OverriddenFunction(bool isTrueOrFalse)
		{
			if(isTrueOrFalse)
				DoWork(""TestInput"");
		}

        protected internal int StrangeFunction()
        {
            return 1 + 2;
        }

        private bool Predicate()
        {
            return false;
        }

        private protected bool Blah(bool returnValue)
        {
            return returnValue && true;
        }

        internal class NestedClass
        {
            public bool isTrue = false;

            internal void RevertWork()
            {
            }
        }
    }

	public interface SomeInterface
	{
		string DoWork(string input);
	}

	internal abstract class SecondClass
	{
		protected SecondClass()
		{}

		protected virtual string ToBeOverridden { get; set; }

		public abstract void OverriddenFunction(bool isTrueOrFalse);
	}

namespace MoreBusyWork
	{
	using System;

		internal class SomeGeneric<T, U> where T : SecondClass
			where U : Program
		{
			public const double PI = 3.1415;

			protected int SomeProp
			{
				get; private set;
			}

			public static bool operator ==(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
                {
                    return left.Equals(right);
                }

			public static bool operator !=(SomeGeneric<T, U> left, SomeGeneric<T, U> right)
			{
				return !left.Equals(right);
			}

			public string DoWork(bool output)
			{
				return ""valid work complete"";
			}

			public string DoWork(string input, string moreData)
			{
				return ""valid work complete"";
			}

			private protected bool Blah(bool returnValue)
			{
				return returnValue && true;
			}
		}

		internal class DerivedGeneric<SecondClass, Program>
		{
			public enum Color
			{
				Red, Green,
					Blue
			}

			protected delegate bool Adder(int a, int b);

			public event EventHandler<EventArgs> PropertiesChanged;

			internal static readonly int NumDocs = 10;
				private string _name;

			public DerivedGeneric()
			{
	
			}

			public X GenericFunction<X>(X argument) where X : SecondClass, new()
			{
				return new X();
			}
		}
	}
}";
    }
}
