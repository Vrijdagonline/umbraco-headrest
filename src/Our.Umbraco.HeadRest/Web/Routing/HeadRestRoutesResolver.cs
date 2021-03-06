﻿using System.Collections.Generic;

namespace Our.Umbraco.HeadRest.Web.Routing
{
    public abstract class HeadRestRoutesResolver
    {
        internal string Slug { get; private set; }

        protected HeadRestRoutesResolver(string slug)
        {
            Slug = slug;
        }

        public string[] ResolveRoutes(HeadRestRoutesResolverContext context)
        {
            var navigator = context.UmbracoContext.ContentCache.GetXPathNavigator();
            var itterator = navigator.Select($"id({context.RootContent.Id})/descendant-or-self::*[@isDoc]");

            var routes = new List<string>();

            while (itterator.MoveNext())
            {
                if (int.TryParse(itterator.Current.Evaluate("string(@id)").ToString(), out int id))
                {
                    Resolve(routes, id, itterator.Current.Evaluate("string(@nodeTypeAlias)").ToString(), context);
                }
            }

            return routes.ToArray();
        }

        public abstract void Resolve(ICollection<string> routes, int nodeId, string contentTypeAlias, HeadRestRoutesResolverContext context);
    }
}
