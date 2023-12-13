using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace NezarkaBookstore
{
	//
	// Model
	//

	public class ModelStore {
		private List<Book> books = new List<Book>();
		private List<Customer> customers = new List<Customer>();

		public IList<Book> GetBooks() {
			return books;
		}

		public Book GetBook(int id) {
			return books.Find(b => b.Id == id);
		}

		public Customer GetCustomer(int id) {
			return customers.Find(c => c.Id == id);
		}

		public static ModelStore LoadFrom(TextReader reader) {
			var store = new ModelStore();

			try {
				if (reader.ReadLine() != "DATA-BEGIN") {
					return null;
				}
				while (true) {
					string line = reader.ReadLine();
					if (line == null) {
						return null;
					} else if (line == "DATA-END") {
						break;
					}

					string[] tokens = line.Split(';');
					switch (tokens[0]) {
						case "BOOK":
							store.books.Add(new Book {
								Id = int.Parse(tokens[1]), Title = tokens[2], Author = tokens[3], Price = int.Parse(tokens[4])
							});
							break;
						case "CUSTOMER":
							store.customers.Add(new Customer {
								Id = int.Parse(tokens[1]), FirstName = tokens[2], LastName = tokens[3]
							});
							break;
						case "CART-ITEM":
							var customer = store.GetCustomer(int.Parse(tokens[1]));
							if (customer == null) {
								return null;
							}
							customer.ShoppingCart.Items.Add(new ShoppingCartItem {
								BookId = int.Parse(tokens[2]), Count = int.Parse(tokens[3])
							});
							break;
						default:
							return null;
					}
				}
			} catch (Exception ex) {
				if (ex is FormatException || ex is IndexOutOfRangeException) {
					return null;
				}
				throw;
			}

			return store;
		}
	}

	public class Book {
		public int Id { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public int Price { get; set; }
	}

	public class Customer {
		private ShoppingCart shoppingCart;

		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public ShoppingCart ShoppingCart {
			get {
				if (shoppingCart == null) {
					shoppingCart = new ShoppingCart();
				}
				return shoppingCart;
			}
			set {
				shoppingCart = value;
			}
		}
	}

	public class ShoppingCartItem {
		public int BookId { get; set; }
		public int Count { get; set; }
	}

	public class ShoppingCart {
		public int CustomerId { get; set; }
		public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
	}

	//
	// View
	//

	public class CommonView {
		public void RenderHeader(Customer customer){
			Console.WriteLine("<!DOCTYPE html>");
			Console.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
			Console.WriteLine("<head>");
			Console.WriteLine( "	<meta charset=\"utf-8\" />");
			Console.WriteLine( "	<title>Nezarka.net: Online Shopping for Books</title>");
			Console.WriteLine("</head>");
			Console.WriteLine("<body>");
			Console.WriteLine( "	<style type=\"text/css\">");
			Console.WriteLine( "		table, th, td {");
			Console.WriteLine( "			border: 1px solid black;");
			Console.WriteLine( "			border-collapse: collapse;");
			Console.WriteLine( "		}");
			Console.WriteLine( "		table {");
			Console.WriteLine( "			margin-bottom: 10px;");
			Console.WriteLine( "		}");
			Console.WriteLine( "		pre {");
			Console.WriteLine( "			line-height: 70%;");
			Console.WriteLine( "		}");
			Console.WriteLine( "	</style>");
			Console.WriteLine( "	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
			Console.WriteLine($"	{customer.FirstName}, here is your menu:");
			Console.WriteLine( "	<table>");
			Console.WriteLine( "		<tr>");
			Console.WriteLine( "			<td><a href=\"/Books\">Books</a></td>");
			Console.WriteLine($"			<td><a href=\"/ShoppingCart\">Cart ({Convert.ToString(customer.ShoppingCart.Items.Count)})</a></td>");
			Console.WriteLine( "		</tr>");
			Console.WriteLine( "	</table>");
		}
		public void RenderFooter(){
			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}
	}
	
	public class BooksView {
		private CommonView commonView;

		public BooksView(CommonView commonView) {
			this.commonView = commonView;
		}

		public void RenderBooks(IList<Book> books, Customer customer) {
			commonView.RenderHeader(customer);
			Console.WriteLine( "	Our books for you:");
			Console.WriteLine("	<table>");
			int i = 0;
			foreach (var book in books)
			{
				if (i%3 == 0 && i != 0){
					Console.WriteLine("		</tr>");
					Console.WriteLine("		<tr>");
				}
				else if (i == 0) Console.WriteLine("		<tr>");
				i++;
				Console.WriteLine("			<td style=\"padding: 10px;\">");
				Console.WriteLine($"				<a href=\"/Books/Detail/{Convert.ToString(book.Id)}\">{book.Title}</a><br />");
				Console.WriteLine($"				Author: {book.Author}<br />");
				Console.WriteLine($"				Price: {Convert.ToString(book.Price)} EUR &lt;<a href=\"/ShoppingCart/Add/{Convert.ToString(book.Id)}\">Buy</a>&gt;");
				Console.WriteLine("			</td>");
			}
			if (i != 0) Console.WriteLine(  "		</tr>");
			Console.WriteLine(	"	</table>");

			commonView.RenderFooter();
		}

		public void RenderBookDetails(Book book, Customer customer) {
			commonView.RenderHeader(customer);

			Console.WriteLine("	Book details:");
			Console.WriteLine($"	<h2>{book.Title}</h2>");
			Console.WriteLine("	<p style=\"margin-left: 20px\">");
			Console.WriteLine($"	Author: {book.Author}<br />");
			Console.WriteLine($"	Price: {Convert.ToString(book.Price)} EUR<br />");
			Console.WriteLine("	</p>");
			Console.WriteLine($"	<h3>&lt;<a href=\"/ShoppingCart/Add/{Convert.ToString(book.Id)}\">Buy this book</a>&gt;</h3>");

			commonView.RenderFooter();
		}
	}

	public class ShoppingCartView {
		private CommonView commonView;


		public ShoppingCartView(CommonView commonView) {
			this.commonView = commonView;
		}

		public void RenderShoppingCart(Customer customer, ModelStore modelStore) {
			commonView.RenderHeader(customer);
			int total = 0;

			Console.WriteLine("	Your shopping cart:");
			Console.WriteLine("	<table>");
			Console.WriteLine("		<tr>");
			Console.WriteLine("			<th>Title</th>");
			Console.WriteLine("			<th>Count</th>");
			Console.WriteLine("			<th>Price</th>");
			Console.WriteLine("			<th>Actions</th>");
			Console.WriteLine("		</tr>");
			foreach (var item in customer.ShoppingCart.Items){
				Book book = modelStore.GetBook(item.BookId);
				int count = item.Count;
				Console.WriteLine("		<tr>");
				Console.WriteLine($"			<td><a href=\"/Books/Detail/{Convert.ToString(book.Id)}\">{book.Title}</a></td>");
				int price = count*book.Price;
				total += price;
				Console.WriteLine($"			<td>{Convert.ToString(item.Count)}</td>");
				if (item.Count == 1) Console.WriteLine($"			<td>{Convert.ToString(book.Price)} EUR</td>");
				else{		
				Console.WriteLine($"			<td>{Convert.ToString(count)} * {Convert.ToString(book.Price)} = {Convert.ToString(price)} EUR</td>");
				}
				Console.WriteLine($"			<td>&lt;<a href=\"/ShoppingCart/Remove/{Convert.ToString(book.Id)}\">Remove</a>&gt;</td>");
				Console.WriteLine("		</tr>");
			}
			Console.WriteLine("	</table>");
			Console.WriteLine($"	Total price of all items: {Convert.ToString(total)} EUR");
			commonView.RenderFooter();
		}

		public void RenderEmptyShoppingCart(Customer customer) {
			commonView.RenderHeader(customer);
			Console.WriteLine("	Your shopping cart is EMPTY.");
			commonView.RenderFooter();
		}
	}

	public class ErrorView {
		public void RenderError() {
			Console.WriteLine("<!DOCTYPE html>");
			Console.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
			Console.WriteLine("<head>");
			Console.WriteLine("	<meta charset=\"utf-8\" />");
			Console.WriteLine("	<title>Nezarka.net: Online Shopping for Books</title>");
			Console.WriteLine("</head>");
			Console.WriteLine("<body>");
			Console.WriteLine("<p>Invalid request.</p>");
			Console.WriteLine("</body>");
			Console.WriteLine("</html>");
		}
	}
	
	//
	// Controller
	//
	
	public class BooksController {
		private ModelStore model;
		private BooksView view;
		private ErrorView errorView;

		public BooksController(ModelStore model, BooksView view, ErrorView errorView) {
			this.model = model;
			this.view = view;
			this.errorView = errorView;
		}

		public void ShowBooks(int customerId) {
			IList<Book> books = model.GetBooks();
			Customer customer = model.GetCustomer(customerId);
			if (books != null && customer != null && model != null) {
				view.RenderBooks(books, customer);
			} else {
				errorView.RenderError();
			}

		}

		public void ShowBookDetails(int bookId, int customerId) {
			Book book = model.GetBook(bookId);
			Customer customer = model.GetCustomer(customerId);
			if (book != null && customer != null && model != null) {
				view.RenderBookDetails(book, customer);
			} else {
				errorView.RenderError();
			}
		}
	}

	public class ShoppingCartController {
		private ModelStore model;
		private ShoppingCartView view;
		private ErrorView errorView;

		public ShoppingCartController(ModelStore model, ShoppingCartView view, ErrorView errorView) {
			this.model = model;
			this.view = view;
			this.errorView = errorView;
		}

		public void ShowShoppingCart(int customerId) {
			Customer customer = model.GetCustomer(customerId);
			if (customer != null && model != null) {
				if (customer.ShoppingCart.Items.Count == 0 || customer.ShoppingCart.Items == null)
					view.RenderEmptyShoppingCart(customer);
				else view.RenderShoppingCart(customer, model);
			} else {
				errorView.RenderError();
			}
		}
		public void AddToShoppingCart(int customerId, int bookId){
			Book book = model.GetBook(bookId);
			Customer customer = model.GetCustomer(customerId);
			if (book != null && customer != null && model != null) {
				bool foundBook = false;
				foreach (var item in customer.ShoppingCart.Items)
				{
				   if (item.BookId == bookId) {
					   item.Count++;
					   foundBook = true;
					   break;
				   }
				}
				if (!foundBook){
					ShoppingCartItem shoppingCartItem = new ShoppingCartItem();
					shoppingCartItem.Count = 1;
					shoppingCartItem.BookId = bookId;
					if (customer.ShoppingCart.Items == null) customer.ShoppingCart.Items = new List<ShoppingCartItem>();
					customer.ShoppingCart.Items.Add(shoppingCartItem);
				}
				ShowShoppingCart(customerId);
			} else {
				errorView.RenderError();
			}
		}
		
		public void RemoveFromShoppingCart(int customerId, int bookId){
			Book book = model.GetBook(bookId);
			Customer customer = model.GetCustomer(customerId);
			if (book != null && customer != null && model != null) {
				bool foundBook = false;
				foreach (var item in customer.ShoppingCart.Items)
				{
				   if (item.BookId == bookId) {
					   item.Count--;
					   foundBook = true;
					   if (item.Count <= 0){
						   customer.ShoppingCart.Items.Remove(item);
					   }
					   break;
				   }
				}
				if (!foundBook){
					errorView.RenderError();
				}
				else{
				ShowShoppingCart(customerId);
				}
			} else {
				errorView.RenderError();
			}
		}
	}
	
	public class ErrorController {
		private ErrorView view;

		public ErrorController(ErrorView view) {
			this.view = view;
		}

		public void ShowError() {
			view.RenderError();
		}
	}

    internal class Program {
		private enum Requests {Books, Detail, ShoppingCart, Add, Remove};
		private static Requests? ProcessRequest(string request){
			string[] link = { "http:", "www.nezarka.net" }; 
			request = request.Replace("http://", "http:/");
			string[] requestParts = request.Split('/');
			if (link[0] != requestParts[0] || link[1] != requestParts[1] || requestParts.Length < 3)
				return null;
			switch (requestParts[2])
			{
				case "Books":
					if (requestParts.Length == 3) return Requests.Books;
					else if (requestParts.Length == 5 && requestParts[3] == "Detail") return Requests.Detail;
					else return null;


				case "ShoppingCart":
					if (requestParts.Length == 3) return Requests.ShoppingCart;
					else if(requestParts.Length == 5 ){
						if (requestParts[3] == "Add") return Requests.Add;
						if (requestParts[3] == "Remove") return Requests.Remove;
						else return null;
					} 
					else return null;

				default:
					return null;
			}
			
		}
		private static int GetIntValueFromRequest(string request){
			string[] requestParts = request.Split('/');
		
			return int.Parse(requestParts[requestParts.Length-1]);
		}
		

        static void Main(string[] args) {
			try{
			    ModelStore modelStore = ModelStore.LoadFrom(Console.In);
				if (modelStore == null) throw new Exception();
				else{
					var commonView = new CommonView();
					var errorView = new ErrorView();
					var booksView = new BooksView(commonView);
					var shoppingCartView = new ShoppingCartView(commonView);
					var booksController = new BooksController(modelStore, booksView, errorView);
					var shoppingCartController = new ShoppingCartController(modelStore, shoppingCartView, errorView);
					var errorController = new ErrorController(errorView);
					string? line;
					while ((line = Console.ReadLine()) != null ){
						string[] inputArray = line.Split(' ');
						try{
							if (inputArray.Length == 3 && inputArray[0] == "GET"){
								int customerId = int.Parse(inputArray[1]);
								Requests? request = ProcessRequest(inputArray[2]);
								switch (request)
								{
									case Requests.Books:
										booksController.ShowBooks(customerId);	
										break;
									case Requests.Detail:
										booksController.ShowBookDetails(GetIntValueFromRequest(inputArray[2]), customerId);											
										break;
									case Requests.ShoppingCart:
										shoppingCartController.ShowShoppingCart(customerId);	
										break;
									case Requests.Add:
										shoppingCartController.AddToShoppingCart(customerId, GetIntValueFromRequest(inputArray[2]));
										break;
									case Requests.Remove:
										shoppingCartController.RemoveFromShoppingCart(customerId, GetIntValueFromRequest(inputArray[2]));
										break;

								    default:
										errorController.ShowError();
										break;
								}

							}
							else errorController.ShowError();
							Console.WriteLine("====");
						}
						catch {
							errorController.ShowError();
							Console.WriteLine("====");
						}
					}
				}
			}
			catch{
			    Console.WriteLine("Data error.");
			}
		}
	}
}
