using BlogManager.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Xml.Linq;
using TestHelpers;
using static TestHelpers.ProgramChecker;

namespace BlogManager.Application
{

    public class User
    {
        public string Firstname { get;  }
        public string Lastname { get; }
        public string Email { get; }

        public User(string firstname, string lastname, string email)
        {
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
        }
    }
    public class Comment
    {
        public User User { get; }
        public string Text { get; }
        public DateTime Created { get; }
        public Comment(User user, string text)
        {
            User = user;
            Text = text;
            Created = DateTime.UtcNow;
        }
    }

    public abstract class Post
    {
        public User User { get; }
        public string Title { get; }
        public abstract string Html { get; }
        public decimal? AverageRating { get; private set; }
        public int RatingCount { get; private set; } 
        public Dictionary<string, int> rate = new Dictionary<string, int>();
        public IReadOnlyList<Comment> Comments => comments.AsReadOnly();
        public List<Comment> comments = new List<Comment> { };

        protected Post (User user,  string title)
        {
            User = user;
            Title = title;
        }

        public void AddComment(User user, string text)
        {
           comments.Add(new Comment(user, text));
        }

        public bool TryRate(User user, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                return false;
            }

            if (rate.ContainsKey(user.Email))
            {
                return false;
            }

            rate[user.Email] = rating;

            if (AverageRating == null)
            {
                AverageRating = rating;
            }
            else
            {
                AverageRating = (AverageRating * RatingCount + rating) / (RatingCount + 1);
            }

            RatingCount++;

            return true;
        }
    }
    }

public class ImagePost : Post
{
    public string Url { get;  }

    public override string Html => "<img src=\"" + Url + "\" />";

    public ImagePost(User user, string title, string url) : base(user, title)
    {
        Url = url;
    }
}

    public class TextPost : Post
    {
    public string Content { get;}

    public override string Html => "<h1>" + Title + "</h1><p>" + Content + "</p>";

    public TextPost (User user, string title, string content) : base(user, title)
    {
        Content = content;
    }
}
    public class Program
    {
        public static void Main(string[] args)
        {
            {
                Console.WriteLine("Teste Klassenimplementierung.");
                foreach (var type in new Type[] { typeof(User), typeof(Comment), typeof(Post), typeof(ImagePost), typeof(TextPost) })
                {
                    CheckAndWrite(() => !type.HasDefaultConstructor(), $"Kein Defaultkonstruktor in {type.Name}.");
                    CheckAndWrite(() => type.IsImmutable(), $"{type.Name} ist immutable.");
                }
                CheckAndWrite(() => typeof(Post).PropertyHasType<IReadOnlyList<Comment>>(nameof(Post.Comments)),
                    "Post.Comments ist vom Typ IReadOnlyList<Comment>.");
                CheckAndWrite(() => typeof(Post).GetProperty(nameof(Post.Html))?.GetMethod?.IsAbstract == true,
                    "Post.Html ist abstrakt.");
            }
            {
                Console.WriteLine("Teste HTML Ausgabe.");
                var user = new User(email: "email1", firstname: "firstname1", lastname: "lastname1");
                Post imagePost = new ImagePost(user, "title", "url");
                Post textPost = new TextPost(user, "title", "content");
                CheckAndWrite(() => imagePost.Html == "<img src=\"url\" />", "ImagePost.Html liefert den richtign HTML String.");
                CheckAndWrite(() => textPost.Html == "<h1>title</h1><p>content</p>", "TextPost.Html liefert den richtign HTML String.");
            }
            {
                Console.WriteLine("Teste Kommentieren");
                var user = new User(email: "email1", firstname: "firstname1", lastname: "lastname1");
                var commentator = new User(email: "email2", firstname: "firstname2", lastname: "lastname2");
                Post post = new ImagePost(user, "title", "url");
                post.AddComment(commentator, "comment");
                CheckAndWrite(() => post.Comments.Count == 1, "Post.AddComment fügt einen Kommentar hinzu.", 2);
                CheckAndWrite(() => post.Comments[0].Created > DateTime.UtcNow.AddMinutes(-1), "Post.AddComment setzt Created auf UtcNow.", 2);
            }
            {
                Console.WriteLine("Teste Rating");
                var user = new User(email: "email2", firstname: "firstname2", lastname: "lastname2");
                var user2 = new User(email: "email2", firstname: "firstname3", lastname: "lastname3");
                Post post = new ImagePost(user, "title", "url");
                CheckAndWrite(() => !post.TryRate(user, 6), "Post.TryRate lehnt ungültige Werte ab.", 2);
                CheckAndWrite(() => post.TryRate(user, 1) && post.RatingCount == 1, "Post.TryRate fügt ein Rating hinzu.", 2);
                CheckAndWrite(() => !post.TryRate(user2, 2) && post.RatingCount == 1, "Post.TryRate liefert false, wenn die Email schon geratet hat.", 2);
            }
            {
                Console.WriteLine("Teste AverageRating");
                var user = new User(email: "email1", firstname: "firstname1", lastname: "lastname1");
                var user2 = new User(email: "email2", firstname: "firstname2", lastname: "lastname2");
                Post post = new ImagePost(user, "title", "url");
                CheckAndWrite(() => post.AverageRating is null, "Post.AverageRating ist null, wenn kein Rating abgegeben wurde.", 2);
                post.TryRate(user, 2);
                post.TryRate(user2, 3);
                CheckAndWrite(() => post.AverageRating == 2.5M, "Post.AverageRating berechnet den Durchschnitt der Ratings.", 2);
            }

            WriteSummary();
        }
    }
