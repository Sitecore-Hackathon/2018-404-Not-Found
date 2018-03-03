using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Operations;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XConnect.Foundation.XConnectExtensions;

namespace XConnect.Feature.PageViewAnalytics.Repository
{
    public class GetPageView
    {
        public static int GetPageViewsCount(string itemId)
        {
            int counter = 0;

            using (var client = XConnectHelper.GetClient())
            {
                try
                {
                    IAsyncQueryable<Sitecore.XConnect.Contact> queryable = client.Contacts
                        .Where(c => c.Interactions.Any(f => f.Events.OfType<PageViewEvent>().Any()))
                        .WithExpandOptions(new Sitecore.XConnect.ContactExpandOptions()
                         {
                             Interactions = new Sitecore.XConnect.RelatedInteractionsExpandOptions()
                             {
                                 Limit = 2000 // Returns top 20 of all contact's interactions - interactions not affected by query
                             }
                         });

                    var enumerable = queryable.GetBatchEnumeratorSync(10);
                    var urlList = new List<string>();
                    while (enumerable.MoveNext())
                    {
                        foreach (var contact in enumerable.Current)
                        {
                            InteractionReference interactionReference = new InteractionReference((Guid)contact.Id, (Guid)contact.Interactions.ElementAt(0).Id);

                            Interaction interaction = client.Get<Interaction>(interactionReference, new InteractionExpandOptions(new string[] { IpInfo.DefaultFacetKey })
                            {
                                Contact = new RelatedContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey })
                            });
                            var pageViews = interaction.Events.OfType<PageViewEvent>().OrderBy(ev => ev.Timestamp).ToList();
                            foreach (var page in pageViews)
                            {
                                // item id is in page 
                                if (page.ItemId.Equals(Guid.Parse(itemId)))
                                {
                                    counter++;
                                }
                                urlList.Add(page.Url);
                            }

                            Console.WriteLine("Page Url: " + pageViews.FirstOrDefault().Url);
                        }
                    }

                    //var count = urlList.GroupBy(x => x).Select(y => new { } x.Count());

                    var count = urlList.GroupBy(n => n).Select(c => new { Key = c.Key, total = c.Count() });
                    return counter;
                }
                catch (XdbExecutionException ex)
                {

                }
                return counter;
            }
        }

    }
}