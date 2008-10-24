using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MdgExample.Models
{
  public class Tree
  {
    public int Age { get; set; }
    public int Height { get; set; }

    public void ValidateHeightForAge()
    {
      if (Height > (1.9 * Age))
      {
        throw new Exception("Height exceeds valid range for the age of this tree");
      }
    }
  }
}
