using FullStackRecipeApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackRecipeApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public AccessControl accessControl { get; set; }
        public IndexModel(ILogger<IndexModel> logger, AccessControl accessControl)
        {
            _logger = logger;
            this.accessControl = accessControl;
        }

        public void OnGet()
        {

        }
    }
}
