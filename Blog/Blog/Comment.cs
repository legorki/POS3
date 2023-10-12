using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Blog
{
    internal class Comment
    {
        public User Users { get; }
        public string Text { get; }
        public DateTime Created { get; }

        public Comment(User user, string text)
        {
            Users = user;
            Text = text;
            Created = DateTime.UtcNow;
        }
    }
}
