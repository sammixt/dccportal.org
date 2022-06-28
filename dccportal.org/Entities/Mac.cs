using System;
using System.Collections.Generic;

namespace dccportal.org.Entities
{
    public partial class Mac
    {
        public string MacNo { get; set; } = null!;
        public string User { get; set; } = null!;
        public DateTime Joined { get; set; }
        public int MacId { get; set; }
    }
}
