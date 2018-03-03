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


namespace XConnect.Foundation.XConnectExtensions
{
    public static class XConnectHelper
    {
        public static XConnectClient GetClient()
        {
            string xconnectCollectionCertificateConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["xconnect.collection.certificate"].ConnectionString;
            string xconnectCollectionConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["xconnect.collection"].ConnectionString; ;
            CertificateWebRequestHandlerModifierOptions options =
            CertificateWebRequestHandlerModifierOptions.Parse("xconnectCollectionCertificateConnectionString");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri(xconnectCollectionConnectionString + "/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri(xconnectCollectionConnectionString + "/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri(xconnectCollectionConnectionString + "/configuration"), clientModifiers, new[] { certificateModifier });

            var config = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                config.Initialize();
            }
            catch (XdbModelConflictException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return new XConnectClient(config);
        }



    }
}
