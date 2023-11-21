using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Class
    {
        public string ClassId { get; set; } = null!;
        public string ClassName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
