using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EazyD.Models
{
    public class DictionaryItemViewModel
    {
        [Required]
        [Remote("IsKeyAvailable", "EazyD")]
        public string Key { get; set; }

        public string Parent { get; set; }

        [Required]
        public string Value { get; set; }
    }
}