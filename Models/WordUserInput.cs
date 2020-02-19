using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WordSearchMVC5.Models
{
    public class WordUserInput
    {
        [Required]
        public int GridSize { get; set; }
        [Required]
        public string UserWords { get; set; }
    }
}