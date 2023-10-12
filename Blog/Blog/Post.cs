using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    internal abstract class Post
    {
        public  User User { get; }
        public  string Title { get; }
        public string Html { get; }
        public decimal? AverageRating { get; private set; }
        public  int RatingCount { get; private set; }
        public IReadOnlyList<Comment> Comments => CommentsList.AsReadOnly();
        public Dictionary<string, int> userRatings = new Dictionary<string, int>() ;
        public List<Comment> CommentsList = new List<Comment>();
   
        protected Post(User user, string title) { 
            User = user;
            Title = title;
        }   

        public void AddComment(User user, string text)
        {
            CommentsList.Add(new Comment(user, text));
        }

        public bool TryRate(User user, int rating)
        {
            if (rating < 1 || rating > 5)
            {
                return false; 
            }

            if (userRatings.ContainsKey(user.Email))
            {
                return false; 
            }

            userRatings[user.Email] = rating;

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

