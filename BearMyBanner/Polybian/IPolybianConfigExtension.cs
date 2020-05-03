using System;
using System.Collections.Generic;

namespace BearMyBanner.Settings
{
    public static class IPolybianConfigExtension
    {
        public static IPolybianConfig InitializeTemplateList(this IPolybianConfig config)
        {
            config.PolybianUnits = new List<PolybianUnit>()
            {
                new PolybianUnit("imperial_infantryman", new List<string>()
                {
                    "3.0.111.1536.1536.756.764.0.0.0",
                    "3.1.111.1536.1536.756.764.0.0.0",
                    "3.2.111.1536.1536.756.764.0.0.0",
                    "3.3.111.1536.1536.756.764.0.0.0",
                    "3.4.111.1536.1536.756.764.0.0.0"
                }),
                new PolybianUnit("imperial_equite", new List<string>()
                {
                    "3.5.111.1536.1536.756.764.0.0.0",
                    "3.6.111.1536.1536.756.764.0.0.0",
                    "3.7.111.1536.1536.756.764.0.0.0",
                    "3.8.111.1536.1536.756.764.0.0.0",
                    "3.9.111.1536.1536.756.764.0.0.0"
                })
            };

            return config;
        }
    }
}
