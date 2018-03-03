using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.ContentEditor.Gutters;
using System;

namespace XConnect.Feature.PageViewAnalytics.Repository
{
    public class PageVisitGutter : GutterRenderer
    {
        private int GetPageVisit(Item DatabaseItem)
        {
            Database Web = Database.GetDatabase("web");
            Item WebItem = Web.GetItem(DatabaseItem.ID);

            if (WebItem != null)
            {
                return GetPageView.GetPageViewsCount(WebItem.ID.ToString());
            }
            return 0;
        }

        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            ///sitecore/content/Applications/Content Editor/Gutters
            int count = GetPageVisit(item);
            GutterIconDescriptor descriptor = new GutterIconDescriptor();
            if (count != 0)
            {
                descriptor.Icon = "Applications/32x32/bullet_ball_blue.png";
                descriptor.Tooltip = "Item visit count is " + count;
                descriptor.Click = String.Format("item:load(ID={0})", item.ID);
                return descriptor;
            }
            return descriptor;
        }
    }
}