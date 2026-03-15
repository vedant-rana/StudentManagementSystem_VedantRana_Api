using System;
using System.Collections.Generic;

namespace StudentManagementSystem.DBContext;

public partial class Class
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
