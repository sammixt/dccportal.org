using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Post
    {
        public Post()
        {
            Members = new HashSet<Member>();
        }

        public string PostId { get; set; } = null!;
        public string? PostName { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
