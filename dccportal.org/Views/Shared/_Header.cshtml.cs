using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace dccportal.org.Views.Shared
{
    public class _Header : PageModel
    {
        private readonly ILogger<_Header> _logger;

        public _Header(ILogger<_Header> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
