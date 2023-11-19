namespace VirtualMethodsVsInterfaceImplementation {

	class A {
		public virtual void m() { Console.WriteLine("A.m() code"); }
	}

	class B : A {
		public override void m() { Console.WriteLine("B.m() code"); }
	}

    interface I1 {
		public void m();
	}

	class X : I1 {
		public void m() { Console.WriteLine("X.m() code"); }
	}

	class Y : X, I1 {
		public new void m() { Console.WriteLine("Y.m() code"); }
	}

	class Z : I1 {
		public void m() { Console.WriteLine("Z.m() code"); }
	}

	internal class Program {
		static void Main(string[] args) {
			A a = new B();
			a.m();

			X x = new Y();
			x.m();
		}
	}
}