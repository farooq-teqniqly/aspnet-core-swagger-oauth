﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models
{
    public class AuthOptions
    {
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string AuthUrl { get; set; }
        public string ClientIdUri { get; set; }
        public string TokenUrl { get; set; }
    }
}
