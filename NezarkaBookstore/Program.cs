using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net;

namespace nezarka
{

   //
    // Model
    //

    class ModelStore
    {
        private List<Book> books = new List<Book>();
        private List<Customer> customers = new List<Customer>();

        public IList<Book> GetBooks()
        {
            return books;
        }

        public Book GetBook(int id)
        {
            return books.Find(b => b.Id == id);
        }

        public Customer GetCustomer(int id)
        {
            return customers.Find(c => c.Id == id);
        }

        public static ModelStore LoadFrom(TextReader reader)
        {
            var store = new ModelStore();

            try
            {
                if (reader.ReadLine() != "DATA-BEGIN")
                {
                    return null;
                }
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        return null;
                    }
                    else if (line == "DATA-END")
                    {
                        break;
                    }

                    string[] tokens = line.Split(';');
                    switch (tokens[0])
                    {
                        case "BOOK":
                            store.books.Add(new Book
                            {
                                Id = int.Parse(tokens[1]),
                                Title = tokens[2],
                                Author = tokens[3],
                                Price = decimal.Parse(tokens[4])
                            });
                            break;
                        case "CUSTOMER":
                            store.customers.Add(new Customer
                            {
                                Id = int.Parse(tokens[1]),
                                FirstName = tokens[2],
                                LastName = tokens[3]
                            });
                            break;
                        case "CART-ITEM":
                            var customer = store.GetCustomer(int.Parse(tokens[1]));
                            if (customer == null)
                            {
                                return null;
                            }
                            customer.ShoppingCart.Items.Add(new ShoppingCartItem
                            {
                                BookId = int.Parse(tokens[2]),
                                BookCount = int.Parse(tokens[3])
                            });
                            break;
                        default:
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is IndexOutOfRangeException)
                {
                    return null;
                }
                return null;
                throw;
            }

