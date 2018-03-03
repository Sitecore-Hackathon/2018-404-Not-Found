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
            CertificateWebRequestHandlerModifierOptions options =
            CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=BDB624ADFC98D42D2CCC9E02FB2FBFA8C3919B23");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            var collectionClient = new CollectionWebApiClient(new Uri("https://sc90.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://sc90.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://sc90.xconnect/configuration"), clientModifiers, new[] { certificateModifier });

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
