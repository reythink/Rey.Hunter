using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rey.Authority.Configuration {
    public class AuthorityOptions {
        public IServiceCollection Services { get; }

        public AuthorityOptions(IServiceCollection services) {
            this.Services = services;
        }
    }
}