            return store;
        }
    }

    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
    }

    class Customer
    {
        private ShoppingCart shoppingCart;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ShoppingCart ShoppingCart
        {
            get
            {
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart();
                }
                return shoppingCart;
            }
            set
            {
                shoppingCart = value;
            }
        }
    }

    class ShoppingCartItem
    {
        public int BookId { get; set; }
        public int BookCount { get; set; }
    }

    class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
    }
    
    internal class Program
    {
        static void writeHeadTable(string name, string count)
        {
            var head = """
                <!DOCTYPE html>
                <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
                <head>
                	<meta charset="utf-8" />
                	<title>Nezarka.net: Online Shopping for Books</title>
                </head>
                <body>
                	<style type="text/css">
                		table, th, td {
                			border: 1px solid black;
                			border-collapse: collapse;
                		}
                		table {
                			margin-bottom: 10px;
                		}
                		pre {
                			line-height: 70%;
                		}
                	</style>
                	<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>
                """;
            Console.WriteLine(head);
            Console.WriteLine($"	{name}, here is your menu:");
            var table = """
                        <table>
                                <tr>
                                        <td><a href="/Books">Books</a></td>
                """;
            Console.WriteLine(table);
            Console.WriteLine($"\t\t\t<td><a href=\"/ShoppingCart\">Cart ({count})</a></td>");
            var end = """
                                </tr>
                        </table>
                """;
            Console.WriteLine(end);
        }

        static void InvalidRequest()
        {
            var invalid = """
                <!DOCTYPE html>
                <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
                <head>
                	<meta charset="utf-8" />
                	<title>Nezarka.net: Online Shopping for Books</title>
                </head>
                <body>
                <p>Invalid request.</p>
                </body>
                </html>
                """;
            Console.WriteLine(invalid);
        }

        static void cBook(ModelStore ms, string cusid)
        {
            var b = ms.GetBooks();
            var cus = ms.GetCustomer(int.Parse(cusid));
            if (cus == null || b == null)
            { InvalidRequest(); }
            else
            {
                writeHeadTable(cus.FirstName, Convert.ToString(cus.ShoppingCart.Items.Count));
                int writed = 0;
                var wr1 = """
                        Our books for you:
                        <table>
                """;
                var tr = """
                                </tr>
                                <tr>
                """;
                Console.WriteLine(wr1);
                foreach (var bk in b)
                {
                    if (writed % 3 == 0 && writed != 0)
                        Console.WriteLine(tr);
                    else if (writed == 0)
                        Console.WriteLine("\t\t<tr>");
                    Console.WriteLine("\t\t\t<td style=\"padding: 10px;\">");
                    Console.WriteLine($"\t\t\t\t<a href=\"/Books/Detail/{bk.Id}\">{bk.Title}</a><br />");
                    Console.WriteLine($"\t\t\t\tAuthor: {bk.Author}<br />");
                    Console.WriteLine($"\t\t\t\tPrice: {bk.Price} EUR &lt;<a href=\"/ShoppingCart/Add/{bk.Id}\">Buy</a>&gt;");
                    Console.WriteLine("\t\t\t</td>");
                    writed++;
                }
                var wr2 = """

                    """;
                if (writed == 0)
                {
                    wr2 = """
                	</table>
                </body>
                </html>
                """;
                }
                else
                {
                    wr2 = """
                		</tr>
                	</table>
                </body>
                </html>
                """;
                }
                Console.WriteLine(wr2);
            }
        }

        static bool checkIT(string[] IT)
        {
            bool ss = true;
            if (IT.Length != 3) { ss = false; }
            try
            {
                if (IT[0] != "GET") { ss = false; }
                int _ = Convert.ToInt32(IT[1]);
                string[] html = IT[2].Split('/');
                if (html.Length != 4 && html.Length != 6) { ss = false; }
                if (html[0] != "http:") { ss = false; }
                if (html[1] != "") { ss = false; }
                if (html[2] != "www.nezarka.net") { ss = false; }
                if ((html[3] != "Books") && html[3] != "ShoppingCart") { ss = false; }
                if (html.Length == 6)
                {
                    if (html[4] != "Detail" && html[4] != "Add" && html[4] != "Remove") { ss = false; }
                    int __ = Convert.ToInt32(html[5]);
                }
            }
            catch { ss = false; }
            return ss;
        }

        static void cBooksDetail_BookId_(ModelStore ms, string cusid, int bid)
        {
            var b = ms.GetBook(bid);
            var cus = ms.GetCustomer(int.Parse(cusid));
            if (cus == null || b == null)
            { InvalidRequest(); }
            else
            {
                writeHeadTable(cus.FirstName, Convert.ToString(cus.ShoppingCart.Items.Count));
                Console.WriteLine("\tBook details:");
                Console.WriteLine($"\t<h2>{b.Title}</h2>");
                Console.WriteLine("\t<p style=\"margin-left: 20px\">");
                Console.WriteLine($"\tAuthor: {b.Author}<br />");
                Console.WriteLine($"\tPrice: {b.Price} EUR<br />");
                Console.WriteLine("\t</p>");
                Console.WriteLine($"\t<h3>&lt;<a href=\"/ShoppingCart/Add/{b.Id}\">Buy this book</a>&gt;</h3>");
                Console.WriteLine("</body>");
                Console.WriteLine("</html>");
            }
        }

        static void cShoppingCart(ModelStore ms, string cusid)
        {
            var cus = ms.GetCustomer(int.Parse(cusid));
            var x = ms.GetBooks();
            if (cus == null || x == null)
            { InvalidRequest(); }
            else
            {
                writeHeadTable(cus.FirstName, Convert.ToString(cus.ShoppingCart.Items.Count));
                if (cus.ShoppingCart.Items == null || cus.ShoppingCart.Items.Count == 0)
                {
                    var idk = """
                            Your shopping cart is EMPTY.
                    </body>
                    </html>
                    """;
                    Console.WriteLine(idk);
                }
                else
                {
                    var wr1 = """
                        Your shopping cart:
                        <table>
                	        <tr>
                		        <th>Title</th>
                		        <th>Count</th>
                		        <th>Price</th>
                		        <th>Actions</th>
                	        </tr>
                """;
                    Console.WriteLine(wr1);
                    var totalPrice = 0;
                    foreach (var bid in cus.ShoppingCart.Items)
                    {
                        var b = ms.GetBook(bid.BookId);
                        Console.WriteLine("\t\t<tr>");
                        Console.WriteLine($"\t\t\t<td><a href=\"/Books/Detail/{b.Id}\">{b.Title}</a></td>");
                        Console.WriteLine($"\t\t\t<td>{bid.BookCount}</td>");
                        if (bid.BookCount == 1)
                        { Console.WriteLine($"\t\t\t<td>{b.Price} EUR</td>"); totalPrice += Convert.ToInt32(b.Price); }
                        else { Console.WriteLine($"\t\t\t<td>{bid.BookCount} * {b.Price} = {bid.BookCount * b.Price} EUR</td>"); totalPrice += Convert.ToInt32(bid.BookCount * b.Price); }
                        Console.WriteLine($"\t\t\t<td>&lt;<a href=\"/ShoppingCart/Remove/{b.Id}\">Remove</a>&gt;</td>");
                        Console.WriteLine("\t\t</tr>");
                    }
                    Console.WriteLine($"\t</table>");
                    Console.WriteLine($"\tTotal price of all items: {totalPrice} EUR");
                    Console.WriteLine("</body>");
                    Console.WriteLine("</html>");
                }
            }
        }

        static ModelStore addBook(ModelStore ms, string cusid, string bookid)
        {
            var cus = ms.GetCustomer(int.Parse(cusid));
            int bi = Convert.ToInt32(bookid);
            var x = ms.GetBook(bi);
            bool itsin = false;
            if (cus != null && x != null)
            {
                foreach (var b in cus.ShoppingCart.Items)
                {
                    if (b.BookId == bi)
                    {
                        itsin = true;
                        b.BookCount++;
                        break;
                    }
                }
                if (!itsin)
                {
                    ShoppingCartItem item = new ShoppingCartItem();
                    item.BookCount = 1;
                    item.BookId = bi;
                    if (cus.ShoppingCart.Items == null) { cus.ShoppingCart.Items = new List<ShoppingCartItem>(); }
                    cus.ShoppingCart.Items.Add(item);
                }
            } else { InvalidRequest(); }
            return ms;
        }

        static ModelStore removeBook(ModelStore ms, string cusid, string bookid)
        {
            var cus = ms.GetCustomer(int.Parse(cusid));
            int bi = Convert.ToInt32(bookid);
            var x = ms.GetBook(bi);
            bool itsin = false;
            if (cus != null && x != null)
            {
                foreach (var b in cus.ShoppingCart.Items)
                {
                    if (b.BookId == bi)
                    {
                        itsin = true;
                        b.BookCount--;
                        if (b.BookCount <= 0)
                            cus.ShoppingCart.Items.Remove(b);
                        break;
                    }
                }

                if (!itsin)
                {
                    InvalidRequest();
                }
                else { cShoppingCart(ms, cusid); }
            }
            else { InvalidRequest(); }
            return ms;
        }

        static void Main(string[] args)
        {
            ModelStore mStore = ModelStore.LoadFrom(Console.In);
            if (mStore == null)
                Console.WriteLine("Data error.");
            else
            {
               try
                { 
                    string? line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        string[] getIdArg = line.Split(' ');
                        if (!checkIT(getIdArg))
                        {
                            InvalidRequest();
                        }
                        else
                        {
                            string comanda = "";
                            string[] paneboze = getIdArg[2].Substring(23).Split('/');
                            try { comanda = paneboze[0] + "/" + paneboze[1]; }
                            catch { comanda = paneboze[0]; }
                            var ccc = mStore.GetCustomer(Convert.ToInt32(getIdArg[1]));
                            /*if (ccc == null || ccc.ShoppingCart.Items == null)
                                InvalidRequest();
                            else
                            {*/
                                switch (comanda)
                                {
                                    case "Books":
                                        cBook(mStore, getIdArg[1]);
                                        break;
                                    case "Books/Detail":
                                        cBooksDetail_BookId_(mStore, getIdArg[1], Convert.ToInt32(paneboze[2]));
                                        break;
                                    case "ShoppingCart":
                                        cShoppingCart(mStore, getIdArg[1]);
                                        break;
                                    case "ShoppingCart/Add":
                                        mStore = addBook(mStore, getIdArg[1], paneboze[2]);
                                        cShoppingCart(mStore, getIdArg[1]);
                                        break;
                                    case "ShoppingCart/Remove":
                                        mStore = removeBook(mStore, getIdArg[1], paneboze[2]);
                                        break;
                                    default:
                                        InvalidRequest();
                                        break;
                                }
                            
                        }
                        Console.WriteLine("====");
                    }
                }
                catch { InvalidRequest(); Console.WriteLine("===="); }
            }

        }
    }
}