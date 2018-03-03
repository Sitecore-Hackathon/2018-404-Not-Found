using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Schema;
using System;
using System.Threading.Tasks;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.Xdb.Common.Web;
using Sitecore.XConnect.Collection.Model;
using System.Collections.Generic;
using XConnect.Foundation.XConnectExtensions;

namespace XConnect.Feature.AddXdbContact.Sitecore.Admin
{
    public partial class AddXdbContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void btn_Create_Click(object sender, EventArgs e)
        {
            var itemList = new List<KeyValuePair<Guid, string>>() {
                       new KeyValuePair<Guid, string>(Guid.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"),"/Home"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{76E2550E-A389-4465-9CA7-9E3EAF479924} "), "/Home/About"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{9B4A0F5B-CE8A-4338-9CF9-A085CCD508C5}"),"/Home/Contact"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{E5FC0DA8-337B-4C78-9B0F-FB3266689D42}"),"/Home/Products"),
                       new KeyValuePair<Guid, string>(Guid.Parse(" {78C2D04A-7C74-4561-B5C2-4E571D3EA6A7}"), "/Home/Register"),

                       new KeyValuePair<Guid, string>(Guid.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"),"/Home"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{76E2550E-A389-4465-9CA7-9E3EAF479924} "), "/Home/About"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{9B4A0F5B-CE8A-4338-9CF9-A085CCD508C5}"),"/Home/Contact"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{E5FC0DA8-337B-4C78-9B0F-FB3266689D42}"),"/Home/Products"),

                       new KeyValuePair<Guid, string>(Guid.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"),"/Home"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{76E2550E-A389-4465-9CA7-9E3EAF479924} "), "/Home/About"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{9B4A0F5B-CE8A-4338-9CF9-A085CCD508C5}"),"/Home/Contact"),

                       new KeyValuePair<Guid, string>(Guid.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"),"/Home"),
                       new KeyValuePair<Guid, string>(Guid.Parse("{76E2550E-A389-4465-9CA7-9E3EAF479924} "), "/Home/About"),

                       new KeyValuePair<Guid, string>(Guid.Parse("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"),"/Home")
                       };

            foreach (var item in itemList)
            {
                setData(item);
            }

        }

        protected void setData(KeyValuePair<Guid, string> keyValuePair)
        {
            // Initialize a client using the validated configuration
            using (var client = XConnectHelper.GetClient())
            {
                try
                {
                    var channelId = Guid.Parse("52B75873-4CE0-4E98-B63A-B535739E6180"); // "email newsletter" channel

                    // Create a new contact with the identifier
                    Contact knownContact = new Contact();

                    PersonalInformation personalInfoFacet = new PersonalInformation();

                    personalInfoFacet.FirstName = "Abhi" + Guid.NewGuid().ToString();
                    personalInfoFacet.LastName = "Marwah";
                    personalInfoFacet.JobTitle = "Sitecore Architect";
                    personalInfoFacet.Gender = "Male";
                    personalInfoFacet.Nickname = "Aussie";
                    client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                    EmailAddressList emails = new EmailAddressList(new EmailAddress("test@test.test", true), "Email");

                    client.SetFacet(knownContact, emails);


                    PageViewEvent pageView = new PageViewEvent(DateTime.Now.ToUniversalTime(), keyValuePair.Key, 1, "en");
                    pageView.ItemLanguage = "en";
                    pageView.Duration = new TimeSpan(3000);
                    pageView.SitecoreRenderingDevice = new SitecoreDeviceData(new Guid("{fe5d7fdf-89c0-4d99-9aa3-b5fbd009c9f3}"), "Default");
                    pageView.Url = keyValuePair.Value;


                    client.AddContact(knownContact);

                    // Create a new interaction for that contact
                    Interaction interaction = new Interaction(knownContact, InteractionInitiator.Brand, channelId, "");

                    // Add events - all interactions must have at least one event
                    interaction.Events.Add(pageView);

                    IpInfo ipInfo = new IpInfo("127.0.0.1");

                    ipInfo.BusinessName = "Sitecore Consultancy";

                    client.SetFacet<IpInfo>(interaction, IpInfo.DefaultFacetKey, ipInfo);


                    // Add the contact and interaction
                    client.AddInteraction(interaction);

                    // Submit contact and interaction - a total of two operations
                    client.Submit();
                }
                catch (XdbExecutionException ex)
                {
                    // Deal with exception
                }
            }
        }
    }
}